using Lpp.Objects.Dynamic;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.PCORI.Terms
{
    public class Sex : TermImplementation
    {
        public Sex(PCORIQueryBuilder.DataContext db)
            : base(ModelTermsFactory.SexID, db)
        {
        }

        public override Objects.Dynamic.IPropertyDefinition[] InnerSelectPropertyDefinitions()
        {
            return new[] { 
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "Sex",
                    Type = "System.String"
                }
            };
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> InnerSelectBindings(Type selectType, ParameterExpression sourceTypeParameter)
        {
            return new[] { 
                Expression.Bind( 
                        selectType.GetProperty("Sex"), 
                        Expression.Property( sourceTypeParameter, sourceTypeParameter.Type.GetProperty("Sex") ) 
                   )
            };
        }        

        public override Objects.Dynamic.IPropertyDefinition[] GroupKeyPropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "Sex",
                    Type = "System.String"
                }
            };
        }

        public override IEnumerable<MemberBinding> GroupKeySelectBindings(Type selectType, ParameterExpression sourceTypeParameter)
        {
            return new []{
                Expression.Bind(selectType.GetProperty("Sex"), Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("Sex")))
            };
        }

        public override Objects.Dynamic.IPropertyDefinition[] FinalSelectPropertyDefinitions()
        {
            return InnerSelectPropertyDefinitions();
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> FinalSelectBindings(Type selectType, ParameterExpression sourceTypeParameter)
        {
            //There is no good way to determine if the type of sourceTypeParameter is an IGrouping, so assume that it will always be grouped
            return new[] { Expression.Bind(selectType.GetProperty("Sex"), Expressions.ChildPropertyExpression(sourceTypeParameter, "Key", "Sex")) };
        }
    }
}
