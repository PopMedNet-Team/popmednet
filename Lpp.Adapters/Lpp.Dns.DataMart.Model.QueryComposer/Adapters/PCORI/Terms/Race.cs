using Lpp.Objects.Dynamic;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.PCORI.Terms
{
    public class Race : TermImplementation
    {

        public Race(PCORIQueryBuilder.DataContext db)
            : base(ModelTermsFactory.RaceID, db)
        {
        }

        public override Objects.Dynamic.IPropertyDefinition[] InnerSelectPropertyDefinitions()
        {
            //return the property value direct, if aggregation applied it will happen in the final select bindings after the grouping has been applied
            return new[] { 
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "Race",
                    Type = "System.String"
                }
            }; 
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> InnerSelectBindings(Type selectType, ParameterExpression sourceTypeParameter)
        {
            return new[] { 
                Expression.Bind( 
                        selectType.GetProperty("Race"), 
                        Expression.Property( sourceTypeParameter, sourceTypeParameter.Type.GetProperty("Race") ) 
                   )
            };
        }

        public override Objects.Dynamic.IPropertyDefinition[] GroupKeyPropertyDefinitions()
        {
            return new[] { 
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "Race",
                    Type = "System.String"
                }
            }; 
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> GroupKeySelectBindings(Type selectType, ParameterExpression sourceTypeParameter)
        {
            return new[]{
                Expression.Bind(selectType.GetProperty("Race"), Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("Race")))
            };            
        }

        public override Objects.Dynamic.IPropertyDefinition[] FinalSelectPropertyDefinitions()
        {
            return new[] { 
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "Race",
                    Type = "System.String"
                }
            }; 
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> FinalSelectBindings(Type selectType, System.Linq.Expressions.ParameterExpression sourceTypeParameter)
        {
            //There is no good way to determine if the type of sourceTypeParameter is an IGrouping, so assume that it will always be grouped
            return new[] { Expression.Bind(selectType.GetProperty("Race"), Expressions.ChildPropertyExpression(sourceTypeParameter, "Key", "Race")) };
        }
    }
}
