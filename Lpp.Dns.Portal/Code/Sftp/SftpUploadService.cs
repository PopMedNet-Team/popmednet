using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive.Linq;
using Lpp.Audit;
//using Lpp.Data;
//using Lpp.Dns.Model;
using Lpp.Utilities.Legacy;
using System.ServiceModel.Activation;
using log4net;
using System.Collections.Generic;
using System;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    class SftpUploadService : ISftpUploadWcfService
    {
        [Import]
        public ILog Log { get; set; }

        [Import]
        public Lpp.Mvc.Controls.IFileUploadProvider Provider { get; set; }

        public IList<FileDescriptor> ListFiles(string Host, string Port, string Account, string Password, string Folder)
        {
            try
            {
                Log.Info("Executing ListFiles");
                List<FileDescriptor> resultListing = new List<FileDescriptor>();
                using (SFTPClient client = new SFTPClient(Host, int.Parse(Port), Account, Password))
                {
                    client.ListDirectory(Folder).ForEach(f => resultListing.Add(new FileDescriptor() { FileName = f.Name, FileType = System.IO.Path.GetExtension(f.FullName), IsDirectory = f.IsDirectory, IsRegularFile = f.IsRegularFile, FullName = f.FullName, Size = f.Length.ToString(), DisplayName = f.FullName + " " + f.Length.ToString() + "kb" }));
                    client.Disconnect();
                }
                return resultListing;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return null;
        }

        public int UploadFile(string Host, int Port, string userName, string password, string filePath, Guid RequestId)
        {
            try
            {
                Log.Info("Executing UploadFile");
                using (var client = new SFTPClient(Host, Port, userName, password))
                {
                    Provider.UploadDocument(RequestId, System.IO.Path.GetFileName(filePath), client.FileStream(filePath));
                    client.Disconnect();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return 0;
        }
    }
}
