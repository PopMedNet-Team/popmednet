using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Lpp.Audit;
using Lpp.Audit.UI;
using Lpp.Composition;
using Lpp.Dns.Data;
using Lpp.Dns.Portal.Controllers;
using Lpp.Dns.Portal.Models;
using Lpp.Mvc;

namespace Lpp.Dns.Portal.Controls
{
    [PartMetadata(ExportScope.Key, TransactionScope.Id)]
    abstract class OrgChildSelector<TEntity, TEntityId, TController> : IAuditPropertyValueSelector<TEntityId>
        where TEntity : class
        where TController : System.Web.Mvc.Controller, IOrgChildCrudController
    {
        //[Import]
        //public IRepository<DnsDomain, TEntity> Entities { get; set; }

        protected abstract string ToString(TEntity e);

        public IEnumerable<IAuditProperty<TEntityId>> AppliesTo { get; private set; }

        public OrgChildSelector(params IAuditProperty<TEntityId>[] appliesTo) { AppliesTo = appliesTo; }

        public ClientControlDisplay RenderSelector(TEntityId initialState)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var e = Entities.Find(initialState);
            //return new ClientControlDisplay
            //{
            //    ValueAsString = initialState.ToString(),
            //    Render = (html, onChanged) => html.Partial<Views.Organizations.OrgChildSelector>().WithModel(new OrgChildSelectorModel
            //                                    {
            //                                        ValueDisplay = e == null ? null : ToString(e),
            //                                        Children = url => url.RoutedComputation((TController c) => c.ForSelection()),
            //                                        OnChangeFunction = onChanged
            //                                    })
            //};
        }

        public TEntityId ParsePostedValue(string value)
        {
            if (typeof(TEntityId) == typeof(Guid)) return (TEntityId)(object)Guid.Parse(value);
            else return (TEntityId)Convert.ChangeType(value, typeof(TEntityId));
        }
    }
}