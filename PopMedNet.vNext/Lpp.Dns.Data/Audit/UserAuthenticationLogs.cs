using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Events;
using Lpp.Utilities;
using Lpp.Utilities.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lpp.Dns.Data.Audit
{
    [Table("LogsUserAuthentication")]
    public class UserAuthenticationLogs : AuditLog
    {
        public UserAuthenticationLogs()
        {
            this.EventID = EventIdentifiers.User.Authentication.ID;
            this.ID = DatabaseEx.NewGuid();
        }
        [Key, Column(Order = 3), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ID { get; set; }
        public bool Success { get; set; }
        [MaxLength(40)]
        public string IPAddress { get; set; }
        [MaxLength(50)]
        public string Environment { get; set; }
        [MaxLength(500)]
        public string Details { get; set; }
        [MaxLength(200)]
        public string Source { get; set; }
        [MaxLength(20)]
        public string DMCVersion { get; set; }
    }

    internal class UserAuthenticationLogsDTOMapping : EntityMappingConfiguration<UserAuthenticationLogs, UserAuthenticationDTO>
    {
        public override System.Linq.Expressions.Expression<Func<UserAuthenticationLogs, UserAuthenticationDTO>> MapExpression
        {
            get
            {
                return d => new UserAuthenticationDTO
                {
                    DateTime = d.TimeStamp,
                    ID = d.ID,
                    Description = d.Description,
                    IPAddress = d.IPAddress,
                    Environment = d.Environment,
                    Details = d.Success ? d.Details : "",
                    DMCVersion = d.DMCVersion
                };
            }
        }
    }
}
