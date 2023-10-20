using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.QueryComposer.TermRegistration
{
    /// <summary>
    /// Coordinates interrogating processor packages for IModelTerms.
    /// </summary>
    public class TermsRegistrationManager : IDisposable
    {
        internal static IEnumerable<Dns.DTO.DataModelProcessorDTO> DataModelProcessors;

        readonly string AppDomainBaseFolder;
        readonly string PackagesFolder;
        readonly Queue<AppDomainManager> AdapterAppDomains = new Queue<AppDomainManager>();

        /// <summary>
        /// Initializes a new TermsRegistrationManager using the specified AppDomain base folder, and packages folder.
        /// </summary>
        /// <param name="appDomainBaseFolder"></param>
        /// <param name="packagesFolder"></param>
        public TermsRegistrationManager(string appDomainBaseFolder, string packagesFolder)
        {
            if (string.IsNullOrEmpty(appDomainBaseFolder))
                throw new ArgumentException("The path to the AppDomain's base folder cannot be null or empty.", "appDomainBaseFolder");

            if (string.IsNullOrEmpty(packagesFolder))
                throw new ArgumentException("The path to the folder containing the processor packages cannot be empty.", "packagesFolder");            

            AppDomainBaseFolder = appDomainBaseFolder;
            PackagesFolder = packagesFolder;

            InitializeAppDomainBase();
        }

        void InitializeAppDomainBase()
        {
            if (!Directory.Exists(AppDomainBaseFolder))
                Directory.CreateDirectory(AppDomainBaseFolder);

            //get the applications current base folder where all the assemblies are, privatebinpath will not be null when run in website but will be under test.
            string currentDomainBaseFolder = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath ?? AppDomain.CurrentDomain.BaseDirectory;

            //Needs to exist in the second appdomains base directory to allow for the interface types to load correctly.
            File.Copy(Path.Combine(currentDomainBaseFolder, "Lpp.QueryComposer.dll"), Path.Combine(AppDomainBaseFolder, "Lpp.QueryComposer.dll"), true);
        }

        /// <summary>
        /// Gets all the registerd package identifers from the database, interrogates the packages, and updates the Terms in the database as needed.
        /// </summary>
        public void Run()
        {
            IEnumerable<string> identifiers;
            using (var db = new Lpp.Dns.Data.DataContext())
            {
                identifiers = db.RequestTypes.Where(rt => !string.IsNullOrEmpty(rt.PackageIdentifier)).Select(rt => rt.PackageIdentifier).Distinct().ToArray();
            }

            var providers = Interrogate(identifiers);

            //Using Distinct if we initialize DataModelProcessorsDTO didn't seem to work. Anonymous type allowed it.
            var dmProcessors = (from p in providers
                                select new
                                {
                                    ModelID = p.ModelID,
                                    ProcessorID = p.ProcessorID,
                                    Processor = p.Processor
                                }).Distinct().ToList();

            DataModelProcessors = dmProcessors.Select(p => new Dns.DTO.DataModelProcessorDTO() { ModelID = p.ModelID, Processor = p.Processor, ProcessorID = p.ProcessorID });

            var comparer = new IModelTermComparer();
            IEnumerable<IModelTerm> terms = providers.SelectMany(p => p.Terms).Distinct(comparer);

            //update the main list of terms to make sure the term exists in the system.
            UpdateTerms(terms);

            foreach (var termProvider in providers.GroupBy(g => g.ModelID))
            {
                //update the association between the model and the term
                terms = termProvider.SelectMany(p => p.Terms).Distinct(comparer);
                UpdateModelTerms(termProvider.Key, terms);
            }

        }

        /// <summary>
        /// Interrogates the specified packages for any IModelTermProviders, and returns all the IModelTerms provided by any found provider.
        /// </summary>
        /// <param name="identifiers">The package identifiers to interrogate, if more than one package exits the highest version will be used.</param>
        /// <returns></returns>
        public IEnumerable<IModelTermProvider> Interrogate(IEnumerable<string> identifiers)
        {
            if (identifiers == null || !identifiers.Any())
                throw new ArgumentException("The identifiers for the packages to interogate must not be null or empty.", "identifiers");

            var packages = System.IO.Directory.GetFiles(PackagesFolder, "*.zip");
            if (packages == null || packages.Length == 0)
                return Enumerable.Empty<IModelTermProvider>();

            List<IModelTermProvider> termProviders = new List<IModelTermProvider>();
            foreach (string identifier in identifiers)
            {

                var pgks = packages.Where(f => System.Text.RegularExpressions.Regex.IsMatch(System.IO.Path.GetFileName(f), identifier + @".\d*.\d*.\d*.\d*.zip", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline, TimeSpan.FromSeconds(3))).ToArray();
                if (pgks == null || pgks.Length == 0)
                    continue;

                //determine the highest version
                var version = pgks.Select(p => Version.Parse(System.IO.Path.GetFileNameWithoutExtension(p).Substring(identifier.Length + 1)))
                                       .OrderByDescending(v => v.Major)
                                       .ThenByDescending(v => v.Minor)
                                       .ThenByDescending(v => v.Build)
                                       .ThenByDescending(v => v.Revision)
                                       .FirstOrDefault();

                //interogate the package in a separate appdomain to make see if any IModelTerms need to be registered
                string filepath = System.IO.Path.Combine(PackagesFolder, string.Format("{0}.{1}.zip", identifier, version));
                                
                AppDomainManager mgr = new AppDomainManager(AppDomainBaseFolder, filepath);
                AdapterAppDomains.Enqueue(mgr);
                var providers = mgr.Interrogate().ToArray();
                if (providers.Length > 0)
                {
                    termProviders.AddRange(providers);
                }

            }

            return termProviders;
        }

        /// <summary>
        /// Updates the database from the provided terms with any that are missing or needs to be updated.
        /// </summary>
        /// <param name="terms"></param>
        public void UpdateTerms(IEnumerable<IModelTerm> terms)
        {
            using (var db = new Lpp.Dns.Data.DataContext())
            {
                //The Terms that are passed are from all the adapters so Distinct them.
                var termIDs = terms.Select(x => x.ID).Distinct().ToArray();

                var existingTerms = db.Terms.Where(x => termIDs.Contains(x.ID)).ToArray();

                var changed = false;

                //The Terms that are passed may have already been altered so generate a list of already altered terms based on their ID's.
                List<Guid> checkedIDs = new List<Guid>();

                foreach (var term in terms)
                {
                    if (!checkedIDs.Contains(term.ID))
                    {
                        var existingTerm = existingTerms.Where(x => x.ID == term.ID).FirstOrDefault();

                        if (existingTerm == null)
                        {
                            db.Terms.Add(new Dns.Data.Term
                            {
                                ID = term.ID,
                                Name = term.Name,
                                ReferenceUrl = term.ReferenceUrl,
                                OID = term.OID,
                            });

                            if (!changed)
                                changed = true;

                            checkedIDs.Add(term.ID);
                        }
                        else
                        {
                            if (!string.Equals(term.Name, existingTerm.Name))
                            {
                                existingTerm.Name = term.Name;
                                if (!changed)
                                    changed = true;
                            }

                            if (!string.Equals(term.Description, existingTerm.Description))
                            {
                                existingTerm.Description = term.Description;
                                if (!changed)
                                    changed = true;
                            }

                            if (!string.Equals(term.OID, existingTerm.OID))
                            {
                                existingTerm.OID = term.OID;
                                if (!changed)
                                    changed = true;
                            }

                            if (!string.Equals(term.ReferenceUrl, existingTerm.ReferenceUrl))
                            {
                                existingTerm.ReferenceUrl = term.ReferenceUrl;
                                if (!changed)
                                    changed = true;
                            }

                            checkedIDs.Add(term.ID);
                        }
                    }
                }

                if (changed)
                    db.SaveChanges();
            }
        }

        /// <summary>
        /// Updates the database with any missing model term associations.
        /// </summary>
        /// <param name="modelID"></param>
        /// <param name="terms"></param>
        public void UpdateModelTerms(Guid modelID, IEnumerable<IModelTerm> terms)
        {
            using (var db = new Lpp.Dns.Data.DataContext())
            {
                foreach (var trm in terms)
                {
                    db.Database.ExecuteSqlCommand("IF(NOT EXISTS(SELECT NULL FROM DataModelSupportedTerms WHERE DataModelID = @p0 AND TermID = @p1)) INSERT INTO DataModelSupportedTerms (DataModelID, TermID) VALUES (@p0, @p1)", modelID, trm.ID);
                }
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~TermsRegistrationManager()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                while (AdapterAppDomains.Count > 0){
                    AppDomainManager appDomain = AdapterAppDomains.Dequeue();
                    appDomain.Dispose();
                    appDomain = null;
                };
            }
        }
    }

    /// <summary>
    /// A comparer that compares IModelTerms using their ID field as the property to test for equality.
    /// </summary>
    public class IModelTermComparer : IEqualityComparer<IModelTerm>
    {
        /// <summary>
        /// Compares if the IModelTerm.ID values are equal.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(IModelTerm x, IModelTerm y)
        {
            if (x == null && y == null)
                return true;

            if ((x != null && y == null) || (x == null && y != null))
                return false;

            return x.ID.Equals(y.ID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(IModelTerm obj)
        {
            return obj.GetHashCode();
        }
    }
}
