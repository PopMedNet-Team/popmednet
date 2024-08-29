using System;
using System.IO;
using PopMedNet.TrxToHtml.Parser;

namespace PopMedNet.TrxToHtml
{
	public static class ReportGenerator
	{
		public static void GenerateReport(string fileName)
		{
			SupportedFormats fileVersion = new VersionFinder().GetFileVersion(fileName);
			if (fileVersion != SupportedFormats.vs2010)
			{
				Console.WriteLine("File {0} is not a recognized as a valid trx. Only VS2010 are supported", fileName);
				return;
			}
			Console.WriteLine("Processing {0} trx file", fileVersion.ToString());
			string html = new HtmlConverter(new TrxParser().ParseCurrentRun(fileName)).GetHtml();
			using (TextWriter textWriter = File.CreateText(fileName + ".htm"))
			{
				textWriter.Write(html);
			}
			Console.WriteLine("Tranformation Succeed. OutputFile: " + fileName + ".htm\n");
		}

		public static void GenerateReport(string fileName, string previousRun)
		{
			SupportedFormats fileVersion = new VersionFinder().GetFileVersion(fileName);
			if (fileVersion != SupportedFormats.vs2010)
			{
				Console.WriteLine("File {0} is not a recognized as a valid trx. Only VS2010 are supported", fileName);
				return;
			}
			Console.WriteLine("Processing {0} trx file", fileVersion.ToString());
			TestRunResult currentRunResult = new TrxParser().ParseCurrentRun(fileName);
			TestRunResult previousRunResult = new TrxParser().ParsePreviousRun(previousRun);
			string html = new HtmlConverter(currentRunResult, previousRunResult).GetHtml();
			using (TextWriter textWriter = File.CreateText(fileName + ".htm"))
			{
				textWriter.Write(html);
			}
			Console.WriteLine("Tranformation Succeed. OutputFile: " + fileName + ".htm\n");
		}
	}
}
