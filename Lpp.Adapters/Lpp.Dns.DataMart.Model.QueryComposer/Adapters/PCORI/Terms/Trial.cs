using Lpp.Objects.Dynamic;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.PCORI.Terms
{
    public class Trial : TermImplementation
    {
        public Trial(PCORIQueryBuilder.DataContext db)
            : base(ModelTermsFactory.TrialID, db)
        {
        }
        public override IPropertyDefinition[] InnerSelectPropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "TrialID",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "ParticipantID",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                {
                    Name = "Trial_PatientID",
                    Type = "System.String"
                }
            };
        }

        public override IEnumerable<MemberBinding> InnerSelectBindings(Type selectType, ParameterExpression sourceTypeParameter)
        {
            //if the sourceType is Patient do not return bindings
            if(sourceTypeParameter.Type == typeof(Lpp.Dns.DataMart.Model.PCORIQueryBuilder.Model.Patient))
            {
                return Enumerable.Empty<MemberBinding>();
            }

            string patientIDProperty = sourceTypeParameter.Type == typeof(PCORIQueryBuilder.Model.ClinicalTrial) ? "PatientID" : "Trial_PatientID";
            

            return new[] {
                Expression.Bind(
                        selectType.GetProperty("TrialID"),
                        Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("TrialID"))
                    ),
                Expression.Bind(
                    selectType.GetProperty("ParticipantID"),
                    Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("ParticipantID"))
                    ),
                Expression.Bind(
                    selectType.GetProperty("Trial_PatientID"),
                    Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty(patientIDProperty))
                    )
            };
        }

        public override IPropertyDefinition[] GroupKeyPropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                {
                    Name = "TrialID",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                {
                    Name = "ParticipantID",
                    Type = "System.String"
                }
            };
        }

        public override IEnumerable<MemberBinding> GroupKeySelectBindings(Type selectType, ParameterExpression sourceTypeParameter)
        {
            return new[] {
                Expression.Bind(selectType.GetProperty("TrialID"), Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("TrialID"))),
                Expression.Bind(selectType.GetProperty("ParticipantID"), Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("ParticipantID")))
            };
        }

        public override IPropertyDefinition[] FinalSelectPropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "TrialID",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "ParticipantID",
                    Type = "System.String"
                }
            };
        }
        public override IEnumerable<MemberBinding> FinalSelectBindings(Type selectType, ParameterExpression sourceTypeParameter)
        {
            return new[] {
                Expression.Bind(selectType.GetProperty("TrialID"), Expressions.ChildPropertyExpression(sourceTypeParameter, "Key", "TrialID")),
                Expression.Bind(selectType.GetProperty("ParticipantID"), Expressions.ChildPropertyExpression(sourceTypeParameter, "Key", "ParticipantID"))
            };
        }

    }
}
