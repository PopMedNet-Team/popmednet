using System.Web;

namespace PopMedNet.TrxToHtml.Parser
{
	public class ErrorInfo
	{
		private string message = string.Empty;

		public string Message
		{
			get
			{
				return HttpUtility.HtmlEncode(message);
			}
			set
			{
				message = value;
			}
		}

		public string StackTrace { get; set; }

		public string StdOut { get; set; }

		public object StdErr { get; set; }
	}
}
