using System;
using System.Linq;
using System.Web.Mvc;
using Lpp.Dns.Data;
using Lpp.Dns.Portal.Models;
using Lpp.Mvc;
using Lpp.Mvc.Controls;
using Lpp.Utilities.Legacy;

namespace Lpp.Dns.Portal.Controllers
{
    public abstract class OrgChildCrudController<TEntity, TEditPostModel, TEditModel, TController> :
        ChildCrudController<TEntity, TEditPostModel, TEditModel, Organization, TController>,
        IOrgChildCrudController
        where TEntity : class, Lpp.Objects.IEntityWithName, Lpp.Objects.IEntityWithID, new()
        where TEditPostModel : ICrudPostModel<TEntity>
        where TController : OrgChildCrudController<TEntity, TEditPostModel, TEditModel, TController>
    {
        protected override Organization LoadParent( ChildCreateGetModel model )
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return base.Prnt( model ) ?? Auth.CurrentUser.Organization;
        }

        protected override IQueryable<Organization> GetByID( IQueryable<Organization> ps, Guid id )
        {
            return ps.Where( o => o.ID == id );
        }

        public ComputationResult<Controls.RenderOrgChildrenList> ForSelection()
        {
            return new ComputationResult<Controls.RenderOrgChildrenList>( 
                ( orgId, search ) => html => html.Partial<Views.Crud.ForSelectionList>().WithModel( 
                    ForSelectionModel( new ChildrenListGetModel { ParentID = orgId, SearchTerm = search } ) )
            );
        }

        public ActionResult ForSelectionBody( ChildrenListGetModel get )
        {
            return View<Views.Crud.ForSelectionList>().WithModel( ForSelectionModel( get ) );
        }

        EntitiesForSelectionModel ForSelectionModel( ChildrenListGetModel get )
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var ee = Entities.All.Where( FilterByParent( get.ParentId ) );
            //if ( !get.SearchTerm.NullOrEmpty() ) 
            //{
            //    var s = Search( get.SearchTerm );
            //    if ( s != null ) ee = ee.Where( s );
            //}

            //return ee
            //    .ListModel( get, Sort )
            //    .EntitiesForSelection( e => e.Id, e => e.Name, (url,m) => url.Action( (TController c) => c.ForSelectionBody( m ) ) );
        }
    }
    
}