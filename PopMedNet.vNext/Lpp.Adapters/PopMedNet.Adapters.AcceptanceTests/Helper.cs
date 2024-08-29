using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Adapters.AcceptanceTests
{
    [TestClass, Ignore]
    public class HelperTasks
    {
        [TestMethod, Ignore]
        public void ScaffoldTestsFromFolder()
        {
            StringBuilder sb = new StringBuilder();
            string sourceFolder = @".\Resources\PCORNet Queries";
            using (var writer = new StringWriter(sb))
            {
                foreach (var filename in Directory.EnumerateFiles(sourceFolder))
                {

                    /**
                            [DataTestMethod, DataRow("Age_Stratification_#1")]
                            public virtual void Age_Stratification_1(string filename)
                            {
                                var result = RunRequest(filename);
                                
                                //TODO: Asserts as directed by HPHCI
                            }
                    **/

                    writer.WriteLine($"\t\t[DataTestMethod, DataRow(\"{ Path.GetFileNameWithoutExtension(filename) }\")]");
                    writer.WriteLine($"\t\tpublic virtual void { Path.GetFileNameWithoutExtension(filename).Replace("#","") }(string filename)");
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tvar result = RunRequest(filename);");
                    writer.WriteLine("");
                    writer.WriteLine("\t\t\t//TODO: Asserts as directed by HPHCI");
                    writer.WriteLine("\t\t}");
                    writer.WriteLine("");
                }

                writer.Flush();
            }

            File.WriteAllText(@".\test-skeletons.txt", sb.ToString());

        }

    }
}
