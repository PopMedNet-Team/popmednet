using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PopMedNet.TrxToHtml.Parser
{
	public class TestRunResult : I3ValueBar
	{
		public IEnumerable<TestMethodRun> TestMethodRunList;

		public List<TestClassRun> TestClassList;

		public string RunInfo { get; set; }

		public int TotalMethods
		{
			get
			{
				return TestMethodRunList.Count();
			}
		}

		public IEnumerable<string> Computers
		{
			get
			{
				return TestMethodRunList.Select((TestMethodRun t) => t.ComputerName).Distinct();
			}
		}

		public int Passed
		{
			get
			{
				return TestMethodRunList.Where((TestMethodRun t) => t.Status == "Passed").Count();
			}
		}

		public int Failed
		{
			get
			{
				return TestMethodRunList.Where((TestMethodRun t) => t.Status == "Failed").Count();
			}
		}

		public int Inconclusive
		{
			get
			{
				return TestMethodRunList.Where((TestMethodRun t) => t.Status == "Inconclusive" || t.Status == "NotRunnable" || t.Status == "Aborted" || t.Status == "NotExecuted").Count();
			}
		}

		public double TotalPercent
		{
			get
			{
				if (TestClassList.Count > 0)
				{
					return Math.Round(TestClassList.Average((TestClassRun c) => c.Percent));
				}
				return 0.0;
			}
		}

		public double PercentOK
		{
			get
			{
				return Math.Round(100.0 * ((double)Passed / (double)TotalMethods), 0);
			}
		}

		public double PercentKO
		{
			get
			{
				return Math.Round(100.0 * ((double)Failed / (double)TotalMethods), 0);
			}
		}

		public double PercentIgnored
		{
			get
			{
				return Math.Round(100.0 * ((double)Inconclusive / (double)TotalMethods), 0);
			}
		}

		public TimeSpan TimeTaken
		{
			get
			{
				return TimeSpan.FromTicks(TestClassList.Sum((TestClassRun c) => c.Duration.Ticks));
			}
		}

		public string Name { get; set; }

		public IEnumerable<AssemblyName> Assemblies
		{
			get
			{
				return TestClassList.Select((TestClassRun c) => c.AssemblyName).Distinct(new AssemblyNameComparer());
			}
		}

		public string UserName { get; set; }

		public IEnumerable<TestMethodRun> TopSlowerMethods
		{
			get
			{
				return TestMethodRunList.OrderByDescending((TestMethodRun m) => m.Duration).Take(5);
			}
		}

		public TestRunResult(string name, string runUser, IEnumerable<TestMethodRun> items)
		{
			Name = name;
			UserName = runUser;
			TestMethodRunList = new List<TestMethodRun>(items);
			TestClassList = new List<TestClassRun>();
			foreach (string item in from t in TestMethodRunList.Distinct(new TestMethodRun())
									select t.TestClass)
			{
				string c = item;
				TestClassList.Add(new TestClassRun(c, from t in items
													  where t.TestClass == c
													  select (t)));
			}
		}
	}
}
