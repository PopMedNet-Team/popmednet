using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lpp.Dns.DataMart.Model;
using System.IO;

namespace Lpp.Dns.DataMart.Model.Processors.Tests
{
    [TestClass]
    public class WSModelProcessorTests
    {
        //string SERVICE_URL = "http://localhost:11938/SampleModelProcessorRestService.svc";
        //string SERVICE_URL = "http://localhost:8888/SampleModelProcessorRestService.svc";
        //string SERVICE_URL = "http://localhost:59066/SummaryQueryRestService.svc";
        //string SERVICE_URL = "http://localhost:8888/SummaryQueryRestService.svc";
        //string SERVICE_URL = "http://localhost:9090/pmn";
        string SERVICE_URL = "http://localhost:8888/pmn";

        string FILEPATH = @"C:\Users\daniel\Documents\drn\UploadSequenceDiagram.jpg";

        string SQL_TEXT = @"Select 
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
                        From (
			    SELECT 
				    '(0-1' as AgeGroup,  				
				    'All'  as gender, 				
				    Period, Code, DXName, Setting, Events as Event, Members as Member 
			    FROM 				
				    ICD9_diagnosis  
			    WHERE  				
				    code IN ('144')  				
				    AND  				
				    Age_Group in ('(0-1')
 Union All 

			    SELECT 
				    '2-4' as AgeGroup,  				
				    'All'  as gender, 				
				    Period, Code, DXName, Setting, Events as Event, Members as Member 
			    FROM 				
				    ICD9_diagnosis  
			    WHERE  				
				    code IN ('144')  				
				    AND  				
				    Age_Group in ('2-4')
 Union All 

			    SELECT 
				    '5-9' as AgeGroup,  				
				    'All'  as gender, 				
				    Period, Code, DXName, Setting, Events as Event, Members as Member 
			    FROM 				
				    ICD9_diagnosis  
			    WHERE  				
				    code IN ('144')  				
				    AND  				
				    Age_Group in ('5-9')
 Union All 

			    SELECT 
				    '10-14' as AgeGroup,  				
				    'All'  as gender, 				
				    Period, Code, DXName, Setting, Events as Event, Members as Member 
			    FROM 				
				    ICD9_diagnosis  
			    WHERE  				
				    code IN ('144')  				
				    AND  				
				    Age_Group in ('10-14')
 Union All 

			    SELECT 
				    '15-18' as AgeGroup,  				
				    'All'  as gender, 				
				    Period, Code, DXName, Setting, Events as Event, Members as Member 
			    FROM 				
				    ICD9_diagnosis  
			    WHERE  				
				    code IN ('144')  				
				    AND  				
				    Age_Group in ('15-18')
 Union All 

			    SELECT 
				    '19-21' as AgeGroup,  				
				    'All'  as gender, 				
				    Period, Code, DXName, Setting, Events as Event, Members as Member 
			    FROM 				
				    ICD9_diagnosis  
			    WHERE  				
				    code IN ('144')  				
				    AND  				
				    Age_Group in ('19-21')
 Union All 

			    SELECT 
				    '22-44' as AgeGroup,  				
				    'All'  as gender, 				
				    Period, Code, DXName, Setting, Events as Event, Members as Member 
			    FROM 				
				    ICD9_diagnosis  
			    WHERE  				
				    code IN ('144')  				
				    AND  				
				    Age_Group in ('22-44')
 Union All 

			    SELECT 
				    '45-64' as AgeGroup,  				
				    'All'  as gender, 				
				    Period, Code, DXName, Setting, Events as Event, Members as Member 
			    FROM 				
				    ICD9_diagnosis  
			    WHERE  				
				    code IN ('144')  				
				    AND  				
				    Age_Group in ('45-64')
 Union All 

			    SELECT 
				    '65-74' as AgeGroup,  				
				    'All'  as gender, 				
				    Period, Code, DXName, Setting, Events as Event, Members as Member 
			    FROM 				
				    ICD9_diagnosis  
			    WHERE  				
				    code IN ('144')  				
				    AND  				
				    Age_Group in ('65-74')
 Union All 

			    SELECT 
				    '75+)' as AgeGroup,  				
				    'All'  as gender, 				
				    Period, Code, DXName, Setting, Events as Event, Members as Member 
			    FROM 				
				    ICD9_diagnosis  
			    WHERE  				
				    code IN ('144')  				
				    AND  				
				    Age_Group in ('75+)')) as OuterTable 
	                    Group by AgeGroup, gender, Period, Code, DXName,Setting
                    ) 
                    as SummaryData 

