using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using Lpp.Composition;
using Lpp.Dns.HealthCare.ESPQueryBuilder.Data;
using Lpp.Dns.HealthCare.ESPQueryBuilder.Data.Entities;
using Lpp.Dns.HealthCare.ESPQueryBuilder.Models;
using Lpp.Mvc;
using Lpp.Dns.Portal;
using Lpp.Dns.Data;
using Lpp.Utilities;

namespace Lpp.Dns.HealthCare.ESPQueryBuilder.Code
{
    public class HQMFBuilder
    {
        private const string PMN_MDPHNet_OID = "1.1.1.1.0.0.0";
        private const string MDPHNet_Network_Identifier = "MDPHNet";
        private const string ValueSet_Race_OID = "1.1.1.1.1.1.1";
        private const string ValueSet_Diagnosis_OID = "1.1.1.1.1.1.2";
        private const string ValueSet_Diagnosis_CodeSystem = "2.16.840.1.113883.6.103";
        private const string ObservationCriteria_Age_Code = "424144002";

        public static byte[] BuildHQMF(IDnsRequestContext request, ESPQueryBuilderModel m, IAuthenticationService Auth)
        {
            byte[] HQMFBytes;
            XmlNode node, clonedNode, nodeDataCriteriaSection, nodePopulationCriteriaSection, nodePatientPopulationCriteria, nodeStratifierCriteria;
            XmlElement el;
            string localVariableName;

            string ageCriteriaExtension;
            ArrayList ageStratificationExtensions = new ArrayList();

            // Load the HQMF boilerplate template into an XmlDocument.
            XmlDocument HQMFBoilerplateXmlDoc = new XmlDocument();
            Stream s = typeof(ESPQueryBuilderModelPlugin).Assembly.GetManifestResourceStream("Lpp.Dns.HealthCare.ESPQueryBuilder.Code.ESPQueryHQMFBoilerplate.xml");
            HQMFBoilerplateXmlDoc.Load(s);

            // Since xmlns attributes are specified in the boilerplate, we need to use an XmlNamespaceManager in our XPATH statements.
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(HQMFBoilerplateXmlDoc.NameTable);
            nsmgr.AddNamespace("hl7", "urn:hl7-org:v3");
            nsmgr.AddNamespace("ihe", "urn:ihe:iti:svs:2008");

            // Load the Report control checkboxes into an array.
            string[] reportOn = m.Report.Split(',');

            //================
            // HEADER SECTION
            //================
            
            // id node (unique request identifier)

            node = HQMFBoilerplateXmlDoc.SelectSingleNode("/hl7:QualityMeasureDocument/hl7:id", nsmgr);
            ((XmlElement)node).SetAttribute("root", PMN_MDPHNet_OID);
            ((XmlElement)node).SetAttribute("extension", request.RequestID.ToString());

            // Title node

            node = HQMFBoilerplateXmlDoc.SelectSingleNode("/hl7:QualityMeasureDocument/hl7:title", nsmgr);
            node.InnerText = request.Header.Name;

            // Author section
            
            node = HQMFBoilerplateXmlDoc.SelectSingleNode("/hl7:QualityMeasureDocument/hl7:author", nsmgr);
            SetChildNodeInnerText(Auth.CurrentUser.FirstName, node, ".//hl7:name/hl7:given", nsmgr);
            SetChildNodeInnerText(Auth.CurrentUser.LastName, node, ".//hl7:name/hl7:family", nsmgr);
            SetChildNodeInnerText(Auth.CurrentUser.Title, node, ".//hl7:name/hl7:prefix", nsmgr);

            // Custodian section
            
            node = HQMFBoilerplateXmlDoc.SelectSingleNode("/hl7:QualityMeasureDocument/hl7:custodian", nsmgr);
            SetChildNodeAttributeValue(PMN_MDPHNet_OID, node, ".//hl7:id", "root", nsmgr);
            SetChildNodeAttributeValue(MDPHNet_Network_Identifier, node, ".//hl7:id", "extension", nsmgr);
            SetChildNodeInnerText(Auth.CurrentUser.FirstName, node, ".//hl7:name/hl7:given", nsmgr);
            SetChildNodeInnerText(Auth.CurrentUser.LastName, node, ".//hl7:name/hl7:family", nsmgr);
            SetChildNodeInnerText(Auth.CurrentUser.Title, node, ".//hl7:name/hl7:prefix", nsmgr);

            //==============
            // BODY SECTION
            //==============

            // Measure Period (Observation Period)
            
            node = HQMFBoilerplateXmlDoc.SelectSingleNode("/hl7:QualityMeasureDocument/hl7:controlVariable/hl7:measurePeriod", nsmgr);
            SetChildNodeAttributeValue(PMN_MDPHNet_OID, node, ".//hl7:id", "root", nsmgr);
            SetChildNodeAttributeValue(m.StartPeriodDate == null ? "" : m.StartPeriodDate.Value.ToString("yyyyMMdd"), node, ".//hl7:value/hl7:low", "value", nsmgr);
            SetChildNodeAttributeValue(m.EndPeriodDate == null ? "" : m.EndPeriodDate.Value.ToString("yyyyMMdd"), node, ".//hl7:value/hl7:high", "value", nsmgr);

            // Measure Description (Request Type and Request Description)
            
            node = HQMFBoilerplateXmlDoc.SelectSingleNode("/hl7:QualityMeasureDocument/hl7:component/hl7:measureDescriptionSection", nsmgr);
            SetChildNodeInnerText(request.RequestType.Name, node, ".//hl7:title", nsmgr);
            SetChildNodeInnerText(request.Header.Description, node, ".//hl7:text", nsmgr);

            //======================
            // DATA CRITERIA SECTION
            //======================

            // Get the dataCriteriaSection node that we will be working within

            nodeDataCriteriaSection = HQMFBoilerplateXmlDoc.SelectSingleNode("/hl7:QualityMeasureDocument/hl7:component/hl7:dataCriteriaSection", nsmgr);

            // Demographics id node

            SetChildNodeAttributeValue(PMN_MDPHNet_OID, nodeDataCriteriaSection, "./hl7:definition/hl7:observationDefinition/hl7:id[@extension='Demographics']", "root", nsmgr);

            // Encounters id node

            SetChildNodeAttributeValue(PMN_MDPHNet_OID, nodeDataCriteriaSection, "./hl7:definition/hl7:encounterDefinition/hl7:id[@extension='Encounters']", "root", nsmgr);

            // Problems id node

            SetChildNodeAttributeValue(PMN_MDPHNet_OID, nodeDataCriteriaSection, "./hl7:definition/hl7:observationDefinition/hl7:id[@extension='Problems']", "root", nsmgr);

            // Allergies id node

            SetChildNodeAttributeValue(PMN_MDPHNet_OID, nodeDataCriteriaSection, "./hl7:definition/hl7:observationDefinition/hl7:id[@extension='Allergies']", "root", nsmgr);

            // Procedures id node

            SetChildNodeAttributeValue(PMN_MDPHNet_OID, nodeDataCriteriaSection, "./hl7:definition/hl7:procedureDefinition/hl7:id[@extension='Procedures']", "root", nsmgr);

            // Results id node

            SetChildNodeAttributeValue(PMN_MDPHNet_OID, nodeDataCriteriaSection, "./hl7:definition/hl7:observationDefinition/hl7:id[@extension='Results']", "root", nsmgr);

            // Vitals id node

            SetChildNodeAttributeValue(PMN_MDPHNet_OID, nodeDataCriteriaSection, "./hl7:definition/hl7:observationDefinition/hl7:id[@extension='Vitals']", "root", nsmgr);

            // Medications id node

            SetChildNodeAttributeValue(PMN_MDPHNet_OID, nodeDataCriteriaSection, "./hl7:definition/hl7:substanceAdministrationDefinition/hl7:id[@extension='Medications']", "root", nsmgr);

            // RX id node

            SetChildNodeAttributeValue(PMN_MDPHNet_OID, nodeDataCriteriaSection, "./hl7:definition/hl7:supplyDefinition/hl7:id[@extension='RX']", "root", nsmgr);

            // Race Value Set

            node = nodeDataCriteriaSection.SelectSingleNode("./hl7:definition/hl7:valueSet//ihe:ValueSet[@id='" + ValueSet_Race_OID + "']/ihe:ConceptList", nsmgr);
            if (node != null && !m.Race.IsNullOrEmpty() && m.Race.Trim() != string.Empty)
            {
                if (node.FirstChild != null)
                {
                    // Remove the "sample <Concept> node" XML comment.
                    node.RemoveChild(node.FirstChild);
                }
                foreach (string race in m.Race.Split(','))
                {
                    if (string.IsNullOrWhiteSpace(race))
                        continue;

                    el = HQMFBoilerplateXmlDoc.CreateElement("Concept", nsmgr.LookupNamespace("ihe"));
                    el.SetAttribute("code", race.Trim());
                    el.SetAttribute("displayName", RaceSelectionList.GetName(Convert.ToInt32(race)));
                    el.SetAttribute("codeSystem", PMN_MDPHNet_OID);
                    node.AppendChild(el);
                }
            }

            // Diagnosis Value Set (ICD-9 Diagnosis requests ONLY!)

            node = nodeDataCriteriaSection.SelectSingleNode("./hl7:definition[hl7:valueSet//ihe:ValueSet[@id='" + ValueSet_Diagnosis_OID + "']]", nsmgr);
            node = node.SelectSingleNode(".//ihe:ConceptList", nsmgr);
            if (node != null && m.Codes != null)
            {
                if (node.FirstChild != null)
                {
                    // Remove the "sample <Concept> node" XML comment.
                    node.RemoveChild(node.FirstChild);
                }
                foreach (string code in m.Codes.Split(','))
                {
                    if (string.IsNullOrWhiteSpace(code))
                        continue;

                    el = HQMFBoilerplateXmlDoc.CreateElement("Concept", nsmgr.LookupNamespace("ihe"));
                    el.SetAttribute("code", code.Trim());
                    el.SetAttribute("displayName", string.Empty);
                    el.SetAttribute("codeSystem", ValueSet_Diagnosis_CodeSystem);
                    node.AppendChild(el);
                }
            }

            // Demographics Criteria: Age

            node = nodeDataCriteriaSection.SelectSingleNode("./hl7:entry[hl7:observationCriteria/hl7:code[@code='" + ObservationCriteria_Age_Code + "']]", nsmgr);
            ageCriteriaExtension = "ageBetween" + (string.IsNullOrWhiteSpace(m.MinAge) ? "0" : m.MinAge) + "and" + (string.IsNullOrWhiteSpace(m.MaxAge) ? "120" : m.MaxAge);
            SetChildNodeInnerText(ageCriteriaExtension, node, "./hl7:localVariableName", nsmgr);
            SetChildNodeAttributeValue(PMN_MDPHNet_OID, node, "./hl7:observationCriteria/hl7:id", "root", nsmgr);
            SetChildNodeAttributeValue(ageCriteriaExtension, node, "./hl7:observationCriteria/hl7:id", "extension", nsmgr);
            SetChildNodeAttributeValue(m.MinAge, node, "./hl7:observationCriteria/hl7:value/hl7:low", "value", nsmgr);
            SetChildNodeAttributeValue(m.MaxAge, node, "./hl7:observationCriteria/hl7:value/hl7:high", "value", nsmgr);

            if (reportOn.Contains(((int)ReportSelectionCode.Age).ToString()) && m.AgeStratificationByNumberOfYears != null && m.AgeStratificationByNumberOfYears > 0)
            {
                int startAge = 0, endAge = (int)m.AgeStratificationByNumberOfYears - 1;

                while (startAge.ToInt32() <= m.MaxAge.ToInt32())
                {
                    clonedNode = node.CloneNode(true);
                    node.ParentNode.InsertAfter(clonedNode, node);
                    node = clonedNode;

                    localVariableName = "stratifyAgeBetween" + startAge.ToString() + "and" + endAge.ToString();
                    SetChildNodeInnerText(localVariableName, node, "./hl7:localVariableName", nsmgr);
                    SetChildNodeAttributeValue(localVariableName, node, "./hl7:observationCriteria/hl7:id", "extension", nsmgr);
                    SetChildNodeAttributeValue(startAge.ToString(), node, "./hl7:observationCriteria/hl7:value/hl7:low", "value", nsmgr);
                    SetChildNodeAttributeValue(endAge.ToString(), node, "./hl7:observationCriteria/hl7:value/hl7:high", "value", nsmgr);

                    ageStratificationExtensions.Add(localVariableName);
                    startAge = endAge + 1;
                    endAge += (int)m.AgeStratificationByNumberOfYears;
                }
            }

            // Demographics Criteria: Gender

            node = nodeDataCriteriaSection.SelectSingleNode("./hl7:entry/hl7:observationCriteria/hl7:id[@extension='genderMale']", nsmgr);
            ((XmlElement)node).SetAttribute("root", PMN_MDPHNet_OID);
            node = nodeDataCriteriaSection.SelectSingleNode("./hl7:entry/hl7:observationCriteria/hl7:id[@extension='genderFemale']", nsmgr);
            ((XmlElement)node).SetAttribute("root", PMN_MDPHNet_OID);

            // Demographics Criteria: Race

            node = nodeDataCriteriaSection.SelectSingleNode("./hl7:entry/hl7:observationCriteria/hl7:id[@extension='raceCriteria']", nsmgr);
            ((XmlElement)node).SetAttribute("root", PMN_MDPHNet_OID);

            // Encounter Criteria: Diagnosis Codes (ICD-9 Diagnosis requests ONLY!)

            node = nodeDataCriteriaSection.SelectSingleNode("./hl7:entry[hl7:encounterCriteria/hl7:id[@extension='ICD9Diagnosis']]", nsmgr);
            // Remove the entire "definition" node if it's a Reportable Disease request!
            node.ParentNode.RemoveChild(node);

            // Encounter Criteria: Disease free text (Reportable Disease requests ONLY!)

            node = nodeDataCriteriaSection.SelectSingleNode("./hl7:entry[hl7:encounterCriteria/hl7:id[@extension='diseaseFreeText']]", nsmgr);
            node.ParentNode.RemoveChild(node);
            //============================
            // POPULATION CRITERIA SECTION
            //============================

            // Get the populationCriteriaSection node that we will be working within

            nodePopulationCriteriaSection = HQMFBoilerplateXmlDoc.SelectSingleNode("/hl7:QualityMeasureDocument/hl7:component/hl7:populationCriteriaSection", nsmgr);

            // Population Criteria: id and title nodes

            SetChildNodeAttributeValue(PMN_MDPHNet_OID, nodePopulationCriteriaSection, "./hl7:id", "root", nsmgr);
            SetChildNodeInnerText(request.RequestType.Name.Replace(" ", "").Replace("-", ""), nodePopulationCriteriaSection, "./hl7:title", nsmgr);

            //----------------------------------------
            // Patient Population Criteria sub-section
            //----------------------------------------

            // Get the patientPopulationCriteria node that we will insert nodes into

            nodePatientPopulationCriteria = nodePopulationCriteriaSection.SelectSingleNode("./hl7:entry/hl7:patientPopulationCriteria", nsmgr);

            // id node

            SetChildNodeAttributeValue(PMN_MDPHNet_OID, nodePatientPopulationCriteria, ".//hl7:id", "root", nsmgr);

            // Age

            nodePatientPopulationCriteria.AppendChild(BuildChildPreconditionNode(HQMFBoilerplateXmlDoc, ageCriteriaExtension, "observationReference", nsmgr));

            // Diagnosis (ICD-9 Diagnosis requests ONLY!)

            // Gender

            switch (m.Sex)
            {
                case (int)SexSelectionCode.Male:
                    nodePatientPopulationCriteria.AppendChild(BuildChildPreconditionNode(HQMFBoilerplateXmlDoc, "genderMale", "observationReference", nsmgr));
                    break;
                
                case (int)SexSelectionCode.Female:
                    nodePatientPopulationCriteria.AppendChild(BuildChildPreconditionNode(HQMFBoilerplateXmlDoc, "genderFemale", "observationReference", nsmgr));
                    break;

                case (int)SexSelectionCode.Both:
                    XmlElement elOuterPrecondition = HQMFBoilerplateXmlDoc.CreateElement("precondition", nsmgr.LookupNamespace("hl7"));
                    nodePatientPopulationCriteria.AppendChild(elOuterPrecondition);
                    XmlElement elOR = HQMFBoilerplateXmlDoc.CreateElement("atLeastOneTrue", nsmgr.LookupNamespace("hl7"));
                    elOuterPrecondition.AppendChild(elOR);
                    elOR.AppendChild(BuildChildPreconditionNode(HQMFBoilerplateXmlDoc, "genderMale", "observationReference", nsmgr));
                    elOR.AppendChild(BuildChildPreconditionNode(HQMFBoilerplateXmlDoc, "genderFemale", "observationReference", nsmgr));
                    break;
            }

            // Race

            nodePatientPopulationCriteria.AppendChild(BuildChildPreconditionNode(HQMFBoilerplateXmlDoc, "raceCriteria", "observationReference", nsmgr));

            //--------------------------------
            // Stratifier Criteria sub-section
            //--------------------------------

            // Get the stratifierCriteria node that we will insert nodes into

            nodeStratifierCriteria = nodePopulationCriteriaSection.SelectSingleNode("./hl7:entry/hl7:stratifierCriteria", nsmgr);

            // id node

            SetChildNodeAttributeValue(PMN_MDPHNet_OID, nodeStratifierCriteria, ".//hl7:id", "root", nsmgr);

            // Age

            if (reportOn.Contains(((int)ReportSelectionCode.Age).ToString()))
            {
                foreach (string extension in ageStratificationExtensions)
                {
                    nodeStratifierCriteria.AppendChild(BuildChildPreconditionNode(HQMFBoilerplateXmlDoc, extension, "observationReference", nsmgr));
                }
            }

            // Gender

            if (reportOn.Contains(((int)ReportSelectionCode.Sex).ToString()))
            {
                switch (m.Sex)
                {
                    case (int)SexSelectionCode.Male:
                        nodeStratifierCriteria.AppendChild(BuildChildPreconditionNode(HQMFBoilerplateXmlDoc, "genderMale", "observationReference", nsmgr));
                        break;

                    case (int)SexSelectionCode.Female:
                        nodeStratifierCriteria.AppendChild(BuildChildPreconditionNode(HQMFBoilerplateXmlDoc, "genderFemale", "observationReference", nsmgr));
                        break;

                    case (int)SexSelectionCode.Both:
                        nodeStratifierCriteria.AppendChild(BuildChildPreconditionNode(HQMFBoilerplateXmlDoc, "genderMale", "observationReference", nsmgr));
                        nodeStratifierCriteria.AppendChild(BuildChildPreconditionNode(HQMFBoilerplateXmlDoc, "genderFemale", "observationReference", nsmgr));
                        break;
                }
            }

            // Race

            if (reportOn.Contains(((int)ReportSelectionCode.Race).ToString()))
            {
                nodeStratifierCriteria.AppendChild(BuildChildPreconditionNode(HQMFBoilerplateXmlDoc, "raceCriteria", "observationReference", nsmgr));
            }

            //=====================
            // OUTPUT THE HQMF XML
            //=====================

            // Use an XmlTextWriter to "pretty print" the XML, so that it will be indented and easily readable.
            MemoryStream ms = new MemoryStream();
            XmlTextWriter xtw = new XmlTextWriter(ms, Encoding.UTF8);
            xtw.Formatting = Formatting.Indented;
            HQMFBoilerplateXmlDoc.WriteContentTo(xtw);
            xtw.Flush();
            ms.Flush();

            // Have to rewind the MemoryStream in order to read its contents.
            ms.Position = 0;

            // Read MemoryStream contents into a StreamReader.
            using (StreamReader sr = new StreamReader(ms))
            {
                // Extract the tet from the StreamReader.
                HQMFBytes = Encoding.UTF8.GetBytes(sr.ReadToEnd());
            }

            // Clean up.
            ms.Close();
            xtw.Close();

            return HQMFBytes;
        }

