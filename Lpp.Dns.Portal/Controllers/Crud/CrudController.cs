using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using log4net;
using Lpp.Audit;
using Lpp.Dns.Data;
using Lpp.Mvc;
using Lpp.Mvc.Controls;
using Lpp.Security;
using Lpp.Utilities.Legacy;

namespace Lpp.Dns.Portal.Controllers
{
    public abstract class CrudController<TEntity, TCreateGetModel, TEntityEditPostModel, TListGetModel, TEditModel, TListModel, TController> 
        : BaseController, IHaveCreateMethod<TCreateGetModel>, IAmListController<TListGetModel>
        where TEntity : class, new()
        where TCreateGetModel : ICrudCreateModel
        where TEntityEditPostModel : ICrudPostModel<TEntity>
        where TListGetModel : struct, IListGetModel
        where TController : CrudController<TEntity, TCreateGetModel, TEntityEditPostModel, TListGetModel, TEditModel, TListModel, TController>
        where TListModel : ICrudListModel<TEntity, TListGetModel>, new()
    {
        [Import]
        protected internal IAuditService<Lpp.Dns.Model.DnsDomain> Audit { get; set; }

        [Import]
        protected internal IAuthenticationService Auth { get; set; }
        
        [Import]
        protected internal ILog Log { get; set; }

        [Import]
        public IClientSettingsService Settings { get; set; }

        [ImportMany]
        internal IEnumerable<IPermissionsTemplate<TEntity>> PermissionTemplates { get; set; }

        protected virtual TEntity Find( Guid id ) 
        {
            return DataContext.Set<TEntity>().Find(id) as TEntity;
        }

        protected virtual void RunValidationRules( TEntityEditPostModel model, TEntity e ) { }

        protected virtual IQueryable<TEntity> All( TListGetModel getModel ) 
        {
            return DataContext.Secure<TEntity>(Auth.ApiIdentity);
        }

        public virtual SortHelper<TEntity> Sort 
        { 
            get 
            { 
                return new SortHelper<TEntity>(); 
            } 
        }

        protected abstract void ApplyPostModel( TEntity e, TEntityEditPostModel model );

        protected virtual TEntity New( TCreateGetModel model ) 
        { 
            return new TEntity(); 
        }

        protected virtual TCreateGetModel CreateFromList( TListGetModel listModel ) 
        { 
            return default( TCreateGetModel ); 
        }

        protected abstract TEditModel EditModel( TEntity e, string returnTo );

        protected virtual ActionResult RedirectAfterPost( TEntity e, TEntityEditPostModel post )
        {
            if ( post.ReturnTo.NullOrEmpty() ) 
                return RedirectToAction<TController>( c => c.List( new TListGetModel() ) );

            return Redirect( post.ReturnTo );
        }

        protected virtual void OnCrudEvent( TEntity e, Events.CrudEventKind ev ) { }

        protected virtual Expression<Func<TEntity, bool>> IsDeletedProperty 
        { 
            get 
            { 
                return null; 
            } 
        }

        protected virtual Lpp.Security.Data.SecurityTarget SecTarget( TEntity e ) {
            
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var o = e as ISecurityObject; 
            //return o == null ? null : Sec.Target( o ); 
        }

        protected virtual bool CanCreateNew( TCreateGetModel model ) 
        { 
            return true; 
        }

        protected virtual bool CanList() 
        { 
            return true; 
        }

        protected virtual bool CanEdit( TEntity e ) 
        { 
            return Can( e )( SecPrivileges.Crud.Edit ); 
        }

        protected virtual bool CanRead( TEntity e ) 
        { 
            return Can( e )( SecPrivileges.Crud.Read ); 
        }

        protected virtual bool CanDelete( TEntity e ) 
        { 
            return Can( e )( SecPrivileges.Crud.Delete ); 
        }

        protected virtual bool CanManageSecurity( TEntity e ) 
        { 
            return Can( e )( SecPrivileges.ManageSecurity ); 
        }

        protected virtual string ListSettingsKey() 
        { 
            return typeof( TEntity ).Name + "s"; 
        }
        
        private TEntity _canCacheFor;
        private TEntity _newCreated;
        private TCreateGetModel _currentCreateModel;
        private Func<SecurityPrivilege, bool> _canCache;

        protected Func<SecurityPrivilege, bool> Can( TEntity e )
        {
            //TODO: update Can logic to use new security

            return _ => true;

            //if ( IsNew( e ) )
            //{
            //    if ( CanCreateNew( _currentCreateModel ) ) return p => p != SecPrivileges.Crud.Delete;
            //    else return _ => false;
            //}

            //if ( _canCache == null || !ReferenceEquals( _canCacheFor, e ) )
            //{
            //    var t = SecTarget( e );
            //    if ( t == null ) _canCache = _ => true;
            //    else _canCache = Security.Can( t, Auth.CurrentUser );
            //    _canCacheFor = e;
            //}

            //return _canCache;
        }

        protected bool IsNew( TEntity e ) 
        { 
            return ReferenceEquals( e, _newCreated ); 
        }

        protected virtual TEditModel SecureModel( TEntity e, TEditModel m )
        {
            var sec = m as ICrudSecObjectEditModel;
            if ( sec != null )
            {
                sec.AllowSave = CanEdit( e );
                sec.AllowDelete = !IsNew( e ) && CanDelete( e );
                sec.ShowAcl = CanManageSecurity( e );
            }

            return m;
        }

        public virtual ActionResult Edit( Guid id, string returnTo )
        {            
            var e = Find( id );

            if ( e == null ) 
                return HttpNotFound();

            if ( !CanRead( e ) ) 
                throw new UnauthorizedAccessException();

            return this.View( SecureModel( e, EditModel( e, returnTo ) ) );
        }

        [HttpPost]
        public virtual ActionResult Edit( TEntityEditPostModel model )
        {
            var e = Find( model.ID);
            if ( e == null ) return HttpNotFound();
            if ( model.IsDelete() )
            {
                if ( !CanDelete( e ) ) throw new UnauthorizedAccessException();
                return Delete( e, model );
            }
            else if ( model.IsSave() )
            {
                if ( !CanEdit( e ) ) throw new UnauthorizedAccessException();
                return Post( e, model );
            }

            return HttpNotFound();
        }

        public virtual ActionResult Create( TCreateGetModel model )
        {
            if ( !CanCreateNew( model ) ) 
                throw new UnauthorizedAccessException();

            var e = _newCreated = New( model );

            _currentCreateModel = model;

            return this.View( SecureModel( e, EditModel( e, model.ReturnTo ) ) );
        }

        [HttpPost]
        public virtual ActionResult Create( TCreateGetModel createModel, TEntityEditPostModel editModel )
        {

            _currentCreateModel = createModel;

            if (!CanCreateNew(_currentCreateModel))
                throw new UnauthorizedAccessException();

            _newCreated = New(createModel);
            var set = DataContext.Set<TEntity>();
            set.Add(_newCreated);

            return Post(_newCreated, editModel);
        }

        ActionResult Post( TEntity e, TEntityEditPostModel model )
        {
            bool isNew = IsNew(e);
            try
            {
                if (!Validate(model, e))
                {
                    return this.View(SecureModel(e, EditModel(e, model.ReturnTo)));
                }

                ApplyPostModel(e, model);

                if (isNew)
                {
                    throw new Lpp.Utilities.CodeToBeUpdatedException();
                    //PermissionTemplates.GetDefaultPermissions(e).ForEach(x => Security.SetAcl(x.Key, x));
                }

                DataContext.SaveChanges();
                
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;

                Log.Error(ex);
                DnsResult.FromException(ex).ErrorMessages.ForEach(m => ModelState.AddModelError("", m));
                return this.View(SecureModel(e, EditModel(e, model.ReturnTo)));
            }

            try
            {
                OnCrudEvent(e, isNew ? Events.CrudEventKind.Added : Events.CrudEventKind.Changed);
                DataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;

                Log.Error(ex);
            }

            return RedirectAfterPost(e, model);
        }

        protected virtual ActionResult Delete( TEntity e, TEntityEditPostModel post )
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var dp = from p in Maybe.Value( IsDeletedProperty )
            //         from b in p.Body as MemberExpression
            //         from m in b.Member as PropertyInfo
            //         select m;
            //if ( dp.Kind != MaybeKind.Value ) return HttpNotFound();

            //dp.Value.SetValue( e, true, null );
            //OnCrudEvent( e, Events.CrudEventKind.Removed );
            //UnitOfWork.Commit();
            //return RedirectAfterPost( e, post );
        }

