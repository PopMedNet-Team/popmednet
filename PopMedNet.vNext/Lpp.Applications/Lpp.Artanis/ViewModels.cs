using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Artanis
{
    public class ViewModels
    {
        public static void GenTS(string path, string Config)
        {
            Configuration Json;
            using (StreamReader r = new StreamReader(Path.GetFullPath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Configs\\" + Config + ".json")))
            {
                string json = r.ReadToEnd();
                Json = JsonConvert.DeserializeObject<Configuration>(json);
            }
            Assembly assembly = Assembly.LoadFrom(path + "\\" + Json.DtoNamespace + "\\bin\\" + Json.DtoNamespace + ".dll");
            var types = assembly.GetTypes().Where(x => Attribute.IsDefined(x, typeof(DataContractAttribute)));
            string viewModelsFile = Path.GetFullPath(path + Json.OutputPath);
            var enums = types.Where(t => t.IsEnum);
            Dictionary<Type, Int32> typeByBaseCount = new Dictionary<Type, int>();
            foreach (var type in types.Where(t => !t.IsEnum).Distinct())
            {
                int count = 0;
                var baseType = type.BaseType;
                while (baseType != null)
                {
                    count = count + 1;
                    baseType = baseType.BaseType;
                }

                typeByBaseCount.Add(type, count);
            }
            types = typeByBaseCount.OrderBy(p => p.Value).Select(p => p.Key);
            using (StreamWriter outputFile = new StreamWriter(viewModelsFile, false))
            {
                outputFile.WriteLine("/// <reference path='../../node_modules/@types/knockout.mapping/index.d.ts' />");
                outputFile.WriteLine("/// <reference path='Lpp."+ Json.TSNamespace +".Interfaces.ts' />");

                outputFile.WriteLine("module " + Json.TSNamespace + ".ViewModels {");
                outputFile.WriteLine("\t export class ViewModel<D>{");
                outputFile.WriteLine("\t \t constructor() {");
                outputFile.WriteLine("\t \t }");
                outputFile.WriteLine("\t \t public update(obj: any) {");
                outputFile.WriteLine("\t \t \t for(var prop in obj) {");
                outputFile.WriteLine("\t \t \t \t this[prop](obj[prop]);");
                outputFile.WriteLine("\t \t \t }");
                outputFile.WriteLine("\t \t }");
                outputFile.WriteLine("\t }");
                outputFile.WriteLine("\t export class EntityDtoViewModel<T> extends ViewModel<T> {");
                outputFile.WriteLine("\t \t constructor(BaseDTO?: T)");
                outputFile.WriteLine("\t \t {");
                outputFile.WriteLine("\t \t \t  super();");
                outputFile.WriteLine("\t \t }");
                outputFile.WriteLine("\t \t  public toData(): " + Json.TSNamespace + ".Interfaces.IEntityDto {");
                outputFile.WriteLine("\t \t \t  return {");
                outputFile.WriteLine("\t \t \t };");
                outputFile.WriteLine("\t \t }");
                outputFile.WriteLine("\t }");
                outputFile.WriteLine("\t export class EntityDtoWithIDViewModel<T> extends EntityDtoViewModel<T> {");
                outputFile.WriteLine("\t \t public ID: KnockoutObservable<any>;");
                outputFile.WriteLine("\t \t public Timestamp: KnockoutObservable<any>;");
                outputFile.WriteLine("\t \t constructor(BaseDTO?: T)");
                outputFile.WriteLine("\t \t {");
                outputFile.WriteLine("\t \t \t super(BaseDTO);");
                outputFile.WriteLine("\t \t \t if (BaseDTO == null) {");
                outputFile.WriteLine("\t \t \t \t this.ID = ko.observable<any>();");
                outputFile.WriteLine("\t \t \t \t this.Timestamp = ko.observable<any>();");
                outputFile.WriteLine("\t \t \t }");
                outputFile.WriteLine("\t \t }");
                outputFile.WriteLine("\t \t  public toData(): " + Json.TSNamespace + ".Interfaces.IEntityDto {");
                outputFile.WriteLine("\t \t \t  return {");
                outputFile.WriteLine("\t \t \t \t ID: this.ID(),");
                outputFile.WriteLine("\t \t \t \t Timestamp: this.Timestamp(),");
                outputFile.WriteLine("\t \t \t };");
                outputFile.WriteLine("\t \t }");
                outputFile.WriteLine("\t }");
                StringBuilder sbBase = new StringBuilder();
                foreach (var type in types)
                {
                    StringBuilder classTop = new StringBuilder("");
                    StringBuilder classBottom = new StringBuilder("");
                    classBottom.AppendLine("\t \t public toData(): " + Json.TSNamespace + ".Interfaces.I" + type.Name + "{");
                    classBottom.AppendLine("\t \t \t  return {");
                    StringBuilder classMiddle1 = new StringBuilder("");
                    classMiddle1.AppendLine("\t \t constructor(" + type.Name + "?: "+ Json.TSNamespace +".Interfaces.I" + type.Name + ")");
                    classMiddle1.AppendLine("\t \t  {");
                    classMiddle1.AppendLine("\t \t \t  super();");
                    classMiddle1.AppendLine("\t \t \t if (" + type.Name + "== null) {");
                    StringBuilder classMiddle2 = new StringBuilder("");
                    classMiddle2.AppendLine("\t \t \t  }else{");
                    if (!type.IsEnum)
                    {

                        if (type.BaseType.Name != "Object")
                        {
                            if (type.BaseType.Name == "EntityDtoWithID" || type.BaseType.Name == "EntityDto")
                                sbBase.AppendLine("\t export class " + type.Name.Substring(type.Name.LastIndexOf(".") + 1).Replace("DTO", "ViewModel") + " extends " + type.BaseType.Name.Substring(type.Name.LastIndexOf(".") + 1).Replace("DTO", "") + "ViewModel<" + Json.TSNamespace + ".Interfaces.I" + type.Name + ">{");
                            else
                                sbBase.AppendLine("\t export class " + type.Name.Substring(type.Name.LastIndexOf(".") + 1).Replace("DTO", "ViewModel") + " extends " + type.BaseType.Name.Substring(type.Name.LastIndexOf(".") + 1).Replace("DTO", "ViewModel") + "{");
                        }
                        else
                            sbBase.AppendLine("\t export class " + type.Name.Substring(type.Name.LastIndexOf(".") + 1).Replace("DTO", "ViewModel") + " extends ViewModel<" + Json.TSNamespace + ".Interfaces.I" + type.Name + ">{");
                        var delcaredProps = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                        var props = type.GetProperties();
                        #region InnerStuff
                        foreach (var prop in props)
                        {
                            string returnType;
                            if (prop.PropertyType.Name.Contains("Nullable"))
                            {
                                if (prop.PropertyType.GenericTypeArguments[0].FullName.Contains(Json.EnumNamespace))
                                    returnType = Json.TSNamespace+".Enums." + prop.PropertyType.GenericTypeArguments[0].Name;
                                else
                                    returnType = ViewModelHelpers.ReturnTypeCheck(prop.PropertyType.GenericTypeArguments[0].FullName);
                            }
                            else if (prop.PropertyType.Name.Contains("IEnumerable") || prop.PropertyType.Name.Contains("List`1"))
                            {
                                var parent = prop.PropertyType.GenericTypeArguments[0];
                                if (parent.Name.Contains("IEnumerable") || parent.Name.Contains("IPropertyDefinition") || parent.Name.Contains("Guid") || parent.Name.Contains("Dictionary") || parent.Name.Contains("KeyValuePair"))
                                    returnType = "any";
                                else if (parent.Name.Contains("String"))
                                    returnType = "string[]";
                                else if (parent.FullName.Contains(Json.EnumNamespace))
                                    returnType = Json.TSNamespace + ".Enums." + parent.Name;
                                else if (parent.Name.Contains("Int"))
                                    returnType = "number[]";
                                else
                                    returnType = parent.Name;
                            }
                            else if (prop.PropertyType.FullName.Contains(Json.DtoNamespace))
                            {
                                if (prop.PropertyType.FullName.Contains(Json.EnumNamespace))
                                    returnType = Json.TSNamespace + ".Enums." + prop.PropertyType.Name;
                                else
                                    returnType = prop.PropertyType.Name;
                            }
                            else
                                returnType = ViewModelHelpers.ReturnTypeCheck(prop.PropertyType.FullName);
                            if (prop.PropertyType.FullName.Contains("System.Nullable"))
                            {
                                if (returnType == "string" || returnType == "number" || returnType == "boolean" || returnType == "any" || returnType == "Date" || returnType.Contains(Json.TSNamespace+".Enum"))
                                {
                                    classBottom.AppendLine("\t \t \t \t" + prop.Name + ": this." + prop.Name + "(),");
                                    classMiddle1.AppendLine("\t \t \t \t this." + prop.Name + " = ko.observable<any>();");
                                    classMiddle2.AppendLine("\t \t \t \t this." + prop.Name + " = ko.observable(" + type.Name + "." + prop.Name + ");");
                                }
                                else if (prop.PropertyType.Name.Contains("IEnumerable"))
                                {
                                    if (returnType == "string[]" || returnType == "number[]")
                                    {
                                        classBottom.AppendLine("\t \t \t \t" + prop.Name + ": this." + prop.Name + " == null ? null : this." + prop.Name + "().map((item) => {return item;}),");
                                        classMiddle1.AppendLine("\t \t \t \t this." + prop.Name + " = ko.observableArray<" + returnType.Replace("[]", "") + ">();");
                                        classMiddle2.AppendLine("\t \t \t \t this." + prop.Name + " = ko.observableArray<" + returnType.Substring(returnType.LastIndexOf(".") + 1).Replace("DTO", "ViewModel").Replace("[]", "") + ">(" + type.Name + "." + prop.Name + " == null ? null : " + type.Name + prop.Name + ".map((item) => {return item;}));");
                                    }
                                    else
                                    {
                                        classBottom.AppendLine("\t \t \t \t" + prop.Name + ": this." + prop.Name + " == null ? null : this." + prop.Name + "().map((item) => {return item.toData();}),");
                                        classMiddle1.AppendLine("\t \t \t \t this." + prop.Name + " = ko.observableArray<" + returnType.Replace("[]", "") + ">();");
                                        classMiddle2.AppendLine("\t \t \t \t this." + prop.Name + " = ko.observableArray<" + returnType.Substring(returnType.LastIndexOf(".") + 1).Replace("DTO", "ViewModel").Replace("[]", "") + ">(" + type.Name + "." + prop.Name + " == null ? null : " + type.Name + prop.Name + ".map((item) => {return new " + returnType.Substring(returnType.LastIndexOf(".") + 1).Replace("DTO", "ViewModel") + "(item);}));");
                                    }

                                }
                                else if (prop.PropertyType.FullName.Contains(Json.TSNamespace+".DTO"))
                                {
                                    classBottom.AppendLine("\t \t \t \t" + prop.Name + ": this." + prop.Name + "(),");
                                    classMiddle1.AppendLine("\t \t \t \t this." + prop.Name + " = new " + returnType.Substring(returnType.LastIndexOf(".") + 1).Replace("DTO", "ViewModel") + "();");
                                    classMiddle2.AppendLine("\t \t \t \t this." + prop.Name + " = new " + returnType.Substring(returnType.LastIndexOf(".") + 1).Replace("DTO", "ViewModel") + "(" + type.Name + "." + prop.Name + ");");
                                }
                            }
                            else
                            {
                                if (returnType == "string" || returnType == "number" || returnType == "boolean" || returnType == "any" || returnType == "Date" || returnType.Contains(Json.TSNamespace+".Enum"))
                                {
                                    if (prop.PropertyType.Name.Contains("IEnumerable"))
                                    {
                                        classBottom.AppendLine("\t \t \t \t" + prop.Name + ": this." + prop.Name + "(),");
                                        classMiddle1.AppendLine("\t \t \t \t this." + prop.Name + " = ko.observableArray<any>();");
                                        classMiddle2.AppendLine("\t \t \t \t this." + prop.Name + " = ko.observableArray<" + returnType + ">(" + type.Name + "." + prop.Name + " == null ? null : " + type.Name + "." + prop.Name + ".map((item) => {return item;}));");

                                    }
                                    else if (prop.PropertyType.Name.Contains("Dictionary") || prop.PropertyType.Name.Contains("KeyValuePair"))
                                    {
                                        classBottom.AppendLine("\t \t \t \t" + prop.Name + ": ko.mapping.toJS(this." + prop.Name + "()),");
                                        classMiddle1.AppendLine("\t \t \t \t this." + prop.Name + " = ko.observable<any>({});");
                                        classMiddle2.AppendLine("\t \t \t \t this." + prop.Name + " = ko.observable(" + type.Name + "." + prop.Name + ");");
                                    }
                                    else
                                    {
                                        classBottom.AppendLine("\t \t \t \t" + prop.Name + ": this." + prop.Name + "(),");
                                        classMiddle1.AppendLine("\t \t \t \t this." + prop.Name + " = ko.observable<any>();");
                                        classMiddle2.AppendLine("\t \t \t \t this." + prop.Name + " = ko.observable(" + type.Name + "." + prop.Name + ");");
                                    }


                                }
                                else if (prop.PropertyType.Name.Contains("IEnumerable"))
                                {
                                    if (returnType == "string[]" || returnType == "number[]")
                                    {
                                        classBottom.AppendLine("\t \t \t \t" + prop.Name + ": this." + prop.Name + " == null ? null : this." + prop.Name + "().map((item) => {return item;}),");
                                        classMiddle1.AppendLine("\t \t \t \t this." + prop.Name + " = ko.observableArray<" + returnType.Replace("[]", "") + ">();");
                                        classMiddle2.AppendLine("\t \t \t \t this." + prop.Name + " = ko.observableArray<" + returnType.Substring(returnType.LastIndexOf(".") + 1).Replace("DTO", "ViewModel").Replace("[]", "") + ">(" + type.Name + "." + prop.Name + " == null ? null : " + type.Name + "." + prop.Name + ".map((item) => {return item;}));");
                                    }
                                    else
                                    {
                                        classBottom.AppendLine("\t \t \t \t" + prop.Name + ": this." + prop.Name + " == null ? null : this." + prop.Name + "().map((item) => {return item.toData();}),");
                                        classMiddle1.AppendLine("\t \t \t \t this." + prop.Name + " = ko.observableArray<" + returnType.Substring(returnType.LastIndexOf(".") + 1).Replace("DTO", "ViewModel").Replace("[]", "") + ">();");
                                        classMiddle2.AppendLine("\t \t \t \t this." + prop.Name + " = ko.observableArray<" + returnType.Substring(returnType.LastIndexOf(".") + 1).Replace("DTO", "ViewModel").Replace("[]", "") + ">(" + type.Name + "." + prop.Name + " == null ? null : " + type.Name + "." + prop.Name + ".map((item) => {return new " + returnType.Substring(returnType.LastIndexOf(".") + 1).Replace("DTO", "ViewModel") + "(item);}));");
                                    }
                                }
                                else if (prop.PropertyType.Name.Contains("List"))
                                {
                                    classBottom.AppendLine("\t \t \t \t" + prop.Name + ": this." + prop.Name + " == null ? null : this." + prop.Name + "().map((item) => {return item.toData();}),");
                                    classMiddle1.AppendLine("\t \t \t \t this." + prop.Name + " = ko.observableArray<" + returnType.Substring(returnType.LastIndexOf(".") + 1).Replace("DTO", "ViewModel").Replace("[]", "") + ">();");
                                    classMiddle2.AppendLine("\t \t \t \t this." + prop.Name + " = ko.observableArray<" + returnType.Substring(returnType.LastIndexOf(".") + 1).Replace("DTO", "ViewModel").Replace("[]", "") + ">(" + type.Name + "." + prop.Name + " == null ? null : " + type.Name + "." + prop.Name + ".map((item) => {return new " + returnType.Substring(returnType.LastIndexOf(".") + 1).Replace("DTO", "ViewModel") + "(item);}));");

                                }
                                else if (prop.PropertyType.FullName.Contains(Json.DtoNamespace))
                                {
                                    if (prop.PropertyType.Name.Contains("List`1"))
                                    {
                                        classBottom.AppendLine("\t \t \t \t" + prop.Name + ": this." + prop.Name + " == null ? null : this." + prop.Name + "().map((item) => {return item.toData();}),");
                                        classMiddle1.AppendLine("\t \t \t \t this." + prop.Name + " = ko.observableArray<any>();");
                                        classMiddle2.AppendLine("\t \t \t \t this." + prop.Name + " = ko.observableArray<" + returnType.Substring(returnType.LastIndexOf(".") + 1).Replace("DTO", "ViewModel") + ">(" + type.Name + "." + prop.Name + " == null ? null : " + type.Name + "." + prop.Name + ".map((item) => {return new " + returnType.Substring(returnType.LastIndexOf(".") + 1).Replace("DTO", "ViewModel") + "(item);}));");

                                    }
                                    else
                                    {
                                        classBottom.AppendLine("\t \t \t \t" + prop.Name + ": this." + prop.Name + ".toData(),");
                                        classMiddle1.AppendLine("\t \t \t \t this." + prop.Name + " = new " + returnType.Substring(returnType.LastIndexOf(".") + 1).Replace("DTO", "ViewModel") + "();");
                                        classMiddle2.AppendLine("\t \t \t \t this." + prop.Name + " = new " + returnType.Substring(returnType.LastIndexOf(".") + 1).Replace("DTO", "ViewModel") + "(" + type.Name + "." + prop.Name + ");");
                                    }

                                }
                                else
                                {

                                }
                            }

                        }
                        #endregion
                        #region InnerStuffDeclaredOnly
                        foreach (var prop in delcaredProps)
                        {
                            string returnType;
                            if (prop.PropertyType.Name.Contains("Nullable"))
                            {
                                if (prop.PropertyType.GenericTypeArguments[0].FullName.Contains(Json.EnumNamespace))
                                    returnType = Json.TSNamespace +".Enums." + prop.PropertyType.GenericTypeArguments[0].Name;
                                else
                                    returnType = ViewModelHelpers.ReturnTypeCheck(prop.PropertyType.GenericTypeArguments[0].FullName);
                            }
                            else if (prop.PropertyType.Name.Contains("IEnumerable") || prop.PropertyType.Name.Contains("List`1"))
                            {
                                var parent = prop.PropertyType.GenericTypeArguments[0];
                                if (parent.Name.Contains("IEnumerable") || parent.Name.Contains("IPropertyDefinition") || parent.Name.Contains("Guid") || parent.Name.Contains("Dictionary") || parent.Name.Contains("KeyValuePair"))
                                    returnType = "any";
                                else if (parent.Name.Contains("String"))
                                    returnType = "string[]";
                                else if (parent.FullName.Contains(Json.EnumNamespace))
                                    returnType = Json.TSNamespace+ ".Enums." + parent.Name;
                                else if (parent.Name.Contains("Int"))
                                    returnType = "number[]";
                                else
                                    returnType = parent.Name;
                            }
                            else if (prop.PropertyType.FullName.Contains(Json.DtoNamespace))
                            {
                                if (prop.PropertyType.FullName.Contains(Json.EnumNamespace))
                                    returnType = Json.TSNamespace + ".Enums." + prop.PropertyType.Name;
                                else
                                    returnType = prop.PropertyType.Name;
                            }
                            else
                                returnType = ViewModelHelpers.ReturnTypeCheck(prop.PropertyType.FullName);
                            if (prop.PropertyType.FullName.Contains("System.Nullable"))
                            {
                                if (returnType == "string" || returnType == "number" || returnType == "boolean" || returnType == "any" || returnType == "Date" || returnType.Contains(Json.TSNamespace + ".Enums"))
                                    classTop.AppendLine("\t \t public " + prop.Name + ": KnockoutObservable<" + returnType + ">;");
                                else if (prop.PropertyType.Name.Contains("IEnumerable"))
                                    classTop.AppendLine("\t \t public " + prop.Name + ": KnockoutObservableArray<" + returnType.Substring(returnType.LastIndexOf(".") + 1).Replace("DTO", "ViewModel").Replace("[]", "") + ">;");
                                else if (prop.PropertyType.Name.Contains("List"))
                                    classTop.AppendLine("\t \t public " + prop.Name + ": KnockoutObservableArray<" + returnType.Substring(returnType.LastIndexOf(".") + 1).Replace("DTO", "ViewModel").Replace("[]", "") + ">;");
                                else if (prop.PropertyType.FullName.Contains(Json.TSNamespace + ".DTO"))
                                {
                                    if (prop.PropertyType.Name.Contains("List`1"))
                                        classTop.AppendLine("\t \t public " + prop.Name + ": KnockoutObservableArray<" + returnType.Substring(returnType.LastIndexOf(".") + 1).Replace("DTO", "ViewModel") + ">;");
                                    else
                                        classTop.AppendLine("\t \t public " + prop.Name + ": " + returnType.Substring(returnType.LastIndexOf(".") + 1).Replace("DTO", "ViewModel") + ";");
                                }
                            }
                            else
                            {
                                if (returnType == "string" || returnType == "number" || returnType == "boolean" || returnType == "any" || returnType == "Date" || returnType.Contains(Json.TSNamespace + ".Enums"))
                                {
                                    if (prop.PropertyType.Name.Contains("IEnumerable"))
                                        classTop.AppendLine("\t \t public " + prop.Name + ": KnockoutObservableArray<" + returnType + ">;");
                                    else
                                        classTop.AppendLine("\t \t public " + prop.Name + ": KnockoutObservable<" + returnType + ">;");
                                }
                                else if (prop.PropertyType.Name.Contains("IEnumerable"))
                                    classTop.AppendLine("\t \t public " + prop.Name + ": KnockoutObservableArray<" + returnType.Substring(returnType.LastIndexOf(".") + 1).Replace("DTO", "ViewModel").Replace("[]", "") + ">;");
                                else if (prop.PropertyType.FullName.Contains(Json.DtoNamespace))
                                {
                                    if (prop.PropertyType.Name.Contains("List`1"))
                                        classTop.AppendLine("\t \t public " + prop.Name + ": KnockoutObservableArray<" + returnType.Substring(returnType.LastIndexOf(".") + 1).Replace("DTO", "ViewModel") + ">;");
                                    else
                                        classTop.AppendLine("\t \t public " + prop.Name + ": " + returnType.Substring(returnType.LastIndexOf(".") + 1).Replace("DTO", "ViewModel") + ";");
                                }
                            }

                        }
                        #endregion
                        classMiddle2.AppendLine("\t \t \t }");
                        classMiddle2.AppendLine("\t \t }");
                        classMiddle1.AppendLine(classMiddle2.ToString());
                        classBottom.AppendLine("\t \t \t  };");
                        classBottom.AppendLine("\t \t  }");
                        classMiddle1.AppendLine(classBottom.ToString());
                        classTop.AppendLine(classMiddle1.ToString());
                        sbBase.AppendLine(classTop.ToString());
                        sbBase.AppendLine("\t }");
                    }
                }
                sbBase.AppendLine("}");
                outputFile.WriteLine(sbBase);
            }
        }
    }
}
