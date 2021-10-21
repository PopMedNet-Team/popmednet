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
                    Name = "PRO_ITEM_NAME",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "PRO_RESPONSE_TEXT",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "PRO_RESPONSE_NUM",
                    Type = typeof(double?).FullName.ToString()
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "PRO_MEASURE_SEQ",
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

            if(sourceTypeParameter.Type == typeof(PCORIQueryBuilder.Model.ReportedOutcome))
            {
                return new[] {
                Expression.Bind(
                        selectType.GetProperty("PRO_CM_ID"),
                        Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("ID"))
                    ),
                Expression.Bind(
                    selectType.GetProperty("PRO_PatientID"),
                    Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("PatientID"))
                    ),
                Expression.Bind(
                    selectType.GetProperty("PRO_ITEM_NAME"),
                    Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("ItemName"))
                    )
                    ,
                Expression.Bind(
                    selectType.GetProperty("PRO_RESPONSE_TEXT"),
                    Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("ResponseText"))
                    ),
                Expression.Bind(
                    selectType.GetProperty("PRO_RESPONSE_NUM"),
                    Expression.Convert(Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("ResponseNumber")), typeof(double?))
                    ),
                Expression.Bind(
                    selectType.GetProperty("PRO_MEASURE_SEQ"),
                    Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("MeasureSequence"))
                    )
            };
            }

            return new[] {
                Expression.Bind(
                        selectType.GetProperty("PRO_CM_ID"),
                        Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("PRO_CM_ID"))
                    ),
                Expression.Bind(
                    selectType.GetProperty("PRO_PatientID"),
                    Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("PRO_PatientID"))
                    ),
                Expression.Bind(
                    selectType.GetProperty("PRO_ITEM_NAME"),
                    Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("PRO_ITEM_NAME"))
                    )
                    ,
                Expression.Bind(
                    selectType.GetProperty("PRO_RESPONSE_TEXT"),
                    Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("PRO_RESPONSE_TEXT"))
                    ),
                Expression.Bind(
                    selectType.GetProperty("PRO_RESPONSE_NUM"),
                    Expression.Convert(Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("PRO_RESPONSE_NUM")), typeof(double?))
                    ),
                Expression.Bind(
                    selectType.GetProperty("PRO_MEASURE_SEQ"),
                    Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("PRO_MEASURE_SEQ"))
                    )
            };
        }

        public override IPropertyDefinition[] GroupKeyPropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                {
                    Name = "PRO_ITEM_NAME",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "PRO_RESPONSE_TEXT",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "PRO_RESPONSE_NUM",
                    Type = typeof(double?).FullName.ToString()
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "PRO_MEASURE_SEQ",
                    Type = "System.String"
                }
            };
        }

        public override IEnumerable<MemberBinding> GroupKeySelectBindings(Type selectType, ParameterExpression sourceTypeParameter)
        {
            return new[] {
                Expression.Bind(selectType.GetProperty("PRO_ITEM_NAME"), Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("PRO_ITEM_NAME"))),
                Expression.Bind(
                    selectType.GetProperty("PRO_RESPONSE_TEXT"),
                    Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("PRO_RESPONSE_TEXT"))
                    ),
                Expression.Bind(
                    selectType.GetProperty("PRO_RESPONSE_NUM"),
                    Expression.Convert(Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("PRO_RESPONSE_NUM")), typeof(double?))
                    ),
                Expression.Bind(
                    selectType.GetProperty("PRO_MEASURE_SEQ"),
                    Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("PRO_MEASURE_SEQ"))
                    )
            };
        }

        public override IPropertyDefinition[] FinalSelectPropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "PRO_ITEM_NAME",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "PRO_RESPONSE_TEXT",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "PRO_RESPONSE_NUM",
                    Type = typeof(double?).FullName.ToString()
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "PRO_MEASURE_SEQ",
                    Type = "System.String"
                }
            };
        }
        public override IEnumerable<MemberBinding> FinalSelectBindings(Type selectType, ParameterExpression sourceTypeParameter)
        {
            return new[] {
                Expression.Bind(selectType.GetProperty("PRO_ITEM_NAME"), Expressions.ChildPropertyExpression(sourceTypeParameter, "Key", "PRO_ITEM_NAME")),
                Expression.Bind(
                    selectType.GetProperty("PRO_RESPONSE_TEXT"),
                    Expressions.ChildPropertyExpression(sourceTypeParameter, "Key", "PRO_RESPONSE_TEXT")
                    ),
                Expression.Bind(
                    selectType.GetProperty("PRO_RESPONSE_NUM"),
                    Expression.Convert(Expressions.ChildPropertyExpression(sourceTypeParameter, "Key", "PRO_RESPONSE_NUM"), typeof(double?))
                    ),
                Expression.Bind(
                    selectType.GetProperty("PRO_MEASURE_SEQ"),
                    Expressions.ChildPropertyExpression(sourceTypeParameter, "Key", "PRO_MEASURE_SEQ")
                    )
            };
        }

    }
}
