using Lpp.Objects.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.PCORI.Terms
{
    public class PatientCount : TermImplementation
    {
        public PatientCount(PCORIQueryBuilder.DataContext db)
            : base(Guid.Empty, db)
        {

        }

        public override bool HasFields
        {
            get
            {
                return true;
            }
        }

        //this represents the count based on the Patients.ID.
        public override bool HasCountAggregate
        {
            get
            {
                return true;
            }
        }

        public override bool HasStratifications
        {
            get
            {
                return false;
            }
        }

        public override Objects.Dynamic.IPropertyDefinition[] InnerSelectPropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO {
                    Name = "ID",
                    As = "PatientID",
                    Type = "System.String",
                    Aggregate = "Count"
                }
            };
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> InnerSelectBindings(Type selectType, ParameterExpression sourceTypeParameter)
        {
            //Assumes that the sourceTypeParameter is going to be a pcori.Patient.
            //if (sourceTypeParameter.Type != typeof(PCORIQueryBuilder.Model.Patient))
            //    throw new ArgumentException("Expected source parameter expression of type PCORIQueryBuild.Model.Patient, actual: " + sourceTypeParameter.Type.Name);

            //binding on dynamic class for joining to clinical trials table
            if(selectType.Name == "ClinicalTrial_Join" || selectType.Name == "PRO_CM_Join")
            {
                return new[] {
                Expression.Bind(
                        selectType.GetProperty("PatientID"),
                        Expression.Property( sourceTypeParameter, sourceTypeParameter.Type.GetProperty("PatientID") )
                   )
                };

            }

            //binding on Patient class
            return new[] { 
                Expression.Bind( 
                        selectType.GetProperty("PatientID"), 
                        Expression.Property( sourceTypeParameter, sourceTypeParameter.Type.GetProperty("ID") ) 
                   )
            };
        }

        public override Objects.Dynamic.IPropertyDefinition[] FinalSelectPropertyDefinitions()
        {
            return new[] {
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO {
                    Name = "PatientID",
                    As = "Patients",
                    Type = "System.Int32",
                    Aggregate = "Sum"
                }
            };
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> FinalSelectBindings(Type selectType, ParameterExpression sourceTypeParameter)
        {
            return new[] { Expression.Bind(selectType.GetProperty("Patients"), Expressions.Count(sourceTypeParameter)) };
        }

        public override Objects.Dynamic.IPropertyDefinition[] GroupKeyPropertyDefinitions()
        {
            return new Objects.Dynamic.IPropertyDefinition[0];
        }

        public override IEnumerable<MemberBinding> GroupKeySelectBindings(Type selectType, ParameterExpression sourceTypeParameter)
        {
            return Enumerable.Empty<MemberBinding>();
        }
    }
}
