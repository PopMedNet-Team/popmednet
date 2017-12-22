using Lpp.Dns.DTO;
using Lpp.Objects;
using Lpp.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data
{
    public class UserSetting : Entity
    {
        [Key, Column(Order = 1)]
        public Guid UserID { get; set; }
        public virtual User User { get; set; }

        [Key, Column(Order = 2)]
        public string Key { get; set; }

        public string Setting { get; set; }
    }


    internal class UserSettingDtoMappingConfiguration : EntityMappingConfiguration<UserSetting, UserSettingDTO>
    {
        public override System.Linq.Expressions.Expression<Func<UserSetting, UserSettingDTO>> MapExpression
        {
            get
            {
                return s => new UserSettingDTO
                {
                    Key = s.Key,
                    Setting = s.Setting,
                    UserID = s.UserID
                };
            }
        }
    }
}
