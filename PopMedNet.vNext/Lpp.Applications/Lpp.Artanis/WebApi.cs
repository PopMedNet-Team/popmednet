using Lpp.Objects;
using Microsoft.AspNet.SignalR;
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
    public class WebApi
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
                outputFile.WriteLine("/// <reference path='../../node_modules/@types/jquery/index.d.ts' />");
                outputFile.WriteLine("/// <reference path='Lpp."+ Json.TSNamespace + ".ViewModels.ts' />");
                outputFile.WriteLine("declare var ServiceUrl: string;");
                if(Json.TSNamespace == "Dns")
                    outputFile.WriteLine("declare var User");
                outputFile.WriteLine("module "+ Json.TSNamespace + ".WebApi {");
                foreach (var controller in controllers)
                {
                    if (!controller.CustomAttributes.Any(x => x.AttributeType.Name == "ClientEntityIgnoreAttribute"))
                    {
                        var actions = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                        outputFile.WriteLine("\t export class " + controller.Name.Replace("Controller", "") + "{");
                        bool hasGet = false;
                        bool hasList = false;
                        bool hasInsert = false;
                        bool hasUpdate = false;
                        bool hasInsertOrUpdate = false;
                        bool hasDelete = false;
                        bool hasPermissions = false;
                        foreach (var action in actions)
                        {
                            var x = action.GetCustomAttributes().Where(attr => attr.GetType() == typeof(HttpGetAttribute) || attr.GetType() == typeof(HttpPutAttribute) || attr.GetType() == typeof(HttpPostAttribute) || attr.GetType() == typeof(HttpDeleteAttribute)).ToArray();
                            if (!action.GetCustomAttributes().Any(z => z.GetType() == typeof(ClientEntityIgnoreAttribute)))
                            {
                                if (x.Count() > 0)
                                {
                                    string[] excludeNames = new string[] { "Insert", "Update", "InsertOrUpdate", "Delete", "Get", "List" };
                                    #region HttpPost
                                    if (x.Any(y => y.TypeId.ToString() == "System.Web.Http.HttpPostAttribute"))
                                    {
                                        if (!hasUpdate || !hasInsert || !hasInsertOrUpdate)
                                        {
                                            if (!hasUpdate || !hasInsert || !hasInsertOrUpdate)
                                            {
                                                var returnType = "";
                                                if (action.ReturnType.FullName.Contains("IQueryable"))
                                                {
                                                    if (action.ReturnType.GenericTypeArguments[0].Name.Contains("Queryable"))
                                                        returnType = action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].Name + "[]";
                                                    else if (action.ReturnType.GenericTypeArguments[0].Name == "HttpResponseMessage")
                                                        returnType = "any[]";
                                                    else
                                                        returnType = action.ReturnType.GenericTypeArguments[0].Name + "[]";
                                                }
                                                else
                                                {
                                                    if (action.ReturnType.Name == "Task" || action.ReturnType.Name == "HttpResponseMessage" || action.ReturnType.GenericTypeArguments[0].Name == "HttpResponseMessage")
                                                        returnType = "any[]";
                                                    else if (action.ReturnType.GenericTypeArguments[0].Name.Contains("Enumerable"))
                                                        returnType = action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].Name + "[]";
                                                    else
                                                        returnType = action.ReturnType.GenericTypeArguments[0].Name + "[]";
                                                }
                                                if (action.Name == "Insert")
                                                    hasInsert = true;
                                                else if (action.Name == "Update")
                                                    hasUpdate = true;
                                                else if (action.Name == "InsertOrUpdate")
                                                    hasInsertOrUpdate = true;
                                                var y = action.GetParameters().AsEnumerable();
                                                var returnString = ApiHelpers.MakePost(controller.Name.Replace("Controller", ""), action.Name, y, returnType, Json.TSNamespace + ".Interfaces", false);
                                                outputFile.WriteLine(returnString.ToString());
                                            }
                                        }
                                        else
                                        {
                                            var returnType = "";
                                            if (action.ReturnType.FullName.Contains("IQueryable"))
                                            {
                                                if (action.ReturnType.GenericTypeArguments[0].Name.Contains("Queryable"))
                                                    returnType = action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].Name + "[]";
                                                else if (action.ReturnType.GenericTypeArguments[0].Name == "HttpResponseMessage")
                                                    returnType = "any[]";
                                                else
                                                    returnType = action.ReturnType.GenericTypeArguments[0].Name + "[]";
                                            }
                                            else
                                            {
                                                if (action.ReturnType.Name == "Task" || action.ReturnType.Name == "HttpResponseMessage" || action.ReturnType.GenericTypeArguments[0].Name == "HttpResponseMessage")
                                                    returnType = "any[]";
                                                else if (action.ReturnType.GenericTypeArguments[0].Name.Contains("Enumerable"))
                                                    returnType = action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].Name + "[]";
                                                else
                                                    returnType = action.ReturnType.GenericTypeArguments[0].Name + "[]";
                                            }
                                            var y = action.GetParameters().AsEnumerable();
                                            var returnString = ApiHelpers.MakePost(controller.Name.Replace("Controller", ""), action.Name, y, returnType, Json.TSNamespace + ".Interfaces", false);
                                            outputFile.WriteLine(returnString.ToString());
                                        }
                                    }
                                    #endregion
                                    #region HttpGet
                                    if (x.Any(y => y.TypeId.ToString() == "System.Web.Http.HttpGetAttribute"))
                                    {
                                        if (excludeNames.Contains(action.Name))
                                        {
                                            if (!hasGet || !hasPermissions || !hasList)
                                            {
                                                bool isOdata = false;
                                                var returnType = "";
                                                if (action.ReturnType.FullName.Contains("IQueryable") || action.ReturnType.FullName.Contains("IEnumerable") && !action.ReturnType.FullName.Contains("KeyValuePair"))
                                                {
                                                    if (action.ReturnType.GenericTypeArguments[0].Name.Contains("Queryable") || action.ReturnType.GenericTypeArguments[0].Name.Contains("Enumerable"))
                                                        returnType = action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].Name + "[]";
                                                    else
                                                        returnType = action.ReturnType.GenericTypeArguments[0].Name + "[]";
                                                    isOdata = true;

                                                }
                                                else if (action.Name == "GetPermissions")
                                                {
                                                    returnType = "any[]";
                                                    isOdata = true;
                                                }
                                                else
                                                {
                                                    if (action.ReturnType.GenericTypeArguments.Count() > 0 && action.ReturnType.GenericTypeArguments[0].Name == "HttpResponseMessage" && action.ReturnType.GenericTypeArguments[0].Name.Contains("KeyValuePair"))
                                                        returnType = "any[]";
                                                    else if (action.ReturnType.GenericTypeArguments.Count() > 0 && action.ReturnType.GenericTypeArguments[0].Name.Contains("Enumerable"))
                                                        returnType = action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].Name + "[]";
                                                    else if (action.ReturnType.Name.Contains("DTO"))
                                                        returnType = action.ReturnType.Name + "[]";
                                                    else if (action.ReturnType.Name == "Task" && action.Name == "Delete")
                                                        returnType = "any[]";
                                                    else if (action.ReturnType.Name == "HttpResponseMessage")
                                                        returnType = "any[]";
                                                    else
                                                        returnType = action.ReturnType.GenericTypeArguments[0].Name + "[]";
                                                }

                                                var y = action.GetParameters().AsEnumerable();
                                                var returnString = ApiHelpers.MakeGet(controller.Name.Replace("Controller", ""), action.Name, y, returnType, Json.TSNamespace + ".Interfaces", isOdata);
                                                outputFile.WriteLine(returnString.ToString());
                                                if (action.Name == "Get")
                                                    hasGet = true;
                                                else if (action.Name == "GetPermissions")
                                                    hasPermissions = true;
                                                else if (action.Name == "List")
                                                    hasList = true;
                                            }
                                        }
                                        else
                                        {
                                            bool isOdata = false;
                                            var returnType = "";
                                            if (action.ReturnType.FullName.Contains("IQueryable") || action.ReturnType.FullName.Contains("IEnumerable"))
                                            {
                                                if (action.ReturnType.GenericTypeArguments[0].Name.Contains("Queryable") || action.ReturnType.GenericTypeArguments[0].Name.Contains("Enumerable"))
                                                    returnType = action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].Name + "[]";
                                                else
                                                    returnType = action.ReturnType.GenericTypeArguments[0].Name + "[]";
                                                isOdata = true;

                                            }
                                            else if (action.Name == "GetPermissions")
                                            {
                                                returnType = "any[]";
                                                isOdata = true;
                                            }
                                            else
                                            {
                                                if (action.ReturnType.GenericTypeArguments.Count() > 0 && action.ReturnType.GenericTypeArguments[0].Name == "HttpResponseMessage")
                                                    returnType = "any[]";
                                                else if (action.ReturnType.GenericTypeArguments.Count() > 0 && action.ReturnType.GenericTypeArguments[0].Name.Contains("Enumerable"))
                                                    returnType = action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].Name + "[]";
                                                else if (action.ReturnType.Name.Contains("DTO"))
                                                    returnType = action.ReturnType.Name + "[]";
                                                else if (action.ReturnType.Name == "HttpResponseMessage")
                                                    returnType = "any[]";
                                                else
                                                    returnType = action.ReturnType.GenericTypeArguments[0].Name + "[]";
                                            }

                                            var y = action.GetParameters().AsEnumerable();
                                            var returnString = ApiHelpers.MakeGet(controller.Name.Replace("Controller", ""), action.Name, y, returnType, Json.TSNamespace + ".Interfaces", isOdata);
                                            outputFile.WriteLine(returnString.ToString());
                                        }
                                    }
                                    #endregion
                                    #region HttpPut
                                    else if (x.Any(y => y.TypeId.ToString() == "System.Web.Http.HttpPutAttribute"))
                                    {
                                        if (excludeNames.Contains(action.Name))
                                        {
                                            if (!hasInsert || !hasInsertOrUpdate || !hasUpdate)
                                            {
                                                var returnType = "";
                                                if (action.ReturnType.FullName.Contains("IQueryable"))
                                                {
                                                    if (action.ReturnType.GenericTypeArguments[0].Name.Contains("Queryable"))
                                                        returnType = action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].Name + "[]";
                                                    else if (action.ReturnType.GenericTypeArguments[0].Name == "HttpResponseMessage")
                                                        returnType = "any[]";
                                                    else
                                                        returnType = action.ReturnType.GenericTypeArguments[0].Name + "[]";
                                                }
                                                else
                                                {
                                                    if (action.ReturnType.Name == "Task" || action.ReturnType.Name == "HttpResponseMessage" || action.ReturnType.GenericTypeArguments[0].Name == "HttpResponseMessage")
                                                        returnType = "any[]";
                                                    else if (action.ReturnType.GenericTypeArguments[0].Name.Contains("Enumerable"))
                                                        returnType = action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].Name + "[]";
                                                    else
                                                        returnType = action.ReturnType.GenericTypeArguments[0].Name + "[]";
                                                }

                                                if (action.Name == "Insert")
                                                    hasInsert = true;
                                                else if (action.Name == "Update")
                                                    hasUpdate = true;
                                                else if (action.Name == "InsertOrUpdate")
                                                    hasInsertOrUpdate = true;
                                                var y = action.GetParameters().AsEnumerable();
                                                var returnString = ApiHelpers.MakePut(controller.Name.Replace("Controller", ""), action.Name, y, returnType, Json.TSNamespace + ".Interfaces", false);
                                                outputFile.WriteLine(returnString.ToString());
                                            }
                                        }
                                        else
                                        {
                                            var returnType = "";
                                            if (action.ReturnType.FullName.Contains("IQueryable"))
                                            {
                                                if (action.ReturnType.GenericTypeArguments[0].Name.Contains("Queryable"))
                                                    returnType = action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].Name + "[]";
                                                else if (action.ReturnType.GenericTypeArguments[0].Name == "HttpResponseMessage")
                                                    returnType = "any[]";
                                                else
                                                    returnType = action.ReturnType.GenericTypeArguments[0].Name + "[]";
                                            }
                                            else
                                            {
                                                if (action.ReturnType.Name == "Task" || action.ReturnType.Name == "HttpResponseMessage" || action.ReturnType.GenericTypeArguments[0].Name == "HttpResponseMessage")
                                                    returnType = "any[]";
                                                else if (action.ReturnType.GenericTypeArguments[0].Name.Contains("Enumerable"))
                                                    returnType = action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].Name + "[]";
                                                else
                                                    returnType = action.ReturnType.GenericTypeArguments[0].Name + "[]";
                                            }
                                            var y = action.GetParameters().AsEnumerable();
                                            var returnString = ApiHelpers.MakePut(controller.Name.Replace("Controller", ""), action.Name, y, returnType, Json.TSNamespace + ".Interfaces", false);
                                            outputFile.WriteLine(returnString.ToString());
                                        }
                                    }
                                    #endregion
                                    #region HttpDelete
                                    else if (x.Any(y => y.TypeId.ToString() == "System.Web.Http.HttpDeleteAttribute"))
                                    {
                                        if (excludeNames.Contains(action.Name))
                                        {
                                            if (!hasDelete)
                                            {
                                                var returnType = "";
                                                if (action.ReturnType.FullName.Contains("IQueryable"))
                                                {
                                                    if (action.ReturnType.GenericTypeArguments[0].Name.Contains("Queryable"))
                                                        returnType = action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].Name + "[]";
                                                    else if (action.ReturnType.GenericTypeArguments[0].Name == "HttpResponseMessage")
                                                        returnType = "any[]";
                                                    else
                                                        returnType = action.ReturnType.GenericTypeArguments[0].Name + "[]";
                                                }
                                                else
                                                {
                                                    if (action.ReturnType.Name == "Task" || action.ReturnType.Name == "HttpResponseMessage" || action.ReturnType.GenericTypeArguments[0].Name == "HttpResponseMessage")
                                                        returnType = "any[]";
                                                    else if (action.ReturnType.GenericTypeArguments[0].Name.Contains("Enumerable"))
                                                        returnType = action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].Name + "[]";
                                                    else
                                                        returnType = action.ReturnType.GenericTypeArguments[0].Name + "[]";
                                                }

                                                if (action.Name == "Delete")
                                                {
                                                    hasDelete = true;
                                                }

                                                var y = action.GetParameters().AsEnumerable();
                                                var returnString = ApiHelpers.MakeDelete(controller.Name.Replace("Controller", ""), action.Name, y, returnType, Json.TSNamespace + ".Interfaces", false);
                                                outputFile.WriteLine(returnString.ToString());
                                            }
                                        }
                                        else
                                        {
                                            var returnType = "";
                                            if (action.ReturnType.FullName.Contains("IQueryable"))
                                            {
                                                if (action.ReturnType.GenericTypeArguments[0].Name.Contains("Queryable"))
                                                    returnType = action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].Name + "[]";
                                                else if (action.ReturnType.GenericTypeArguments[0].Name == "HttpResponseMessage")
                                                    returnType = "any[]";
                                                else
                                                    returnType = action.ReturnType.GenericTypeArguments[0].Name + "[]";
                                            }
                                            else
                                            {
                                                if (action.ReturnType.Name == "Task" || action.ReturnType.Name == "HttpResponseMessage" || action.ReturnType.GenericTypeArguments[0].Name == "HttpResponseMessage")
                                                    returnType = "any[]";
                                                else if (action.ReturnType.GenericTypeArguments[0].Name.Contains("Enumerable"))
                                                    returnType = action.ReturnType.GenericTypeArguments[0].GenericTypeArguments[0].Name + "[]";
                                                else
                                                    returnType = action.ReturnType.GenericTypeArguments[0].Name + "[]";
                                            }
                                            var y = action.GetParameters().AsEnumerable();
                                            var returnString = ApiHelpers.MakeDelete(controller.Name.Replace("Controller", ""), action.Name, y, returnType, Json.TSNamespace + ".Interfaces", false);
                                            outputFile.WriteLine(returnString.ToString());
                                        }
                                    }
                                    #endregion
                                }
                            }

                        }
                        outputFile.WriteLine("\t }");
                    }

                }
                var hubs = assembly.GetTypes().Where(type => typeof(Hub).IsAssignableFrom(type) && !type.FullName.Contains("ClientEntityIgnore"));
                outputFile.WriteLine(ApiHelpers.HelperClass().ToString());
                if (Json.TSNamespace == "Dns")
                {
                    outputFile.WriteLine(ApiHelpers.SignalRClass().ToString());
                    foreach (var hub in hubs)
                    {
                        outputFile.WriteLine("\t export class " + hub.Name + "{");
                        outputFile.WriteLine(@"        static _proxy: SignalR.Hub.Proxy = null;
        public static Proxy(): SignalR.Hub.Proxy {
            if (this._proxy == null)
                this._proxy = SignalRConnection().createHubProxy('RequestsHub');

            return this._proxy;
            }


        public static NotifyCrud(NotifyFunction: (data: Dns.Interfaces.INotificationCrudDTO) => void) {
			this.Proxy().on('NotifyCrud', NotifyFunction);
        }");
                        if (!hub.CustomAttributes.Any(x => x.AttributeType.Name == "ClientEntityIgnoreAttribute"))
                        {
                            //var actions = hub.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                            var actions = hub.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                            foreach (var action in actions)
                            {
                                if (action.CustomAttributes.Any(x => x.AttributeType.Name == "BroadcastEndPoint"))
                                {
                                    var returnType = action.ReturnType.Name;
                                    if (returnType == "String")
                                        returnType = "string";
                                    outputFile.WriteLine(@"        public static " + action.Name + @"(On" + action.Name + @": (" + returnType + @") => void) {
			this.Proxy().on('" + action.Name + @"', On" + action.Name + @");
		}
                        ");
                                }
                            }
                        }
                        outputFile.WriteLine("}");
                    } 
                }
                outputFile.WriteLine("}");
            }
        }
    }
}
