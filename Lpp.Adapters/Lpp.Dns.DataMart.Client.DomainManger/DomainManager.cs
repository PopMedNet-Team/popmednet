using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using log4net;

namespace Lpp.Dns.DataMart.Client.DomainManger
{
    public class DomainManager : IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        readonly string RootFolder;
        readonly ISet<LifetimeSponsor> Sponsors;
        AppDomain _appDomain = null;
        AssemblyLoadProxy _loadProxy = null;
        string _packagePath = null;
        string _appDomainName = string.Empty;

        public DomainManager(string rootFolder)
        {
            Sponsors = new HashSet<LifetimeSponsor>();
            RootFolder = rootFolder;
            log.Debug("DomainManager created with RootFolder specified as: " + RootFolder);
        }

        public bool Load(string packageIdentifier, string adapterVersion)
        {
            return Load(string.Format("{0}.{1}", packageIdentifier, adapterVersion));
        }

        /// <summary>
        /// Loads all the assemblies found in the specified adapter package.
        /// </summary>
        /// <param name="packageIdentifierAndVersion">The request type package identifier + request adapter version.</param>
        /// <returns></returns>
        bool Load(string packageIdentifierAndVersion)
        {
            _appDomainName = packageIdentifierAndVersion;
            _packagePath = Path.Combine(RootFolder, packageIdentifierAndVersion + ".zip");
            log.Debug("Begin loading of " + _packagePath);

            if(!File.Exists(_packagePath))
            {
                throw new FileNotFoundException("The file: " + packageIdentifierAndVersion + ".zip was not found in " + RootFolder + ".");
            }

            log.Debug("Creating plugin appdomain named: " + _appDomainName);
            //create the appdomain, base directory should be set to the packages base directory which should have the assembly containing the proxy in it
            AppDomainSetup setupInfo = new AppDomainSetup();
            setupInfo.ApplicationBase = RootFolder;
            setupInfo.ApplicationName = _appDomainName;
            setupInfo.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            _appDomain = AppDomain.CreateDomain(packageIdentifierAndVersion, AppDomain.CurrentDomain.Evidence, setupInfo);
            log.Debug("Finished creating AppDomain: " + packageIdentifierAndVersion);

            try
            {
                log4net.Repository.Hierarchy.Hierarchy hierachy = (log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository(); 
                log.Debug("Creating domain proxy for AppDomain: " + _appDomainName);
                //create the domain proxy
                Type proxyType = typeof(AssemblyLoadProxy);
                _loadProxy = (AssemblyLoadProxy)_appDomain.CreateInstanceFrom(proxyType.Assembly.Location, proxyType.FullName).Unwrap();

                //System.Runtime.Remoting.Lifetime.ILease leaseObj = System.Runtime.Remoting.RemotingServices.GetLifetimeService(_loadProxy) as System.Runtime.Remoting.Lifetime.ILease;
                //if (leaseObj != null)
                //{
                //    Sponsors.Add(new LifetimeSponsor(leaseObj));
                    
                //}

                //load the domain proxy with the assemblies for the package
                log.Debug("Loading the package: " + _packagePath + " into the AppDomain: " + _appDomainName);
                _loadProxy.LoadPackage(_packagePath);

                log.Debug("Initializing cross-domain logging for AppDomain: " + _appDomainName);
                _loadProxy.InitializeLogging(new Logging.CrossDomainParentAppender(), hierachy.Threshold);
            }
            catch (Exception ex)
            {
                log.Error("An error occurred initializing the domain proxy for AppDomain: " + _appDomainName, ex);
                return false;
            }

            return true;
        }

        public Lpp.Dns.DataMart.Model.IModelProcessor GetProcessor(Guid processorID)
        {
            log.Debug("Begin GetProcessor for ProcessorID: " + processorID.ToString("D"));
            
            Lpp.Dns.DataMart.Model.IModelProcessor processor = _loadProxy.GetProcessor(processorID);
            
            log.Debug("End GetProcessor for ProcessorID: " + processorID.ToString("D"));

            System.Runtime.Remoting.Lifetime.ILease leaseObj = System.Runtime.Remoting.RemotingServices.GetLifetimeService((MarshalByRefObject)processor) as System.Runtime.Remoting.Lifetime.ILease;
            if (leaseObj != null)
            {
                Sponsors.Add(new LifetimeSponsor(leaseObj));
            }
            
            return processor;
        }

        public IEnumerable<string> GetLoadedAssemblyNames()
        {
            return _loadProxy.LoadedAssemblyNames();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DomainManager()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                log.Debug("Begin dispose of AppDomain: " + _appDomainName);

                if (Sponsors.Count > 0)
                {
                    log.Debug("Cleaning up cross-domain lifetime sponsors for AppDomain: " + _appDomainName);
                    foreach (LifetimeSponsor sponsor in Sponsors)
                    {
                        sponsor.Dispose();
                    }
                }

                if (_loadProxy != null)
                {
                    log.Debug("Closing domain proxy for AppDomain: " + _appDomainName);
                    try
                    {
                        _loadProxy.Close();
                        System.Runtime.Remoting.RemotingServices.Disconnect(_loadProxy);
                    }
                    catch { }
                    _loadProxy = null;
                }
                if (_appDomain != null)
                {
                    log.Debug("Unloading AppDomain: " + _appDomainName);
                    AppDomain.Unload(_appDomain);
                }
            }
        }
    }
}
