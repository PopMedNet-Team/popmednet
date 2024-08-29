using Lpp.Utilities.WebSites.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;
using Lpp.Objects;

namespace Lpp.Dns.Api.DataMartClient
{
    /// <summary>
    /// Actions for adapter processor packages.
    /// </summary>
    /// <remarks>This controller is intentionally not included in the Lpp.Dns.NetClient generated file so that a dependency is not needed on Lpp.Dns.DTO.DataMartClient.</remarks>
    [ClientEntityIgnore]
    public class AdaptersController : LppApiController<Lpp.Dns.Data.DataContext>
    {
        /// <summary>
        /// packages folder
        /// </summary>
        public readonly string PackagesFolder;
        /// <summary>
        /// Adapters controller
        /// </summary>
        public AdaptersController()
        {
            var folder = System.Web.Configuration.WebConfigurationManager.AppSettings["AdapterPackages.Folder"];
            if (string.IsNullOrWhiteSpace(folder))
            {
                folder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
            }

            PackagesFolder = folder;
        }
        /// <summary>
        /// packages folder
        /// </summary>
        /// <param name="packagesFolder"></param>
        public AdaptersController(string packagesFolder)
        {
            PackagesFolder = packagesFolder;
        }

        /// <summary>
        /// Gets an adapter package based on the specified package identifier and version.
        /// </summary>
        /// <param name="identifier">The adapter package identifier.</param>
        /// <param name="version">The version of the package.</param>
        /// <returns>A zip file containing the adapter plugin and supporting dependencies.</returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetPackage(string identifier, string version)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The package identifier is required.");
            }

            if (string.IsNullOrWhiteSpace(version))
            {
                //try to get the current version
                version = await GetCurrentVersion(identifier);
                
                if(string.IsNullOrWhiteSpace(version))
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The version of the package is required.");
            }

            string filename = System.IO.Path.Combine(PackagesFolder, string.Format("{0}.{1}.zip", identifier, version));

            if (!System.IO.File.Exists(filename))
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The requested package was not found.");
            }

            var fileInfo = new System.IO.FileInfo(filename);
            var content = new StreamContent(fileInfo.OpenRead());
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/zip");
            content.Headers.ContentLength = fileInfo.Length;
            content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = fileInfo.Name,
                Size = fileInfo.Length
            };

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = content
            };
        }

        /// <summary>
        /// Get an adapter package based on the specified request.
        /// </summary>
        /// <param name="requestID">The ID of the request the package is required for.</param>
        /// <returns>A zip file containing the adapter plugin and supporting dependencies.</returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetPackageForRequest(Guid requestID)
        {
            var info = await DataContext.Requests.Where(r => r.ID == requestID).Select(r => new { Identifier = r.RequestType.PackageIdentifier, Version = r.AdapterPackageVersion }).FirstOrDefaultAsync();

            if (info == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Dns request not found.");
            }

            if (string.IsNullOrWhiteSpace(info.Identifier))
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "The identifier for the request type has not been configured correctly.");
            }

            if (string.IsNullOrWhiteSpace(info.Version))
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "The version for the request was not set correctly, unable to determine version.");
            }

            return await GetPackage(info.Identifier, info.Version);
        }

        /// <summary>
        /// Gets the latest version number for the specified package identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>The version number.</returns>
        [HttpGet]
        public async Task<string> GetCurrentVersion(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The package identifier is required."));
            }

            return await Task.Run<string>(() =>
            {

                var packages = System.IO.Directory.EnumerateFiles(PackagesFolder, identifier + ".*.zip").ToArray();
                if (packages == null || packages.Length == 0)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No packages found for the specified identifier: " + identifier));
                }

                //determine versions and return the highest
                var version = packages.Select(p => Version.Parse(System.IO.Path.GetFileNameWithoutExtension(p).Substring(identifier.Length + 1)))
                                       .OrderByDescending(v => v.Major)
                                       .ThenByDescending(v => v.Minor)
                                       .ThenByDescending(v => v.Build)
                                       .ThenByDescending(v => v.Revision)
                                       .FirstOrDefault();

                return version.ToString();
            }); 
        }

        /// <summary>
        /// Gets the RequestType identifier and latest version for the specified model ID and processor ID.
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="processorID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Lpp.Dns.DTO.DataMartClient.RequestTypeIdentifier> GetRequestTypeIdentifier(Guid modelID, Guid processorID)
        {
            //var identifier = await DataContext.RequestTypeDataModels
            //                            .Where(rtm => rtm.DataModelID == modelID && rtm.RequestType.ProcessorID == processorID)
            //                            .Select(rtm => rtm.RequestType.PackageIdentifier)
            //                            .FirstOrDefaultAsync();

            var identifier = await DataContext.RequestTypes.Where(rt => rt.ProcessorID == processorID && !string.IsNullOrEmpty(rt.PackageIdentifier)).Select(rt => rt.PackageIdentifier).FirstOrDefaultAsync();

            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "A package identifier was not found for the specified model ID and processor ID."));
            }

            return await Task.Run<Lpp.Dns.DTO.DataMartClient.RequestTypeIdentifier>(() =>
            {

                var packages = System.IO.Directory.EnumerateFiles(PackagesFolder, identifier + ".*.zip")
                                    .Where(f => System.Text.RegularExpressions.Regex.IsMatch(f, identifier + @"\.[\d]+\.[\d]+\.[\d]+\.[\d]+\.zip", System.Text.RegularExpressions.RegexOptions.IgnoreCase|System.Text.RegularExpressions.RegexOptions.Singleline)).ToArray();
                if (packages == null || packages.Length == 0)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "No packages found for the specified identifier: " + identifier));
                }

                //determine versions and return the highest
                var version = packages.Select(p => Version.Parse(System.IO.Path.GetFileNameWithoutExtension(p).Substring(identifier.Length + 1)))
                                       .OrderByDescending(v => v.Major)
                                       .ThenByDescending(v => v.Minor)
                                       .ThenByDescending(v => v.Build)
                                       .ThenByDescending(v => v.Revision)
                                       .FirstOrDefault();

                return new DTO.DataMartClient.RequestTypeIdentifier{ Identifier = identifier, Version = version.ToString() };
            }); 
        }

        
    }
}
