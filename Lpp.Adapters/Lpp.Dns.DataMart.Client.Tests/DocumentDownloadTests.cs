using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting.Lifetime;
using System.Security.Permissions;

namespace Lpp.Dns.DataMart.Client.Tests
{
    [TestClass]
    public class DocumentDownloadTests
    {
        static readonly log4net.ILog logger;

        static DocumentDownloadTests()
        {
            log4net.Config.XmlConfigurator.Configure();
            logger = log4net.LogManager.GetLogger(typeof(DocumentDownloadTests));
        }

        readonly Guid _requestID;
        readonly Guid _dataPartnerDataMartID;
        //readonly Guid _analysisDataMartID;
        readonly string _inputFilesFolder;
        readonly DataMart.Lib.NetWorkSetting _networkSetting;

        public DocumentDownloadTests()
        {
            _requestID = new Guid("455120CE-78D7-45DA-8A22-A93900E3B774");
            _dataPartnerDataMartID = new Guid("FB62CB4F-2F48-4DD7-B40D-A73D00DBA196");
            //_analysisDataMartID = new Guid("FE0AD3E6-DBAE-40CB-9EC3-A8B00089D3E1");
            _inputFilesFolder = @"C:\work\DR\" + _dataPartnerDataMartID.ToString("D");

            if (!Directory.Exists(_inputFilesFolder))
            {
                logger.Debug($"Input files folder does not exist, creating: { _inputFilesFolder }");
                Directory.CreateDirectory(_inputFilesFolder);
            }

            string[] existingFiles = Directory.GetFiles(_inputFilesFolder);
            if(existingFiles.Length > 0)
            {
                logger.Debug($"Input files folder has existing files, deleting { existingFiles.Length } files.");
                for(int i = 0; i < existingFiles.Length; i++)
                {
                    File.Delete(existingFiles[i]);
                }
            }

            logger.Debug("Creating network setting: Url=\"https://api-pmnuat.popmednet.org\", Username=\"\", Password=\"\"");
            _networkSetting = new DataMart.Lib.NetWorkSetting
            {
                NetworkName = "PMN UAT",
                HubWebServiceUrl = "https://api-pmnuat.popmednet.org",
                Username = "{your username}",
                EncryptedPassword = "{your password}"
            };
        }

        [TestMethod]
        public async Task DownloadFromAPI()
        {
            var request = await DataMart.Lib.DnsServiceManager.GetRequests(_networkSetting, new[] { _requestID }, _dataPartnerDataMartID).SkipWhile(r => r.ID != _requestID).Take(1).FirstOrDefaultAsync();

            DateTime start = DateTime.Now;
            

            var documentsWithStream = request.Documents.Select(d => new Model.DocumentWithStream(d.ID, new Model.Document(d.ID, d.Document.MimeType, d.Document.Name, d.Document.IsViewable, Convert.ToInt32(d.Document.Size), d.Document.Kind), new DataMart.Lib.Classes.DocumentChunkStream(d.ID, _networkSetting))).ToArray();

            logger.Debug($"Beginning download of { documentsWithStream.Length } input documents for datapartner.");

            foreach (var document in documentsWithStream)
            {
                try
                {

                    logger.Debug($"Begin download of document: {document.Document.Filename } [ID: {document.ID.ToString("D") }]");
                    using(FileStream destination = File.Create(Path.Combine(_inputFilesFolder, document.Document.Filename)))
                    {
                        document.Stream.CopyTo(destination);
                        destination.Flush();
                        destination.Close();
                    }
                    logger.Debug($"Finished download of document: {document.Document.Filename } [ID: {document.ID.ToString("D") }]");

                }
                catch(Exception ex)
                {
                    logger.Error("Error downloading file:" + document.Document.Filename, ex);
                    throw;
                }
            }

            logger.Debug("Finished downloading " + documentsWithStream.Length + " input documents for datapartner. Elapsed:" + (DateTime.Now - start));
        }