                Left JOIN 
                    (  
	                    Select AgeGroup, gender as Sex, Year, Sum(Member) as EnrollmentMembers 
	                    From (
			    SELECT 
				    '(0-1' as AgeGroup,  
				    'All'  as gender, 
				    Year, DrugCov, MedCov, Members as Member 
			    FROM Enrollment  
			    WHERE 
				    MedCov = 'Y'  AND  
				    Age_Group in ('(0-1') 
 Union All 

			    SELECT 
				    '2-4' as AgeGroup,  
				    'All'  as gender, 
				    Year, DrugCov, MedCov, Members as Member 
			    FROM Enrollment  
			    WHERE 
				    MedCov = 'Y'  AND  
				    Age_Group in ('2-4') 
 Union All 

			    SELECT 
				    '5-9' as AgeGroup,  
				    'All'  as gender, 
				    Year, DrugCov, MedCov, Members as Member 
			    FROM Enrollment  
			    WHERE 
				    MedCov = 'Y'  AND  
				    Age_Group in ('5-9') 
 Union All 

			    SELECT 
				    '10-14' as AgeGroup,  
				    'All'  as gender, 
				    Year, DrugCov, MedCov, Members as Member 
			    FROM Enrollment  
			    WHERE 
				    MedCov = 'Y'  AND  
				    Age_Group in ('10-14') 
 Union All 

			    SELECT 
				    '15-18' as AgeGroup,  
				    'All'  as gender, 
				    Year, DrugCov, MedCov, Members as Member 
			    FROM Enrollment  
			    WHERE 
				    MedCov = 'Y'  AND  
				    Age_Group in ('15-18') 
 Union All 

			    SELECT 
				    '19-21' as AgeGroup,  
				    'All'  as gender, 
				    Year, DrugCov, MedCov, Members as Member 
			    FROM Enrollment  
			    WHERE 
				    MedCov = 'Y'  AND  
				    Age_Group in ('19-21') 
 Union All 

			    SELECT 
				    '22-44' as AgeGroup,  
				    'All'  as gender, 
				    Year, DrugCov, MedCov, Members as Member 
			    FROM Enrollment  
			    WHERE 
				    MedCov = 'Y'  AND  
				    Age_Group in ('22-44') 
 Union All 

			    SELECT 
				    '45-64' as AgeGroup,  
				    'All'  as gender, 
				    Year, DrugCov, MedCov, Members as Member 
			    FROM Enrollment  
			    WHERE 
				    MedCov = 'Y'  AND  
				    Age_Group in ('45-64') 
 Union All 

			    SELECT 
				    '65-74' as AgeGroup,  
				    'All'  as gender, 
				    Year, DrugCov, MedCov, Members as Member 
			    FROM Enrollment  
			    WHERE 
				    MedCov = 'Y'  AND  
				    Age_Group in ('65-74') 
 Union All 

			    SELECT 
				    '75+)' as AgeGroup,  
				    'All'  as gender, 
				    Year, DrugCov, MedCov, Members as Member 
			    FROM Enrollment  
			    WHERE 
				    MedCov = 'Y'  AND  
				    Age_Group in ('75+)') ) as OuterEnrollmentTable 
	                    Group by AgeGroup, gender, Year   
                    ) 
                    as EnrollmentData 

                ON 
	                EnrollmentData.AgeGroup = SummaryData.Agegroup AND 
	                EnrollmentData.sex = Summarydata.Sex AND 
	                EnrollmentData.Year = Summarydata.Period 
";

        string REQUEST_ID = "1";

        //[TestMethod]
        public void Request()
        {
            try
            {
                string requestToken;
                IModelProcessor processor = PostRequest(out requestToken);
                Assert.AreEqual(REQUEST_ID, requestToken);
                processor.Close("1");
            }
            catch(Exception e)
            {
                Assert.Fail(e.Message, e);
            }
        }

