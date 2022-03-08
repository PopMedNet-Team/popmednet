using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace PopMedNet.TrxToHtml.Parser
{
	public class TrxParser
	{
		private XNamespace ns = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010";		

		public TestRunResult ParseCurrentRun(string filePath)
		{
			XDocument xDocument = XDocument.Load(filePath);
			XNamespace ns = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010";
			string value = xDocument.Document.Root.Attribute("name").Value;
			string value2 = xDocument.Document.Root.Attribute("runUser").Value;
			List<XElement> source = xDocument.Descendants(ns + "UnitTest").ToList();
			List<XElement> inner = xDocument.Descendants(ns + "UnitTestResult").ToList();
			IEnumerable<TestMethodRun> items = from u in source
											   let id = u.Element(ns + "Execution").Attribute("id").Value
											   let desc = GetSafeValue(u, ns + "Description")
											   let testClass = GetSafeAttrValue(u, ns + "TestMethod", "className")
											   join r in inner on id equals r.Attribute("executionId").Value
											   orderby testClass
											   select new TestMethodRun
											   {
												   TestClass = testClass,
												   TestMethodName = u.Attribute("name").Value,
												   Description = desc,
												   Status = r.Attribute("outcome").Value,
												   Duration = ParseDuration(r, "duration"),
												   ErrorInfo = ParseErrorInfo(r),
												   ComputerName = r.Attribute("computerName").Value
											   };
			return new TestRunResult(value, value2, items);
		}

		public TestRunResult ParsePreviousRun(string filePath)
		{
			XDocument xDocument = XDocument.Load(filePath);
			XNamespace ns = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010";
			string value = xDocument.Document.Root.Attribute("name").Value;
			string value2 = xDocument.Document.Root.Attribute("runUser").Value;
			List<XElement> source = xDocument.Descendants(ns + "UnitTest").ToList();
			List<XElement> inner = xDocument.Descendants(ns + "UnitTestResult").ToList();
			IEnumerable<TestMethodRun> items = from u in source
											   let id = u.Element(ns + "Execution").Attribute("id").Value
											   let desc = GetSafeValue(u, ns + "Description")
											   let testClass = GetSafeAttrValue(u, ns + "TestMethod", "className")
											   join r in inner on id equals r.Attribute("executionId").Value
											   orderby testClass
											   select new TestMethodRun
											   {
												   TestClass = testClass,
												   TestMethodName = u.Attribute("name").Value,
												   Description = desc,
												   Status = r.Attribute("outcome").Value,
												   Duration = ParseDuration(r, "duration"),
												   ErrorInfo = ParseErrorInfo(r),
												   ComputerName = r.Attribute("computerName").Value
											   };
			return new TestRunResult(value, value2, items);
		}

		ErrorInfo ParseErrorInfo(XElement r)
		{
			ErrorInfo errorInfo = new ErrorInfo();
			if (r.Descendants(ns + "Output").Any() && r.Descendants(ns + "Output").Descendants(ns + "ErrorInfo").Any() && r.Descendants(ns + "Output").Descendants(ns + "ErrorInfo").Descendants(ns + "Message")
				.Any())
			{
				errorInfo.Message = r.Descendants(ns + "Output").Descendants(ns + "ErrorInfo").Descendants(ns + "Message")
					.FirstOrDefault()
					.Value;
			}
			if (r.Descendants(ns + "StackTrace").Any())
			{
				errorInfo.StackTrace = r.Descendants(ns + "StackTrace").FirstOrDefault().Value;
			}
			if (r.Descendants(ns + "DebugTrace").Any())
			{
				errorInfo.StdOut = r.Descendants(ns + "DebugTrace").FirstOrDefault().Value.Replace("\r\n", "<br />");
			}
			return errorInfo;
		}

		string GetSafeValue(XElement el, XName name)
		{
			string result = string.Empty;
			if (el.Element(name) != null)
			{
				result = el.Element(name).Value;
			}
			return result;
		}

		string GetSafeAttrValue(XElement el, XName name, XName atribName)
		{
			string result = string.Empty;
			if (el.Element(name) != null && el.Element(name).Attribute(atribName) != null)
			{
				result = el.Element(name).Attribute(atribName).Value;
			}
			return result;
		}

		TimeSpan ParseDuration(XElement el, string attName)
		{
			TimeSpan result = new TimeSpan(0L);
			if (el.Attribute(attName) != null)
			{
				return TimeSpan.Parse(el.Attribute(attName).Value);
			}
			return result;
		}
	}
}