        [TestMethod]
        public async Task DownloadInSeparateAppDomain()
        {
            using(var domainManager = new DomainManager())
            {
                domainManager.Load();
                var downloader = domainManager.GetDownloader();

                var request = await DataMart.Lib.DnsServiceManager.GetRequests(_networkSetting, new[] { _requestID }, _dataPartnerDataMartID).SkipWhile(r => r.ID != _requestID).Take(1).FirstOrDefaultAsync();

                DateTime start = DateTime.Now;

                var documentsWithStream = request.Documents.Select(d => new Model.DocumentWithStream(d.ID, new Model.Document(d.ID, d.Document.MimeType, d.Document.Name, d.Document.IsViewable, Convert.ToInt32(d.Document.Size), d.Document.Kind), new DataMart.Lib.Classes.DocumentChunkStream(d.ID, _networkSetting))).ToArray();

                logger.Debug($"Beginning download of { documentsWithStream.Length } input documents for datapartner.");

                downloader.Download(_inputFilesFolder, documentsWithStream);

                logger.Debug("Finished downloading " + documentsWithStream.Length + " input documents for datapartner. Elapsed:" + (DateTime.Now - start));
            }
        }
    }

    internal interface IDownloader
    {
        void Download(string inputFilesFolder, IEnumerable<Model.DocumentWithStream> documents);
    }

