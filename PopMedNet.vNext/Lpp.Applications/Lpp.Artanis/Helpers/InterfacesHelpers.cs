using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Artanis
{
    public class InterfacesHelpers
    {
        public static string ReturnTypeCheck(string type)
        {
            string returnValue;
            switch (type)
            {
                case "System.Int16":
                case "System.Int32":
                case "System.Byte":
                case "System.Short":
                case "System.Int64":
                case "System.Decimal":
                case "System.Single":
                case "System.Double":
                case "System.Long":
                    returnValue = "number";
                    break;
                case "System.String":
                    returnValue = "string";
                    break;
                case "System.DateTime":
                case "System.DateTimeOffset":
                    returnValue = "Date";
                    break;
                case "System.Guid":
                case "":
                case "dynamic":
                case "System.Object":
                    returnValue = "any";
                    break;
                case "System.Boolean":
                    returnValue = "boolean";
                    break;
                default:
                    returnValue = "any";
                    break;
            }

            return returnValue;
        }
    }
}
