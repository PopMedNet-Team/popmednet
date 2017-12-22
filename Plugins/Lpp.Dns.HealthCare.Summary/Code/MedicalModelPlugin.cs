using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;
using Lpp.Mvc;
using Lpp.Mvc.Composition;
using System.ServiceModel;
using Lpp.Composition;
using System.IO;
using System.Text;
using Lpp.Data;

namespace Lpp.Dns.Models.Medical
{
    [Export( typeof( IDnsModelPlugin ) ), ExportScope(UnitOfWorkScope.Id)]
    public class MedicalModelPlugin : IDnsModelPlugin
    {
        [Import] public IRepository<MedicalDomain, StratificationCategoryLookUp> Stratifications { get; set; }

        private static readonly IEnumerable<IDnsModel> _models = new[] { 
            Dns.Model( new Guid( "4C99FA21-CDEA-4B09-B95B-EEBDDA05ADEA" ), "Summary Query",
                SummaryRequestType.All.Select( t => Dns.RequestType( t.Id, t.Name ) ) ) 
        };
        public IEnumerable<IDnsModel> Models
        {
            get { return _models; }
        }

        public Func<HtmlHelper, IHtmlString> DisplayRequest( IDnsRequestContext context )
        {
            var doc = context.Documents.FirstOrDefault();
            if ( doc == null ) return html => new MvcHtmlString( "Query not yet created" );

            return html =>
            {
                using ( var s = doc.OpenBody() ) return new MvcHtmlString( new StreamReader( s ).ReadToEnd() );
            };
        }

        public Func<HtmlHelper, IHtmlString> DisplayResponse( IDnsResponseContext context )
        {
            throw new NotImplementedException();
        }

        public Func<HtmlHelper,IHtmlString> EditRequestView( IDnsRequestContext context )
        {
            return html => html.Partial<Views.Summary.Create>( new Models.MedicalRequestModel
                {
                    RequestType = SummaryRequestType.All.FirstOrDefault( rt => rt.Id == context.RequestType.Id ),
                    Stratifications = Stratifications.All.Where( s => s.StratificationType == "age" ).ToList()
                } );
        }

        public DnsRequestTransaction EditRequestPost( IDnsRequestContext request, IDnsPostContext post )
        {
            var m = post.GetModel<Medical.Models.MedicalRequestModel>();
            var sqlText = GenerateSqlQuery( m );
            if ( string.IsNullOrEmpty( sqlText ) ) return DnsRequestTransaction.Failed( "Failed to create the SQL Query" );

            var sqlTextBytes = Encoding.UTF8.GetBytes( sqlText );
            var doc = Dns.Document( "SQL Text", "text/plain", () => new MemoryStream( sqlTextBytes ), () => sqlTextBytes.Length );
            var existingDoc = request.Documents.FirstOrDefault();

            return new DnsRequestTransaction
            {
                NewDocuments = existingDoc != null ? null : new[] { doc },
                UpdateDocuments = existingDoc == null ? null : 
                    new[] { new { existingDoc, doc } }.ToDictionary( x => x.existingDoc, x => x.doc ),
                RemoveDocuments = request.Documents.Skip( 1 )
            };
        }

        private string GenerateSqlQuery( Medical.Models.MedicalRequestModel m )
        {
            var strat = m.AgeStratification == null ? null : Stratifications.Find( m.AgeStratification.Value, "age" );
            if ( strat == null ) return null;

            var stratValues = strat.ClassificationText.Split( ',' );
            var codes = string.Join( ",", m.Codes.Split( ',' ).Select( c => string.Format( "'{0}'", c.Trim() ) ) );

            return string.Format( @"
                Select 
	                SummaryData.*,  
	                EnrollmentData.EnrollmentMembers as [Total Enrollment in Strata(Members)],
	                round(SummaryData.Members / EnrollmentData.EnrollmentMembers * 1000, 1) as [Prevalence Rate (Users per 1000 enrollees)],
	                round(SummaryData.Events / EnrollmentData.EnrollmentMembers * 1000, 1) as [Event Rate (Events per 1000 enrollees)], 
	                round(SummaryData.Events/SummaryData.Members ,1) as [Events Per member] 
                From 
                    (  
	                    Select 
		                    AgeGroup, gender as Sex, Period, Code as DXCode, DXName, Setting, 
		                    Sum(Event) as Events, Sum(Member) as Members 
                        From ({0}) as OuterTable 
	                    Group by AgeGroup, gender, Period, Code, DXName,Setting
                    ) 
                    as SummaryData 

                Left JOIN 
                    (  
	                    Select AgeGroup, gender as Sex, Year, Sum(Member) as EnrollmentMembers 
	                    From ({1}) as OuterEnrollmentTable 
	                    Group by AgeGroup, gender, Year   
                    ) 
                    as EnrollmentData 

                ON 
	                EnrollmentData.AgeGroup = SummaryData.Agegroup AND 
	                EnrollmentData.sex = Summarydata.Sex AND 
	                EnrollmentData.Year = Summarydata.Period 
                ",
                string.Join( "\n Union All \n", stratValues.Select( v => GenerateDiagQuery( v, codes ) ) ),
                string.Join( "\n Union All \n", stratValues.Select( GenerateEnrollmentQuery ) ) 
            );
        }

        string GenerateDiagQuery( string age, string codes )
        {
            return string.Format( @"
			    SELECT 
				    '{0}' as AgeGroup,  				
				    'All'  as gender, 				
				    Period, Code, DXName, Setting, Events as Event, Members as Member 
			    FROM 				
				    ICD9_diagnosis  
			    WHERE  				
				    code IN ({1})  				
				    AND  				
				    Age_Group in ('{0}')", age, codes );
        }


        string GenerateEnrollmentQuery( string age )
        {
            return string.Format( @"
			    SELECT 
				    '{0}' as AgeGroup,  
				    'All'  as gender, 
				    Year, DrugCov, MedCov, Members as Member 
			    FROM Enrollment  
			    WHERE 
				    MedCov = 'Y'  AND  
				    Age_Group in ('{0}') ", age );
        }
    }
}