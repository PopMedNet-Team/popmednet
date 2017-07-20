using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Dns.DTO.DataMartClient;

namespace Lpp.Dns.DataMart.Lib.Classes
{
    public class HubRequest
    {
        public Request Source { get; set; }

        public Guid DataMartId { get; set; }
        public string DataMartName { get; set; }
        public string DataMartOrgId { get; set; }
        public string DataMartOrgName { get; set; }

        public int NetworkId { get; set; }
        public string NetworkName { get; set; }
        public Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus RoutingStatus { get; set; }
        public DocumentWithID[] Documents { get; set; }
        public string RejectReason { get; set; }

        public string ProjectName { get; set; }

        public Lpp.Dns.DataMart.Model.IModelProcessor Processor { get; set; }
        public IDictionary<string, string> Properties { get; set; }
        public HubRequestRights Rights { get; set; }

        public string SubmittedDataMarts { get; set; }
        public string CreatedByMessage { get; set; }

        public Lpp.Dns.DataMart.Model.RequestMetadata CreateInterfaceMetadata()
        {
            return new Lpp.Dns.DataMart.Model.RequestMetadata
            {
                RequestTypeId = Source.RequestTypeID.ToString(),
                RequestTypeName = Source.RequestTypeName,
                IsMetadataRequest = Source.IsMetadataRequest,
                Identifier = Source.Identifier,
                MSRequestID = Source.MSRequestID,
                ModelID = Source.ModelID,
                CreatedOn = Source.CreatedOn,
                DueDate = Source.DueDate,
                Priority = (int)Source.Priority,
                Name = Source.Name,
                Description = Source.Description,
                AdditionalInstructions = Source.AdditionalInstructions,
                PurposeOfUse = Source.PurposeOfUse,
                PhiDisclosureLevel = Source.PhiDisclosureLevel,
                ReportAggregationLevel = Source.ReportAggregationLevel,
                RequestorCenter = Source.RequestorCenter,
                WorkplanType = Source.WorkPlanType,
                TaskOrder = Source.TaskOrder,
                ActivityProject = Source.ActivityProject,                
                Activity = Source.Activity,
                ActivityDescription = Source.ActivityDescription,
                SourceActivity = Source.SourceActivity,
                SourceActivityProject = Source.SourceActivityProject,
                SourceTaskOrder = Source.SourceTaskOrder,
                DataMartId = DataMartId.ToString(),
                DataMartName = DataMartName,
                DataMartOrganizationId = DataMartOrgId,
                DataMartOrganizationName = DataMartOrgName
            };
        }

        public void Assign( HubRequest from )
        {
            Source = from.Source;
            DataMartId = from.DataMartId;
            DataMartName = from.DataMartName;
            DataMartId = from.DataMartId;
            DataMartName = from.DataMartName;
            DataMartOrgId = from.DataMartOrgId;
            DataMartOrgName = from.DataMartOrgName;
            NetworkId = from.NetworkId;
            NetworkName = from.NetworkName;
            RoutingStatus = from.RoutingStatus;
            Documents = from.Documents;
            RejectReason = from.RejectReason;
            Rights = from.Rights;
            SubmittedDataMarts = from.SubmittedDataMarts;
            CreatedByMessage = from.CreatedByMessage;
        }
    }
}