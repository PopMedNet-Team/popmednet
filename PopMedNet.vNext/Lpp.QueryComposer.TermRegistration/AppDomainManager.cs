using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lpp.QueryComposer.TermRegistration
{
    public class AppDomainManager : IDisposable
    {
        readonly string PackagePath;
        readonly string AppDomainBase;
        readonly ISet<LifetimeSponsor> Sponsors = new HashSet<LifetimeSponsor>();
        AppDomain _appDomain = null;
        PackageInterrogationProxy _interrogationProxy = null;

        public AppDomainManager(string appDomainBase, string packagePath)
        {
            AppDomainBase = appDomainBase;
            PackagePath = packagePath;
        }

        public IEnumerable<IModelTermProvider> Interrogate()
        {
            if (!File.Exists(PackagePath))
            {
                throw new FileNotFoundException("The file: " + Path.GetFileName(PackagePath) + " was not found in " + Path.GetDirectoryName(PackagePath) + ".");
            }            

            string domainName = Path.GetFileNameWithoutExtension(PackagePath);
            AppDomainSetup setupInfo = new AppDomainSetup();
            setupInfo.ApplicationName = domainName;
            setupInfo.ApplicationBase = AppDomainBase;
            _appDomain = AppDomain.CreateDomain(domainName, AppDomain.CurrentDomain.Evidence, setupInfo);

            Type proxyType = typeof(PackageInterrogationProxy);
            _interrogationProxy = (PackageInterrogationProxy)_appDomain.CreateInstanceFrom(proxyType.Assembly.Location, proxyType.FullName).Unwrap();
            
            return _interrogationProxy.Interrogate(PackagePath).Select(
                    termProvider => {
                        System.Runtime.Remoting.Lifetime.ILease leaseObj = System.Runtime.Remoting.RemotingServices.GetLifetimeService((ProxyModelTermProvider)termProvider) as System.Runtime.Remoting.Lifetime.ILease;
                        if (leaseObj != null)
                        {
                            Sponsors.Add(new LifetimeSponsor(leaseObj));
                        }

                        return termProvider;
                    }
                );
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AppDomainManager()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Sponsors.Count > 0)
                {
                    foreach (LifetimeSponsor sponsor in Sponsors)
                    {
                        sponsor.Dispose();
                    }
                }

                if(_interrogationProxy != null)
                {
                    try
                    {
                        _interrogationProxy.Dispose();
                        System.Runtime.Remoting.RemotingServices.Disconnect(_interrogationProxy);
                    }
                    catch { }
                    _interrogationProxy = null;
                }

                if (_appDomain != null)
                {
                    AppDomain.Unload(_appDomain);
                }
            }
        }
    }
}
