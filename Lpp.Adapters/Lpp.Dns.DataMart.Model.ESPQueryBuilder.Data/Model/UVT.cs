using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.ESPQueryBuilder.Model
{
    public abstract class UVT<T>
    {
        [Column("item_code")]
        public abstract T Code { get; set; }

        [Column("item_text")]
        public string Text { get; set; }
    }

    [Table("uvt_agegroup_10yr", Schema = "esp_mdphnet")]
    public class UVT_AgeGroup10yr : UVT<string>
    {
        [Key]
        public override string Code { get; set; }
    }

    [Table("uvt_agegroup_5yr", Schema = "esp_mdphnet")]
    public class UVT_AgeGroup5yr : UVT<string>
    {
        [Key]
        public override string Code { get; set; }
    }

    [Table("uvt_agegroup_ms", Schema = "esp_mdphnet")]
    public class UVT_AgeGroupMS : UVT<string>
    {
        [Key]
        public override string Code { get; set; }
    }

    [Table("uvt_center", Schema = "esp_mdphnet")]
    public class UVT_Center : UVT<string>
    {
        [Key]
        public override string Code { get; set; }
    }

    [Table("uvt_detected_condition", Schema = "esp_mdphnet")]
    public class UVT_DetectedCondition : UVT<string>
    {
        [Key]
        public override string Code { get; set; }
    }

    [Table("uvt_detected_criteria", Schema = "esp_mdphnet")]
    public class UVT_DetectedCriteria : UVT<string>
    {
        [Key]
        public override string Code { get; set; }
    }

    [Table("uvt_detected_status", Schema = "esp_mdphnet")]
    public class UVT_DetectedStatus : UVT<string>
    {
        [Key]
        public override string Code { get; set; }
    }

    [Table("uvt_dx", Schema = "esp_mdphnet")]
    public class UVT_Dx : UVT<string>
    {
        [Key]
        public override string Code { get; set; }
    }

    [Table("uvt_dx_3dig", Schema = "esp_mdphnet")]
    public class UVT_Dx3Digit : UVT<string>
    {
        [Key]
        public override string Code { get; set; }
    }

    [Table("uvt_dx_4dig", Schema = "esp_mdphnet")]
    public class UVT_Dx4Digit : UVT<string>
    {
        public override string Code { get; set; }

        [Key, Column("item_code_with_dec")]
        public string CodeWithDecimal { get; set; }
    }

    [Table("uvt_dx_5dig", Schema = "esp_mdphnet")]
    public class UVT_Dx5Digit : UVT<string>
    {
        public override string Code { get; set; }

        [Key, Column("item_code_with_dec", Order = 1)]
        public string CodeWithDecimal { get; set; }
    }

    [Table("uvt_encounter", Schema = "esp_mdphnet")]
    public class UVT_Encounter : UVT<string>
    {
        [Key]
        public override string Code { get; set; }
    }

    [Table("uvt_period", Schema = "esp_mdphnet")]
    public class UVT_Period : UVT<int>
    {
        [Key]
        public override int Code { get; set; }
    }

    [Table("uvt_provider", Schema = "esp_mdphnet")]
    public class UVT_Provider : UVT<string>
    {
        [Key]
        public override string Code { get; set; }
    }

    [Table("uvt_race", Schema = "esp_mdphnet")]
    public class UVT_Race : UVT<int>
    {
        [Key]
        public override int Code { get; set; }
    }

    [Table("uvt_sex", Schema = "esp_mdphnet")]
    public class UVT_Sex : UVT<string>
    {
        [Key, Column("item_code")]
        public override string Code { get; set; }
    }

    [Table("uvt_site", Schema = "esp_mdphnet")]
    public class UVT_Site : UVT<string>
    {
        [Key]
        public override string Code { get; set; }
    }

    [Table("uvt_zip5", Schema = "esp_mdphnet")]
    public class UVT_Zip5 : UVT<string>
    {
        [Key]
        public override string Code { get; set; }
    }

    [Table("uvt_race_ethnicity", Schema = "esp_mdphnet")]
    public class UVT_Race_Ethnicity : UVT<int>
    {
        [Key]
        public override int Code { get; set; }
    }


}
