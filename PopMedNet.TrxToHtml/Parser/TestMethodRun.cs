using System;
using System.Collections.Generic;

namespace PopMedNet.TrxToHtml.Parser
{
	public class TestMethodRun : IEqualityComparer<TestMethodRun>
	{
		private string description = string.Empty;

		private string status = string.Empty;

		public string TestClass { get; set; }

		public string TestMethodName { get; set; }

		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				description = value;
			}
		}

		public string Message { get; set; }

		public TimeSpan Duration { get; set; }

		public string StackTrace { get; set; }

		public string Status { get; set; }

		public ErrorInfo ErrorInfo { get; set; }

		public string ComputerName { get; set; }

		public bool Equals(TestMethodRun x, TestMethodRun y)
		{
			return x.TestClass == y.TestClass;
		}

		public int GetHashCode(TestMethodRun obj)
		{
			return GetHashCode();
		}

		public TestMethodRun()
		{
			ErrorInfo = new ErrorInfo();
		}
	}
}