        private static void SetChildNodeInnerText(string innerText, XmlNode parentNode, string xpath, XmlNamespaceManager nsmgr)
        {
            XmlNodeList TargetNodes = parentNode.SelectNodes(xpath, nsmgr);
            if (TargetNodes.Count == 1)
            {
                TargetNodes[0].InnerText = innerText;
            }
        }

        private static void SetChildNodeAttributeValue(string value, XmlNode parentNode, string xpath, string attributeName, XmlNamespaceManager nsmgr)
        {
            XmlNodeList TargetNodes = parentNode.SelectNodes(xpath, nsmgr);
            if (TargetNodes.Count == 1)
            {
                ((XmlElement)TargetNodes[0]).SetAttribute(attributeName, value);
            }
        }

        private static XmlElement BuildChildPreconditionNode(XmlDocument doc, string extension, string referenceNodeName, XmlNamespaceManager nsmgr)
        {
            XmlElement elPrecondition = doc.CreateElement("precondition", nsmgr.LookupNamespace("hl7"));
            XmlElement elReference = doc.CreateElement(referenceNodeName, nsmgr.LookupNamespace("hl7"));
            XmlElement elId = doc.CreateElement("id", nsmgr.LookupNamespace("hl7"));

            elId.SetAttribute("root", PMN_MDPHNet_OID);
            elId.SetAttribute("extension", extension);
            
            elReference.AppendChild(elId);
            elPrecondition.AppendChild(elReference);
            return elPrecondition;
        }
    }
}
