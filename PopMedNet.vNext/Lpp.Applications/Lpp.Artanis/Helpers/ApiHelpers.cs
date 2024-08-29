using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Artanis
{
    public class ApiHelpers
    {
        private static string BaseNameSpace = null;
        private static string EnumNameSpace = null;
        private static string TSNameSpace = null;
        public static void Initalize(string baseNameSpace, string enumNameSpace, string tsNameSpace)
        {
            BaseNameSpace = baseNameSpace;
            EnumNameSpace = enumNameSpace;
            TSNameSpace = tsNameSpace;
        } 
        public static StringBuilder MakeGet(string controller, string name, IEnumerable<ParameterInfo> paramList, string returnType, string interfaces, bool isOdata)
        {
            StringBuilder sb = new StringBuilder();
            var paramString = "";
            StringBuilder paramBody = new StringBuilder();
            paramBody.AppendLine("\t \t \t var params = '';");
            foreach (var param in paramList)
            {
                bool isList = param.ParameterType.FullName.Contains("Enumerable");

                if (param.ParameterType.GenericTypeArguments.Count() > 0)
                {
                    var type = TypeCheck(param.ParameterType.GenericTypeArguments[0].FullName, param.ParameterType.GenericTypeArguments[0].Name, isList);
                    if (paramString == "")
                    {
                        paramString += param.Name + ": " + type + "";
                        if (type.Contains("[]"))
                        {
                            if (type.Contains("string"))
                            {
                                paramBody.AppendLine("\t \t \t if (" + param.Name + " != null)");
                                paramBody.AppendLine("\t \t \t \t for(var j = 0; j < " + param.Name + ".length; j++) { params += '&" + param.Name + "=' + encodeURIComponent(" + param.Name + "[j]); }");
                            }
                            else
                            {
                                paramBody.AppendLine("\t \t \t if (" + param.Name + " != null)");
                                paramBody.AppendLine("\t \t \t \t for(var j = 0; j < " + param.Name + ".length; j++) { params += '&" + param.Name + "=' + " + param.Name + "[j]; }");
                            }
                        }
                        else if (type == "string")
                            paramBody.AppendLine("\t \t \t if (" + param.Name + " != null) params += '&" + param.Name + "=' + encodeURIComponent(" + param.Name + ");");
                        else
                            paramBody.AppendLine("\t \t \t if (" + param.Name + " != null) params += '&" + param.Name + "=' + " + param.Name + ";");
                    }
                    else
                    {
                        paramString += ", " + param.Name + ": " + type + "";
                        if (type.Contains("[]"))
                        {
                            if (type.Contains("string"))
                            {
                                paramBody.AppendLine("\t \t \t if (" + param.Name + " != null)");
                                paramBody.AppendLine("\t \t \t \t for(var j = 0; j < " + param.Name + ".length; j++) { params += '&" + param.Name + "=' + encodeURIComponent(" + param.Name + "[j]); }");
                            }
                            else
                            {
                                paramBody.AppendLine("\t \t \t if (" + param.Name + " != null)");
                                paramBody.AppendLine("\t \t \t \t for(var j = 0; j < " + param.Name + ".length; j++) { params += '&" + param.Name + "=' + " + param.Name + "[j]; }");
                            }
                        }
                        else if (type == "string")
                            paramBody.AppendLine("\t \t \t if (" + param.Name + " != null) params += '&" + param.Name + "=' + encodeURIComponent(" + param.Name + ");");
                        else
                            paramBody.AppendLine("\t \t \t if (" + param.Name + " != null) params += '&" + param.Name + "=' + " + param.Name + ";");
                    }
                }
                else
                {
                    var type = TypeCheck(param.ParameterType.FullName, param.ParameterType.Name, isList);
                    if (paramString == "")
                    {
                        paramString += param.Name + ": " + type + "";
                        if (type == "string[]")
                        {
                            paramBody.AppendLine("\t \t \t if (" + param.Name + " != null)");
                            paramBody.AppendLine("\t \t \t \t for(var j = 0; j < " + param.Name + ".length; j++) { params += '&" + param.Name + "=' + encodeURIComponent(" + param.Name + "[j]); }");
                        }
                        else if (type == "string")
                            paramBody.AppendLine("\t \t \t if (" + param.Name + " != null) params += '&" + param.Name + "=' + encodeURIComponent(" + param.Name + ");");
                        else
                            paramBody.AppendLine("\t \t \t if (" + param.Name + " != null) params += '&" + param.Name + "=' + " + param.Name + ";");
                    }
                    else
                    {
                        paramString += ", " + param.Name + ": " + type + "";
                        if (type == "string[]")
                        {
                            paramBody.AppendLine("\t \t \t if (" + param.Name + " != null)");
                            paramBody.AppendLine("\t \t \t \t for(var j = 0; j < " + param.Name + ".length; j++) { params += '&" + param.Name + "=' + encodeURIComponent(" + param.Name + "[j]); }");
                        }
                        else if (type == "string")
                            paramBody.AppendLine("\t \t \t if (" + param.Name + " != null) params += '&" + param.Name + "=' + encodeURIComponent(" + param.Name + ");");
                        else
                            paramBody.AppendLine("\t \t \t if (" + param.Name + " != null) params += '&" + param.Name + "=' + " + param.Name + ";");
                    }
                }


            }
            if (isOdata)
            {
                if (paramString == "")
                {

                    if (returnType == "Guid[][]" || returnType == "Guid[]" || returnType == "Guid" || returnType == "any[]")
                        sb.AppendLine("\t \t public static " + name + "($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<any[]>{");
                    else if (returnType.Contains("Boolean"))
                        sb.AppendLine("\t \t public static " + name + "($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<boolean[]>{");
                    else
                        sb.AppendLine("\t \t public static " + name + "($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<"+ interfaces + ".I" + returnType + ">{");
                    paramBody.AppendLine(@"             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;");
                }
                else
                {
                    if (returnType == "Guid[][]" || returnType == "Guid[]" || returnType == "Guid" || returnType == "any[]")
                        sb.AppendLine("\t \t public static " + name + "(" + paramString + ",$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<any[]>{");
                    else if (returnType.Contains("Boolean")) sb.AppendLine("\t \t public static " + name + "(" + paramString + ",$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<boolean[]>{");
                    else
                        sb.AppendLine("\t \t public static " + name + "(" + paramString + ",$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<" + interfaces + ".I" + returnType + ">{");
                    paramBody.AppendLine(@"             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;");
                }

            }
            else
            {
                if (paramString == "")
                {
                    if (returnType == "Guid[][]" || returnType == "Guid[]" || returnType == "Guid" || returnType == "any[]")
                        sb.AppendLine("\t \t public static " + name + "( doNotHandleFail?: boolean):JQueryDeferred<any[]>{");
                    else if (returnType.Contains("Boolean"))
                        sb.AppendLine("\t \t public static " + name + "( doNotHandleFail?: boolean):JQueryDeferred<boolean[]>{");
                    else
                        sb.AppendLine("\t \t public static " + name + "( doNotHandleFail?: boolean):JQueryDeferred<" + interfaces + ".I" + returnType + ">{");
                }
                else
                {
                    if (returnType == "Guid[][]" || returnType == "Guid[]" || returnType == "Guid" || returnType == "any[]")
                        sb.AppendLine("\t \t public static " + name + "(" + paramString + ", doNotHandleFail?: boolean):JQueryDeferred<any[]>{");
                    else if (returnType.Contains("Boolean"))
                        sb.AppendLine("\t \t public static " + name + "(" + paramString + ", doNotHandleFail?: boolean):JQueryDeferred<boolean[]>{");
                    else
                        sb.AppendLine("\t \t public static " + name + "(" + paramString + ", doNotHandleFail?: boolean):JQueryDeferred<" + interfaces + ".I" + returnType + ">{");
                }
            }
            paramBody.AppendLine("\t \t \t if (params.length > 0)");
            paramBody.AppendLine("\t \t \t \t params = '?' + params.substr(1);");
            sb.AppendLine(paramBody.ToString());
            if (returnType == "Guid[][]" || returnType == "Guid[]" || returnType == "Guid" || returnType == "any[]")
                sb.AppendLine("\t \t \t return Helpers.GetAPIResult<any[]>('" + controller + "/" + name + "' + params, doNotHandleFail);");
            else if (returnType.Contains("Boolean"))
                sb.AppendLine("\t \t \t return Helpers.GetAPIResult<boolean[]>('" + controller + "/" + name + "' + params, doNotHandleFail);");
            else
                sb.AppendLine("\t \t \t return Helpers.GetAPIResult<"+ interfaces +".I" + returnType + ">('" + controller + "/" + name + "' + params, doNotHandleFail);");
            sb.AppendLine("\t \t }");
            return sb;
        }
        public static StringBuilder MakePut(string controller, string name, IEnumerable<ParameterInfo> paramList, string returnType, string interfaces, bool isOdata)
        {
            bool isList = false;
            StringBuilder sb = new StringBuilder();
            var paramString = "";
            var paramName = "";
            foreach (var param in paramList)
            {
                if (param.ParameterType.GenericTypeArguments.Count() > 0)
                {
                    if (param.ParameterType.FullName.Contains("Enumerable"))
                        isList = true;
                    var type = TypeCheck(param.ParameterType.GenericTypeArguments[0].FullName, param.ParameterType.GenericTypeArguments[0].Name, isList);
                    if (paramString == "")
                    {
                        paramString += param.Name + ": " + type + "";
                        paramName = param.Name;
                    }
                    else
                    {
                        paramString += ", " + param.Name + ": " + type + "";
                        paramName = param.Name;
                    }
                }
                else
                {
                    if (param.ParameterType.FullName.Contains("Enumerable"))
                        isList = true;
                    var type = TypeCheck(param.ParameterType.FullName, param.ParameterType.Name, isList);
                    if (paramString == "")
                    {
                        paramString += param.Name + ": " + type + "";
                        paramName = param.Name;
                    }
                    else
                    {
                        paramString += ", " + param.Name + ": " + type + "";
                        paramName = param.Name;
                    }
                }


            }
            if (paramString == "")
            {
                if (returnType == "Guid[][]" || returnType == "Guid[]" || returnType == "Guid" || returnType == "any[]")
                    sb.AppendLine("\t \t public static " + name + "( doNotHandleFail?: boolean):JQueryDeferred<any[]>{");
                else if (returnType.Contains("Boolean"))
                    sb.AppendLine("\t \t public static " + name + "( doNotHandleFail?: boolean):JQueryDeferred<boolean[]>{");
                else
                    sb.AppendLine("\t \t public static " + name + "( doNotHandleFail?: boolean):JQueryDeferred<" + interfaces + ".I" + returnType + ">{");
            }
            else
            {
                if (returnType == "Guid[][]" || returnType == "Guid[]" || returnType == "Guid" || returnType == "any[]")
                    sb.AppendLine("\t \t public static " + name + "(" + paramString + ", doNotHandleFail?: boolean):JQueryDeferred<any[]>{");
                else if (returnType.Contains("Boolean"))
                    sb.AppendLine("\t \t public static " + name + "(" + paramString + ", doNotHandleFail?: boolean):JQueryDeferred<boolean[]>{");
                else
                    sb.AppendLine("\t \t public static " + name + "(" + paramString + ", doNotHandleFail?: boolean):JQueryDeferred<" + interfaces + ".I" + returnType + ">{");
            }
            if (paramString == "")
            {
                if (returnType == "Guid[][]" || returnType == "Guid[]" || returnType == "Guid" || returnType == "any[]")
                    sb.AppendLine("\t \t \t return Helpers.PutAPIValue<any[]>('" + controller + "/" + name + "', doNotHandleFail);");
                else if (returnType.Contains("Boolean"))
                    sb.AppendLine("\t \t \t return Helpers.PutAPIValue<boolean[]>('" + controller + "/" + name + "', doNotHandleFail);");
                else
                    sb.AppendLine("\t \t \t return Helpers.PutAPIValue<" + interfaces + ".I" + returnType + ">('" + controller + "/" + name + "', doNotHandleFail);");
            }
            else
            {
                if (returnType == "Guid[][]" || returnType == "Guid[]" || returnType == "Guid" || returnType == "any[]")
                    sb.AppendLine("\t \t \t return Helpers.PutAPIValue<any[]>('" + controller + "/" + name + "', " + paramName + ", doNotHandleFail);");
                else if (returnType.Contains("Boolean"))
                    sb.AppendLine("\t \t \t return Helpers.PutAPIValue<boolean[]>('" + controller + "/" + name + "', " + paramName + ", doNotHandleFail);");
                else
                    sb.AppendLine("\t \t \t return Helpers.PutAPIValue<" + interfaces + ".I" + returnType + ">('" + controller + "/" + name + "', " + paramName + ", doNotHandleFail);");
            }
            sb.AppendLine("\t \t }");
            return sb;
        }
        public static StringBuilder MakePost(string controller, string name, IEnumerable<ParameterInfo> paramList, string returnType, string interfaces, bool isOdata)
        {
            bool isList = false;
            StringBuilder sb = new StringBuilder();
            var paramString = "";
            var paramName = "";
            foreach (var param in paramList)
            {
                if (param.ParameterType.GenericTypeArguments.Count() > 0)
                {
                    if (param.ParameterType.FullName.Contains("Enumerable"))
                        isList = true;
                    var type = TypeCheck(param.ParameterType.GenericTypeArguments[0].FullName, param.ParameterType.GenericTypeArguments[0].Name, isList);
                    if (paramString == "")
                    {
                        paramString += param.Name + ": " + type + "";
                        paramName = param.Name;
                    }
                    else
                    {
                        paramString += ", " + param.Name + ": " + type + "";
                        paramName = param.Name;
                    }
                }
                else
                {
                    if (param.ParameterType.FullName.Contains("Enumerable"))
                        isList = true;
                    var type = TypeCheck(param.ParameterType.FullName, param.ParameterType.Name, isList);
                    if (paramString == "")
                    {
                        paramString += param.Name + ": " + type + "";
                        paramName = param.Name;
                    }
                    else
                    {
                        paramString += ", " + param.Name + ": " + type + "";
                        paramName = param.Name;
                    }
                }


            }
            if (paramString == "")
            {
                if (returnType == "Guid[][]" || returnType == "Guid[]" || returnType == "Guid" || returnType == "any[]")
                    sb.AppendLine("\t \t public static " + name + "( doNotHandleFail?: boolean):JQueryDeferred<any[]>{");
                else if (returnType.Contains("Boolean"))
                    sb.AppendLine("\t \t public static " + name + "( doNotHandleFail?: boolean):JQueryDeferred<boolean[]>{");
                else
                    sb.AppendLine("\t \t public static " + name + "( doNotHandleFail?: boolean):JQueryDeferred<" + interfaces + ".I" + returnType + ">{");
            }
            else
            {
                if (returnType == "Guid[][]" || returnType == "Guid[]" || returnType == "Guid" || returnType == "any[]")
                    sb.AppendLine("\t \t public static " + name + "(" + paramString + ", doNotHandleFail?: boolean):JQueryDeferred<any[]>{");
                else if (returnType.Contains("Boolean"))
                    sb.AppendLine("\t \t public static " + name + "(" + paramString + ", doNotHandleFail?: boolean):JQueryDeferred<boolean[]>{");
                else
                    sb.AppendLine("\t \t public static " + name + "(" + paramString + ", doNotHandleFail?: boolean):JQueryDeferred<" + interfaces + ".I" + returnType + ">{");
            }
            if (paramString == "")
            {
                if (returnType == "Guid[][]" || returnType == "Guid[]" || returnType == "Guid" || returnType == "any[]")
                    sb.AppendLine("\t \t \t return Helpers.PostAPIValue<any[]>('" + controller + "/" + name + "', doNotHandleFail);");
                else if (returnType.Contains("Boolean"))
                    sb.AppendLine("\t \t \t return Helpers.PostAPIValue<boolean[]>('" + controller + "/" + name + "', doNotHandleFail);");
                else
                    sb.AppendLine("\t \t \t return Helpers.PostAPIValue<" + interfaces + ".I" + returnType + ">('" + controller + "/" + name + "', doNotHandleFail);");
            }
            else
            {
                if (returnType == "Guid[][]" || returnType == "Guid[]" || returnType == "Guid" || returnType == "any[]")
                    sb.AppendLine("\t \t \t return Helpers.PostAPIValue<any[]>('" + controller + "/" + name + "', " + paramName + ", doNotHandleFail);");
                else if (returnType.Contains("Boolean"))
                    sb.AppendLine("\t \t \t return Helpers.PostAPIValue<boolean[]>('" + controller + "/" + name + "', " + paramName + ", doNotHandleFail);");
                else
                    sb.AppendLine("\t \t \t return Helpers.PostAPIValue<" + interfaces + ".I" + returnType + ">('" + controller + "/" + name + "', " + paramName + ", doNotHandleFail);");
            }
            sb.AppendLine("\t \t }");
            return sb;
        }
        public static StringBuilder MakeDelete(string controller, string name, IEnumerable<ParameterInfo> paramList, string returnType, string interfaces, bool isOdata)
        {
            bool isList = false;
            StringBuilder sb = new StringBuilder();
            var paramString = "";
            var paramName = "";
            StringBuilder paramBody = new StringBuilder();
            paramBody.AppendLine("\t \t \t var params = '';");
            foreach (var param in paramList)
            {
                if (param.ParameterType.GenericTypeArguments.Count() > 0)
                {
                    if (param.ParameterType.FullName.Contains("Enumerable"))
                        isList = true;
                    var type = TypeCheck(param.ParameterType.GenericTypeArguments[0].FullName, param.ParameterType.GenericTypeArguments[0].Name, isList);
                    if (paramString == "")
                    {

                        paramString += param.Name + ": " + type + "";
                        if (type.Contains("[]"))
                        {
                            if (type.Contains("string"))
                            {
                                paramBody.AppendLine("\t \t \t if (" + param.Name + " != null)");
                                paramBody.AppendLine("\t \t \t \t for(var j = 0; j < " + param.Name + ".length; j++) { params += '&" + param.Name + "=' + encodeURIComponent(" + param.Name + "[j]); }");
                                paramName = param.Name;
                            }
                            else
                            {
                                paramBody.AppendLine("\t \t \t if (" + param.Name + " != null)");
                                paramBody.AppendLine("\t \t \t \t for(var j = 0; j < " + param.Name + ".length; j++) { params += '&" + param.Name + "=' + " + param.Name + "[j]; }");
                                paramName = param.Name;
                            }
                        }
                        else if (type == "string")
                        {
                            paramBody.AppendLine("\t \t \t if (" + param.Name + " != null) params += '&" + param.Name + "=' + encodeURIComponent(" + param.Name + ");");
                            paramName = param.Name;
                        }
                        else
                        {
                            paramBody.AppendLine("\t \t \t if (" + param.Name + " != null) params += '&" + param.Name + "=' + " + param.Name + ";");
                            paramName = param.Name;
                        }
                    }
                    else
                    {
                        paramString += ", " + param.Name + ": " + type + "";
                        if (type.Contains("[]"))
                        {
                            if (type.Contains("string"))
                            {
                                paramBody.AppendLine("\t \t \t if (" + param.Name + " != null)");
                                paramBody.AppendLine("\t \t \t \t for(var j = 0; j < " + param.Name + ".length; j++) { params += '&" + param.Name + "=' + encodeURIComponent(" + param.Name + "[j]); }");
                                paramName = param.Name;
                            }
                            else
                            {
                                paramBody.AppendLine("\t \t \t if (" + param.Name + " != null)");
                                paramBody.AppendLine("\t \t \t \t for(var j = 0; j < " + param.Name + ".length; j++) { params += '&" + param.Name + "=' + " + param.Name + "[j]; }");
                                paramName = param.Name;
                            }
                        }
                        else if (type == "string")
                        {
                            paramBody.AppendLine("\t \t \t if (" + param.Name + " != null) params += '&" + param.Name + "=' + encodeURIComponent(" + param.Name + ");");
                            paramName = param.Name;
                        }
                        else
                        {
                            paramBody.AppendLine("\t \t \t if (" + param.Name + " != null) params += '&" + param.Name + "=' + " + param.Name + ";");
                            paramName = param.Name;
                        }
                    }
                }
                else
                {
                    if (param.ParameterType.FullName.Contains("Enumerable"))
                        isList = true;
                    var type = TypeCheck(param.ParameterType.FullName, param.ParameterType.Name, isList);
                    if (paramString == "")
                    {
                        paramString += param.Name + ": " + type + "";
                        if (type == "string[]")
                        {
                            paramBody.AppendLine("\t \t \t if (" + param.Name + " != null)");
                            paramBody.AppendLine("\t \t \t \t for(var j = 0; j < " + param.Name + ".length; j++) { params += '&" + param.Name + "=' + encodeURIComponent(" + param.Name + "[j]); }");
                        }
                        else if (type == "string")
                            paramBody.AppendLine("\t \t \t if (" + param.Name + " != null) params += '&" + param.Name + "=' + encodeURIComponent(" + param.Name + ");");
                        else
                            paramBody.AppendLine("\t \t \t if (" + param.Name + " != null) params += '&" + param.Name + "=' + " + param.Name + ";");
                    }
                    else
                    {
                        paramString += ", " + param.Name + ": " + type + "";
                        if (type == "string[]")
                        {
                            paramBody.AppendLine("\t \t \t if (" + param.Name + " != null)");
                            paramBody.AppendLine("\t \t \t \t for(var j = 0; j < " + param.Name + ".length; j++) { params += '&" + param.Name + "=' + encodeURIComponent(" + param.Name + "[j]); }");
                        }
                        else if (type == "string")
                            paramBody.AppendLine("\t \t \t if (" + param.Name + " != null) params += '&" + param.Name + "=' + encodeURIComponent(" + param.Name + ");");
                        else
                            paramBody.AppendLine("\t \t \t if (" + param.Name + " != null) params += '&" + param.Name + "=' + " + param.Name + ";");
                    }
                }
            }
            if (paramString == "")
            {
                if (returnType == "Guid[][]" || returnType == "Guid[]" || returnType == "Guid" || returnType == "any[]")
                    sb.AppendLine("\t \t public static " + name + "( doNotHandleFail?: boolean):JQueryDeferred<any[]>{");
                else if (returnType.Contains("Boolean"))
                    sb.AppendLine("\t \t public static " + name + "( doNotHandleFail?: boolean):JQueryDeferred<boolean[]>{");
                else
                    sb.AppendLine("\t \t public static " + name + "( doNotHandleFail?: boolean):JQueryDeferred<"+ interfaces +".I" + returnType + ">{");
            }
            else
            {
                if (returnType == "Guid[][]" || returnType == "Guid[]" || returnType == "Guid" || returnType == "any[]")
                    sb.AppendLine("\t \t public static " + name + "(" + paramString + ", doNotHandleFail?: boolean):JQueryDeferred<any[]>{");
                else if (returnType.Contains("Boolean"))
                    sb.AppendLine("\t \t public static " + name + "(" + paramString + ", doNotHandleFail?: boolean):JQueryDeferred<boolean[]>{");
                else
                    sb.AppendLine("\t \t public static " + name + "(" + paramString + ", doNotHandleFail?: boolean):JQueryDeferred<"+ interfaces +".I" + returnType + ">{");
            }
            paramBody.AppendLine("\t \t \t if (params.length > 0)");
            paramBody.AppendLine("\t \t \t \t params = '?' + params.substr(1);");
            sb.AppendLine(paramBody.ToString());
            if (returnType == "Guid[][]" || returnType == "Guid[]" || returnType == "Guid" || returnType == "any[]")
                sb.AppendLine("\t \t \t return Helpers.DeleteAPIValue<any[]>('" + controller + "/" + name + "' + params, doNotHandleFail);");
            else if (returnType.Contains("Boolean"))
                sb.AppendLine("\t \t \t return Helpers.DeleteAPIValue<boolean[]>('" + controller + "/" + name + "' + params, doNotHandleFail);");
            else
                sb.AppendLine("\t \t \t return Helpers.DeleteAPIValue<" + interfaces + ".I" + returnType + ">('" + controller + "/" + name + "' + params, doNotHandleFail);");
            sb.AppendLine("\t \t }");
            return sb;
        }
        public static StringBuilder HelperClass()
        {
            return new StringBuilder(@"     export class Helpers {
            static failMethod: Function;
            public static RegisterFailMethod(method: Function) {
                this.failMethod = method;
            }

            static FixStringDatesInResults(results) {
                results.forEach((data) => {
                    for (var field in data) {
                        if (data[field]) {
                            if ($.isArray(data[field])) {
                                this.FixStringDatesInResults(data[field]);
                            } else if (data[field].substring && data[field].match(/^\d{4}-\d{2}-\d{2}T{1}\d{2}:\d{2}:\d{2}(\.\d*)?Z?$/g)) {
                                if (data[field].indexOf('Z') > -1) {
                                    data[field] = new Date(data[field]);
                                } else {
                                    data[field] = new Date(data[field] + 'Z');
                                }
                            }
                        }
                    }
                });
            }

            public static GetAPIResult<T>(url: string, doNotHandleFail?: boolean): JQueryDeferred<T> {
                var d = jQuery.Deferred<T>();

                if (!jQuery.support.cors) {
                    url = '/api/get?Url=' + encodeURIComponent(url);
                } else {
                    url = ServiceUrl + url;
                }

                $.ajax({
                    type: 'GET',
                    url: url,
                    dataType: 'json',
                }).done((result) => {
                    if (result == null || result.results == null) {
                        d.resolve();
                        return;
                    } 

                    if (!$.isArray(result.results))
                        result.results = [result.results];

                    //Fix dates from strings into real dates.
                    this.FixStringDatesInResults(result.results);

                    d.resolve(<any>result.results);
                }).fail((e, description, error) => {
                    if (this.failMethod && !doNotHandleFail)
                        this.failMethod(e);
                    d.reject(<any>e);
                });

                return d;
            }

            static PostAPIValue<T>(url: string, value: any, doNotHandleFail?: boolean): JQueryDeferred<T> {
                var d = jQuery.Deferred<T>();
                if (!jQuery.support.cors) {
                    url = '/api/post?Url=' + encodeURIComponent(url);
                } else {
                    url = ServiceUrl + url;
                }

                $.ajax({
                    type: 'POST',
                    url: url,
                    dataType: 'json',
                    data: value == null ? null : JSON.stringify(value),
                    contentType: 'application/json; charset=utf-8',
                    timeout: 60000
                }).done((result) => {
                    if (result == null) {
                        d.resolve();
                        return;
                    } else if (result.results != null) {
                        if (!$.isArray(result.results)) {
                            d.resolve(<any>[result.results]);
                            return;
                        } else {
                            d.resolve(<any>result.results);
                            return;
                        }
                    } else {
                        if (!$.isArray(result)) {
                            d.resolve(<any>[result]);
                            return;
                        }
                    }

                    d.resolve(<any>result);
                }).fail((e) => {
                    if (this.failMethod && !doNotHandleFail)
                        this.failMethod(e);

                    d.reject(<any>e);
                });


                return d;
            }

            static PutAPIValue<T>(url: string, value: any, doNotHandleFail?: boolean): JQueryDeferred<T> {
                var d = jQuery.Deferred<T>();
                if (!jQuery.support.cors) {
                    url = '/api/put?Url=' + encodeURIComponent(url);
                } else {
                    url = ServiceUrl + url;
                }

                $.ajax({
                    type: 'PUT',
                    url: url,
                    dataType: 'json',
                    data: value == null ? null : JSON.stringify(value),
                    contentType: 'application/json; charset=utf-8',
                }).done((result) => {
                    if (result == null) {
                        d.resolve();
                        return;
                    } else if (result.results != null) {
                        if (!$.isArray(result.results)) {
                            d.resolve(<any>[result.results]);
                            return;
                        } else {
                            d.resolve(<any>result.results);
                            return;
                        }
                    } else {
                        if (!$.isArray(result)) {
                            d.resolve(<any>[result]);
                            return;
                        }
                    }

                    d.resolve(<any>result);
                }).fail((e) => {
                    if (this.failMethod && !doNotHandleFail)
                        this.failMethod(e);

                    d.reject(<any>e);
                });

                return d;
            }

            static DeleteAPIValue<T>(url: string, doNotHandleFail?: boolean): JQueryDeferred<T> {
                var d = jQuery.Deferred<T>();
                if (!jQuery.support.cors) {
                    url = '/api/delete?Url=' + encodeURIComponent(url);
                } else {
                    url = ServiceUrl + url;
                }

                $.ajax({
                    type: 'DELETE',
                    url: url,
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                }).done((result) => {
                    if (result == null) {
                        d.resolve();
                        return;
                    } else if (result.results != null) {
                        if (!$.isArray(result.results)) {
                            d.resolve(<any>[result.results]);
                            return;
                        } else {
                            d.resolve(<any>result.results);
                            return;
                        }
                    } else {
                        if (!$.isArray(result)) {
                            d.resolve(<any>[result]);
                            return;
                        }
                    }

                    d.resolve(<any>result);
                }).fail((e) => {
                    if (this.failMethod && !doNotHandleFail)
                        this.failMethod(e);

                    d.reject(<any>e);
                });

                return d;
            }
        }");
        }
        public static StringBuilder SignalRClass()
        {
            return new StringBuilder(@"     var _SignalRConnection: SignalR.Hub.Connection = null;
     export function SignalRConnection() : SignalR.Hub.Connection {
            if (_SignalRConnection == null) {
                _SignalRConnection = $.hubConnection(ServiceUrl + ' / signalr', null);

                _SignalRConnection.qs = { 'Auth': User.AuthToken };
                _SignalRConnection.start();
            }

            return _SignalRConnection;
     }");
        }
        public static string TypeCheck(string type, string name, bool isList)
        {
            string returnValue;
            switch (type)
            {
                case "System.Int32":
                case "System.Byte":
                case "System.Short":
                case "System.Int64":
                case "System.Decimal":
                case "System.Single":
                case "System.Double":
                case "System.Long":
                    returnValue = "number" + (isList == true ? "[]" : "");
                    break;
                case "System.String":
                    returnValue = "string" + (isList == true ? "[]" : "");
                    break;
                case "System.DateTime":
                case "System.DateTimeOffset":
                    returnValue = "Date" + (isList == true ? "[]" : "");
                    break;
                case "System.Guid":
                case "Guid":
                case "":
                case "System.Net.Http.HttpResponseMessage":
                case "dynamic":
                    returnValue = "any" + (isList == true ? "[]" : "");
                    break;
                case "System.Boolean":
                    returnValue = "boolean" + (isList == true ? "[]" : "");
                    break;
                default:
                    if (type.Contains(EnumNameSpace))
                        returnValue = TSNameSpace + ".Enums." + name + (isList == true ? "[]" : "");
                    else if(type.Contains(BaseNameSpace))
                        returnValue = TSNameSpace + ".Interfaces.I" + name + (isList == true ? "[]" : "");
                    else
                        returnValue = TSNameSpace + ".Interfaces.I" + name + (isList == true ? "[]" : "");
                    break;
            }

            return returnValue;
        }
    }
}
