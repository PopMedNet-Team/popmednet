using Lpp.Dns.DTO.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data.Audit
{
    [Table("LogsRequestCommentChange")]
    public class RequestCommentChangeLog : ChangeLog
    {
        public RequestCommentChangeLog()
        {
            this.EventID = EventIdentifiers.Request.RequestCommentChange.ID;
        }

        [Key, Column(Order = 3)]
        public Guid CommentID { get; set; }
        public virtual Comment Comment { get; set; }
    }
}
