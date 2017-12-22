using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Lpp.Dns.Data;
using Lpp.Utilities.Legacy;

namespace Lpp.Dns.General.Metadata.RequestMetadataCollection
{
    public class OrganizationMetadata 
    {

        public static void Export(XmlWriter xmlWriter, IEnumerable<Organization> organizations, IEnumerable<Registry> registries, Lpp.Dns.Portal.IPluginService plugins)
        {
            try
            {
                xmlWriter.WriteStartDocument(true);
                xmlWriter.WriteStartElement("organizations", "urn://popmednet/organizations/metadata");
                xmlWriter.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                xmlWriter.WriteAttributeString("xmlns", "xs", null, "http://www.w3.org/2001/XMLSchema");

                foreach(Organization o in organizations)
                {
                    Organization org = o;
                    xmlWriter.WriteStartElement("organization");
                    
                    #region Organization Header
                    xmlWriter.WriteStartElement("header");
                        InsertStringValue(xmlWriter, "id", org.ID.ToString());
                        InsertStringValue(xmlWriter, "name", org.Name);
                        InsertStringValue(xmlWriter, "acronym", org.Acronym);
                        InsertStringValue(xmlWriter, "parentorganization", org.ParentOrganization != null ? org.ParentOrganization.Name : string.Empty);
                        InsertStringValue(xmlWriter, "contactfullname", org.ContactFirstName + " " + org.ContactLastName);
                        InsertStringValue(xmlWriter, "contactemail", org.ContactEmail);
                        InsertStringValue(xmlWriter, "description", org.OrganizationDescription);
                        InsertStringValue(xmlWriter, "collabrequest", org.SpecialRequirements);
                        InsertStringValue(xmlWriter, "researchcapabilities", org.ObservationClinicalExperience);
                    xmlWriter.WriteEndElement(); // end header element                    
                    #endregion
                    // There are a number of ways to represent these values.  I've chosen to write each of them out, howver another 
                    // approach is the have a string enumeration similar to request status where we'd repeat the element, something 
                    // like "<participation>clinicaltrials</participation><participation>pragmaticclinicaltrials</participation> ..."
                    xmlWriter.WriteStartElement("participation");
                        InsertStringValue(xmlWriter, "observationalresearch", org.ObservationalParticipation.ToString());
                        InsertStringValue(xmlWriter, "clinicaltrials", org.ProspectiveTrials.ToString());
                        InsertStringValue(xmlWriter, "pragmaticclinicaltrials", org.PragmaticClinicalTrials.ToString());
                    xmlWriter.WriteEndElement(); // end participation
                    xmlWriter.WriteStartElement("typedatacollected");
                        InsertStringValue(xmlWriter, "inpatient", org.InpatientClaims.ToString());
                        InsertStringValue(xmlWriter, "outpatient", org.OutpatientClaims.ToString());
                        InsertStringValue(xmlWriter, "pharmacydispensings", org.OutpatientClaims.ToString());
                        InsertStringValue(xmlWriter, "enrollment", org.EnrollmentClaims.ToString());
                        InsertStringValue(xmlWriter, "demographics", org.DemographicsClaims.ToString());
                        InsertStringValue(xmlWriter, "laboratoryresults", org.LaboratoryResultsClaims.ToString());
                        InsertStringValue(xmlWriter, "vitalsigns", org.VitalSignsClaims.ToString());
                        InsertStringValue(xmlWriter, "biorepositories", org.Biorepositories.ToString());
                        InsertStringValue(xmlWriter, "patientreportedoutcomes", org.PatientReportedOutcomes.ToString());
                        InsertStringValue(xmlWriter, "patientreportedbehaviors", org.PatientReportedBehaviors.ToString());
                        InsertStringValue(xmlWriter, "prescriptionorders", org.PrescriptionOrders.ToString());
                        InsertStringValue(xmlWriter, "Other", org.OtherClaims.ToString());
                        InsertStringValue(xmlWriter, "Other", org.OtherClaimsText);
                    xmlWriter.WriteEndElement(); // end typedatacollected
                    xmlWriter.WriteStartElement("datamodels");
                        InsertStringValue(xmlWriter, "mscdm", org.DataModelMSCDM.ToString());
                        InsertStringValue(xmlWriter, "hmornvdw", org.DataModelHMORNVDW.ToString());
                        InsertStringValue(xmlWriter, "esp", org.DataModelESP.ToString());
                        InsertStringValue(xmlWriter, "i2b2", org.DataModelI2B2.ToString());
                        InsertStringValue(xmlWriter, "omop", org.DataModelOMOP.ToString());
                        InsertStringValue(xmlWriter, "other", org.DataModelOther.ToString());
                        InsertStringValue(xmlWriter, "otherText", org.DataModelOtherText);
                    xmlWriter.WriteEndElement(); // end datamodels
                    xmlWriter.WriteStartElement("electronichealthrecordssystems");
                        foreach(var e in org.EHRSes)
                        {
                            xmlWriter.WriteStartElement("ehr");
                            InsertStringValue(xmlWriter, "type", e.Type.ToString());
                            InsertStringValue(xmlWriter, "system", e.System.ToString());
                            InsertStringValue(xmlWriter, "startyear", e.StartYear.HasValue ? e.StartYear.Value.ToString() : null);
                            InsertStringValue(xmlWriter, "endyear", e.EndYear.HasValue ? e.EndYear.Value.ToString() : null);
                            xmlWriter.WriteEndElement(); // end ehr
                        };
                    xmlWriter.WriteEndElement(); // end electronichealthrecordssystems
                    xmlWriter.WriteStartElement("registries");
                        foreach(var regOrg in org.Registries)
                        {                            
                            Registry registry = registries.Single(rr => rr.ID == regOrg.RegistryID);

                            xmlWriter.WriteStartElement("registry");
                            InsertStringValue(xmlWriter, "type", registry.Type.ToString());
                            InsertStringValue(xmlWriter, "id", registry.ID.ToString());
                            InsertStringValue(xmlWriter, "name", registry.Name);
                            InsertStringValue(xmlWriter, "description", registry.Description);
                            InsertStringValue(xmlWriter, "RoPRUrl", registry.RoPRUrl);
                            InsertStringValue(xmlWriter, "memberdescription", regOrg.Description);
                            xmlWriter.WriteStartElement("classification");
                            foreach(var item in registry.Items.Where(i => i.Category == "Classification"))
                            {
                                InsertStringValue(xmlWriter, "item", item.Title);
                            };
                            xmlWriter.WriteEndElement(); // end classification

                            xmlWriter.WriteStartElement("purpose");
                            foreach(var item in registry.Items.Where(i => i.Category == "Purpose"))
                            {
                                InsertStringValue(xmlWriter, "item", item.Title);
                            };
                            xmlWriter.WriteEndElement(); // end purpose

                            xmlWriter.WriteStartElement("conditionofinterest");
                            foreach(var item in registry.Items.Where(i => i.Category == "Condition of Interest"))
                            {
                                InsertStringValue(xmlWriter, "item", item.Title);
                            };
                            xmlWriter.WriteEndElement(); // end purpose

                            xmlWriter.WriteEndElement(); // end registry
                        };
                    xmlWriter.WriteEndElement(); // end registries element

                    xmlWriter.WriteEndElement(); // end organization element
                    xmlWriter.Flush();
                };
                xmlWriter.WriteEndElement(); // end organizations element
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

        private static void InsertStringValue(XmlWriter xmlWriter, string elementName, String stringValue)
        {
            if (stringValue != null && !stringValue.NullOrEmpty())
                xmlWriter.WriteElementString(elementName, stringValue.ToString());
            else
            {
                xmlWriter.WriteStartElement(elementName);
                xmlWriter.WriteAttributeString("xsi", "nil", null, "true");
                xmlWriter.WriteEndElement();
            }
        }
    }
}