using Lpp.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Lpp.Artanis
{
    public class NetClient
    {
        public static void GenTS(string path, string Config)
        {
            Configuration Json;
            using (StreamReader r = new StreamReader(Path.GetFullPath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Configs\\" + Config + ".json")))
            {
                string json = r.ReadToEnd();
                Json = JsonConvert.DeserializeObject<Configuration>(json);
            }

            Assembly assembly = Assembly.LoadFrom(path + "\\" + Json.ApiNamespace + "\\bin\\" + Json.ApiNamespace + ".dll");

            var controllers = assembly.GetTypes().Where(type => typeof(ApiController).IsAssignableFrom(type) && !type.FullName.Contains("ClientEntityIgnore"));

            string apiFile = Path.GetFullPath(path + Json.OutputPath);

            ApiHelpers.Initalize(Json.DtoNamespace, Json.EnumNamespace, Json.TSNamespace);

            using (StreamWriter outputFile = new StreamWriter(apiFile, false))
            {
                outputFile.WriteLine(@"//disable the missing comment rule warning until auto comments are completed
#pragma warning disable 1591

using "+ Json.DtoNamespace +@";
using Lpp.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;");
                outputFile.WriteLine("");

                outputFile.WriteLine("namespace "+ Json.NetclientNamespace);
                outputFile.WriteLine("{");
                outputFile.WriteLine("\t public class "+ Json.NetclientVariable + " : HttpClientEx");
                outputFile.WriteLine("\t {");
                outputFile.WriteLine("\t \t public "+ Json.NetclientVariable + "(string host) : base(host) { }");
                outputFile.WriteLine("\t \t public "+ Json.NetclientVariable + "(string host, string userName, string password) : base(host, userName, password) { }");
                outputFile.WriteLine("");
                StringBuilder sbBase = new StringBuilder();
                StringBuilder sbBottom = new StringBuilder();
                foreach (var controller in controllers)
                {
                    if (!controller.CustomAttributes.Any(x => x.AttributeType.Name == "ClientEntityIgnoreAttribute"))
                    {
                        var name = controller.Name.Replace("Controller", "");
                        sbBase.AppendLine("\t \t" + name + " _" + name + " = null;");
                        sbBase.AppendLine(@"        public " + name + @" " + name + @"
                            {
                                get {
                                    if (_" + name + @" == null)
                                        _" + name + @" = new " + name + @"(this);

                                    return _" + name + @";
                                }
                            }");
                        if (controller.BaseType.Name.Contains("LppApiDataController"))
                        {
                            string singleClassName;
                            if (name.EndsWith("es") && name != "Responses" && name != "RequestTypes" && name != "Registries" && name != "NetworkMessages" && name != "Templates")
                                singleClassName = name.Substring(0, name.Length - 2);
                            else if (name.EndsWith("ies"))
                                singleClassName = name.Substring(0, name.Length - 3) + "y";
                            else if (name.EndsWith("s") && name != "RequestType")
                                singleClassName = name.Substring(0, name.Length - 1);
                            else
                                singleClassName = name;
                            sbBottom.AppendLine("\t public class " + name + " : HttpClientDataEndpoint<" + Json.NetclientVariable + ", " + singleClassName + "DTO>");
                            sbBottom.AppendLine("\t {");
                            sbBottom.AppendLine("\t \t public " + name + "(" + Json.NetclientVariable + " client) : base(client, \"/" + name + "\") {}");
                        }
                        else
                        {
                            sbBottom.AppendLine("\t public class " + name);
                            sbBottom.AppendLine("\t {");
                            sbBottom.AppendLine("\t \t readonly " + Json.NetclientVariable + " Client;");
                            sbBottom.AppendLine("\t \t readonly string Path;");
                            sbBottom.AppendLine("\t \t public " + name + "(" + Json.NetclientVariable + " client)");
                            sbBottom.AppendLine("\t \t {");
                            sbBottom.AppendLine("\t \t \t this.Client = client;");
                            sbBottom.AppendLine("\t \t \t this.Path = \"/" + name + "\";");
                            sbBottom.AppendLine("\t \t }");
                        }


                        var actions = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                        foreach (var action in actions)
                        {
                            var x = action.GetCustomAttributes().Where(attr => attr.GetType() != typeof(ClientEntityIgnoreAttribute) && attr.GetType() == typeof(HttpGetAttribute) || attr.GetType() == typeof(HttpPutAttribute) || attr.GetType() == typeof(HttpPostAttribute) || attr.GetType() == typeof(HttpDeleteAttribute)).ToArray();
                            if (x.Count() > 0)
                            {
                                List<string> excludeNames = new List<string>();
                                if (controller.BaseType.Name.Contains("LppApiDataController"))
                                {
                                    var names = new string[] { "Insert", "Update", "InsertOrUpdate", "Delete", "Get", "List" };
                                    excludeNames.AddRange(names);
                                }
                                if (!excludeNames.Contains(action.Name))
                                {
                                    if (!action.GetCustomAttributes().Any(z => z.GetType() == typeof(ClientEntityIgnoreAttribute)))
                                    {
                                        #region HttpPost
                                        if (x.Any(y => y.TypeId.ToString() == "System.Web.Http.HttpPostAttribute"))
                                        {
                                            var actionParams = action.GetParameters().AsEnumerable();
                                            var paramBody = "";
                                            var paramHead = "";
                                            var returnType = "";
                                            var querableReturnType = "";
                                            if (action.ReturnType.GenericTypeArguments.Count() > 0)
                                            {
                                                if (action.ReturnType.Name.Contains("Enumerable") || action.ReturnType.GenericTypeArguments[0].Name.Contains("IEnumerable"))
                                                {
                                                    if (action.ReturnType.GenericTypeArguments[0].Name.Contains("IEnumerable"))
                                                    {
                                                        returnType = "System.Collections.Generic.IEnumerable<" + action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].FullName + ">";
                                                        querableReturnType = action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].FullName;
                                                    }
                                                    else
                                                    {
                                                        returnType = "System.Collections.Generic.IEnumerable<" + action.ReturnType.GenericTypeArguments[0].FullName + ">";
                                                        querableReturnType = action.ReturnType.GenericTypeArguments[0].FullName;
                                                    }

                                                }
                                                else if (action.ReturnType.Name.Contains("Queryable") || action.ReturnType.GenericTypeArguments[0].Name.Contains("Queryable"))
                                                {
                                                    if (action.ReturnType.GenericTypeArguments[0].Name.Contains("Queryable"))
                                                    {
                                                        returnType = "System.Linq.IQueryable<" + action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].FullName + ">";
                                                        querableReturnType = action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].FullName;
                                                    }
                                                    else
                                                    {
                                                        returnType = "System.Linq.IQueryable<" + action.ReturnType.GenericTypeArguments[0].FullName + ">";
                                                        querableReturnType = action.ReturnType.GenericTypeArguments[0].FullName;
                                                    }

                                                }
                                                else
                                                {
                                                    returnType = action.ReturnType.GenericTypeArguments[0].FullName;
                                                    querableReturnType = action.ReturnType.GenericTypeArguments[0].FullName;
                                                }

                                            }
                                            var type = "";
                                            foreach (var param in actionParams)
                                            {
                                                if (param.ParameterType.Name.Contains("Enumerable"))
                                                    type = "System.Collections.Generic.IEnumerable<" + param.ParameterType.GenericTypeArguments[0].FullName + ">";
                                                else if (param.ParameterType.Name.Contains("Queryable"))
                                                    type = "System.Linq.IQueryable<" + param.ParameterType.GenericTypeArguments[0].FullName + ">";
                                                else if (param.ParameterType.Name.Contains("Nullable"))
                                                    type = "System.Nullable<" + param.ParameterType.GenericTypeArguments[0].FullName + ">";
                                                else
                                                    type = param.ParameterType.FullName;
                                                if (paramHead == "")
                                                    paramHead += type + " " + param.Name;
                                                else
                                                    paramHead += ", " + type + " " + param.Name;
                                                if (paramBody != "")
                                                    paramBody += ", " + param.Name;
                                                else
                                                    paramBody = param.Name;
                                            }
                                            if (returnType != "")
                                                sbBottom.AppendLine("\t \t public async Task<" + returnType + "> " + action.Name + "(" + paramHead + ")");
                                            else
                                                sbBottom.AppendLine("\t \t public async Task " + action.Name + "(" + paramHead + ")");
                                            sbBottom.AppendLine("\t \t {");
                                            if (returnType == "System.Net.Http.HttpResponseMessage")
                                            {
                                                if (type != "")
                                                {
                                                    if (paramBody != "")
                                                        sbBottom.AppendLine("\t \t \t var result = await Client.Post<" + type + ">(Path + \"/" + action.Name + "\", " + paramBody + ");");
                                                    else
                                                        sbBottom.AppendLine("\t \t \t var result = await Client.Post<" + type + ">(Path + \"/" + action.Name + "\");");
                                                }
                                                else
                                                {
                                                    if (paramBody != "")
                                                        sbBottom.AppendLine("\t \t \t var result = await Client.Post(Path + \"/" + action.Name + "\", " + paramBody + ");");
                                                    else
                                                        sbBottom.AppendLine("\t \t \t var result = await Client.Post(Path + \"/" + action.Name + "\");");
                                                }

                                                sbBottom.AppendLine("\t \t \t return result;");
                                            }
                                            else if (returnType == "")
                                            {
                                                if (paramBody != "")
                                                    sbBottom.AppendLine("\t \t \t var result = await Client.Post(Path + \"/" + action.Name + "\", " + paramBody + ");");
                                                else
                                                    sbBottom.AppendLine("\t \t \t var result = await Client.Post(Path + \"/" + action.Name + "\");");
                                            }
                                            else
                                            {
                                                if (paramBody != "")
                                                    sbBottom.AppendLine("\t \t \t var result = await Client.Post<" + type + ", " + querableReturnType + ">(Path + \"/" + action.Name + "\", " + paramBody + ");");
                                                else if (type == "" && paramBody != "")
                                                    sbBottom.AppendLine("\t \t \t var result = await Client.Post<" + querableReturnType + ">(Path + \"/" + action.Name + "\", " + paramBody + ");");
                                                else if (type == "" && paramBody == "")
                                                    sbBottom.AppendLine("\t \t \t var result = await Client.Post<" + querableReturnType + ">(Path + \"/" + action.Name + "\");");
                                                else
                                                    sbBottom.AppendLine("\t \t \t var result = await Client.Post<" + type + ", " + querableReturnType + ">(Path + \"/" + action.Name + "\");");
                                            }
                                            if (returnType != "System.Net.Http.HttpResponseMessage" && action.ReturnType.Name.Contains("Enumerable"))
                                            {
                                                sbBottom.AppendLine("\t \t \t return result.ReturnList();");
                                            }
                                            else if (returnType != "System.Net.Http.HttpResponseMessage" && action.ReturnType.Name.Contains("IQueryable"))
                                            {
                                                sbBottom.AppendLine("\t \t \t return result.ReturnList();");
                                            }
                                            else if (action.ReturnType.GenericTypeArguments.Count() > 0 && action.ReturnType.GenericTypeArguments[0].Name.Contains("Enumerable"))
                                                sbBottom.AppendLine("\t \t \t return result.ReturnList();");
                                            else if (action.ReturnType.GenericTypeArguments.Count() > 0 && action.ReturnType.GenericTypeArguments[0].Name.Contains("Queryable"))
                                                sbBottom.AppendLine("\t \t \t return result.ReturnList();");
                                            else if (returnType == "")
                                                sbBottom.AppendLine("\t \t \t return;");
                                            else if (returnType != "System.Net.Http.HttpResponseMessage" && !returnType.Contains("Enumerable") && !returnType.Contains("IQueryable"))
                                            {
                                                sbBottom.AppendLine("\t \t \t return result.ReturnSingleItem();");
                                            }
                                            sbBottom.AppendLine("\t \t }");
                                        }
                                        #endregion
                                        #region HttpGet
                                        if (x.Any(y => y.TypeId.ToString() == "System.Web.Http.HttpGetAttribute"))
                                        {
                                            var actionParams = action.GetParameters().AsEnumerable();
                                            var upperbody = new StringBuilder();
                                            var paramBody = "";
                                            var paramHead = "";
                                            var returnType = "";
                                            var querableReturnType = "";

                                            if (action.ReturnType.GenericTypeArguments.Count() > 0)
                                            {
                                                if (action.ReturnType.Name.Contains("Enumerable") || action.ReturnType.GenericTypeArguments[0].Name.Contains("IEnumerable"))
                                                {
                                                    if (action.ReturnType.GenericTypeArguments[0].Name.Contains("IEnumerable"))
                                                    {
                                                        returnType = "System.Collections.Generic.IEnumerable<" + action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].FullName + ">";
                                                        querableReturnType = action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].FullName;
                                                    }
                                                    else
                                                    {
                                                        returnType = "System.Collections.Generic.IEnumerable<" + action.ReturnType.GenericTypeArguments[0].FullName + ">";
                                                        querableReturnType = action.ReturnType.GenericTypeArguments[0].FullName;
                                                    }
                                                }
                                                else if (action.ReturnType.Name.Contains("Queryable") || action.ReturnType.GenericTypeArguments[0].Name.Contains("Queryable"))
                                                {
                                                    if (action.ReturnType.GenericTypeArguments[0].Name.Contains("Queryable"))
                                                    {
                                                        returnType = "System.Linq.IQueryable<" + action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].FullName + ">";
                                                        querableReturnType = action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].FullName;
                                                    }
                                                    else
                                                    {
                                                        returnType = "System.Linq.IQueryable<" + action.ReturnType.GenericTypeArguments[0].FullName + ">";
                                                        querableReturnType = action.ReturnType.GenericTypeArguments[0].FullName;
                                                    }

                                                }
                                                else
                                                {
                                                    returnType = action.ReturnType.GenericTypeArguments[0].FullName;
                                                }

                                            }
                                            else
                                            {
                                                returnType = action.ReturnType.FullName;
                                            }

                                            foreach (var param in actionParams)
                                            {
                                                var type = "";
                                                if (param.ParameterType.Name.Contains("Enumerable"))
                                                {
                                                    type = "System.Collections.Generic.IEnumerable<" + param.ParameterType.GenericTypeArguments[0].FullName + ">";
                                                }
                                                else if (param.ParameterType.Name.Contains("Queryable"))
                                                {
                                                    type = "System.Linq.IQueryable<" + param.ParameterType.GenericTypeArguments[0].FullName + ">";
                                                }
                                                else if (param.ParameterType.Name.Contains("Nullable"))
                                                {
                                                    //type = "System.Nullable<" + param.ParameterType.GenericTypeArguments[0].FullName + ">";
                                                    type = param.ParameterType.GenericTypeArguments[0].FullName + "?";
                                                }
                                                else if(param.ParameterType == typeof(string))
                                                {
                                                    type = "string";
                                                }
                                                else
                                                {
                                                    type = param.ParameterType.FullName;
                                                }

                                                if (paramHead == "")
                                                {
                                                    paramHead += type + " " + param.Name;
                                                }
                                                else
                                                {
                                                    paramHead += ", " + type + " " + param.Name;
                                                }

                                                if (param.ParameterType.Name.Contains("Enumerable"))
                                                {
                                                    if (paramBody != "")
                                                    {
                                                        upperbody.AppendLine("\t \t \t var " + param.Name + "QueryString = string.Join(\"&\", " + param.Name +".Select(i => string.Format(\"{0}={1}\", \"" + param.Name + "\", System.Net.WebUtility.UrlEncode(i.ToString()))));");

                                                        //paramBody = paramBody.Substring(0, paramBody.Length - 5) + " + " + param.Name + "QueryString + \"&\"";
                                                        paramBody = paramBody + " + " + param.Name + "QueryString + \"&\"";

                                                    }
                                                    else
                                                    {
                                                        upperbody.AppendLine("\t \t \t var " + param.Name + "QueryString = string.Join(\"&\", " + param.Name + ".Select(i => string.Format(\"{0}={1}\", \"" + param.Name + "\", System.Net.WebUtility.UrlEncode(i.ToString()))));");

                                                        paramBody = "\" + " + param.Name + "QueryString + \"&\"";
                                                    }
                                                }
                                                else
                                                {
                                                    bool canNotBeNull = (param.ParameterType.IsPrimitive || param.ParameterType.IsValueType || param.ParameterType == typeof(Guid));

                                                    if (paramBody != "")
                                                    {
                                                        if (canNotBeNull)
                                                        {
                                                            paramBody = paramBody.Substring(0, paramBody.Length - 1) + "" + param.Name + "=\" + System.Net.WebUtility.UrlEncode(" + param.Name + ".ToString()) + \"&\"";
                                                        }
                                                        else
                                                        {
                                                            paramBody = paramBody.Substring(0, paramBody.Length - 1) + "" + param.Name + "=\" + (" + param.Name + " == null ? \"\" : System.Net.WebUtility.UrlEncode(" + param.Name + ".ToString())) + \"&\"";
                                                        }                                                        
                                                    }
                                                    else
                                                    {
                                                        if (canNotBeNull)
                                                        {
                                                            paramBody = param.Name + "=\" + System.Net.WebUtility.UrlEncode(" + param.Name + ".ToString()) + \"&\"";
                                                        }
                                                        else
                                                        {
                                                            paramBody = param.Name + "=\" + (" + param.Name + " == null ? \"\" : System.Net.WebUtility.UrlEncode(" + param.Name + ".ToString())) + \"&\"";
                                                        }
                                                        
                                                    }
                                                }

                                            }

                                            if (action.ReturnType.FullName.Contains("IQueryable") || action.ReturnType.FullName.Contains("IEnumerable"))
                                            {
                                                if (paramHead == "")
                                                {
                                                    sbBottom.AppendLine("\t \t public async Task<" + returnType + "> " + action.Name + "(string oDataQuery = null)");
                                                }
                                                else
                                                {
                                                    sbBottom.AppendLine("\t \t public async Task<" + returnType + "> " + action.Name + "(" + paramHead + ", string oDataQuery = null)");
                                                }

                                            }
                                            else if (returnType == "")
                                            {
                                                sbBottom.AppendLine("\t \t public async Task " + action.Name + "(" + paramHead + ")");
                                            }
                                            else
                                            {
                                                sbBottom.AppendLine("\t \t public async Task<" + returnType + "> " + action.Name + "(" + paramHead + ")");
                                            }

                                            sbBottom.AppendLine("\t \t {");

                                            if (returnType == "System.Net.Http.HttpResponseMessage")
                                            {
                                                sbBottom.AppendLine(upperbody.ToString());
                                                if (paramBody.Trim() == "")
                                                {
                                                    sbBottom.AppendLine("\t \t \t return await Client._Client.GetAsync(Client._Host + Path + \"/" + action.Name + "\");");
                                                }
                                                else
                                                {
                                                    sbBottom.AppendLine("\t \t \t return await Client._Client.GetAsync(Client._Host + Path + \"/" + action.Name + "?" + paramBody + ");");
                                                }
                                            }
                                            else if (returnType == "")
                                            {
                                                if (paramBody != "")
                                                {
                                                    sbBottom.AppendLine(upperbody.ToString());
                                                    sbBottom.AppendLine("\t \t \t var result = await Client.Get(Path + \"/" + action.Name + "?" + paramBody + ");");
                                                }
                                                else
                                                {
                                                    sbBottom.AppendLine(upperbody.ToString());
                                                    sbBottom.AppendLine("\t \t \t var result = await Client.Get(Path + \"/" + action.Name + "\");");
                                                }
                                            }
                                            else
                                            {
                                                if (returnType.Contains("Enumerable"))
                                                {
                                                    if (paramBody != "")
                                                    {
                                                        sbBottom.AppendLine(upperbody.ToString());
                                                        sbBottom.AppendLine("\t \t \t var result = await Client.Get<" + querableReturnType + ">(Path + \"/" + action.Name + "?" + paramBody + ", oDataQuery);");
                                                    }
                                                    else
                                                    {
                                                        sbBottom.AppendLine(upperbody.ToString());
                                                        sbBottom.AppendLine("\t \t \t var result = await Client.Get<" + querableReturnType + ">(Path + \"/" + action.Name + "\", oDataQuery);");
                                                    }
                                                }
                                                else if (returnType.Contains("Queryable"))
                                                {
                                                    if (paramBody != "")
                                                    {
                                                        sbBottom.AppendLine(upperbody.ToString());
                                                        sbBottom.AppendLine("\t \t \t var result = await Client.Get<" + querableReturnType + ">(Path + \"/" + action.Name + "?" + paramBody + ", oDataQuery);");
                                                    }
                                                    else
                                                    {
                                                        sbBottom.AppendLine(upperbody.ToString());
                                                        sbBottom.AppendLine("\t \t \t var result = await Client.Get<" + querableReturnType + ">(Path + \"/" + action.Name + "\", oDataQuery);");
                                                    }
                                                }
                                                else
                                                {
                                                    if (paramBody != "")
                                                    {
                                                        sbBottom.AppendLine(upperbody.ToString());
                                                        sbBottom.AppendLine("\t \t \t var result = await Client.Get<" + returnType + ">(Path + \"/" + action.Name + "?" + paramBody + ");");
                                                    }
                                                    else
                                                    {
                                                        sbBottom.AppendLine(upperbody.ToString());
                                                        sbBottom.AppendLine("\t \t \t var result = await Client.Get<" + returnType + ">(Path + \"/" + action.Name + "\");");
                                                    }
                                                }
                                            }


                                            if (returnType != "System.Net.Http.HttpResponseMessage" && action.ReturnType.Name.Contains("Enumerable"))
                                                sbBottom.AppendLine("\t \t \t return result.ReturnList();");
                                            else if (returnType != "System.Net.Http.HttpResponseMessage" && action.ReturnType.Name.Contains("IQueryable"))
                                                sbBottom.AppendLine("\t \t \t return result.ReturnList();");
                                            else if (action.ReturnType.GenericTypeArguments.Count() > 0 && action.ReturnType.GenericTypeArguments[0].Name.Contains("Enumerable"))
                                                sbBottom.AppendLine("\t \t \t return result.ReturnList();");
                                            else if (action.ReturnType.GenericTypeArguments.Count() > 0 && action.ReturnType.GenericTypeArguments[0].Name.Contains("Queryable"))
                                                sbBottom.AppendLine("\t \t \t return result.ReturnList();");
                                            else if (returnType != "System.Net.Http.HttpResponseMessage" && !action.ReturnType.Name.Contains("IQueryable") && !action.ReturnType.Name.Contains("Enumerable"))
                                                sbBottom.AppendLine("\t \t \t return result.ReturnSingleItem();");

                                            sbBottom.AppendLine("\t \t }");

                                        }
                                        #endregion
                                        #region HttpPut
                                        else if (x.Any(y => y.TypeId.ToString() == "System.Web.Http.HttpPutAttribute"))
                                        {
                                            var actionParams = action.GetParameters().AsEnumerable();
                                            var paramBody = "";
                                            var paramHead = "";
                                            var returnType = "";
                                            var querableReturnType = "";
                                            if (action.ReturnType.GenericTypeArguments.Count() > 0)
                                            {
                                                if (action.ReturnType.Name.Contains("Enumerable") || action.ReturnType.GenericTypeArguments[0].Name.Contains("IEnumerable"))
                                                {
                                                    if (action.ReturnType.GenericTypeArguments[0].Name.Contains("IEnumerable"))
                                                    {
                                                        returnType = "System.Collections.Generic.IEnumerable<" + action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].FullName + ">";
                                                        querableReturnType = action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].FullName;
                                                    }
                                                    else
                                                    {
                                                        returnType = "System.Collections.Generic.IEnumerable<" + action.ReturnType.GenericTypeArguments[0].FullName + ">";
                                                        querableReturnType = action.ReturnType.GenericTypeArguments[0].FullName;
                                                    }
                                                }
                                                else if (action.ReturnType.Name.Contains("Queryable") || action.ReturnType.GenericTypeArguments[0].Name.Contains("Queryable"))
                                                {
                                                    if (action.ReturnType.GenericTypeArguments[0].Name.Contains("Queryable"))
                                                    {
                                                        returnType = "System.Linq.IQueryable<" + action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].FullName + ">";
                                                        querableReturnType = action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].FullName;
                                                    }
                                                    else
                                                    {
                                                        returnType = "System.Linq.IQueryable<" + action.ReturnType.GenericTypeArguments[0].FullName + ">";
                                                        querableReturnType = action.ReturnType.GenericTypeArguments[0].FullName;
                                                    }

                                                }
                                                else
                                                {
                                                    if (action.ReturnType.GenericTypeArguments[0].FullName == "System.Net.Http.HttpResponseMessage")
                                                    {
                                                    }
                                                    returnType = action.ReturnType.GenericTypeArguments[0].FullName;
                                                    querableReturnType = action.ReturnType.GenericTypeArguments[0].FullName;
                                                }

                                            }
                                            var type = "";
                                            foreach (var param in actionParams)
                                            {
                                                //var type = "";
                                                if (param.ParameterType.Name.Contains("Enumerable"))
                                                {
                                                    type = "System.Collections.Generic.IEnumerable<" + param.ParameterType.GenericTypeArguments[0].FullName + ">";
                                                }
                                                else if (param.ParameterType.Name.Contains("Queryable"))
                                                {
                                                    type = "System.Linq.IQueryable<" + param.ParameterType.GenericTypeArguments[0].FullName + ">";
                                                }
                                                else if (param.ParameterType.Name.Contains("Nullable"))
                                                {
                                                    type = "System.Nullable<" + param.ParameterType.GenericTypeArguments[0].FullName + ">";
                                                }
                                                else
                                                {
                                                    type = param.ParameterType.FullName;
                                                }
                                                if (paramHead == "")
                                                {
                                                    paramHead += type + " " + param.Name;
                                                }
                                                else
                                                {
                                                    paramHead += ", " + type + " " + param.Name;
                                                }
                                                if (paramBody != "")
                                                {
                                                    paramBody += ", " + param.Name;
                                                }
                                                else
                                                {
                                                    paramBody = param.Name;
                                                }

                                            }

                                            sbBottom.AppendLine("\t \t public async Task<" + returnType + "> " + action.Name + "(" + paramHead + ")");
                                            sbBottom.AppendLine("\t \t {");
                                            if (returnType == "System.Net.Http.HttpResponseMessage")
                                            {
                                                if (paramBody != "")
                                                    sbBottom.AppendLine("\t \t \t var result = await Client.Put<" + type + ">(Path + \"/" + action.Name + "\", " + paramBody + ");");
                                                else
                                                    sbBottom.AppendLine("\t \t \t var result = await Client.Put<" + type + ">(Path + \"/" + action.Name + "\");");

                                                sbBottom.AppendLine("\t \t \t return result;");
                                            }
                                            else
                                            {
                                                if (paramBody != "")
                                                    sbBottom.AppendLine("\t \t \t var result = await Client.Put<" + returnType + ", " + querableReturnType + ">(Path + \"/" + action.Name + "\", " + paramBody + ");");
                                                else
                                                    sbBottom.AppendLine("\t \t \t var result = await Client.Put<" + returnType + ", " + querableReturnType + ">(Path + \"/" + action.Name + "\");");
                                            }
                                            if (returnType != "System.Net.Http.HttpResponseMessage" && action.ReturnType.Name.Contains("Enumerable"))
                                            {
                                                sbBottom.AppendLine("\t \t \t return result.ReturnList();");
                                            }
                                            else if (returnType != "System.Net.Http.HttpResponseMessage" && action.ReturnType.Name.Contains("IQueryable"))
                                            {
                                                sbBottom.AppendLine("\t \t \t return result.ReturnList();");
                                            }
                                            else if (returnType != "System.Net.Http.HttpResponseMessage" && !returnType.Contains("Enumerable") && !returnType.Contains("IQueryable"))
                                            {
                                                sbBottom.AppendLine("\t \t \t return result.ReturnSingleItem();");
                                            }
                                            sbBottom.AppendLine("\t \t }");
                                        }
                                        #endregion
                                        #region HttpDelete
                                        else if (x.Any(y => y.TypeId.ToString() == "System.Web.Http.HttpDeleteAttribute"))
                                        {
                                            var actionParams = action.GetParameters().AsEnumerable();
                                            var paramBody = "";
                                            var paramHead = "";
                                            var querableReturnType = "";
                                            foreach (var param in actionParams)
                                            {
                                                var type = "";
                                                if (param.ParameterType.Name.Contains("Enumerable"))
                                                {
                                                    type = "System.Collections.Generic.IEnumerable<" + param.ParameterType.GenericTypeArguments[0].FullName + ">";
                                                }
                                                else if (param.ParameterType.Name.Contains("Queryable"))
                                                {
                                                    type = "System.Linq.IQueryable<" + param.ParameterType.GenericTypeArguments[0].FullName + ">";
                                                }
                                                else if (param.ParameterType.Name.Contains("Nullable"))
                                                {
                                                    type = "System.Nullable<" + param.ParameterType.GenericTypeArguments[0].FullName + ">";
                                                }
                                                else
                                                {
                                                    type = param.ParameterType.FullName;
                                                }
                                                if (paramHead == "")
                                                {
                                                    paramHead += type + " " + param.Name;
                                                }
                                                else
                                                {
                                                    paramHead += ", " + type + " " + param.Name;
                                                }
                                                if (paramBody != "")
                                                {
                                                    paramBody += ", " + param.Name;
                                                }
                                                else
                                                {
                                                    paramBody = param.Name;
                                                }

                                            }
                                            sbBottom.AppendLine("\t \t public async Task " + action.Name + "(" + paramHead + ")");
                                            sbBottom.AppendLine("\t \t {");
                                            sbBottom.AppendLine("\t \t \t await Client.Delete(Path + \"/" + action.Name + "\", " + paramBody + ");");
                                            sbBottom.AppendLine("\t \t }");
                                        }
                                        #endregion
                                    }
                                }
                            }
                        }

                        sbBottom.AppendLine("\t }");
                    }
                }
                sbBase.AppendLine("\t }");
                
                sbBase.AppendLine(sbBottom.ToString());
                sbBase.AppendLine("}");
                outputFile.WriteLine(sbBase);
                outputFile.WriteLine("#pragma warning restore 1591");
            }
        }
    }
}
