using System;
using System.Collections.Generic;
using System.Linq;
using Lpp.Dns.Data;
using Lpp.Dns.Portal.Models;

namespace Lpp.Dns.Portal
{
    public interface IRequestService
    {
        RequestHeader CreateHeader( Request request );
        IRequestContext CreateRequest( Project project, Lpp.Dns.Portal.PluginRequestType pluginRequestType );
        DnsResult DeleteRequest( IRequestContext ctx );
        Request CopyRequest( IRequestContext ctx );
        IRequestContext GetRequestContext( Guid requestID );

        DnsResult AddDataMarts( IRequestContext ctx, IEnumerable<IDnsDataMart> dataMarts );
        DnsResult RemoveDataMarts( IRequestContext ctx, IEnumerable<Guid> dataMartIDs );

        DnsResult SubmitRequest( IRequestContext ctx );
        DnsResult ApproveRequest( IRequestContext ctx );
        DnsResult RejectRequest( IRequestContext ctx );
        DnsResult UpdateRequest( RequestUpdateOperation operationUpdate );
        DnsResult ValidateRequest( IRequestContext ctx );
        DnsResult TimeShift( IRequestContext ctx, TimeSpan timeDifference );

        /// <summary>
        /// Gets all granted requests by project. A null project means all granted projects.
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        //IQueryable<Request> GetGrantedRequests( Project proj );
        IQueryable<Request> GetGrantedRequests(Guid? projectID);
        IQueryable<Project> GetGrantedProjects( Guid requestType );
        IQueryable<Project> GetVisibleProjects();
        IQueryable<DataMart> GetGrantedDataMarts(Project project, Guid requestTypeID, Guid modelID);
        IQueryable<DataMart> GetGrantedDataMarts(Project project, PluginRequestType pluginRequestType);

        /// <summary>
        /// Get a dictionary of requests types grouped by request type id that the current user is authorized for.
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        IDictionary<Guid, PluginRequestType> GetGrantedRequestTypes( Guid? projectID );
        /// <summary>
        /// Get a dictionary of requests types grouped by request type model that the current user is authorized for.
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        IDictionary<IDnsModel, PluginRequestType[]> GetGrantedRequestTypesByModel(Guid? projectID);

        /// <summary>
        /// Turns given request into a "request template".
        /// Templates cannot be modified, submitted, approved, or made back into ordinary requests.
        /// They are intended for future reuse, such as scheduling or sharing.
        /// </summary>
        DnsResult MakeTemplate( IRequestContext ctx );
    }

    public class RequestUpdateOperation
    {
        public IRequestContext Context { get; set; }
        public IDnsPostContext Post { get; set; }
        public RequestHeader Header { get; set; }
        public Guid? ProjectID { get; set; }
        public IEnumerable<IDnsDataMart> AssignedDataMarts { get; set; }
    }

}