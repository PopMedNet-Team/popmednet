using Lpp.Objects;
using Lpp.Security;
using Lpp.Utilities.Objects;
using Lpp.Utilities.Security;
using Lpp.Workflow.Engine;
using Lpp.Workflow.Engine.Database;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core;
using System.Security;

namespace Lpp.Utilities.WebSites.Controllers
{
    public class LppApiWorkflowController<TEntity, TDto, TDataContext, TPermissions, TActivity, TCompletionRequest, TCompletionResponse> : LppApiDataController<TEntity, TDto, TDataContext, TPermissions>
        where TEntity : EntityWithID, IWorkflowEntity
        where TDto : EntityDtoWithID, new()
        where TDataContext : DbContext, ISecurityContextProvider<TPermissions>, IWorkflowDataContext, new()
        where TPermissions : IPermissionDefinition
        where TActivity : class, IDbWorkflowActivity
        where TCompletionRequest : ICompletionRequest<TDto>, new()
        where TCompletionResponse : ICompletionResponse<TDto>, new()
    {

        /// <summary>
        /// Completes a workflow activity with the given parameters.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="demandActivityResultID"></param>
        /// <returns></returns>
        public virtual async Task<TCompletionResponse> CompleteWorkflowActivity(TCompletionRequest request, Func<TEntity, WorkflowActivityContext> GetWorkFlowActivity, Func<TEntity, Task> CompleteEntityPreSave, Func<TEntity, Task> CompletePostSave) {

            Dictionary<TDto, TEntity> map;
            try
            {
                map = await LoadDTOs(new TDto[] {request.Dto});
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.UnwindException()));
            }

            var entity = map.First().Value;


            if (CompleteEntityPreSave != null)
                await CompleteEntityPreSave(entity);

            var workflow = new Workflow<TDataContext, TEntity>(DataContext, entity, Identity, () => GetWorkFlowActivity(entity));
            await workflow.InitializeActivityAsync();


            var validationResult = await workflow.Validate(request.DemandActivityResultID);
            if (!validationResult.Success)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, validationResult.Errors));

            try
            {
                await DataContext.SaveChangesAsync();
            }
            catch (DbUpdateException dbe)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, dbe.UnwindException()));
            }
            catch (DbEntityValidationException ve)
            {
                string validationErrors = string.Join("<br/>", ve.EntityValidationErrors.Select(v => string.Join("<br/>", v.ValidationErrors.Select(e => e.PropertyName + ": " + e.ErrorMessage).ToArray())).ToArray());

                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, validationErrors));
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.UnwindException()));
            }

            UpdateDTOs(ref map);

            if (CompletePostSave != null)
                await CompletePostSave(entity);

            var response = new TCompletionResponse();

            response.Entity = map.Select(m => m.Key).First();
            response.Uri = await workflow.Complete(request.Data, request.DemandActivityResultID, request.Comment);

            return response;
        }
    }
}