        bool Validate( TEntityEditPostModel model, TEntity u )
        {
            RunValidationRules( model, u );
            return ModelState.IsValid;
        }

        protected void Rule( bool error, string message ) 
        { 
            if ( error ) 
                ModelState.AddModelError( "", message ); 
        }

        [NoAjaxNavigation]
        public ActionResult List( TListGetModel req )
        {
            if ( !CanList() ) 
                throw new UnauthorizedAccessException();

            req.Sort = req.Sort ?? Settings.GetSetting( ListSettingsKey() + SettingsKeys.Sort );
            req.SortDirection = req.SortDirection ?? Settings.GetSetting( ListSettingsKey() + SettingsKeys.SortDirection );
            req.PageSize = req.PageSize ?? Settings.GetSetting( ListSettingsKey() + SettingsKeys.PageSize );
            return this.View( ListModel( req ) );
        }

        [AjaxCall]
        public ActionResult ListBody( TListGetModel req )
        {
            if ( !CanList() )
                throw new UnauthorizedAccessException();

            Settings.SetSettings( new SortedList<string,string>
            {
                { ListSettingsKey() + SettingsKeys.Sort, req.Sort },
                { ListSettingsKey() + SettingsKeys.SortDirection, req.SortDirection },
                { ListSettingsKey() + SettingsKeys.PageSize, req.PageSize }
            } );

            return this.View( ListModel( req ) );
        }

