using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.PCORIQueryBuilder.Model
{
    [Table("PCORNET_TRIAL")]
    public class ClinicalTrial : Lpp.Objects.Entity
    {
        [Key, Column("PATID", Order = 0)]
        public string PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [Key, Column("TRIALID", Order = 1)]
        public string TrialID { get; set; }

        [Key, Column("PARTICIPANTID", Order = 2)]
        public string ParticipantID { get; set; }

        [Column("TRIAL_SITEID")]
        public string TrialCoordinatingCenter { get; set; }

        [Column("TRIAL_ENROLL_DATE")]
        public DateTime? EnrolledOn { get; set; }

        [Column("TRIAL_END_DATE")]
        public DateTime? EndedOn { get; set; }

        [Column("TRIAL_WITHDRAW_DATE")]
        public DateTime? WithdrewOn { get; set; }

        [Column("TRIAL_INVITE_CODE")]
        public string InvitationCode { get; set; }
    }
}
