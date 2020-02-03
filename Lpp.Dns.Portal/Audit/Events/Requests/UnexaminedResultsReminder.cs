using System;
using System.Linq;
using System.Runtime.InteropServices;
using Lpp.Audit;
using System.ComponentModel;
using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.Portal.Events
{
    [Guid("E39A0001-A4CA-46B8-B7EF-A22200E72B08")]
	[DisplayName( "Results Reminder" )]
	public class UnexaminedResultsReminder : RequestEventBase
	{
		public string DataMarts { get; set; }

		public UnexaminedResultsReminder( Request r )
			: base( r )
		{
			this.DataMarts = string.Join( ", ", from rt in r.DataMarts where rt.Status == RoutingStatus.Completed select rt.DataMart.Name );
		}
	}
}