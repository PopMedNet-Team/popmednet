using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.IO;
using System.ComponentModel.Composition;
using Lpp.Composition;
using log4net;
using Lpp.Utilities.Legacy;
using Lpp.Dns.Data;
using Lpp.Dns.General.Metadata.Models;

namespace Lpp.Dns.General.Metadata.RequestMetadataCollection
{
    public class RequestMetadata
    {
        public static void Export(XmlWriter xmlWriter, IEnumerable<ExportedRequestSearchResult> requests, List<ExportedRequestSearchRoutingResult> routings, Lpp.Dns.Portal.IPluginService plugins)
        {
            try
            {
                xmlWriter.WriteStartDocument(true);
                xmlWriter.WriteStartElement("requests", "urn://popmednet/requests/metadata");
                xmlWriter.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                xmlWriter.WriteAttributeString("xmlns", "xs", null, "http://www.w3.org/2001/XMLSchema");

                foreach(var rr in requests)
                {
                    var request = rr;
                    xmlWriter.WriteStartElement("request");
                    #region Request Header
                    xmlWriter.WriteStartElement("header");
                    xmlWriter.WriteStartElement("requestType");
                    InsertStringValue(xmlWriter, "name", request.RequestType);
                    InsertStringValue(xmlWriter, "id", request.RequestTypeID.ToString());
                    xmlWriter.WriteEndElement(); // end requestType element 
                    InsertStringValue(xmlWriter, "id", request.Identifier.ToString());
                    InsertStringValue(xmlWriter, "name", request.RequestName);
                    InsertStringValue(xmlWriter, "requestid", request.MSRequestID);
                    InsertStringValue(xmlWriter, "description", request.Description);
                    xmlWriter.WriteElementString("priority", "Normal");
                    InsertDateValue(xmlWriter, "dueDate", request.DueDate);
                    InsertDateValue(xmlWriter, "submittedOn", request.SubmittedOn);
                    InsertUserValue(xmlWriter, "createdBy", request.CreatedByUserName, request.CreatedByOrganization, request.CreatedByEmail);
                    InsertDateValue(xmlWriter, "createdOn", request.CreatedOn);
                    InsertUserValue(xmlWriter, "updatedBy", request.UpdatedByUserName, request.UpdatedByOrganization, request.UpdatedByEmail);
                    InsertDateValue(xmlWriter, "updatedOn", request.UpdatedOn);
                    InsertStringValue(xmlWriter, "purposeOfUse", request.PurposeOfUse);
                    xmlWriter.WriteElementString("phiDisclosureLevel", request.LevelOfPHIDisclosure);        
                    xmlWriter.WriteStartElement("activity");
                    InsertStringValue(xmlWriter, "id", request.ActivityProjectID.HasValue ? request.ActivityProjectID.Value.ToString("D") : null);
                    InsertStringValue(xmlWriter, "description", request.ActivityProject);
                    xmlWriter.WriteEndElement(); // end activity element     
                    xmlWriter.WriteStartElement("group");
                    InsertStringValue(xmlWriter, "name", request.Group);
                    InsertStringValue(xmlWriter, "description", null); // TODO: define group description
                    xmlWriter.WriteEndElement(); // end group element    
                    xmlWriter.WriteStartElement("project");
                    InsertStringValue(xmlWriter, "name", request.Project);
                    xmlWriter.WriteElementString("description", request.ProjectDescription);
                    xmlWriter.WriteEndElement(); // end project element 
                    xmlWriter.WriteEndElement(); // end header element                    
                    #endregion
                    #region Body
                    /*
                    xmlWriter.WriteStartElement("body");
                    xmlWriter.WriteStartElement("criteria");
                    xmlWriter.WriteStartElement("codes");

                    xmlWriter.WriteStartElement("idc9Diagnosis");
                    xmlWriter.WriteElementString("code", "250.01");
                    xmlWriter.WriteElementString("description", "description");
                    xmlWriter.WriteEndElement(); // end idc9Diagnosis element
                    xmlWriter.WriteStartElement("observationRange");
                    xmlWriter.WriteElementString("startDate", "2011-03-30T09:30:10Z");
                    xmlWriter.WriteElementString("endDate", "2012-03-30T09:30:10Z");
                    xmlWriter.WriteEndElement(); // end observationRange element
                    xmlWriter.WriteEndElement(); // end criteria element

                    xmlWriter.WriteStartElement("report");
                    xmlWriter.WriteElementString("ageRange", "01-05,06-18,19-30,31-65,66+");
                    xmlWriter.WriteElementString("race", "white,black, ...");
                    xmlWriter.WriteEndElement(); // end report element
                    xmlWriter.WriteEndElement(); // end body element
                    xmlWriter.Flush();
                    */
                    #endregion
                    #region Routing - DataMarts
                    xmlWriter.WriteStartElement("routing");
                    foreach(var requestRouting in routings.Where(rt => rt.RequestID == request.RequestID).GroupBy(k => new { k.DataMartID, k.DataMart, k.Organization, k.Status }))
                    {
                        xmlWriter.WriteStartElement("dataMart");
                        InsertDataMart(xmlWriter, requestRouting.Key.DataMartID, requestRouting.Key.DataMart, requestRouting.Key.Organization);
                        xmlWriter.WriteElementString("status", requestRouting.Key.Status.ToString());

                        //var currentCount = requestRouting.Responses.Max(r => r.Count);
                        //foreach(var response in requestRouting.Responses){
                        //    InsertRoutingInstance(xmlWriter, "instance", response, response.Count == currentCount);
                        //}
                        foreach(var response in requestRouting.OrderByDescending(x => x.ResponseIndex))
                        {
                            InsertRoutingInstance(xmlWriter, "instance", response);
                        }

                        xmlWriter.WriteEndElement();
                    };
                    xmlWriter.WriteEndElement(); // end routing element
                    #endregion
                    xmlWriter.WriteEndElement(); // end request element
                    xmlWriter.Flush();
                };
                xmlWriter.WriteEndElement(); // end requests element
                xmlWriter.WriteEndDocument();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //xmlWriter.Close();
            }
        }

