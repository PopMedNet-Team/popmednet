using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects.ValidationAttributes;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// DTO for Organization
    /// </summary>
    [DataContract]
    public class OrganizationDTO : EntityDtoWithID
    {
        /// <summary>
        /// Organization Name
        /// </summary>
        [DataMember]
        [MaxLength(100)]
        public string Name { get; set; }
        /// <summary>
        ///Organization Acronym
        /// </summary>
        [DataMember]
        [MaxLength(100)]
        public string Acronym { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if Organization is Deleted 
        /// </summary>
        [DataMember]
        public bool Deleted { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if Primary Organization is selected 
        /// </summary>
        [DataMember]
        public bool Primary { get; set; }
        /// <summary>
        /// ID of Parent Organization
        /// </summary>
        [DataMember]
        public Guid? ParentOrganizationID { get; set; }
        /// <summary>
        /// Parent Organization
        /// </summary>
        [DataMember, ReadOnly(true)]
        public string ParentOrganization { get; set; }
        /// <summary>
        /// Contact Email
        /// </summary>
        [DataMember]
        public string ContactEmail { get; set; }
        /// <summary>
        /// Contact First Name
        /// </summary>
        [DataMember]
        public string ContactFirstName { get; set; }
        /// <summary>
        /// Contact Last Name
        /// </summary>
        [DataMember]
        public string ContactLastName { get; set; }
        /// <summary>
        /// Contact Phone
        /// </summary>
        [DataMember]
        public string ContactPhone { get; set; }
        /// <summary>
        /// Special Requirements
        /// </summary>
        [DataMember, MaxLength(1000)]
        public string SpecialRequirements { get; set; }
        /// <summary>
        /// Usage Restrictions
        /// </summary>
        [DataMember, MaxLength(1000)]
        public string UsageRestrictions { get; set; }
        /// <summary>
        /// Organization Description
        /// </summary>
        [DataMember]
        public string OrganizationDescription { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if Pragmatic clinical trials is selected
        /// </summary>
         [DataMember]
        public bool PragmaticClinicalTrials { get; set; }
        /// <summary>
         /// Gets or sets the indicator to specify if Observational participation is selected
        /// </summary>
        [DataMember]
        public bool ObservationalParticipation { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if Prospective trials is selected
        /// </summary>
        [DataMember]
        public bool ProspectiveTrials { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if claims and Billing is enabled
        /// </summary>

        [DataMember]
        public bool EnableClaimsAndBilling { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if EHR is enabled
       /// </summary>
        [DataMember]
        public bool EnableEHRA { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if registries are enabled
        /// </summary>
        [DataMember]
        public bool EnableRegistries { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if Data Model MSCDM is selected
        /// </summary>
        [DataMember]
        public bool DataModelMSCDM { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if Data Model HMORN VDW is selected
        /// </summary>
        [DataMember]
        public bool DataModelHMORNVDW { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if Data Model ESP is selected
        /// </summary>
        [DataMember]
        public bool DataModelESP { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if Data Model I2B2 is selected
        /// </summary>
        [DataMember]
        public bool DataModelI2B2 { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if Data Model OMOP is selected
        /// </summary>
        [DataMember]
        public bool DataModelOMOP { get; set; }

        /// <summary>
        /// Gets or sets the indicator to specify if Data Model Pcori is selected
        /// </summary>
        [DataMember]
        public bool DataModelPCORI { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if Data Model other is selected
        /// </summary>
        [DataMember]
        public bool DataModelOther { get; set; }
        /// <summary>
        /// Data Model Other Text
        /// </summary>
        [DataMember]
        public string DataModelOtherText { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if inpatient claims is selected
        /// </summary>

        [DataMember]
        public bool InpatientClaims { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if outpatient claims is selected
        /// </summary>
        [DataMember]
        public bool OutpatientClaims { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if outpatient pharmacy claims is selected
        /// </summary>
        [DataMember]
        public bool OutpatientPharmacyClaims { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if enrollment claims is selected
        /// </summary>
        [DataMember]
        public bool EnrollmentClaims { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if demographic claims is selected
        /// </summary>
               
        [DataMember]
        public bool DemographicsClaims { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if laboratory results claims is selected
        /// </summary>
        [DataMember]
        public bool LaboratoryResultsClaims { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if vital signs claims is selected
        /// </summary>
        [DataMember]
        public bool VitalSignsClaims { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if other claims is selected
        /// </summary>
        [DataMember]
        public bool OtherClaims { get; set; }
        /// <summary>
        /// Other Claims Text
        /// </summary>
        [DataMember]
        public string OtherClaimsText { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if Biorepositories is selected
        /// </summary>
        [DataMember]
        public bool Biorepositories { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if patient reported outcomes is selected
        /// </summary>
        [DataMember]
        public bool PatientReportedOutcomes { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if patient reported behaviors is selected
        /// </summary>
        [DataMember]
        public bool PatientReportedBehaviors { get; set; }
        /// <summary>
        /// Gets or sets the indicator to specify if prescription order is selected
        /// </summary>
        [DataMember]
        public bool PrescriptionOrders { get; set; }  
        /// <summary>
        /// Gets or sets X509 Key
        /// </summary>
        [DataMember]
        [MaxLength(255)]
        public string X509PublicKey { get; set; }
    }
}
