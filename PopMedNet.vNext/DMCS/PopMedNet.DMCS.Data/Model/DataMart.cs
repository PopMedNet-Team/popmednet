using PopMedNet.DMCS.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PopMedNet.DMCS.Data.Model
{
    [Table("Datamarts")]
    public class DataMart : IDataMartMetadata
    {
        public DataMart()
        {
            this.AutoProcess = 0;
            this.CacheDays = 0;
            this.EncryptCache = false;
            this.EnableExplictCacheRemoval = false;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Acronym { get; set; }
        public string Description { get; set; }
        public Guid? AdapterID { get; set; }
        public string Adapter { get; set; }
        public int CacheDays { get; set; }
        public bool EncryptCache { get; set; }
        public bool EnableExplictCacheRemoval { get; set; }
        public AutoProcesses AutoProcess { get; set; }
        public byte[] PmnTimestamp { get; set; }
        [NotMapped]
        byte[] IDataMartMetadata.Timestamp
        {
            get
            {
                return PmnTimestamp;
            }
        }

        public virtual IEnumerable<UserDataMart> Users { get; set; }
        public virtual IEnumerable<RequestDataMart> Requests { get; set; }
    }

    public interface IDataMartMetadata
    {
        Guid ID { get; }
        string Name { get; }
        string Acronym { get; }
        string Description { get; }
        Guid? AdapterID { get; }
        string Adapter { get; }
        byte[] Timestamp { get; }
    }

    public class DataMartMetadataEqualityComparer : IEqualityComparer<IDataMartMetadata>
    {
        public bool Equals([AllowNull] IDataMartMetadata dm1, [AllowNull] IDataMartMetadata dm2)
        {
            if (dm1 == null && dm2 == null)
                return true;
            else if (dm1 == null || dm2 == null)
                return false;

            return dm1.ID == dm2.ID &&
                dm1.Name == dm2.Name &&
                dm1.Acronym == dm2.Acronym &&
                dm1.Description == dm2.Description &&
                dm1.AdapterID == dm2.AdapterID &&
                dm1.Adapter == dm2.Adapter &&
                ObjectExtensions.ByteEquals(dm1.Timestamp, dm2.Timestamp);
        }

        public int GetHashCode([DisallowNull] IDataMartMetadata dm)
        {
            string stringValues = (dm.Name + dm.Acronym + dm.Description + dm.Adapter).Ensure();
            int hCode = dm.ID.GetHashCode() ^
                stringValues.GetHashCode() ^
                (dm.AdapterID.HasValue ? dm.AdapterID.Value.GetHashCode() : 0.GetHashCode()) ^
                dm.Timestamp.GetHashCode();

            return hCode.GetHashCode();

        }
    }
}
