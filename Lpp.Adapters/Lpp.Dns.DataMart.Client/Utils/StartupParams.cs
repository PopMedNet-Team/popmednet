using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DMClient = Lpp.Dns.DataMart.Client;
using Lpp.Dns.DataMart.Lib;
using Lpp.Dns.DataMart.Lib.Utils;
using log4net;
using Lpp.Dns.DataMart.Lib.Classes;
using Lpp.Dns.DataMart.Model;
using System.Diagnostics.Contracts;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Lpp.Dns.DataMart.Client.Utils
{
    public static class StartupParams
    {
        private const string StartupParamsFileName = "StartupParams.xml";

        public static string GetStartupParamsFilePath()
        {
            string appFilepath = string.Empty;
            try
            {
                appFilepath = string.Format("{0}\\{1}\\{2}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DMClient.Properties.Settings.Default.AppDataFolderName, StartupParamsFileName);
            }
            catch (Exception)
            {
                appFilepath = string.Empty;
            }

            return appFilepath;
        }

        static internal XDocument GetStartupParamsXml(string[] args)
        {
            XDocument StartupParamsXmlDoc = null;

            // Iterate through the command-line args.
            int i = 0;
            while (i + 1 < args.Length) // while the arg AFTER the one we're examining exists...
            {
                // Throw away args until we get to "/addNetwork".
                if (args[i].ToLower() == "/addnetwork")
                {
                    if (args[i + 1].StartsWith("pmndmc:"))
                    {
                        Uri argsUri = new Uri(args[i + 1]);
                        string argsQuery = argsUri.Query;
                        if (argsQuery != null)
                        {
                            NameValueCollection argsCollection = HttpUtility.ParseQueryString(argsQuery);
                            Dictionary<string, string> argsDict = new Dictionary<string,string>();
                            foreach (string key in argsCollection.Keys)
                            {
                                argsDict[key] = argsCollection[key].Trim();
                            }

                            StartupParamsXmlDoc = new XDocument(
                                new XDeclaration("1.0", "utf-8", null),
                                new XElement("StartupParams",
                                    new XElement("Param",
                                        new XAttribute("Name", "addNetwork"),
                                        argsDict.Select(a =>
                                            new XElement("Property",
                                                new XAttribute[] {
                                                    new XAttribute("Name", a.Key),
                                                    new XAttribute("Value", a.Value),
                                                }
                                            )
                                        )
                                    )
                                )
                            );
                        }
                    }
                }
                i++;
            }

            return StartupParamsXmlDoc;
        }

        static internal void WriteStartupParamsToFile(string[] args)
        {
            XDocument StartupParamsXmlDoc = GetStartupParamsXml(args);

            // Save the XML to the file.
            if (StartupParamsXmlDoc != null)
            {
                StartupParamsXmlDoc.Save(StartupParams.GetStartupParamsFilePath());
            }
        }

        static internal XDocument ReadStartupParamsFromFile(bool DeleteAfterRead = false)
        {
            XDocument StartupParamsXmlDoc = null;
            string StartupParamsFilePath = StartupParams.GetStartupParamsFilePath();

            if (System.IO.File.Exists(StartupParamsFilePath))
            {
                StartupParamsXmlDoc = XDocument.Load(StartupParamsFilePath);
                if (DeleteAfterRead)
                {
                    DeleteStartupParamsFile();
                }
            }

            return StartupParamsXmlDoc;
        }

        static internal Dictionary<string, string> GetAddNetworkStartupParamsDictionary(XDocument StartupParamsXmlDoc)
        {
            Dictionary<string, string> StartupParamsDictionary = null;

            if (StartupParamsXmlDoc != null)
            {
                StartupParamsDictionary = StartupParamsXmlDoc.Elements("StartupParams").Elements("Param").Where(p => (string)p.Attribute("Name") == "addNetwork").Elements("Property").ToDictionary(el => (string)el.Attribute("Name"), el => (string)el.Attribute("Value"));
            }

            return StartupParamsDictionary;
        }

        static internal void DeleteStartupParamsFile()
        {
            string StartupParamsFilePath = StartupParams.GetStartupParamsFilePath();

            if (System.IO.File.Exists(StartupParamsFilePath))
            {
                System.IO.File.Delete(StartupParamsFilePath);
            }
        }
    }
}
