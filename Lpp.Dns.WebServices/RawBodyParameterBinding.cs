using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Lpp.Dns.WebServices
{
    //Thanks to http://weblog.west-wind.com/posts/2013/Dec/13/Accepting-Raw-Request-Body-Content-with-ASPNET-Web-API for implementation.

    //TODO: this should be removed and references changed to the implementation in the Lpp.Utilities.Websites.Attributes namespace

    public class RawBodyParameterBinding : HttpParameterBinding
    {
        public RawBodyParameterBinding(HttpParameterDescriptor descriptor) : base(descriptor) { }

        public override Task ExecuteBindingAsync(System.Web.Http.Metadata.ModelMetadataProvider metadataProvider, HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            var binding = actionContext
            .ActionDescriptor
            .ActionBinding;

            if (binding.ParameterBindings.Where(p => p.WillReadBody).Count() > 1 || actionContext.Request.Method == HttpMethod.Get)
            {
                throw new InvalidOperationException("Only a single parameter that will read from the body of the message can be bound to this type of binding, and it cannot be used for a Get.");
            }

            //var type = binding
            //            .ParameterBindings[0]
            //            .Descriptor.ParameterType;
            var type = binding.ParameterBindings.Where(p => p.WillReadBody).First().Descriptor.ParameterType;

            if (type == typeof(string))
            {
                return actionContext.Request.Content
                        .ReadAsStringAsync()
                        .ContinueWith((task) =>
                        {
                            var stringResult = task.Result;
                            SetValue(actionContext, stringResult);
                        });
            }
            else if (type == typeof(byte[]) || type == typeof(IEnumerable<byte>))
            {
                return actionContext.Request.Content
                    .ReadAsByteArrayAsync()
                    .ContinueWith((task) =>
                    {
                        byte[] result = task.Result;
                        SetValue(actionContext, result);
                    });
            }

            throw new InvalidOperationException("Only string and byte[] are supported for [RawBody] parameters");
        }

        public override bool WillReadBody
        {
            get
            {
                return true;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public sealed class RawBodyAttribute : ParameterBindingAttribute
    {
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            if (parameter == null)
                throw new ArgumentException("Invalid parameter");

            return new RawBodyParameterBinding(parameter);
        }
    }
}
