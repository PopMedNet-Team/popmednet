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

namespace Lpp.Dns.General.Metadata.RequestMetadataCollection
{
    public class DataMartMetadata
    {
        public static void Export(XmlWriter xmlWriter, IEnumerable<DataMart> dataMarts, Lpp.Dns.Portal.IPluginService plugins)
        {
            try
            {
                xmlWriter.WriteStartDocument(true);
                xmlWriter.WriteStartElement("datamarts", "urn://popmednet/datamarts/metadata");
                xmlWriter.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                xmlWriter.WriteAttributeString("xmlns", "xs", null, "http://www.w3.org/2001/XMLSchema");

                foreach(var d in dataMarts)
                {
                    DataMart dm = d;
                    xmlWriter.WriteStartElement("datamart");
                    #region DataMart Header
                    xmlWriter.WriteStartElement("header");

                    InsertStringValue(xmlWriter, "id", dm.ID.ToString());
                    InsertStringValue(xmlWriter, "name", dm.Name);
                    InsertStringValue(xmlWriter, "acronym", dm.Acronym);
                    InsertStringValue(xmlWriter, "organization", dm.Organization.Name);
                    InsertStringValue(xmlWriter, "contactfullname", dm.ContactFirstName + " " + dm.ContactLastName);
                    InsertStringValue(xmlWriter, "contactemail", dm.ContactEmail);
                    InsertStringValue(xmlWriter, "healthplandescription", dm.HealthPlanDescription);
                    InsertStringValue(xmlWriter, "collabrequest", dm.SpecialRequirements);
                    InsertStringValue(xmlWriter, "datamodel", dm.DataModel);
                    InsertStringValue(xmlWriter, "datamodelother", dm.OtherDataModel);
                    InsertStringValue(xmlWriter, "dataperiodstartyear", dm.StartDate.HasValue ? dm.StartDate.Value.ToShortDateString() : "");
                    InsertStringValue(xmlWriter, "dataperiodendyear", dm.EndDate.HasValue ? dm.EndDate.Value.ToShortDateString() : "");
                    InsertStringValue(xmlWriter, "dataupdatefrequency", dm.DataUpdateFrequency);

                    xmlWriter.WriteEndElement(); // end header element                    
                    #endregion

                    #region Data Domains

                    xmlWriter.WriteStartElement("datadomains");

                    //InPatient Encounters
                    xmlWriter.WriteStartElement("inpatientencounters");
                    InsertStringValue(xmlWriter, "value", dm.InpatientEncountersAny.ToString());
                    InsertStringValue(xmlWriter, "encounterid", dm.InpatientEncountersEncounterID.ToString());
                    InsertStringValue(xmlWriter, "dateofservice", dm.InpatientDatesOfService.ToString());
                    InsertStringValue(xmlWriter, "providerfacilityid", dm.InpatientEncountersProviderIdentifier.ToString());
                    InsertStringValue(xmlWriter, "icd9diagnosis", dm.InpatientICD9Diagnosis.ToString());
                    InsertStringValue(xmlWriter, "icd10diagnosis", dm.InpatientICD10Diagnosis.ToString());
                    InsertStringValue(xmlWriter, "icd9procedure", dm.InpatientICD9Procedures.ToString());
                    InsertStringValue(xmlWriter, "icd10procedure", dm.InpatientICD10Procedures.ToString());
                    InsertStringValue(xmlWriter, "dischargestatus", dm.InpatientDischargeStatus.ToString());
                    InsertStringValue(xmlWriter, "disposition", dm.InpatientDisposition.ToString());
                    InsertStringValue(xmlWriter, "hphcs", dm.InpatientHPHCS.ToString());
                    InsertStringValue(xmlWriter, "snomed", dm.InpatientSNOMED.ToString());
                    InsertStringValueWithAttribute(xmlWriter, "other", dm.InpatientOther.ToString(), "name", dm.InpatientOtherText);
                    xmlWriter.WriteEndElement(); // end inpatientencounters element                    

                    //outpatient Encounters
                    xmlWriter.WriteStartElement("outpatientencounters");
                    InsertStringValue(xmlWriter, "value", dm.OutpatientEncountersAny.ToString());
                    InsertStringValue(xmlWriter, "encounterid", dm.OutpatientEncountersEncounterID.ToString());
                    InsertStringValue(xmlWriter, "dateofservice", dm.OutpatientDatesOfService.ToString());
                    InsertStringValue(xmlWriter, "providerfacilityid", dm.OutpatientEncountersProviderIdentifier.ToString());
                    InsertStringValue(xmlWriter, "icd9diagnosis", dm.OutpatientICD9Diagnosis.ToString());
                    InsertStringValue(xmlWriter, "icd10diagnosis", dm.OutpatientICD10Diagnosis.ToString());
                    InsertStringValue(xmlWriter, "icd9procedure", dm.OutpatientICD9Procedures.ToString());
                    InsertStringValue(xmlWriter, "icd10procedure", dm.OutpatientICD10Procedures.ToString());
                    InsertStringValue(xmlWriter, "hphcs", dm.OutpatientHPHCS.ToString());
                    InsertStringValue(xmlWriter, "snomed", dm.OutpatientSNOMED.ToString());
                    InsertStringValueWithAttribute(xmlWriter, "other", dm.OutpatientOther.ToString(), "name", dm.OutpatientOtherText);
                    xmlWriter.WriteEndElement(); // end outpatientencounters element                    

                    //enrollment Encounters
                    //xmlWriter.WriteStartElement("enrollmentencounters");
                    //InsertStringValue(xmlWriter, "patientid", dm.ERPatientID.ToString());
                    //InsertStringValue(xmlWriter, "encounterid", dm.EREncounterID.ToString());
                    //InsertStringValue(xmlWriter, "enrollmentdates", dm.ERICD9Diagnosis.ToString());
                    //InsertStringValue(xmlWriter, "encounterdates", dm.ERICD10Diagnosis.ToString());
                    //InsertStringValue(xmlWriter, "clinicalsettings", dm.ERClinicalSetting.ToString());
                    //InsertStringValue(xmlWriter, "icd9diagnosis", dm.ERICD9Diagnosis.ToString());
                    //InsertStringValue(xmlWriter, "icd10diagnosis", dm.ERICD10Diagnosis.ToString());
                    //InsertStringValue(xmlWriter, "hphcs", dm.ERHPHCS.ToString());
                    //InsertStringValue(xmlWriter, "ndc", dm.ERNDC.ToString());
                    //InsertStringValue(xmlWriter, "snomed", dm.ERSNOMED.ToString());
                    //InsertStringValue(xmlWriter, "provideridentiier", dm.ERProviderIdentifier.ToString());
                    //InsertStringValue(xmlWriter, "providerfacility", dm.ERProviderFacility.ToString());
                    //InsertStringValue(xmlWriter, "encountertype", dm.EREncounterType.ToString());
                    //InsertStringValue(xmlWriter, "drg", dm.ERDRG.ToString());
                    //InsertStringValue(xmlWriter, "drgtype", dm.ERDRGType.ToString());
                    //InsertStringValueWithAttribute(xmlWriter, "other", dm.EROther.ToString(), "name", dm.EROtherText);
                    //xmlWriter.WriteEndElement(); // end enrollmentencounters element                    

                    //Laboratory Results
                    xmlWriter.WriteStartElement("laboratoryresults");
                    InsertStringValue(xmlWriter, "value", dm.LaboratoryResultsAny.ToString());
                    InsertStringValue(xmlWriter, "orderdates", dm.LaboratoryResultsOrderDates.ToString());
                    InsertStringValue(xmlWriter, "resultdates", dm.LaboratoryResultsDates.ToString());
                    InsertStringValue(xmlWriter, "testname", dm.LaboratoryResultsTestName.ToString());
                    InsertStringValue(xmlWriter, "testdescription", dm.LaboratoryResultsTestDescriptions.ToString());
                    InsertStringValue(xmlWriter, "testloinc", dm.LaboratoryResultsTestLOINC.ToString());
                    InsertStringValue(xmlWriter, "testsnomed", dm.LaboratoryResultsTestSNOMED.ToString());
                    InsertStringValue(xmlWriter, "testresultsinterpretation", dm.LaboratoryResultsTestResultsInterpretation.ToString());
                    InsertStringValueWithAttribute(xmlWriter, "testother", dm.LaboratoryResultsTestOther.ToString(), "name", dm.LaboratoryResultsTestOtherText);
                    xmlWriter.WriteEndElement(); // end laboratoryresults element                    

                    //prescription orders
                    xmlWriter.WriteStartElement("prescriptionorders");
                    InsertStringValue(xmlWriter, "value", dm.PrescriptionOrdersAny.ToString());
                    InsertStringValue(xmlWriter, "ndc", dm.PrescriptionOrderNDC.ToString());
                    InsertStringValue(xmlWriter, "rxnorm", dm.PrescriptionOrderRxNorm.ToString());
                    InsertStringValueWithAttribute(xmlWriter, "other", dm.PrescriptionOrderOther.ToString(), "name", dm.PrescriptionOrderOtherText);
                    xmlWriter.WriteEndElement(); // end prescription orders element                    

                    //pharmacy dispensings
                    xmlWriter.WriteStartElement("pharmacydispensings");
                    InsertStringValue(xmlWriter, "value", dm.PharmacyDispensingAny.ToString());
                    InsertStringValue(xmlWriter, "dates", dm.PharmacyDispensingDates.ToString());
                    InsertStringValue(xmlWriter, "ndc", dm.PharmacyDispensingNDC.ToString());
                    InsertStringValue(xmlWriter, "rxnorm", dm.PharmacyDispensingRxNorm.ToString());
                    InsertStringValue(xmlWriter, "dayssupply", dm.PharmacyDispensingDaysSupply.ToString());
                    InsertStringValue(xmlWriter, "amountdispensed", dm.PharmacyDispensingAmountDispensed.ToString());
                    InsertStringValueWithAttribute(xmlWriter, "other", dm.PharmacyDispensingOther.ToString(), "name", dm.PharmacyDispensingOtherText);
                    xmlWriter.WriteEndElement(); // end pharmacy dispensings element                    

                    //demographics
                    xmlWriter.WriteStartElement("demographics");
                    InsertStringValue(xmlWriter, "value", dm.DemographicsAny.ToString());
                    InsertStringValue(xmlWriter, "patientid", dm.DemographicsPatientID.ToString());
                    InsertStringValue(xmlWriter, "sex", dm.DemographicsSex.ToString());
                    InsertStringValue(xmlWriter, "dateofbirth", dm.DemographicsDateOfBirth.ToString());
                    InsertStringValue(xmlWriter, "dateofdeath", dm.DemographicsDateOfDeath.ToString());
                    InsertStringValue(xmlWriter, "address", dm.DemographicsAddressInfo.ToString());
                    InsertStringValue(xmlWriter, "race", dm.DemographicsRace.ToString());
                    InsertStringValue(xmlWriter, "ethnicity", dm.DemographicsEthnicity.ToString());
                    InsertStringValueWithAttribute(xmlWriter, "other", dm.DemographicsOther.ToString(), "name", dm.DemographicsOtherText);
                    xmlWriter.WriteEndElement(); // end demographics element                    


                    //patient reported outcomes
                    xmlWriter.WriteStartElement("patientreportedoutcomes");
                    InsertStringValue(xmlWriter, "value", dm.PatientOutcomesAny.ToString());
                    InsertStringValue(xmlWriter, "healthbehavior", dm.PatientOutcomesHealthBehavior.ToString());
                    InsertStringValue(xmlWriter, "hrqol", dm.PatientOutcomesHRQoL.ToString());
                    InsertStringValue(xmlWriter, "reportedoutcome", dm.PatientOutcomesReportedOutcome.ToString());
                    //InsertStringValueWithAttribute(xmlWriter, "instruments", dm.PatientOutcomesInstruments.ToString(), "name", dm.PatientOutcomesInstrumentText);
                    InsertStringValueWithAttribute(xmlWriter, "other", dm.PatientOutcomesOther.ToString(), "name", dm.PatientOutcomesOtherText);
                    xmlWriter.WriteEndElement(); // end patient reported outcomes element                    

                    //patient reported behavior
                    //xmlWriter.WriteStartElement("patientreportedbehavior");
                    //InsertStringValue(xmlWriter, "healthbehavior", dm.PatientOutcomesHealthBehavior.ToString());
                    //InsertStringValueWithAttribute(xmlWriter, "instruments", dm.PatientBehaviorInstruments.ToString(), "name", dm.PatientBehaviorInstrumentText);
                    //InsertStringValueWithAttribute(xmlWriter, "other", dm.PatientBehaviorOther.ToString(), "name", dm.PatientBehaviorOtherText);
                    //xmlWriter.WriteEndElement(); // end patient reported behavior element                    

                    //vital signs
                    xmlWriter.WriteStartElement("vitalsigns");
                    InsertStringValue(xmlWriter, "value", dm.VitalSignsAny.ToString());
                    InsertStringValue(xmlWriter, "temperature", dm.VitalSignsTemperature.ToString());
                    InsertStringValue(xmlWriter, "height", dm.VitalSignsHeight.ToString());
                    InsertStringValue(xmlWriter, "weight", dm.VitalSignsWeight.ToString());
                    InsertStringValue(xmlWriter, "bmi", dm.VitalSignsBMI.ToString());
                    InsertStringValue(xmlWriter, "bloodpressure", dm.VitalSignsBloodPressure.ToString());
                    InsertStringValueWithAttribute(xmlWriter, "other", dm.VitalSignsOther.ToString(), "name", dm.VitalSignsOtherText);
                    xmlWriter.WriteEndElement(); // end vital signs outcomes element    

                    //longtitudinal capture
                    xmlWriter.WriteStartElement("longtitudinalcapture");
                    InsertStringValue(xmlWriter, "value", dm.LongitudinalCaptureAny.ToString());
                    InsertStringValue(xmlWriter, "patientid", dm.LongitudinalCapturePatientID.ToString());
                    InsertStringValue(xmlWriter, "capturestart", dm.LongitudinalCaptureStart.ToString());
                    InsertStringValue(xmlWriter, "capturestop", dm.LongitudinalCaptureStop.ToString());
                    InsertStringValue(xmlWriter, "other", dm.LongitudinalCaptureOther.ToString());
                    xmlWriter.WriteEndElement(); // end longtitudinal capture

                    //bio repositories
                    xmlWriter.WriteStartElement("biorepositories");
                    InsertStringValue(xmlWriter, "value", dm.BiorepositoriesAny.ToString());
                    //InsertStringValue(xmlWriter, "name", dm.BiorepositoriesName.ToString());
                    //InsertStringValue(xmlWriter, "description", dm.BiorepositoriesDescription.ToString());
                    //InsertStringValue(xmlWriter, "diseasename", dm.BiorepositoriesDiseaseName.ToString());
                    //InsertStringValue(xmlWriter, "specimensource", dm.BiorepositoriesSpecimenSource.ToString());
                    //InsertStringValue(xmlWriter, "specimenttype", dm.BiorepositoriesSpecimenType.ToString());
                    //InsertStringValue(xmlWriter, "processingmethod", dm.BiorepositoriesProcessingMethod.ToString());
                    //InsertStringValue(xmlWriter, "snomed", dm.BiorepositoriesSNOMED.ToString());
                    //InsertStringValue(xmlWriter, "storagemethod", dm.BiorepositoriesStorageMethod.ToString());
                    //InsertStringValueWithAttribute(xmlWriter, "other", dm.BiorepositoriesOther.ToString(), "name", dm.BiorepositoriesOtherText);
                    xmlWriter.WriteEndElement(); // end bio repositories outcomes element                    

                    xmlWriter.WriteEndElement(); // end datadomain element                    

                    #endregion

                    #region Installed Models
                    xmlWriter.WriteStartElement("installedmodels");

                    var pluginModels = plugins.GetAllPlugins().SelectMany(p => p.Models.Select(m => new { p, m }));
                    foreach(var mm in dm.Models)
                    {
                        var m = mm;
                        IDnsModel model = null;
                        if (pluginModels.Any(pm => pm.m.ID == m.ModelID))
                        {
                            model = pluginModels.Where(pm => pm.m.ID == m.ModelID).Select(pm => pm.m).FirstOrDefault();
                            InsertStringValue(xmlWriter, "name", model.Name);
                        }
                    };

                    xmlWriter.WriteEndElement(); // end installed models element
                    #endregion

                    xmlWriter.WriteEndElement(); // end data mart element

                    xmlWriter.Flush();
                };
                xmlWriter.WriteEndElement(); // end datamarts element
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
                //if (elementName == "name")
                //    Log.Error("Null name");
                xmlWriter.WriteStartElement(elementName);
                xmlWriter.WriteAttributeString("xsi", "nil", null, "true");
                xmlWriter.WriteEndElement();
            }
        }

        private static void InsertStringValueWithAttribute(XmlWriter xmlWriter, string elementName, String stringValue, string attributeName, string attributeValue)
        {
            if (stringValue != null && !stringValue.NullOrEmpty())
            {
                xmlWriter.WriteStartElement(elementName);
                if (attributeValue != null && !attributeValue.NullOrEmpty())
                {
                    //xmlWriter.WriteStartAttribute(attributeName);
                    xmlWriter.WriteAttributeString(attributeName, attributeValue);
                }
                xmlWriter.WriteString(stringValue);
                xmlWriter.WriteEndElement();
            }
            else
            {
                //if (elementName == "name")
                //    Log.Error("Null name");
                xmlWriter.WriteStartElement(elementName);
                xmlWriter.WriteAttributeString("xsi", "nil", null, "true");
                xmlWriter.WriteEndElement();
            }
        }
    }
}