        private static void InsertUserValue(XmlWriter xmlWriter, string elementName, string username, string organization, string email)
        {
            if (!string.IsNullOrEmpty(username))
            {
                xmlWriter.WriteStartElement(elementName);
                InsertStringValue(xmlWriter, "username", username);
                InsertStringValue(xmlWriter, "organization", organization);
                InsertStringValue(xmlWriter, "email", email);
                xmlWriter.WriteEndElement();
            }
        }

        private static void InsertDateValue(XmlWriter xmlWriter, string elementName, DateTime? dateTime)
        {
            if (dateTime.HasValue)
                xmlWriter.WriteElementString(elementName, dateTime.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"));
            else
            {
                xmlWriter.WriteStartElement(elementName);
                xmlWriter.WriteAttributeString("xsi", "nil", null, "true");
                xmlWriter.WriteEndElement();
            }
        }

        private static void InsertStringValue(XmlWriter xmlWriter, string elementName, string stringValue)
        {
            if (!string.IsNullOrWhiteSpace(stringValue))
                xmlWriter.WriteElementString(elementName, stringValue.ToString());
            else
            {
                //if (elementName == "name")
                //    Log.Error("Null name");
                xmlWriter.WriteStartElement(elementName);
                xmlWriter.WriteAttributeString("xsi", "nil", null, "true");
                xmlWriter.WriteEndElement();
            }
        }

        private static void InsertRoutingInstance(XmlWriter xmlWriter, string elementName, ExportedRequestSearchRoutingResult item)
        {
            xmlWriter.WriteStartElement(elementName);
            InsertUserValue(xmlWriter, "submittedBy", item.SubmittedByUserName, item.SubmitterOrganization, item.SubmitterEmail);
            InsertDateValue(xmlWriter, "submittedOn", item.SubmittedOn);
            if (item.RespondedOn.HasValue)
            {
                InsertUserValue(xmlWriter, "respondedBy", item.RespondedByUserName, item.ResponderOrganization, item.ResponderEmail);
                InsertDateValue(xmlWriter, "respondedOn", item.RespondedOn);
            }
            InsertStringValue(xmlWriter, "isCurrent", item.IsCurrentResponse.ToString());
            xmlWriter.WriteEndElement(); // end routing instance element
        }

        private static void InsertDataMart(XmlWriter xmlWriter, Guid id, string name, string organization)
        {
            InsertStringValue(xmlWriter, "id", id.ToString("D"));
            InsertStringValue(xmlWriter, "name", name);
            InsertStringValue(xmlWriter, "organization", organization);
        }
    }
}