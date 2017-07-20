using Lpp.Dns.DTO.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.PCORI
{
    public abstract class TermImplementation
    {
        /// <summary>
        /// The ID of the term being implemented.
        /// </summary>
        public readonly Guid TermID;

        /// <summary>
        /// A collection of the fields defined by the request that are of this term type.
        /// </summary>
        readonly protected List<QueryComposerFieldDTO> Fields = new List<QueryComposerFieldDTO>();

        /// <summary>
        /// A collection of the critieria defined by the request.
        /// </summary>
        readonly protected List<QueryComposerCriteriaDTO> Criteria = new List<QueryComposerCriteriaDTO>();

        /// <summary>
        /// The current db context.
        /// </summary>
        readonly protected PCORIQueryBuilder.DataContext db;        

        /// <summary>
        /// Instantiates a new term implementation.
        /// </summary>
        /// <param name="termID"></param>
        public TermImplementation(Guid termID, PCORIQueryBuilder.DataContext db)
        {
            this.db = db;
            this.TermID = termID;
        }

        /// <summary>
        /// Gets if the implemenation has any fields.
        /// </summary>
        public virtual bool HasFields
        {
            get { return Fields.Count > 0; }
        }

        /// <summary>
        /// Gets if the implementation has a count aggregate.
        /// </summary>
        public virtual bool HasCountAggregate
        {
            get { return Fields.Any(t => t.Aggregate != null && t.Aggregate.Value == DTO.Enums.QueryComposerAggregates.Count); }
        }

        /// <summary>
        /// Gets if the implementation has any aggregations.
        /// </summary>
        public virtual bool HasAggregates
        {
            get { return Fields.Any(t => t.Aggregate != null); }
        }

        /// <summary>
        /// Gets the aggregations for the term.
        /// </summary>
        public virtual IEnumerable<DTO.Enums.QueryComposerAggregates> Aggregation
        {
            get
            {
                return Fields.Where(t => t.Aggregate != null).Select(t => t.Aggregate.Value);
            }
        }

        /// <summary>
        /// Gets if the implementation has any stratifications.
        /// </summary>
        public virtual bool HasStratifications
        {
            get { return Fields.Any(t => t.StratifyBy != null); }
        }

        /// <summary>
        /// Gets fields that have a stratification value specified.
        /// </summary>
        protected IEnumerable<object> Stratifications
        {
            get { return Fields.Where(f => f.StratifyBy != null).Select(f => f.StratifyBy); }
        }

        /// <summary>
        /// Adds the specified criteria to the terms Criteria collection.
        /// </summary>
        /// <param name="criteria"></param>
        public virtual void RegisterCriteria(QueryComposerCriteriaDTO[] criteria)
        {
            if(criteria != null)
                Criteria.AddRange(criteria);
        }

        /// <summary>
        /// Registers a field definition with the implementation. If the field type is not correct it will raise an exception.
        /// </summary>
        /// <param name="field">The field to register.</param>
        public virtual void RegisterQueryComposerField(QueryComposerFieldDTO field)
        {
            if (field.Type != TermID)
                throw new ArgumentException("The field does not have the correct type to register with this term implementation. Actual:" + field.Type.ToString("D") + "  Expected:" + this.TermID.ToString("D"));

            Fields.Add(field);
        }

        /// <summary>
        /// Removes all fields that are Count aggregates.
        /// </summary>
        /// <remarks>Based on business rule, only count will be against the Patient ID which will be in its own term implementation.</remarks>
        public virtual void RemoveCountAggregates()
        {
            Fields.RemoveAll(f => f.Aggregate != null && f.Aggregate.Value == DTO.Enums.QueryComposerAggregates.Count);
        }

        /// <summary>
        /// Gets the property definitions for the properties returned by this term for the inner select.
        /// </summary>
        /// <remarks>In the case where this term is getting stratified, it would return the property holding the stratification value.</remarks>
        /// <returns></returns>
        public abstract Lpp.Objects.Dynamic.IPropertyDefinition[] InnerSelectPropertyDefinitions();

        /// <summary>
        /// Gets the MemberBindings for the properties associated to this term in the inner select type.
        /// </summary>
        /// <param name="selectType">The Type for the class returned by the inner/initial select.</param>
        /// <param name="sourceTypeParameter">The ParameterExpression for the source type being bound to.</param>
        /// <remarks>In the case of stratification being applied to this term, this is where the expression that builds the stratifcation value should be implemented and bound to the final value</remarks>
        /// <returns></returns>
        public abstract IEnumerable<MemberBinding> InnerSelectBindings(Type selectType, ParameterExpression sourceTypeParameter);

        /// <summary>
        /// Gets the property definitions for the properties to be included in the grouping key.
        /// </summary>
        /// <returns></returns>
        public abstract Lpp.Objects.Dynamic.IPropertyDefinition[] GroupKeyPropertyDefinitions();

        /// <summary>
        /// Gets the MemberBindings for the properties associated to this term for the group key select type.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<MemberBinding> GroupKeySelectBindings(Type selectType, ParameterExpression sourceTypeParameter);

        /// <summary>
        /// Gets the property definitions for the properties return by this term in the final select.
        /// </summary>
        /// <returns></returns>
        public abstract Lpp.Objects.Dynamic.IPropertyDefinition[] FinalSelectPropertyDefinitions();

        /// <summary>
        /// Gets the MemberBindings for the properties associated to this term in the final select.
        /// </summary>
        /// <param name="selectType"></param>
        /// <param name="sourceTypeParameter"></param>
        /// <returns></returns>
        public abstract IEnumerable<MemberBinding> FinalSelectBindings(Type selectType, ParameterExpression sourceTypeParameter);

    }

    /// <summary>
    /// Inspects a row and modifies the values as applicable.
    /// </summary>
    public interface ITermResultTransformer
    {
        /// <summary>
        /// Inspects the specified row and modifies the values as applicable, the inspected(modified) row is returned.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        Dictionary<string,object> Visit(Dictionary<string, object> row);

        /// <summary>
        /// Updates any property definitions that have changed due to the applied transform.
        /// </summary>
        /// <param name="definitions">The collection of property definitions.</param>
        void TransformPropertyDefinitions(List<Objects.Dynamic.IPropertyDefinition> definitions);
    }
}
