using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities;
using Lpp.Dns.Data;

namespace Lpp.Dns.Api.Tests
{
    [TestClass]
    public class ModelTermInterrogationTests
    {
        const string PackagesFolder = @"..\..\Lpp.Dns.Api\App_Data";

        /// <summary>
        /// Create a list of the most current packages to interrogate.
        /// </summary>
        [TestMethod]
        public void GetPackagesToInterrogate()
        {
            var packages = System.IO.Directory.GetFiles(PackagesFolder, "*.zip");
            if (packages == null || packages.Length == 0)
                Assert.Fail("No packages found at " + PackagesFolder);

            IEnumerable<string> identifiers;
            using (var db = new DataContext())
            {
                identifiers = db.RequestTypes.Where(rt => !string.IsNullOrEmpty(rt.PackageIdentifier)).Select(rt => rt.PackageIdentifier).Distinct().ToArray();
            }

            List<string> packagesToInterrogate = new List<string>();
            foreach (string identifier in identifiers)
            {

                var pgks = packages.Where(f => System.Text.RegularExpressions.Regex.IsMatch(System.IO.Path.GetFileName(f), identifier + @".\d*.\d*.\d*.\d*.zip", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline)).ToArray();
                if (pgks == null || pgks.Length == 0)
                    continue;

                //determine the highest version
                var version = pgks.Select(p => Version.Parse(System.IO.Path.GetFileNameWithoutExtension(p).Substring(identifier.Length + 1)))
                                       .OrderByDescending(v => v.Major)
                                       .ThenByDescending(v => v.Minor)
                                       .ThenByDescending(v => v.Build)
                                       .ThenByDescending(v => v.Revision)
                                       .FirstOrDefault();

                packagesToInterrogate.Add(string.Format("{0}.{1}.zip", identifier, version));
            }

            packagesToInterrogate.ForEach(s => Console.WriteLine(s));
        }

        [TestMethod]
        public void GetTermsThatNeedToBeAdded()
        {
            var termsManager = new Lpp.QueryComposer.TermRegistration.TermsRegistrationManager(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PluginsBase"), PackagesFolder);
            termsManager.Run();
            Console.WriteLine(DateTime.Now.ToString());
        }
    }
}
