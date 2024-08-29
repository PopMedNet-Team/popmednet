using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Dns.DTO;

namespace Lpp.Dns.Data.DistributedRegressionTracking
{
    public class EnhancedEventLogBuilder
    {
        public EnhancedEventLogBuilder()
        {
        }
        
        /// <summary>
        /// Gets or sets the function that gets the request status change events for the request.
        /// </summary>
        public Func<Task<IEnumerable<EnhancedEventLogItemDTO>>> RequestStatusChangeEvents { get; set; }
        /// <summary>
        /// Gets or sets the function that gets the routing status change events for the request.
        /// </summary>
        public Func<Task<IEnumerable<EnhancedEventLogItemDTO>>> RoutingStatusChangeEvents { get; set; }
        /// <summary>
        /// Gets or sets the function that gets the documents complete uploading events for the request.
        /// </summary>
        public Func<Task<IEnumerable<EnhancedEventLogItemDTO>>> DocumentUploadEvents { get; set; }
        /// <summary>
        /// Gets or sets the function that gets the tracking table events for the request.
        /// </summary>
        public Func<Task<IEnumerable<EnhancedEventLogItemDTO>>> TrackingTableEvents { get; set; }
        /// <summary>
        /// Gets or sets the function that gets the events logged by the adapter for the request.
        /// </summary>
        public Func<Task<IEnumerable<EnhancedEventLogItemDTO>>> AdapterLoggedEvents { get; set; }

        /// <summary>
        /// Returns the event log items ordered by iteration, step, and timestamp ascending.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EnhancedEventLogItemDTO>> GetItems()
        {

            //TODO: potential may have to fix step numbers, and do some other post processing
            List<EnhancedEventLogItemDTO> items = new List<EnhancedEventLogItemDTO>();

            if (RequestStatusChangeEvents != null)
            {
                items.AddRange(await RequestStatusChangeEvents());
            }

            if (RoutingStatusChangeEvents != null)
            {
                items.AddRange(await RoutingStatusChangeEvents());
            }

            if (DocumentUploadEvents != null)
            {
                items.AddRange(await DocumentUploadEvents());
            }

            if (TrackingTableEvents != null)
            {
                items.AddRange(await TrackingTableEvents());

            }

            if (AdapterLoggedEvents != null)
            {
                items.AddRange(await AdapterLoggedEvents());

            }

            items.Sort((a, b) => {

                if(a.Step == b.Step)
                {
                    if(a.Timestamp == b.Timestamp)
                    {
                        return a.Source.CompareTo(b.Source);
                    }

                    return a.Timestamp.CompareTo(b.Timestamp);
                }

                return a.Step.CompareTo(b.Step);
            });
            return items;
        }


    }
}
