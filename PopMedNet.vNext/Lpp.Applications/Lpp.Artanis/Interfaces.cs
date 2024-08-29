using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Artanis
{
    public class Interfaces
    {
        public static void GenTS(string path, string Config)
        {
            Configuration Json;
            using (StreamReader r = new StreamReader(Path.GetFullPath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Configs\\" + Config + ".json")))
            {
                string json = r.ReadToEnd();
                Json = JsonConvert.DeserializeObject<Configuration>(json);
            }
            Assembly assembly = Assembly.LoadFrom(path + "\\" + Json.DtoNamespace + "\\bin\\" + Json.DtoNamespace +".dll");
            var types = assembly.GetTypes().Where(x => Attribute.IsDefined(x, typeof(DataContractAttribute)));
            string interfaceFilePath = Path.GetFullPath(path + Json.OutputPath);
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
            using (StreamWriter outputFile = new StreamWriter(interfaceFilePath, false))
            {
                outputFile.WriteLine("declare module " + Json.TSNamespace + ".Structures {");
                outputFile.WriteLine("\t export interface KeyValuePair");
                outputFile.WriteLine("\t {");
                outputFile.WriteLine("\t \t text: string;");
                outputFile.WriteLine("\t \t value: any;");
                outputFile.WriteLine("\t }");
                outputFile.WriteLine("}");
                outputFile.WriteLine("module " + Json.TSNamespace + ".Enums");
                outputFile.WriteLine("{");
                foreach (var singleEnum in enums)
                {
                    outputFile.WriteLine("\t export enum " + singleEnum.Name + "{");
                    var props = singleEnum.GetFields(BindingFlags.Public | BindingFlags.Static);
                    foreach (var prop in props)
                    {
                        outputFile.WriteLine("\t \t" + prop.Name + " = " + prop.GetRawConstantValue() + ",");
                    }
                    outputFile.WriteLine("\t }");
                    //need to loop again for translation
                    outputFile.WriteLine("\t export var " + singleEnum.Name + "Translation: " +  Json.TSNamespace + ".Structures.KeyValuePair[] = [");
                    foreach (var prop in props)
                    {
                        var attrib = prop.CustomAttributes.Where(x => x.AttributeType == typeof(DescriptionAttribute)).Select(x => x.ConstructorArguments[0]).FirstOrDefault().Value != null ? prop.CustomAttributes.Where(x => x.AttributeType == typeof(DescriptionAttribute)).Select(x => x.ConstructorArguments[0]).FirstOrDefault().Value : prop.Name;
                        outputFile.WriteLine("\t \t {value:" + singleEnum.Name + "." + prop.Name + " , text: '" + attrib + "'},");
                    }
                    outputFile.WriteLine("\t ]");
                }
                outputFile.WriteLine("}");
                outputFile.WriteLine("module " +  Json.TSNamespace + ".Interfaces");
                outputFile.WriteLine("{");
                
                outputFile.WriteLine("\t export interface IEntityDtoWithID extends IEntityDto {");
                outputFile.WriteLine("\t \t ID?: any;");
                outputFile.WriteLine("\t \t Timestamp?: any;");
                outputFile.WriteLine("\t }");
                outputFile.WriteLine("\t export interface IEntityDto {");
                outputFile.WriteLine("\t }");
                foreach (var type in types)
                {
                    if (type.BaseType.Name != "Object")
                        outputFile.WriteLine("\t export interface I" + type.Name + " extends I" + type.BaseType.Name + "{");
                    else
                        outputFile.WriteLine("\t export interface I" + type.Name + "{");
                    var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    var kendoprops = type.GetProperties();
                    foreach (var prop in props)
                    {
                        if(type.Name == "AddRemoveDomainUseDTO")
                        {

                        }
                        string returnType;
                        if (prop.PropertyType.Name.Contains("Nullable"))
                        {
                            if (prop.PropertyType.GenericTypeArguments[0].FullName.Contains(Json.EnumNamespace))
                                returnType =  Json.TSNamespace + ".Enums." + prop.PropertyType.GenericTypeArguments[0].Name;
                            else
                                returnType = InterfacesHelpers.ReturnTypeCheck(prop.PropertyType.GenericTypeArguments[0].FullName);
                        }
                        else if (prop.PropertyType.Name.Contains("IEnumerable") || prop.PropertyType.Name.Contains("List`1"))
                        {
                            var parent = prop.PropertyType.GenericTypeArguments[0];
                            if (parent.Name.Contains("IEnumerable") || parent.Name.Contains("IPropertyDefinition") || parent.Name.Contains("Guid") || parent.Name.Contains("Dictionary") || parent.Name.Contains("KeyValuePair"))
                                returnType = "any[]";
                            else if (parent.Name.Contains("String"))
                                returnType = "string[]";
                            else if (parent.FullName.Contains(Json.EnumNamespace))
                                returnType = Json.TSNamespace + ".Enums." + parent.Name + "[]";
                            else if (parent.Name.Contains("Int"))
                                returnType = "number[]";
                            else
                                returnType = "I" + parent.Name + "[]";
                        }
                        else if (prop.PropertyType.FullName.Contains(Json.DtoNamespace))
                        {
                            if (prop.PropertyType.FullName.Contains(Json.EnumNamespace))
                                returnType =  Json.TSNamespace + ".Enums." + prop.PropertyType.Name;
                            else
                                returnType = "I" + prop.PropertyType.Name;
                        }
                        else
                            returnType = InterfacesHelpers.ReturnTypeCheck(prop.PropertyType.FullName);
                        if (prop.PropertyType.FullName.Contains("System.Nullable"))
                            outputFile.WriteLine("\t \t " + prop.Name + "?: " + returnType + ";");
                        else
                            outputFile.WriteLine("\t \t " + prop.Name + ": " + returnType + ";");
                    }
                    outputFile.WriteLine("\t }");
                    //need to loop again for kendo 
                    outputFile.WriteLine("\t export var KendoModel" + type.Name + ": any = {");
                    outputFile.WriteLine("\t \t fields: {");
                    foreach (var prop in kendoprops)
                    {
                        string returnType;
                        if (prop.PropertyType.Name.Contains("Nullable"))
                        {
                            if (prop.PropertyType.GenericTypeArguments[0].FullName.Contains(Json.EnumNamespace))
                                returnType =  Json.TSNamespace.ToLower() + ".enums." + prop.PropertyType.GenericTypeArguments[0].Name.ToLower();
                            else
                                returnType = InterfacesHelpers.ReturnTypeCheck(prop.PropertyType.GenericTypeArguments[0].FullName).ToLower();
                        }
                        else if (prop.PropertyType.Name.Contains("IEnumerable") || prop.PropertyType.Name.Contains("List`1"))
                        {
                            if (prop.PropertyType.GenericTypeArguments[0].Name.Contains("String"))
                                returnType = "string[]";
                            else
                                returnType = "any[]";
                        }
                        else if (prop.PropertyType.FullName.Contains(Json.DtoNamespace))
                        {
                            if (prop.PropertyType.FullName.Contains(Json.EnumNamespace))
                                returnType =  Json.TSNamespace.ToLower() + ".enums." + prop.PropertyType.Name.ToLower();
                            else
                                returnType = "any";
                        }
                        else
                            returnType = InterfacesHelpers.ReturnTypeCheck(prop.PropertyType.FullName).ToLower();
                        if (prop.PropertyType.FullName.Contains("System.Nullable"))
                            outputFile.WriteLine("\t \t \t'" + prop.Name + "': { type:'" + returnType + "', nullable: true},");
                        else
                            outputFile.WriteLine("\t \t \t'" + prop.Name + "': { type:'" + returnType + "', nullable: false},");
                    }
                    outputFile.WriteLine("\t \t }");
                    outputFile.WriteLine("\t }");
                }
                outputFile.WriteLine("}");

            }
        }
    }
}