        //[TestMethod]
        public void RequestDocument()
        {
            try
            {
                string requestToken;
                WSModelProcessor processor = PostRequest(out requestToken);
                //FileStream docStream = new FileStream(FILEPATH, FileMode.Open);
                MemoryStream docStream = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(SQL_TEXT));
                processor.RequestDocument("1", "0", docStream);
                docStream.Close();
                processor.Close("1");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message, e);
            }
        }

        //[TestMethod]
        public void Start()
        {
            try
            {
                string requestToken;
                WSModelProcessor processor = PostRequest(out requestToken);
                MemoryStream docStream = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(SQL_TEXT));
                processor.RequestDocument("1", "0", docStream);
                docStream.Close();
                processor.Start("1", false);
                processor.Close("1");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message, e);
            }
        }

        //[TestMethod]
        public void Stop()
        {
            try
            {
                string requestToken;
                WSModelProcessor processor = PostRequest(out requestToken);
                processor.Stop("1", StopReason.Cancel);
                processor.Close("1");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message, e);
            }
        }

        //[TestMethod]
        public void Status()
        {
            try
            {
                string requestToken;
                WSModelProcessor processor = PostRequest(out requestToken);
                MemoryStream docStream = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(SQL_TEXT));
                processor.RequestDocument("1", "0", docStream);
                docStream.Close();
                processor.Start("1", false);
                RequestStatus status = processor.Status("1");
                Assert.AreEqual(RequestStatus.StatusCode.Complete, status.Code);
                //Assert.AreEqual("Done. Wancheng. Finito.", status.Message);
                processor.Close("1");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message, e);
            }
        }

        //[TestMethod]
        public void Response()
        {
            try
            {
                string requestToken;
                WSModelProcessor processor = PostRequest(out requestToken);
                MemoryStream docStream = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(SQL_TEXT));
                processor.RequestDocument("1", "0", docStream);
                docStream.Close();
                processor.Start("1", false);
                while(processor.Status("1").Code != RequestStatus.StatusCode.Complete);
                Document[] documents = processor.Response("1");
                processor.Close("1");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message, e);
            }
        }

        //[TestMethod]
        public void ResponseDocument()
        {
            try
            {
                string requestToken;
                WSModelProcessor processor = PostRequest(out requestToken);
                MemoryStream docStream = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(SQL_TEXT));
                processor.RequestDocument("1", "0", docStream);
                docStream.Close();
                processor.Start("1", false);
                while(processor.Status("1").Code != RequestStatus.StatusCode.Complete);
                Document[] documents = processor.Response("1");
                //FileStream outStream = new FileStream(@"C:\Users\daniel\tmp\New-" + documents[0].Filename, FileMode.Create);
                Stream contentStream;
                processor.ResponseDocument("1", "0", out contentStream, documents[0].Size);
                using (StreamReader reader = new StreamReader(contentStream))
                {
                    string datasetXml = reader.ReadToEnd();
                }
                //int bytesRead = 0;
                //byte[] buffer = new byte[5000];
                //while ((bytesRead = contentStream.Read(buffer, 0, 5000)) > 0)
                //{
                //    //outStream.Write(buffer, 0, 5000);
                //}
                contentStream.Close();
                //outStream.Close();
                processor.Close("1");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message, e);
            }
        }

        //[TestMethod]
        public void Close()
        {
            try
            {
                string requestToken;
                WSModelProcessor processor = PostRequest(out requestToken);
                processor.Close("1");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message, e);
            }
        }

        private WSModelProcessor PostRequest(out string requestToken)
        {
            WSModelProcessor processor = new WSModelProcessor();
            IDictionary<string, object> settings = new Dictionary<string, object>();
            IDictionary<string, string> requestProperties = new Dictionary<string, string>();
            processor.Settings = settings;
            processor.SetRequestProperties(REQUEST_ID, requestProperties);
            Document[] desiredDocuments;
            settings.Add("ServiceURL", SERVICE_URL);
            Document[] requestDocuments = new Document[1];
            requestDocuments[0] = new Document("0", "text/plain", "SQL Text");
            requestDocuments[0].IsViewable = true;
            //requestDocuments[0].Size = (int) new FileInfo(@"C:\Users\daniel\Documents\drn\UploadSequenceDiagram.jpg").Length;
            requestDocuments[0].Size = SQL_TEXT.Length;
            processor.Request(REQUEST_ID, null, null, requestDocuments, out requestProperties, out desiredDocuments);
            requestToken = requestProperties["RequestToken"];
            return processor;
        }

        
    }
}
