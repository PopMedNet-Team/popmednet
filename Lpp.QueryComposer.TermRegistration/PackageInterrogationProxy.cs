using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace Lpp.QueryComposer.TermRegistration
{
    public class PackageInterrogationProxy : MarshalByRefObject, IDisposable
    {
        System.IO.Stream _stream = null;
        ZipArchive _archive = null;

        public IEnumerable<IModelTermProvider> Interrogate(string path)
        {
            LoadPackage(path);

            Lpp.QueryComposer.IModelTermProvider[] providers = (from a in AppDomain.CurrentDomain.GetAssemblies()
                                                                from t in a.GetTypes()
                                                                where t.GetInterfaces().Any(i => i.FullName == "Lpp.QueryComposer.IModelTermProvider")
                                                                && t.GetConstructor(Type.EmptyTypes) != null
                                                                && !t.IsInterface
                                                                && t != typeof(ProxyModelTermProvider)
                                                                select (Lpp.QueryComposer.IModelTermProvider)Activator.CreateInstance(t)).ToArray();

            if (providers.Length == 0)
                return new IModelTermProvider[0];

            return providers.Select(p => new ProxyModelTermProvider(p)).ToArray();
        }

        void LoadPackage(string path)
        {
            //load the current appdomain up with the assemblies from the package
            if (_archive != null || _stream != null)
                Dispose(true);            

            _archive = new ZipArchive(new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read), ZipArchiveMode.Read);

            ResolveEventHandler resolveHandler = new ResolveEventHandler((sender, e) => {

                string[] filename = e.Name.Split(',');

                //try to get from the current loaded assemblies in the domain first
                var currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                var asx = (from a in currentAssemblies where a.GetName().Name == filename[0] select a).FirstOrDefault();
                if (asx != null)
                    return asx;

                var entry = _archive.GetEntry(filename[0] + ".dll");
                if (entry == null)
                {
                    return null;
                }

                byte[] buffer = LoadFromArchive(entry);

                Assembly assembly = Assembly.Load(buffer);
                return assembly;
            
            });

            AppDomain.CurrentDomain.AssemblyResolve += resolveHandler;

            foreach (ZipArchiveEntry entry in _archive.Entries)
            {
                if (!entry.Name.EndsWith(".dll"))
                    continue;

                byte[] buffer = LoadFromArchive(entry);

                AppDomain.CurrentDomain.Load(buffer);
            }

        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~PackageInterrogationProxy()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_archive != null)
                {
                    _archive.Dispose();
                    _archive = null;
                }
                if (_stream != null)
                {
                    _stream.Close();
                    _stream.Dispose();
                    _stream = null;
                }
            }
        }

        static byte[] LoadFromArchive(ZipArchiveEntry entry)
        {
            byte[] buffer = new byte[entry.Length];
            using (var stream = entry.Open())
            {
                stream.Read(buffer, 0, buffer.Length);
            }
            return buffer;
        }

    }
}
