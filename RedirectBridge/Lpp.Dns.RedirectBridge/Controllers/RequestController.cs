using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Mvc.Application;
using Lpp.Mvc;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Data.Entity.Validation;
using Lpp.Dns.RedirectBridge.Models;
using Lpp.Mvc.Controls;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using Lpp.Composition;

namespace Lpp.Dns.RedirectBridge.Controllers
{
    [AutoRoute]
    [Export, Export(typeof(IController)), PartMetadata(ExportScope.Key, TransactionScope.Id)]
    public class RequestController : BaseController
    {
        [Import]
        public IDnsModelPluginHost Host { get; set; }
        [Import]
        public SessionService Sessions { get; set; }
        //[Import]
        //public IRepository<RedirectDomain, RequestType> RequestTypes { get; set; }

        public string CreateSessionAndGetRedirectUrlForRequest(Guid requestID, string redirectBackUrl)
        {
            return CreateSession(requestID, redirectBackUrl, true);
        }

        public ActionResult CreateSessionAndRedirectForResponse(Guid requestID, string responseToken, string redirectBackUrl)
        {
            var url = CreateSession(requestID, redirectBackUrl, false, responseToken);
            return url == null ? HttpNotFound() : (ActionResult)Redirect(url);
        }

        string CreateSession(Guid requestID, string redirectBackUrl, bool isRequest, string responseToken = null)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var context = Host.GetRequestContext(requestID);
            //if (context == null) return null;

            //var rt = RequestTypes.All.FirstOrDefault(r => r.LocalId == context.RequestType.Id);
            //if (rt == null || string.IsNullOrEmpty(rt.CreateRequestUrl) || rt.Model == null) return null;

            //var preExisting = Sessions.FindSession(requestId);
            //if (preExisting != null) Sessions.Sessions.Remove(preExisting);

            //var sess = Sessions.CreateSession(context.RequestId, redirectBackUrl, responseToken);
            //var encryptedToken = EncryptToken(rt.Model, sess.Id);
            //var url = new UrlHelper(HttpContext.Request.RequestContext);
            //var redirectUrl = string.Format(
            //    isRequest ? rt.CreateRequestUrl : rt.RetrieveResponseUrl,
            //    encryptedToken, url.Absolute("/" +
            //    (isRequest ? Routes.GetRequestServiceUrl("rest") : Routes.GetResponseServiceUrl("rest"))));
            //return redirectUrl;
        }

        static string EncryptToken(Model model, string token)
        {
            //Contract.Requires( !String.IsNullOrEmpty( token ) );
            //Contract.Requires( model != null );
            //Contract.Ensures( //Contract.Result<string>() != null );

            if (model.RSA_Modulus == null || model.RSA_Exponent == null) return token;

            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(new RSAParameters { Modulus = model.RSA_Modulus, Exponent = model.RSA_Exponent });
                return Convert.ToBase64String(rsa.Encrypt(Encoding.ASCII.GetBytes(token), true));
            }
        }
    }
}