using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Mvc;

namespace Lpp.Dns.Portal
{
    class PostContext : IDnsPostContext
    {
        public PostContext(IValueProvider values)
        {
            //Contract.Requires(values != null);
            Values = values;
        }

        public IValueProvider Values { get; private set; }

        private void getModel<TModel>(TModel result, System.ComponentModel.PropertyDescriptorCollection props, string parentContext) where TModel : class, new()
        { 
            // loop each property in the passed in model
            foreach (var p in props.Cast<PropertyDescriptor>())
            {
                // get the property name... if we are in a hierarchal model structure, prepend the parent's name and seperate with an underscore
                var propName = string.IsNullOrEmpty(parentContext) ? p.Name : parentContext + '_' + p.Name;

                try
                {
                    // test to see if this is a nested model of base type "DnsPluginModel"
                    if ((p.PropertyType.BaseType != null) && (p.PropertyType.BaseType.Name.Equals("DnsPluginModel")))
                    {
                        // yes, get its child properties
                        var childProps = p.GetChildProperties();
                        if (childProps.Count > 0)
                        {
                            // get the actual instance (NOT the type!) of the current result's named (parentContext) child model that we just 
                            // got child properties for so we can pass it in as the "subresult"
                            DnsPluginModel childInstanceSubresult = result.GetType().GetProperty(p.Name).GetValue(result, null) as DnsPluginModel;

                            if (childInstanceSubresult != null)
                                getModel<DnsPluginModel>(childInstanceSubresult, childProps, propName);
                        }
                    }
                }
                catch { continue; }

                // pull out the named (propName) value from the response's values, also determine if validation should be skipped for the property.
                //skipping validation should only be done when the field contains html fragments.

                bool skipValidation = false;
                foreach (var att in p.Attributes)
                {
                    if (att is Lpp.Objects.ValidationAttributes.SkipHttpValidationAttribute)
                    {
                        skipValidation = true;
                        break;
                    }
                }

                var v = ((System.Web.Mvc.ValueProviderCollection)Values).GetValue(propName, skipValidation);
                
                if (v == null) continue;
                
                // convert the value v to the target result's property's type
                object value;
                try { value = v.ConvertTo(p.PropertyType); }
                catch { continue; }

                // given the property p, plug the value into the result (into the result's property)
                p.SetValue(result, value);
            }
        }

        public TModel GetModel<TModel>() where TModel : class, new()
        {
            // create a new model to hold the results
            var result = new TModel();

            // pass in the new model, the top-level list of it's properties, and an empty parent context
            getModel<TModel>(result, TypeDescriptor.GetProperties(result), string.Empty);

            return result;
        }
    }
}