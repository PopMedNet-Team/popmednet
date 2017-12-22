using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Model;
using System.Diagnostics.Contracts;
using Lpp.Mvc.Controls;
using Lpp.Mvc;

namespace Lpp.Dns.Portal.Models
{
    public class ChildrenListModel<T, TParent> : CrudListModel<T, ChildrenListGetModel>
        where T : class
        where TParent : class, Lpp.Objects.IEntityWithID, Lpp.Objects.IEntityWithName
    {
        public TParent Parent { get; set; }
        public string ReturnTo { get; set; }
        public Func<UrlHelper, string> ReloadUrl { get; set; }

        public ChildCreateGetModel ForCreate()
        {
            //TODO: A parent object ID is always going to be a Guid type since all ID are Guid, however should update to handle as nullable.
            return new ChildCreateGetModel
            {
                ParentID = Parent == null ? default( Guid ) : Parent.ID,
                ReturnTo = ReturnTo
            };
        }

        public ChildrenListModel<U, TParent> Select<U>( Func<T,U> selector )
            where U : class
        {
            return new ChildrenListModel<U, TParent>
            {
                Parent = this.Parent,
                ReturnTo = this.ReturnTo,
                AllowCreate = this.AllowCreate,
                Items = this.Items.Select( selector )
            };
        }
    }
}