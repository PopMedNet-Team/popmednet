using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Lib.Classes
{
    public enum DatabaseDataMartType : int 
    {
        ServerBased = 0,
        ClientBased = 1,
    }

    public class HubDataMart
    {        
        public string AvailablePeriod
        {
            get;
            set;
        }

        public string ContactEmail
        {
            get;
            set;
        }

        public string ContactFirstName
        {
            get;
            set;
        }

        public string ContactLastName
        {
            get;
            set;
        }

        public string ContactPhone
        {
            get;
            set;
        }

        public Guid DataMartId
        {
            get;
            set;
        }

        public string DataMartName
        {
            get;
            set;
        }

        public string DataMartType
        {
            get;
            set;
        }

        public DatabaseDataMartType DataMartTypeId
        {
            get;
            set;
        }

        public string HealthPlanDescription
        {
            get;
            set;
        }

        public HubUser[] NotificationUsers
        {
            get;
            set;
        }

        public string OrganizationId
        {
            get;
            set;
        }

        public string OrganizationName
        {
            get;
            set;
        }

        public bool RequiresApproval
        {
            get;
            set;
        }

        public string SpecialRequirements
        {
            get;
            set;
        }

        public int[] SupportedQueryTypeIds
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public string UsageRestrictions
        {
            get;
            set;
        }
        
    }
}
