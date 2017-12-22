using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DataMart.Model
{
    /// <summary>
    /// Provides information about Document to be transferred.
    /// </summary>
    [DataContract, Serializable]
    public class Document
    {
        /// <summary>
        /// Document identifier
        /// </summary>
        [DataMember]
        public string DocumentID
        {
            get;
            set;
        }

        /// <summary>
        /// Document mime type
        /// </summary>
        [DataMember]
        public string MimeType
        {
            get;
            set;
        }

        /// <summary>
        /// Document size
        /// </summary>
        [DataMember]
        public int Size
        {
            get;
            set;
        }

        /// <summary>
        /// Is Document viewable on the Portal
        /// </summary>
        [DataMember]
        public bool IsViewable
        {
            get;
            set;
        }

        /// <summary>
        /// The filename of the document.
        /// </summary>
        [DataMember]
        public string Filename
        {
            get;
            set;
        }

        /// <summary>
        /// The document kind (*optional)
        /// </summary>
        [DataMember]
        public string Kind
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="documentID"></param>
        /// <param name="mimeType"></param>
        /// <param name="filename"></param>
        public Document(string documentID, string mimeType, string filename)
        {
            DocumentID = documentID;
            MimeType = mimeType;
            Filename = filename;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentID"></param>
        /// <param name="mimeType"></param>
        /// <param name="filename"></param>
        /// <param name="isViewable"></param>
        /// <param name="size"></param>
        /// <param name="kind"></param>
        public Document(Guid documentID, string mimeType, string filename, bool isViewable, int size, string kind)
        {
            DocumentID = documentID.ToString("D");
            MimeType = mimeType;
            Filename = filename;
            IsViewable = isViewable;
            Size = size;
            Kind = kind;
        }
    }
}
