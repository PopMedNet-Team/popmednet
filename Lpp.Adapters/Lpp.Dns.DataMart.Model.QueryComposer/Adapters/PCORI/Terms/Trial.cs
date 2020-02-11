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
                    Name = "TRIALID",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "PARTICIPANTID",
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

            if(sourceTypeParameter.Type == typeof(PCORIQueryBuilder.Model.ClinicalTrial))
            {
                return new[] {
                    Expression.Bind(
                            selectType.GetProperty("TRIALID"),
                            Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("TrialID"))
                        ),
                    Expression.Bind(
                        selectType.GetProperty("PARTICIPANTID"),
                        Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("ParticipantID"))
                        ),
                    Expression.Bind(
                        selectType.GetProperty("Trial_PatientID"),
                        Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("PatientID"))
                        )
                };
            }

            return new[] {
                Expression.Bind(
                        selectType.GetProperty("TRIALID"),
                        Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("TRIALID"))
                    ),
                Expression.Bind(
                    selectType.GetProperty("PARTICIPANTID"),
                    Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("PARTICIPANTID"))
                    ),
                Expression.Bind(
                    selectType.GetProperty("Trial_PatientID"),
                    Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("Trial_PatientID"))
                    )
            };
        }

        public override IPropertyDefinition[] GroupKeyPropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                {
                    Name = "TRIALID",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                {
                    Name = "PARTICIPANTID",
                    Type = "System.String"
                }
            };
        }

        public override IEnumerable<MemberBinding> GroupKeySelectBindings(Type selectType, ParameterExpression sourceTypeParameter)
        {
            return new[] {
                Expression.Bind(selectType.GetProperty("TRIALID"), Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("TRIALID"))),
                Expression.Bind(selectType.GetProperty("PARTICIPANTID"), Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("PARTICIPANTID")))
            };
        }

        public override IPropertyDefinition[] FinalSelectPropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "TRIALID",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "PARTICIPANTID",
                    Type = "System.String"
                }
            };
        }
        public override IEnumerable<MemberBinding> FinalSelectBindings(Type selectType, ParameterExpression sourceTypeParameter)
        {
            return new[] {
                Expression.Bind(selectType.GetProperty("TRIALID"), Expressions.ChildPropertyExpression(sourceTypeParameter, "Key", "TRIALID")),
                Expression.Bind(selectType.GetProperty("PARTICIPANTID"), Expressions.ChildPropertyExpression(sourceTypeParameter, "Key", "PARTICIPANTID"))
            };
        }

    }
}
