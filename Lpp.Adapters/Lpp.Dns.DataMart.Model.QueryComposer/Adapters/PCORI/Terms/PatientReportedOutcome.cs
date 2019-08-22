using Lpp.Objects.Dynamic;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.PCORI.Terms
{
    public class PatientReportedOutcome : TermImplementation
    {
        public PatientReportedOutcome(PCORIQueryBuilder.DataContext db) : base(ModelTermsFactory.PatientReportedOutcomeID, db) { }

        public override IPropertyDefinition[] InnerSelectPropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "PRO_CM_ID",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "PRO_PatientID",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "ItemName",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "ResponseText",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "ResponseNumber",
                    Type = typeof(double?).FullName.ToString()
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "MeasureSequence",
                    Type = "System.String"
                }
            };
        }

        public override IEnumerable<MemberBinding> InnerSelectBindings(Type selectType, ParameterExpression sourceTypeParameter)
        {

            //if the sourceType is Patient do not return bindings
            if (sourceTypeParameter.Type == typeof(Lpp.Dns.DataMart.Model.PCORIQueryBuilder.Model.Patient))
            {
                return Enumerable.Empty<MemberBinding>();
            }

            string patientIDProperty = sourceTypeParameter.Type == typeof(PCORIQueryBuilder.Model.ReportedOutcome) ? "PatientID" : "PRO_PatientID";
            string IDProperty = sourceTypeParameter.Type == typeof(PCORIQueryBuilder.Model.ReportedOutcome) ? "ID" : "PRO_CM_ID";

            return new[] {
                Expression.Bind(
                        selectType.GetProperty("PRO_CM_ID"),
                        Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty(IDProperty))
                    ),
                Expression.Bind(
                    selectType.GetProperty("PRO_PatientID"),
                    Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty(patientIDProperty))
                    ),
                Expression.Bind(
                    selectType.GetProperty("ItemName"),
                    Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("ItemName"))
                    )
                    ,
                Expression.Bind(
                    selectType.GetProperty("ResponseText"),
                    Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("ResponseText"))
                    ),
                Expression.Bind(
                    selectType.GetProperty("ResponseNumber"),
                    Expression.Convert(Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("ResponseNumber")), typeof(double?))
                    ),
                Expression.Bind(
                    selectType.GetProperty("MeasureSequence"),
                    Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("MeasureSequence"))
                    )
            };
        }

        public override IPropertyDefinition[] GroupKeyPropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                {
                    Name = "ItemName",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "ResponseText",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "ResponseNumber",
                    Type = typeof(double?).FullName.ToString()
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "MeasureSequence",
                    Type = "System.String"
                }
            };
        }

        public override IEnumerable<MemberBinding> GroupKeySelectBindings(Type selectType, ParameterExpression sourceTypeParameter)
        {
            return new[] {
                Expression.Bind(selectType.GetProperty("ItemName"), Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("ItemName"))),
                Expression.Bind(
                    selectType.GetProperty("ResponseText"),
                    Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("ResponseText"))
                    ),
                Expression.Bind(
                    selectType.GetProperty("ResponseNumber"),
                    Expression.Convert(Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("ResponseNumber")), typeof(double?))
                    ),
                Expression.Bind(
                    selectType.GetProperty("MeasureSequence"),
                    Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("MeasureSequence"))
                    )
            };
        }

        public override IPropertyDefinition[] FinalSelectPropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "ItemName",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "ResponseText",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "ResponseNumber",
                    Type = typeof(double?).FullName.ToString()
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "MeasureSequence",
                    Type = "System.String"
                }
            };
        }
        public override IEnumerable<MemberBinding> FinalSelectBindings(Type selectType, ParameterExpression sourceTypeParameter)
        {
            return new[] {
                Expression.Bind(selectType.GetProperty("ItemName"), Expressions.ChildPropertyExpression(sourceTypeParameter, "Key", "ItemName")),
                Expression.Bind(
                    selectType.GetProperty("ResponseText"),
                    Expressions.ChildPropertyExpression(sourceTypeParameter, "Key", "ResponseText")
                    ),
                Expression.Bind(
                    selectType.GetProperty("ResponseNumber"),
                    Expression.Convert(Expressions.ChildPropertyExpression(sourceTypeParameter, "Key", "ResponseNumber"), typeof(double?))
                    ),
                Expression.Bind(
                    selectType.GetProperty("MeasureSequence"),
                    Expressions.ChildPropertyExpression(sourceTypeParameter, "Key", "MeasureSequence")
                    )
            };
        }

    }
}
