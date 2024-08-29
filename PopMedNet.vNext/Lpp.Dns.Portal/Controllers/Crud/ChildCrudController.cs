using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
//using Lpp.Data;
using Lpp.Dns.Model;
using Lpp.Dns.Portal.Models;
using Lpp.Mvc;
using Lpp.Mvc.Controls;
using Lpp.Security;
using Lpp.Utilities.Legacy;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal.Controllers
{
    public abstract class ChildCrudController<TEntity, TEditPostModel, TEditModel, TParent, TController> :
        CrudController<TEntity, ChildCreateGetModel, TEditPostModel, ChildrenListGetModel, TEditModel, ChildrenListModel<TEntity, TParent>, TController>,
        IHaveCreateMethod<ChildCreateGetModel>
        where TEntity : class, Lpp.Objects.IEntityWithID, new()
        where TParent : class, Lpp.Objects.IEntityWithDeleted, Lpp.Objects.IEntityWithID, Lpp.Objects.IEntityWithName
        where TEditPostModel : ICrudPostModel<TEntity>
        where TController : ChildCrudController<TEntity, TEditPostModel, TEditModel, TParent, TController>
    {
        //[Import] internal IRepository<DnsDomain,TParent> Parents { get; set; }
        
        [Import]
        internal DnsAclService DnsAclService { get; set; }

        protected abstract SortHelper<TParent> ParentSort { get; }

        protected abstract IQueryable<TParent> GetByID( IQueryable<TParent> ps, Guid id );

        protected abstract Guid CreatePrivilegeID { get; }

        protected abstract Expression<Func<TEntity, bool>> FilterByParent( Guid parentID );

        protected abstract Expression<Func<TEntity, bool>> Search( string searchTerm );

        protected virtual Expression<Func<TParent, bool>> IsParentDeleted() 
        { 
            return p => p.Deleted; 
        }

        public ActionResult ParentsForSelection( ListGetModel model )
        {
            return View<Views.Crud.ForSelectionList>().WithModel( ParentsForSelectionModel( model ) );
        }

        public EntitiesForSelectionModel ParentsForSelectionModel( ListGetModel model )
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var deleted = IsParentDeleted();
            //return Security
            //    .AllGrantedObjects( Parents.All.Where( o => !deleted.Invoke(o) ).Expand(), Auth.CurrentUser, CreatePrivilege )
            //    .Where( o => !o.IsDeleted )
            //    .ListModel( model, ParentSort )
            //    .EntitiesForSelection( p => p.Id, p => p.Name, (url,m) => url.Action( (TController c) => c.ParentsForSelection( m ) ) );
        }

        protected override bool CanCreateNew( ChildCreateGetModel model ) 
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return Security.HasPrivilege( Sec.Target( Prnt( model ) ), Auth.CurrentUser, CreatePrivilege ); 
        }
        
        protected virtual TParent LoadParent( ChildCreateGetModel model )
        {
            return ( Equals( model.ParentID, Guid.Empty ) ? null : DataContext.Set<TParent>().Find( model.ParentID ) );
        }
        
        protected override ChildCreateGetModel CreateFromList( ChildrenListGetModel listModel )
        {
            return new ChildCreateGetModel { ParentID = listModel.ParentID };
        }

        protected abstract TParent NewParent(TParent oldParent, Guid? newParentID);

        public override ChildrenListModel<TEntity, TParent> ListModel( ChildrenListGetModel req )
        {
            var m = base.ListModel(req);
            m.Parent = DataContext.Set<TParent>().SingleOrDefault(p => p.ID == req.ParentID);
            return m;
        }
    }
}