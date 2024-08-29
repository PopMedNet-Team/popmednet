using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PopMedNet.TrxToHtml.Parser
{
	public class TestClassRun : I3ValueBar
	{
		private string fullname;

		public IEnumerable<TestMethodRun> TestMethods;

		public string Name
		{
			get
			{
				if (fullname.IndexOf(',') > 0)
				{
					return fullname.Substring(0, fullname.IndexOf(','));
				}
				return fullname;
			}
		}

		public AssemblyName AssemblyName
		{
			get
			{
				int num = fullname.IndexOf(',');
				return new AssemblyName(fullname.Substring(num + 1, fullname.Length - num - 1));
			}
		}

		public double Percent
		{
			get
			{
				TestMethods.Count();
				double num = Math.Round((1.0 - Failed / ((double)Total - Ignored)) * 100.0, 2);
				if (double.IsNaN(num))
				{
					return 0.0;
				}
				return Math.Max(0.0, num);
			}
		}

		public double Success
		{
			get
			{
				return TestMethods.Where((TestMethodRun m) => m.Status == "Passed").Count();
			}
		}

		public double Failed
		{
			get
			{
				return TestMethods.Where((TestMethodRun m) => m.Status == "Failed").Count();
			}
		}

		public double Ignored
		{
			get
			{
				return TestMethods.Where((TestMethodRun m) => m.Status == "Inconclusive" || m.Status == "NotExecuted").Count();
			}
		}

		public string Status
		{
			get
			{
				string result = "Failed";
				IEnumerable<IGrouping<string, TestMethodRun>> source = from m in TestMethods
																	   group m by m.Status;
				int num = source.Where((IGrouping<string, TestMethodRun> k) => k.Key == "Failed").Count();
				int num2 = source.Where((IGrouping<string, TestMethodRun> k) => k.Key == "Ignored" || k.Key == "NotRunnable" || k.Key == "Aborted" || k.Key == "NotExecuted").Count();
				if (num2 > 0)
				{
					result = "Ignored";
				}
				if (num == 0 && num2 == 0)
				{
					result = "Succeed";
				}
				return result;
			}
		}

		public TimeSpan Duration
		{
			get
			{
				return TimeSpan.FromTicks(TestMethods.Sum((TestMethodRun m) => m.Duration.Ticks));
			}
		}

		public string FullName
		{
			get
			{
				return fullname;
			}
		}

		public int Total
		{
			get
			{
				return TestMethods.Count();
			}
		}

		public double PercentIgnored
		{
			get
			{
				return Math.Round(100.0 * (Ignored / (double)Total), 0);
			}
		}

		public double PercentKO
		{
			get
			{
				return Math.Round(100.0 * (Failed / (double)Total), 0);
			}
		}

		public double PercentOK
		{
			get
			{
				return Math.Round(100.0 * (Success / (double)Total), 0);
			}
		}

		public TestClassRun(string name, IEnumerable<TestMethodRun> methods)
		{
			fullname = name;
			TestMethods = new List<TestMethodRun>(methods);
		}

		public double Regressions(TestClassRun prevRun)
		{
			return TestMethods.Where((TestMethodRun m) => m.Status == "Failed" && prevRun.TestMethods.Where((TestMethodRun x) => x.TestMethodName == m.TestMethodName).Any((TestMethodRun x) => x.Status == "Passed")).Count();
		}
	}
}
