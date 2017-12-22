using System;
using System.ComponentModel.Composition;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Serialization;
using Lpp.Dns.RedirectBridge.Models;
using Lpp.Mvc;
using Lpp.Mvc.Controls;
using Lpp.Utilities.Legacy;

namespace Lpp.Dns.RedirectBridge.Controllers
{
    [CompiledViews(typeof(Views.Plugins.List))]
    public class PluginsController : BaseController
    {
        //[Import]
        //public IRepository<RedirectDomain, Model> Models { get; set; }
        //[Import]
        //public IUnitOfWork<RedirectDomain> UnitOfWork { get; set; }
        const int DefaultPageSize = 20;

        public ActionResult List(ListGetModel r)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return this.View(Models.All.ListModel(r, _sort, DefaultPageSize));
        }

        public ActionResult ListBody(ListGetModel r)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return View(Models.All.ListModel(r, _sort, DefaultPageSize));
        }

        [HttpPost]
        public ActionResult Upload()
        {
            if (Request.Files.Count == 0) return RedirectToAction<PluginsController>(c => c.List(new ListGetModel()));
            var file = Request.Files[0];

            try
            {
                var serialized = new StreamReader(file.InputStream).ReadToEnd();
                var modelConfig = DeserializeConfiguration(serialized);
                return ValidateUploadedConfig(modelConfig) ?
                    this.View("ConfirmUpload", new UploadConfirmModel { Configuration = modelConfig, Serialized = serialized }) :
                    this.View("UploadError", new UploadConfirmModel { Configuration = modelConfig });
            }
            catch (Exception ex)
            {
                AddModelStateErrorsFromException(ex);
                return this.View("UploadError", new UploadConfirmModel());
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ConfirmUpload(string xml)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //try
            //{
            //    var modelConfig = DeserializeConfiguration(xml);
            //    if (modelConfig != null && ValidateUploadedConfig(modelConfig))
            //    {
            //        try
            //        {
            //            Models.Add(EntityFromConfig(modelConfig));
            //            UnitOfWork.Commit();
            //            return RedirectToAction<PluginsController>(c => c.List(new ListGetModel()));
            //        }
            //        catch (DbEntityValidationException ex)
            //        {
            //            foreach (var e in ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors))
            //            {
            //                ModelState.AddModelError("", e.ErrorMessage);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            AddModelStateErrorsFromException(ex);
            //        }

            //        return this.View("UploadError", new UploadConfirmModel { Configuration = modelConfig });
            //    }
            //}
            //catch
            //{
            //    // This exception is swallowed, because it should have been caught in the Upload() action.
            //    // If we got this far, this means deserialization and validation went ok already.
            //    // And if we still got an exception here, this can only mean that somebody is
            //    // fiddling with HTML and/or HTTP, so screw them - just redirect to the list as if nothing happened
            //}

            //return RedirectToAction<PluginsController>(c => c.List(new ListGetModel()));
        }

        public ActionResult View(Guid modelId)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var m = Models.Find(modelId);
            //if (m == null) return HttpNotFound();

            //return this.View(new ModelConfiguration
            //{
            //    Id = m.Id,
            //    Name = m.Name,
            //    Description = m.Description,
            //    Version = m.Version,
            //    ModelProcessorId = m.ModelProcessorId,
            //    PublicKey = m.RSA_Modulus != null && m.RSA_Exponent != null ?
            //        new RSAPublicKey { ModulusBase64 = Convert.ToBase64String(m.RSA_Modulus), ExponentBase64 = Convert.ToBase64String(m.RSA_Exponent) }
            //        : null,
            //    RequestTypes = m.RequestTypes.Select(r => new ModelRequestType
            //    {
            //        Id = r.LocalId,
            //        Name = r.Name,
            //        Description = r.Description,
            //        CreateRequestUrl = r.CreateRequestUrl,
            //        RetrieveResponseUrl = r.RetrieveResponseUrl,
            //        IsMetadataRequest = r.IsMetadataRequest
            //    }).ToArray()
            //});
        }

        public ActionResult Remove(string id, string returnUrl)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var m = from mid in Maybe.Value(id)
            //        from gid in new Guid(id)
            //        from mdl in Models.Find(gid)
            //        from _ in Remove(mdl)
            //        from __ in Commit()
            //        select mdl;

            //return string.IsNullOrEmpty(returnUrl) ?
            //    this.RedirectToAction<PluginsController>(c => c.List(new ListGetModel())) :
            //    this.Redirect(returnUrl);
        }

        int Remove(Model m) 
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();
            //Models.Remove(m); 
            //return 0; 
        }
        
        int Commit() 
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();
            //UnitOfWork.Commit(); 
            //return 0; 
        }

        private Model EntityFromConfig(ModelConfiguration modelConfig)
        {
            return new Model
            {
                ID = modelConfig.Id,
                ModelProcessorID = modelConfig.ModelProcessorId,
                Name = modelConfig.Name,
                Description = modelConfig.Description,
                Created = DateTime.UtcNow,
                Version = modelConfig.Version,
                RSA_Modulus = modelConfig.PublicKey == null ? null : Convert.FromBase64String(modelConfig.PublicKey.ModulusBase64),
                RSA_Exponent = modelConfig.PublicKey == null ? null : Convert.FromBase64String(modelConfig.PublicKey.ExponentBase64),
                RequestTypes =
                    modelConfig.RequestTypes
                    .Select(r => new RequestType
                    {
                        LocalId = r.Id,
                        Name = r.Name,
                        IsMetadataRequest = r.IsMetadataRequest,
                        Description = r.Description,
                        CreateRequestUrl = r.CreateRequestUrl,
                        RetrieveResponseUrl = r.RetrieveResponseUrl
                    })
                    .ToList()
            };
        }

        private static ModelConfiguration DeserializeConfiguration(string xml)
        {
            return
                XmlSerializer
                .FromTypes(new[] { typeof(ModelConfiguration) })
                [0]
                .Deserialize(new StringReader(xml))
                as ModelConfiguration;
        }

        private void AddModelStateErrorsFromException(Exception ex)
        {
            while (ex != null)
            {
                ModelState.AddModelError("", ex.Message);
                ex = ex.InnerException;
            }
        }

        bool ValidateUploadedConfig(ModelConfiguration config)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //if (config == null)
            //{
            //    ModelState.AddModelError("", "The file appears to be malformed or empty");
            //    return false;
            //}

            //if (config.Id == Guid.Empty) 
            //    ModelState.AddModelError("Id", "Id appears to be malformed or empty");

            //if (config.ModelProcessorId == Guid.Empty) 
            //    ModelState.AddModelError("ModelProcessorId", "ModelProcessorId appears to be malformed or empty");

            //if (Models.Find(config.Id) != null) 
            //    ModelState.AddModelError("Id", "There is already a model registsred with Id = " + config.Id);

            //if (string.IsNullOrEmpty(config.Name)) 
            //    ModelState.AddModelError("Name", "Name must be defined");

            //ValidateRSAKey(config.PublicKey);

            //if (string.IsNullOrEmpty(config.Version)) 
            //    ModelState.AddModelError("Version", "Version is not defined");

            //if (config.RequestTypes == null || config.RequestTypes.Length == 0) 
            //    ModelState.AddModelError("Request", "No requests are defined for the model. At least one request is required.");

            //int reqNum = 1;
            //foreach (var r in config.RequestTypes.EmptyIfNull())
            //{
            //    ValidateRequestType(r, reqNum++);
            //}

            //return ModelState.IsValid;
        }

        void ValidateRequestType(ModelRequestType r, int index)
        {
            if (r.Id == Guid.Empty) 
                ModelState.AddModelError("Id" + index, "Id for request #" + index + " is empty or malformed");

            if (string.IsNullOrEmpty(r.Name)) 
                ModelState.AddModelError("Name" + index, "Name for request #" + index + " is not defined");

            if (string.IsNullOrEmpty(r.CreateRequestUrl)) 
                ModelState.AddModelError("CreateRequestUrl" + index, "'Create Request' URL for request #" + index + " is not defined");

            if (string.IsNullOrEmpty(r.RetrieveResponseUrl)) 
                ModelState.AddModelError("RetrieveResponseUrl" + index, "'View Response' URL for request #" + index + " is not defined");
        }

        void ValidateRSAKey(RSAPublicKey key)
        {
            if (key != null)
            {
                ValidateBase64(key.ModulusBase64, "PublicKey/Modulus");
                ValidateBase64(key.ExponentBase64, "PublicKey/Exponent");
            }
        }

        void ValidateBase64(string base64, string field)
        {
            if (string.IsNullOrEmpty(base64)) 
                ModelState.AddModelError(field, field + " is not defined.");

            try { 
                Convert.FromBase64String(base64); 
            }
            catch 
            { 
                ModelState.AddModelError(field, field + " is not a valid base-64 string."); 
            }
        }

        static readonly SortHelper<Model> _sort = new SortHelper<Model>()
            .Sort(m => m.Created, false)
            .Sort(m => m.Name)
            .Sort(m => m.Version)
            .Default(m => m.Name);

    }
}