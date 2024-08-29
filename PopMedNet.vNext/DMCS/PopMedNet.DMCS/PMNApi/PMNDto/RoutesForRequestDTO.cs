using System.Collections.Generic;

namespace PopMedNet.DMCS.PMNApi.PMNDto
{
    public class RoutesForRequestsDTO
    {
        public IEnumerable<DMCSRequest> Requests { get; set; }
        public IEnumerable<DMCSRoute> Routes { get; set; }
        public IEnumerable<DMCSResponse> Responses { get; set; }
        public IEnumerable<DMCSRequestDocument> RequestDocuments { get; set; }
        public IEnumerable<DMCSDocument> Documents { get; set; }
    }
}
