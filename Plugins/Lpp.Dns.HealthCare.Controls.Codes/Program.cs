using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.HealthCare.Controls.Codes
{
    class Program
    {
        static void Main(string[] args)
        {
            string codeFile, categoryFile, sqlFile;

            if (args != null && args.Length >= 3)
            {
                codeFile = args[0];
                categoryFile = args[1];
                sqlFile = args[2];
                new CodeBuilder().BuildCodes(codeFile, categoryFile, sqlFile);
                Console.WriteLine("Code and category lists uploaded successfully ");
            }
            else
            {
                Console.WriteLine("Please provide fully qualified file names in the argument for code,category and sql ");
            }
        }
    }
}
