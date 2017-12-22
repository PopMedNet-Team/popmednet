using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.PCORIQueryBuilder.Model
{
    [Table("VITAL")]
    public class Vital : Lpp.Objects.Entity
    {
        [Key, Column("VITALID")]
        public string ID { get; set; }

        [Column("PATID")]
        public string PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [Column("ENCOUNTERID")]
        public string EncounterID { get; set; }
        public virtual Encounter Encounter { get; set; }
        
        [Column("MEASURE_DATE")]
        public DateTime MeasuredOn { get; set; }

        /// <summary>
        /// Time of measurement in format HH:mm, can be null.
        /// </summary>
        [Column("MEASURE_TIME")]
        public string MeasuredTime { get; set; }

        [Column("VITAL_SOURCE")]
        public string Source { get; set; }

        [Column("HT")]
        public double? Height { get; set; }

        [Column("WT")]
        public double? Weight { get; set; }

        [Column("DIASTOLIC")]
        public double? DiastolicBloodPressure { get; set; }

        [Column("SYSTOLIC")]
        public double? SystolicBloodPressure { get; set; }

        [Column("ORIGINAL_BMI")]
        public double? BMI { get; set; }

        [Column("BP_POSITION")]
        public string BloodPressurePosition { get; set; }

        [Column("SMOKING")]
        public string Smoking { get; set; }

        [Column("TOBACCO")]
        public string Tobacco { get; set; }

        [Column("TOBACCO_TYPE")]
        public string TobaccoType { get; set; }
        
    }
}
