using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lpp.Dns.DataMart.Model.Processors.SAS.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string emptyFileText = "Empty SAS Result File";
            StringBuilder messages = new StringBuilder();
            try
            {
                messages.AppendLine("Beginiing SAS Program");
                if (args.Length >= 2)
                {
                    string programFile = args[0];
                    string outputPath = args[1];
                    if (Directory.Exists(outputPath))
                    {
                        string resultFile = outputPath + "\\result.txt";
                        if (File.Exists(resultFile))
                            File.Delete(resultFile);
                        StreamWriter sw = File.CreateText(resultFile);
                        sw.Write(emptyFileText);
                        sw.Close();
                        messages.AppendLine("SAS Processed and Result Created.");
                        messages.AppendLine("SAS Program Ended with success.");
                    }
                    else
                        messages.AppendLine("Specified output path- " + outputPath + " cannot be found.");
                }
                else if (args.Length == 1)
                    messages.AppendLine("Output Path Not Supplied.");
                else 
                    messages.AppendLine("Program File and Output Path Not Supplied.");

                messages.AppendLine("SAS Program Ended for invalid inputs.");
            }
            catch (Exception ex)
            {
                messages.AppendLine(ex.Message);
                messages.AppendLine("SAS Program Ended with error");
            }
            finally
            {
                Console.WriteLine(messages.ToString());
            }
        }
    }
}