    [Serializable]
    internal class Downloader : IDownloader
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(Downloader));

        public void Download(string inputFilesFolder, IEnumerable<Model.DocumentWithStream> documents)
        {
            //int index = 0;
            foreach (var document in documents)
            {
                try
                {

                    logger.Debug($"Begin download of document: {document.Document.Filename } [ID: {document.ID.ToString("D") }]");
                    using (FileStream destination = File.Create(Path.Combine(inputFilesFolder, document.Document.Filename)))
                    {
                        document.Stream.CopyTo(destination);
                        destination.Flush();
                        destination.Close();
                    }
                    logger.Debug($"Finished download of document: {document.Document.Filename } [ID: {document.ID.ToString("D") }]");

                    //index++;

                    //if (index > 5)
                    //    break;
                }
                catch (Exception ex)
                {
                    logger.Error("Error downloading file:" + document.Document.Filename, ex);
                    throw;
                }
            }

            //Parallel.ForEach(documents, (document) => {

            //});
        }
    }

    internal class ProxyDownloader : MarshalByRefObject, IDownloader
    {
        IDownloader _downloader;
        Type _downloaderType;

        public ProxyDownloader() { }

        public ProxyDownloader(IDownloader downloader)
        {
            _downloader = downloader;
            _downloaderType = downloader.GetType();
        }
        
        public void Download(string inputFilesFolder, IEnumerable<Model.DocumentWithStream> documents)
        {
            _downloader.Download(inputFilesFolder, documents);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }

    internal class AssemblyLoadProxy : MarshalByRefObject
    {
        public void InitializeLogging(Logging.CrossDomainParentAppender parentAppender)
        {
            Logging.CrossDomainChildLoggingSetup loggingSetup = (Logging.CrossDomainChildLoggingSetup)Activator.CreateInstance(typeof(Logging.CrossDomainChildLoggingSetup));
            loggingSetup.ConfigureAppender(parentAppender, log4net.Core.Level.All);
        }

        public IDownloader GetDownloader()
        {
            var downloaderTypes = (from a in AppDomain.CurrentDomain.GetAssemblies()
                                   from t in GetLoadableTypes(a)
                                   let interfaces = t.GetInterfaces().DefaultIfEmpty()
                                   where interfaces.Any(i => i == typeof(IDownloader))
                                   && t.GetConstructor(Type.EmptyTypes) != null
                                   && !t.IsInterface
                                   && t != typeof(ProxyDownloader)
                                   select Activator.CreateInstance(t) as IDownloader).FirstOrDefault();

            if (downloaderTypes == null)
                throw new Exception("No types implementing IDownloader were found.");

            return new ProxyDownloader(downloaderTypes);
        }

        static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            try
            {
                return assembly.GetTypes();
            }catch(ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(t => t != null);
            }
        }
    }

    internal class DomainManager : IDisposable
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(DomainManager));
        readonly ISet<LifetimeSponsor> Sponsors;
        LifetimeSponsor _loadProxySponsor = null;
        AppDomain _appDomain = null;
        AssemblyLoadProxy _loadProxy = null;
        bool _isDisposed = false;
        string _appDomainName;

        static DomainManager()
        {
            //THIS IS KEY! The lease time and renew on call time must be set prior to any remoting is done. The default lease time is like 5min, we are bumping to 12hr.
            LifetimeServices.LeaseTime = TimeSpan.FromHours(12);
            LifetimeServices.RenewOnCallTime = TimeSpan.FromHours(4);
        }

        public DomainManager()
        {
            Sponsors = new HashSet<LifetimeSponsor>();
            _appDomainName = "DR_Download_Test";
        }

        public bool Load()
        {
            //create the appdomain, base directory should be set to the packages base directory which should have the assembly containing the proxy in it
            AppDomainSetup setupInfo = new AppDomainSetup();
            setupInfo.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            setupInfo.ApplicationName = _appDomainName;
            setupInfo.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            _appDomain = AppDomain.CreateDomain(_appDomainName, AppDomain.CurrentDomain.Evidence, setupInfo);


            try
            {
                //log4net.Repository.Hierarchy.Hierarchy hierachy = (log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository();
                log.Debug("Creating domain proxy for AppDomain: " + _appDomainName);
                //create the domain proxy
                Type proxyType = typeof(AssemblyLoadProxy);
                _loadProxy = (AssemblyLoadProxy)_appDomain.CreateInstanceFrom(proxyType.Assembly.Location, proxyType.FullName).Unwrap();

                _loadProxySponsor = new LifetimeSponsor(_loadProxy);

                log.Debug("Initializing cross-domain logging for AppDomain: " + _appDomainName);
                _loadProxy.InitializeLogging(new Logging.CrossDomainParentAppender());
            }
            catch (Exception ex)
            {
                //log.Error("An error occurred initializing the domain proxy for AppDomain: " + _appDomainName, ex);
                //return false;

                throw;
            }

            return true;
        }

        public IDownloader GetDownloader()
        {
            var downloader = _loadProxy.GetDownloader();
            if (downloader == null)
                throw new Exception("Load proxy did not create the IDownloader correctly!");

            return downloader;
        }

        public void Dispose()
        {
            if (_isDisposed == false)
            {
                Dispose(true);
                _isDisposed = true;
                GC.SuppressFinalize(this);
            }
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
                        System.Runtime.Remoting.RemotingServices.Disconnect(_loadProxy);
                    }
                    catch { }
                    _loadProxy = null;
                }
                if(_loadProxySponsor != null)
                {
                    _loadProxySponsor.Dispose();
                    _loadProxySponsor = null;
                }

                if (_appDomain != null)
                {
                    log.Debug("Unloading AppDomain: " + _appDomainName);
                    AppDomain.Unload(_appDomain);
                }
            }
        }
    }

    [Serializable]
    [SecurityPermission(SecurityAction.Demand, Infrastructure = true)]
    public sealed class LifetimeSponsor : MarshalByRefObject, ISponsor, IDisposable
    {
        ILease _lease = null;
        
        public LifetimeSponsor(MarshalByRefObject obj)
        {
            _lease = (ILease)System.Runtime.Remoting.RemotingServices.GetLifetimeService(obj);
            _lease.Register(this);
        }

        public TimeSpan Renewal(ILease lease)
        {
            return TimeSpan.FromDays(1);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void Dispose()
        {
            if (_lease != null)
            {
                _lease.Unregister(this);
            }
        }
    }
}
