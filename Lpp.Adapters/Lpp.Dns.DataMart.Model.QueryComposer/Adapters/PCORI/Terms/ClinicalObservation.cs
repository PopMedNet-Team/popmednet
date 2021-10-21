using Lpp.Objects.Dynamic;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.PCORI.Terms
{
    public class ClinicalObservation : TermImplementation
    {
        public ClinicalObservation(PCORIQueryBuilder.DataContext db) : base(ModelTermsFactory.ClinicalObservationsID, db) { }

        public override IPropertyDefinition[] InnerSelectPropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "OBSCLINID",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "OBSCLIN_PatientID",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "OBSCLIN_TYPE",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "OBSCLIN_CODE",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "OBSCLIN_RESULT_NUM",
                    Type = typeof(double?).FullName
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

            if (sourceTypeParameter.Type == typeof(PCORIQueryBuilder.Model.ClinicalObservation))
            {
                return new[] {
                Expression.Bind(
                        selectType.GetProperty("OBSCLINID"),
                        Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("ID"))
                    ),
                Expression.Bind(
                    selectType.GetProperty("OBSCLIN_PatientID"),
                    Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("PatientID"))
                    ),
                Expression.Bind(
                    selectType.GetProperty("OBSCLIN_TYPE"),
                    Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("Type"))
                    ),
                Expression.Bind(
                    selectType.GetProperty("OBSCLIN_CODE"),
                    Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("Code"))
                    ),
                Expression.Bind(
                    selectType.GetProperty("OBSCLIN_RESULT_NUM"),
                    Expression.Convert(Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("QuantitativeResult")), typeof(double?))
                    )
            };
            }

            return new[] {
                Expression.Bind(
                        selectType.GetProperty("OBSCLINID"),
                        Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("OBSCLINID"))
                    ),
                Expression.Bind(
                    selectType.GetProperty("OBSCLIN_PatientID"),
                    Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("OBSCLIN_PatientID"))
                    ),
                Expression.Bind(
                    selectType.GetProperty("OBSCLIN_TYPE"),
                    Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("OBSCLIN_TYPE"))
                    ),
                Expression.Bind(
                    selectType.GetProperty("OBSCLIN_CODE"),
                    Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("OBSCLIN_CODE"))
                    ),
                Expression.Bind(
                    selectType.GetProperty("OBSCLIN_RESULT_NUM"),
                    Expression.Convert(Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("OBSCLIN_RESULT_NUM")), typeof(double?))
                    )
            };
        }

        public override IPropertyDefinition[] GroupKeyPropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                {
                    Name = "OBSCLIN_Type",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                {
                    Name = "OBSCLIN_CODE",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "OBSCLIN_RESULT_NUM",
                    Type = typeof(double?).FullName
                }
            };
        }

        public override IEnumerable<MemberBinding> GroupKeySelectBindings(Type selectType, ParameterExpression sourceTypeParameter)
        {
            return new[] {
                Expression.Bind(selectType.GetProperty("OBSCLIN_TYPE"), Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("OBSCLIN_TYPE"))),
                Expression.Bind(selectType.GetProperty("OBSCLIN_CODE"), Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("OBSCLIN_CODE"))),
                Expression.Bind(
                    selectType.GetProperty("OBSCLIN_RESULT_NUM"),
                    Expression.Convert(Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("OBSCLIN_RESULT_NUM")), typeof(double?))
                    )
            };
        }

        public override IPropertyDefinition[] FinalSelectPropertyDefinitions()
        {
            return new[] {                
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "OBSCLIN_TYPE",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "OBSCLIN_CODE",
                    Type = "System.String"
                },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "OBSCLIN_RESULT_NUM",
                    Type = typeof(double?).FullName
                }
            };
        }

        public override IEnumerable<MemberBinding> FinalSelectBindings(Type selectType, ParameterExpression sourceTypeParameter)
        {
            return new[] {
                Expression.Bind(selectType.GetProperty("OBSCLIN_TYPE"), Expressions.ChildPropertyExpression(sourceTypeParameter, "Key", "OBSCLIN_TYPE")),
                Expression.Bind(selectType.GetProperty("OBSCLIN_CODE"), Expressions.ChildPropertyExpression(sourceTypeParameter, "Key", "OBSCLIN_CODE")),
                Expression.Bind(
                    selectType.GetProperty("OBSCLIN_RESULT_NUM"),
                    Expression.Convert(Expressions.ChildPropertyExpression(sourceTypeParameter, "Key", "OBSCLIN_RESULT_NUM"), typeof(double?))
                    )
            };
        }
        
    }
}
