using System;
using System.Reflection;

namespace PopMedNet.TrxToHtml
{
    public class Program
    {
        static void Main(string[] args)
        {
			Console.WriteLine("trx2html.exe \n  Create HTML reports of VSTS TestRuns. (c)rido'11");
			Console.WriteLine("version:" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + "\n");
			if (args.Length == 0)
			{
				Console.WriteLine("Usage: trx2html <TestResult>.trx");
			}
			else if (args.Length == 1)
			{
				
				ReportGenerator.GenerateReport(args[0]);
			}
			else if (args.Length == 2)
			{
				ReportGenerator.GenerateReport(args[0], args[1]);
			}
		}
    }
}
