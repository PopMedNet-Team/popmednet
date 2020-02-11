using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects.Dynamic;
using Lpp.QueryComposer;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.PCORI.Terms
{
    public class PatientReportedOutcomeEncounter : TermImplementation
    {
        public PatientReportedOutcomeEncounter(PCORIQueryBuilder.DataContext db) : base(ModelTermsFactory.PatientReportedOutcomeEncounterID, db) { }
        public override IEnumerable<MemberBinding> FinalSelectBindings(Type selectType, ParameterExpression sourceTypeParameter)
        {
            return Enumerable.Empty<MemberBinding>();
        }

        public override IPropertyDefinition[] FinalSelectPropertyDefinitions()
        {
            return new IPropertyDefinition[0];
        }

        public override IPropertyDefinition[] GroupKeyPropertyDefinitions()
        {
            return new IPropertyDefinition[0];
        }

        public override IEnumerable<MemberBinding> GroupKeySelectBindings(Type selectType, ParameterExpression sourceTypeParameter)
        {
            return Enumerable.Empty<MemberBinding>();
        }

        public override IEnumerable<MemberBinding> InnerSelectBindings(Type selectType, ParameterExpression sourceTypeParameter)
        {
            return Enumerable.Empty<MemberBinding>();
        }

        public override IPropertyDefinition[] InnerSelectPropertyDefinitions()
        {
            return new IPropertyDefinition[0];
        }
    }
}
