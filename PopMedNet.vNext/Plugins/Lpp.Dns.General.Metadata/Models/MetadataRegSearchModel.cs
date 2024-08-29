using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Lpp.Dns.General.Metadata.Models
{
    [DataContract]
    public class MetadataRegSearchModel
    {
        [DataMember]
        public string Data { get; set; }

        [XmlIgnore]
        public string RequestName { get; set; }

        [XmlIgnore]
        public MetadataSearchRequestType RequestType { get; set; }
    }

    [DataContract]
    public class MetadataRegSearchData
    {
        [DataMember]
        public string Name { get; set; }
        //Classification
        [DataMember]
        public bool ClassificationDiseaseDisorderCondition { get; set; }
        [DataMember]
        public bool ClassificationRareDiseaseDisorderCondition { get; set; }
        [DataMember]
        public bool ClassificationPregnancy { get; set; }
        [DataMember]
        public bool ClassificationProductBiologic { get; set; }
        [DataMember]
        public bool ClassificationProductDevice { get; set; }
        [DataMember]
        public bool ClassificationProductDrug { get; set; }
        [DataMember]
        public bool ClassificationServiceEncounter { get; set; }
        [DataMember]
        public bool ClassificationServiceHospitalization { get; set; }
        [DataMember]
        public bool ClassificationServiceProcedure { get; set; }
        [DataMember]
        public bool ClassificationTransplant { get; set; }
        [DataMember]
        public bool ClassificationTumor { get; set; }
        [DataMember]
        public bool ClassificationVaccine { get; set; }
        //Purpose
        [DataMember]
        public bool PurposeClinicalPracticeAssessment { get; set; }
        [DataMember]
        public bool PurposeEffectiveness { get; set; }
        [DataMember]
        public bool PurposeNaturalHistoryofDisease { get; set; }
        [DataMember]
        public bool PurposePaymentCertification { get; set; }
        [DataMember]
        public bool PurposePostMarketingCommitment { get; set; }
        [DataMember]
        public bool PurposePublicHealthSurveillance { get; set; }
        [DataMember]
        public bool PurposeQualityImprovement { get; set; }
        [DataMember]
        public bool PurposeSafetyOrHarm { get; set; }
        //Condition of Interest
        [DataMember]
        public bool ConditionOfInterestBacterialandFungalDiseases { get; set; }
        [DataMember]
        public bool ConditionOfInterestBehaviorsandMentalDisorders { get; set; }
        [DataMember]
        public bool ConditionOfInterestBloodandLymphConditions { get; set; }
        [DataMember]
        public bool ConditionOfInterestCancersAndOtherNeoplasms { get; set; }
        [DataMember]
        public bool ConditionOfInterestDigestiveSystemDiseases { get; set; }
        [DataMember]
        public bool ConditionOfInterestDiseasesOrAbnormalitiesAtOrBeforeBirth { get; set; }
        [DataMember]
        public bool ConditionOfInterestearNoseAndThroatDiseases { get; set; }
        [DataMember]
        public bool ConditionOfInterestEyeDiseases { get; set; }
        [DataMember]
        public bool ConditionOfInterestGlandAndHormoneRelatedDiseases { get; set; }
        [DataMember]
        public bool ConditionOfInterestHeartAndBloodDiseases { get; set; }
        [DataMember]
        public bool ConditionOfInterestImmuneSystemDiseases { get; set; }
        [DataMember]
        public bool ConditionOfInterestMouthAndToothDiseases { get; set; }
        [DataMember]
        public bool ConditionOfInterestMuscleBoneCartilageDiseases { get; set; }
        [DataMember]
        public bool ConditionOfInterestNervousSystemDiseases { get; set; }
        [DataMember]
        public bool ConditionOfInterestNutritionalAndMetabolicDiseases { get; set; }
        [DataMember]
        public bool ConditionOfInterestOccupationalDiseases { get; set; }
        [DataMember]
        public bool ConditionOfInterestParasiticalDiseases { get; set; }
        [DataMember]
        public bool ConditionOfInterestRespiratoryTractDiseases { get; set; }
        [DataMember]
        public bool ConditionOfInterestSkinAndConnectiveTissueDiseases { get; set; }
        [DataMember]
        public bool ConditionOfInterestSubstanceRelatedDisorders { get; set; }
        [DataMember]
        public bool ConditionOfInterestSymptomsAndGeneralPathology { get; set; }
        [DataMember]
        public bool ConditionOfInterestUrinaryTractSexualOrgansAndPregnancyConditions { get; set; }
        [DataMember]
        public bool ConditionOfInterestViralDiseases { get; set; }
        [DataMember]
        public bool ConditionOfInterestWoundsAndInjuries { get; set; }

    }
}
