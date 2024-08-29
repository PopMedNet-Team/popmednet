using System;

namespace PopMedNet.DMCS.PMNApi.DTO
{
    public class DataMartMetadataDTO : PopMedNet.DMCS.Data.Model.IDataMartMetadata
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Acronym { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public Guid? AdapterID { get; set; }
        public string Adapter { get; set; }
        public byte[] Timestamp { get; set; }


        public DMCS.Data.Model.DataMart ToDMCSModel()
        {
            return new Data.Model.DataMart
            {
                ID = this.ID,
                Acronym = this.Acronym,
                Adapter = this.Adapter,
                AdapterID = this.AdapterID,
                Name = this.Name,
                Description = this.Description,
                PmnTimestamp = this.Timestamp
            };
        }
    }   

}