        [NonAction]
        public virtual TListModel ListModel( TListGetModel req )
        {
            return new TListModel
            {
                Items = All( req ).ListModel( req, Sort ),
                AllowCreate = CanCreateNew( CreateFromList( req ) ),
            };
        }

        class SettingsKeys
        {
            public static string Sort           = ".sort";
            public static string SortDirection  = ".sortdir";
            public static string PageSize       = ".pagesize";
        }
    }

    abstract class CrudController<TEntity, TEntityEditPostModel, TListGetModel, TEditModel, TController> :
        CrudController<TEntity, CrudCreateModel, TEntityEditPostModel, TListGetModel, TEditModel, CrudListModel<TEntity, TListGetModel>, TController>
        where TEntity : class, new()
        where TEntityEditPostModel : ICrudPostModel<TEntity>
        where TListGetModel : struct, IListGetModel
        where TController : CrudController<TEntity, TEntityEditPostModel, TListGetModel, TEditModel, TController>
    {
    }

    abstract class CrudController<TEntity, TEntityEditPostModel, TListGetModel, TController> :
        CrudController<TEntity, CrudCreateModel, TEntityEditPostModel, TListGetModel, TEntity, CrudListModel<TEntity, TListGetModel>, TController>
        where TEntity : class, new()
        where TEntityEditPostModel : ICrudPostModel<TEntity>
        where TListGetModel : struct, IListGetModel
        where TController : CrudController<TEntity, TEntityEditPostModel, TListGetModel, TController>
    {
        protected override TEntity EditModel( TEntity e, string returnTo ) 
        { 
            return e; 
        }
    }

    public interface IHaveCreateMethod<TCreateGetModel> 
    { 
        ActionResult Create( TCreateGetModel model ); 
    }

    public interface IHaveCreateMethod 
    { 
        ActionResult Create(); 
    }

    public interface IAmListController<TListGetModel>
    {
        ActionResult List( TListGetModel model );
        ActionResult ListBody( TListGetModel model );
    }
}