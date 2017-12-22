using System;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace Lpp.Dns.Portal
{
    [ServiceContract( Namespace="http://lincolnpeak.com/schemas/DNS4/SftpUpload" )]
    public interface ISftpUploadWcfService
    {
        [OperationContract]
        IList<FileDescriptor> ListFiles(string Host, string Port, string Account, string Password, string Folder);
        [OperationContract]
        int UploadFile(string Host, int Port, string Account, string Password, string FilePath, Guid RequestId);
    }

    [DataContract]
    public class FileDescriptor
    {
        [DataMember]
        public String DisplayName { get; set; }
        [DataMember]
        public String FullName { get; set; }
        [DataMember]
        public String FileType { get; set; }
        [DataMember]
        public String FileName { get; set; }
        [DataMember]
        public bool IsDirectory { get; set; }
        [DataMember]
        public bool IsRegularFile { get; set; }
        [DataMember]
        public String Size { get; set; }
    }
}