using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text.RegularExpressions;
using log4net;
using Lpp.Dns.DataMart.Lib.Classes;
using System.Collections;
using Lpp.Dns.DataMart.Client.Utils;
using Lpp.Dns.DTO.DataMartClient;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Lib
{
    public static class DnsServiceManager
    {
        private static readonly ILog _log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );
        private static int _lastSucessfulPageSize = 64;

        public static IObservable<Request> GetRequests( NetWorkSetting ns, Guid[] ids, Guid dmID )
        {
            if ( ids.NullOrEmpty() ) 
                return Observable.Empty<Request>();

            var nullReqs = (Request[]) null;
            var noError = (Exception)null;

            return
                Observable.Generate(
                    new { reqs = nullReqs, idss = new[] { ids }.AsEnumerable(), DataMartID = dmID , finish = false, error = noError },
                    st => !st.finish,
                    st =>
                    {
                        try
                        {
                            if ( st.error != null ) 
                                return new { st.reqs, st.idss, st.DataMartID, finish = true, st.error };

                            var i = st.idss.FirstOrDefault();
                            var dm = st.DataMartID;
                            if ( i == null ) 
                                return new { st.reqs, idss = st.idss, DataMartID = st.DataMartID, finish = true, error = noError };

                            Request[] reqs = null;
                            using (var web = new Lpp.Dns.DataMart.Client.Lib.DnsApiClient(ns, FindCert(ns)))
                            {
                                var requests = AsyncHelpers.RunSync<IQueryable<DTO.DataMartClient.Request>>(() => web.GetRequests(i.Select(x => x).ToArray(), dm) );
                                reqs = requests.ToArray();                                
                            }
                            
                            return new { reqs, idss = st.idss.Skip(1), DataMartID = st.DataMartID, finish = false, error = noError };
                        }
                        catch ( CommunicationException ex )
                        {
                            if ( ex.Message.ToLower().Contains( "size quota" ) )
                            {
                                return st.idss.First().Length > 1 // When the next segment can be subdivided further, do it
                                    ? new
                                    {
                                        reqs = nullReqs,
                                        idss = st.idss
                                            .SelectMany( i => new[] { i.Take( i.Length/2 ).ToArray(), i.Skip( i.Length/2 ).ToArray() } )
                                            .Where( i => i.Any() ),
                                        DataMartID = st.DataMartID,
                                        finish = false,
                                        error = noError
                                    } 
                                    // Otherwise, return error
                                    : new { reqs = nullReqs, idss = st.idss, DataMartID = st.DataMartID, finish = false, error = (Exception)ex };
                            }

                            throw;
                        }
                    },
                    st => st.error == null ? st.reqs.EmptyIfNull().ToObservable() : Observable.Throw<Request>( st.error ),
                    System.Reactive.Concurrency.Scheduler.Default
                )
                .Catch( ( Exception ex ) =>
                {
                    _log.Error( string.Format( "Unable to get requests for Network: {0}.", ns.NetworkName ), ex );
                    return Observable.Throw<IObservable<Request>>( new GetRequestsFailed( ex ) );
                } )
                .SelectMany( rs => rs );
        }

        public static IObservable<RequestList> GetRequestList(string queryDescription, NetWorkSetting ns, int startIndex, int count, RequestFilter filter, RequestSortColumn? sortColumn, bool? sortAscending )
        {
            var nullReqs = new RequestList();
            var noError = (Exception) null;
            
            if ( count < 0 ) 
                count = int.MaxValue;

            return
                Observable.Generate(new
                    {
                        reqs = nullReqs,
                        nextIndex = startIndex,
                        pageSize = 100,
                        count,
                        finish = false,
                        error = noError
                    },
                    st => !st.finish,
                    st =>
                    {
                        try
                        {
                            if (st.error != null || st.count == 0)
                            {
                                return new { st.reqs, st.nextIndex, st.pageSize, st.count, finish = true, st.error };
                            }

                            Lpp.Dns.DTO.DataMartClient.RequestList list = null;
                            using (var web = new Lpp.Dns.DataMart.Client.Lib.DnsApiClient(ns, FindCert(ns)))
                            {
                                list = Task.Run(() => web.GetRequestList(queryDescription, filter.EffectiveFromDate, filter.EffectiveToDate, filter.DataMartIds, filter.Statuses, (Lpp.Dns.DTO.DataMartClient.RequestSortColumn?)(int?)sortColumn, sortAscending, st.nextIndex, Math.Min(st.count, st.pageSize))).Result;
                            }

                            RequestList reqs = null;
                            if (list == null)
                            {
                                reqs = new RequestList
                                {
                                    Segment = Enumerable.Empty<RequestListRow>().ToArray(),
                                    SortedAscending = true,
                                    SortedByColumn = RequestSortColumn.RequestTime,
                                    StartIndex = 0,
                                    TotalCount = 0
                                };
                            }
                            else
                            {
                                reqs = new RequestList
                                {
                                    Segment = list.Segment,
                                    SortedAscending = list.SortedAscending,
                                    SortedByColumn = list.SortedByColumn,
                                    StartIndex = list.StartIndex,
                                    TotalCount = list.TotalCount
                                };
                            }                            
                            
                            _lastSucessfulPageSize = st.pageSize;
                            
                            
                            return new
                            {
                                reqs,
                                nextIndex = st.nextIndex + reqs.Segment.Count(),
                                st.pageSize, 
                                count = Math.Min( st.count, reqs.TotalCount - reqs.StartIndex ) - reqs.Segment.Count(), 
                                finish = false,
                                error = noError
                            };
                        }
                        catch ( CommunicationException ex )
                        {
                            if ( ex.Message.ToLower().Contains( "size quota" ) )
                            {
                                return st.pageSize > 1 ?
                                    new { reqs = nullReqs, st.nextIndex, pageSize = st.pageSize/2, st.count, finish = false, error = noError } :
                                    new { reqs = nullReqs, st.nextIndex, st.pageSize, st.count, finish = false, error = (Exception)ex };
                            }

                            throw;
                        }
                    },
                    st => st.error == null ? Observable.Return( st.reqs ) : Observable.Throw<RequestList>( st.error ),
                    System.Reactive.Concurrency.Scheduler.Default
                )
                .Catch( ( Exception ex ) =>
                {
                    _log.Error( string.Format( "Unable to get requests for Network: {0}.", ns.NetworkName ), ex );
                    return Observable.Throw<IObservable<RequestList>>( new GetRequestsFailed( ex ) );
                } )
                .SelectMany( ls => ls )
                .Skip( 1 )
                .Aggregate( ( l1, l2 ) => new RequestList
                {
                    Segment = l1.Segment.EmptyIfNull().Concat( l2.Segment.EmptyIfNull() ).ToArray(),
                    SortedAscending = l1.SortedAscending,
                    SortedByColumn = l1.SortedByColumn,
                    StartIndex = l1.StartIndex,
                    TotalCount = l1.TotalCount
                } );
        }

        public static int TestConnections(IEnumerable<NetWorkSetting> nsc)
        {
            return
                nsc.EmptyIfNull()
                .Where( ns => ns != null && !ns.Username.NullOrEmpty() && !ns.Password.NullOrEmpty() )
                .Do( ns => ns.NetworkStatus = LogIn(ns) ? Util.ConnectionOKStatus : Util.ConnectionFailedStatus )
                .Count( ns => ns.NetworkStatus == Util.ConnectionOKStatus );
        }

        #region Public Methods

        public static bool LogIn( NetWorkSetting ns )
        {
            try
            {
                try
                {
                    using (var web = new Lpp.Dns.DataMart.Client.Lib.DnsApiClient(ns, FindCert(ns)))
                    {
                        var profile = Task.Run(() => web.GetProfile()).Result;
                        ns.Profile = new Profile
                        {
                            Email = profile.Email,
                            FullName = profile.FullName,
                            OrganizationName = profile.OrganizationName,
                            Username = profile.Username
                        };
                    }
                    ns.IsAuthenticated = true;
                    ns.NetworkStatus = Util.ConnectionOKStatus;
                    ns.NetworkMessage = string.Empty;
                    return true;
                }
                catch (System.Net.Http.HttpRequestException rex)
                {
                    //Unwrap the raised exception to the original exception - it gets wrapped in generic http exceptions.
                    throw UnwrapException(rex);
                }
                catch (AggregateException aex)
                {
                    //Unwrap the raised exception to the original exception - it gets wrapped in generic Aggregate exceptions.
                    throw UnwrapException(aex);
                }
            }
            catch (System.Net.Http.HttpRequestException rex)
            {
                ns.NetworkStatus = Util.ConnectionFailedStatus;
                ns.IsAuthenticated = false;
                ns.NetworkMessage = rex.Message;
                if (string.Equals(ns.NetworkMessage, "Response status code does not indicate success: 401 (Unauthorized).", StringComparison.OrdinalIgnoreCase))
                {
                    ns.NetworkMessage = "Authentication failed, please check your credentials are correct.";
                }
            }
            catch (System.Net.Sockets.SocketException sockEx)
            {
                ns.NetworkStatus = Util.ConnectionFailedStatus;
                ns.IsAuthenticated = false;
                if (sockEx.ErrorCode == 10061)
                {
                    ns.NetworkMessage = string.Format("Unable to connect, please confirm service url:{1}{1}{0}{1}{1}If the url is correct, and the problem persists please contact your network administrator.", ns.HubWebServiceUrl, Environment.NewLine);
                }
                else
                {
                    ns.NetworkMessage = sockEx.Message;
                }
            }
            catch ( Exception e )
            {
                ns.NetworkStatus = Util.ConnectionFailedStatus;
                ns.IsAuthenticated = false;
                ns.NetworkMessage = e.Message;
            }
            return false;
        }

        static Exception UnwrapException(Exception ex)
        {
            if (ex.InnerException != null)
                return UnwrapException(ex.InnerException);

            return ex;
        }

        public static void LogOut( NetWorkSetting ns )
        {
            ns.IsAuthenticated = false;
            ns.Profile = null;
            ns.NetworkStatus = Util.LoggedOutStatus;
            ns.NetworkMessage = string.Empty;
            ns.DnsRequests = null;
            ns.UserRights = null;
        }

        public static HubDataMart[] GetDataMarts( NetWorkSetting ns )
        {
            try
            {
                using (var web = new Lpp.Dns.DataMart.Client.Lib.DnsApiClient(ns, FindCert(ns)))
                {
                    var datamarts = Task.Run(() => web.GetDataMarts()).Result;
                    return datamarts.Select(dm => new HubDataMart { 
                        DataMartId = dm.ID,
                        DataMartName = dm.Name,
                        OrganizationId = dm.OrganizationID.ToString("D"),
                        OrganizationName = dm.OrganizationName
                    }).ToArray();
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Unable to get Datamarts for Network: {0}.", ns.NetworkName), ex);
                throw new GetDataMartsFailed(ex);
            }
        }

        /// <summary>
        /// Gets the model information for all DataMarts.
        /// </summary>
        /// <param name="ns">The network setting to use.</param>
        /// <returns>IDictionary<Guid, HubModel[]> where the key value is the DataMart ID, and the value is the collection of models for the DataMart.</Guid></returns>
        public static IDictionary<Guid, HubModel[]> GetModels(NetWorkSetting ns)
        {
            try
            {
                using(var web = new Client.Lib.DnsApiClient(ns, FindCert(ns)))
                {
                    var datamarts = Task.Run(() => web.GetDataMarts()).Result;

                    var models = datamarts.ToDictionary(k => k.ID, v => v.Models.Select(m => new HubModel { Id = m.ID, Name = m.Name, ModelProcessorId = m.ProcessorID }).ToArray());
                    return models;
                }

            }catch(Exception ex)
            {
                _log.Error($"Unable to get Models for all DataMarts for Network: { ns.NetworkName }.", ex);
                throw new GetModelsFailed(ex);
            }
        }

        /// <summary>
        /// Returns an array of model configurations identified by datamart id, model id and model processor id.
        /// </summary>
        /// <param name="ns">NetWorkSettings</param>
        /// <returns>HubConfiguration[]</returns>
        public static HubConfiguration[] GetDataMartModelConfigurations(NetWorkSetting ns)
        {
            try
            {
                using (var web = new Lpp.Dns.DataMart.Client.Lib.DnsApiClient(ns, FindCert(ns)))
                {
                    var datamarts = Task.Run(() => web.GetDataMarts()).Result;
                    var models = datamarts.Where(dm => dm.Models != null)
                        .SelectMany(dm => dm.Models.Select(m => new HubConfiguration
                        {
                            DataMartId = dm.ID,
                            UnattendedMode = (Lpp.Dns.DataMart.Lib.Classes.UnattendedMode)(int)dm.UnattendedMode,
                            ModelId = m.ID,
                            ModelProcessorId = m.ProcessorID,
                            Properties = m.Properties
                        }));

                    return models.ToArray();
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Unable to get Datamart Models for Network: {0}.", ns.NetworkName), ex);
                throw new GetModelsFailed(ex);
            }
        }

        public static byte[] GetDocumentChunk( Guid documentID, int offset, int size, NetWorkSetting ns )
        {
            try
            {
                using (var web = new Lpp.Dns.DataMart.Client.Lib.DnsApiClient(ns, FindCert(ns)))
                {
                    var buffer = Task.Run(() => web.GetDocumentChunk(documentID, offset, size)).Result;
                    return buffer.ToArray();
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Unable to get Document Chunk for Document: {0}.", documentID), ex);
                throw new GetDocumentChunkFailed(ex);
            }
        }

        public static Guid[] PostResponseDocuments( string uploadIdentifier, string requestId, Guid dataMartId, Lpp.Dns.DataMart.Model.Document[] documents, NetWorkSetting ns )
        {
            string docString = string.Join(", ", documents.Select(x => string.Format("'{0:D}'", x.Filename)));

            try
            {
                using (var web = new Lpp.Dns.DataMart.Client.Lib.DnsApiClient(ns, FindCert(ns)))
                {

                    _log.Debug($"{uploadIdentifier} - Posting metadata for the following documents to API: {docString} for RequestID: {requestId}, DataMartID: {dataMartId.ToString("D")}");
                    
                    var result = Task.Run(() => web.PostResponseDocuments(new Guid(requestId), dataMartId, documents.Select(d => new Lpp.Dns.DTO.DataMartClient.Document { Name = d.Filename, MimeType = d.MimeType, Size = d.Size, IsViewable = d.IsViewable, Kind = d.Kind }).ToArray())).Result;

                    _log.Debug($"{uploadIdentifier} - Posting metadata complete for the following documents to API: {docString} for RequestID: {requestId}, DataMartID: {dataMartId.ToString("D")}");
                    return result.ToArray();
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("{2} - Unable to post document metadata for the following documents: {1} for RequestID: {0}.", requestId, docString, uploadIdentifier), ex);
                throw new PostResponseDocumentsFailed(ex);
            }
        }

        /// <summary>
        /// Starts sending the document data to the portal and returns a stream of "bytes written so far" values
        /// TODO: Replace IObservable[int] with a specific type that would reflect the semantics
        /// </summary>
        public static IObservable<int> PostResponseDocumentContent(string uploadIdentifier, string requestID, Guid dataMartID, Guid documentId, string documentName, Stream stream, NetWorkSetting ns )
        {
            _log.Debug($"{uploadIdentifier} - Posting document content to API for: {documentName} (ID: {documentId.ToString("D") }); RequestID: {requestID}, DataMartID: {dataMartID.ToString("D")}");

            byte[] buffer = new byte[0x400000];
            return Observable.Generate( 
                new { offset = 0, finish = false },
                st => !st.finish,
                st =>
                {
                    int bytesRead = stream.Read( buffer, 0, buffer.Length );
                    if (bytesRead == 0)
                    {
                        _log.Debug($"{uploadIdentifier} - Finished posting document content to API for: {documentName} (ID: {documentId.ToString("D") }); RequestID: {requestID}, DataMartID: {dataMartID.ToString("D")}");
                        return new { st.offset, finish = true };
                    }

                    byte[] data = buffer;
                    if ( bytesRead < data.Length )
                    {
                        data = new byte[bytesRead];
                        Buffer.BlockCopy( buffer, 0, data, 0, bytesRead );
                    }

                    try
                    {
                        using (var web = new Lpp.Dns.DataMart.Client.Lib.DnsApiClient(ns, FindCert(ns)))
                        {
                            Task.Run(() => web.PostResponseDocumentChunk(documentId, data)).Wait();
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.Error(string.Format("Unable to post Repsonse Doucument Content for Document: {0}.",documentId), ex);
                        throw new PostResponseDocumentContentFailed(ex);
                    }

                    return new { offset = st.offset + bytesRead, finish = false };
                },
                st => st.offset                
            );
        }

        public static void SetRequestStatus( HubRequest request, HubRequestStatus status, IDictionary<string, string> requestProperties, NetWorkSetting ns )
        {
            try
            {
                using (var web = new Lpp.Dns.DataMart.Client.Lib.DnsApiClient(ns, FindCert(ns)))
                {
                    Task.Run(() => web.SetRequestStatus(request.Source.ID, request.DataMartId, status.Code, status.Message, requestProperties.EmptyIfNull().Select(p => new DTO.DataMartClient.RoutingProperty { Name = p.Key, Value = p.Value }).ToArray())).Wait();
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Unable to set request status for requestID: {0}.", request.Source.ID), ex);
                throw new SetRequestStatusFailed(ex);
            }
        }

        public static bool CheckUserRight( HubRequest request, HubRequestRights rights, NetWorkSetting ns )
        {
            return (request.Rights & rights) > 0;
        }

        public static Lpp.Dns.DTO.DataMartClient.RequestTypeIdentifier GetRequestTypeIdentifier(NetWorkSetting ns, Guid modelID, Guid processorID)
        {
            try
            {
                using (var web = new Lpp.Dns.DataMart.Client.Lib.DnsApiClient(ns, FindCert(ns)))
                {
                    return Task.Run(() => web.GetRequestTypeIdentifier(modelID, processorID)).Result;
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Unable to get requests identifier for Network: {0}.", ns.NetworkName), ex);
                throw new GetRequestTypeIdentifierException(ex);
            }
        }

        public static void DownloadPackage(NetWorkSetting ns, RequestTypeIdentifier packageIdentifier)
        {
            try
            {
                using (var web = new Lpp.Dns.DataMart.Client.Lib.DnsApiClient(ns, FindCert(ns)))
                {
                    string filepath = System.IO.Path.Combine(Lpp.Dns.DataMart.Client.Utils.Configuration.PackagesFolderPath, packageIdentifier.PackageName());
                    using (var stream = Task.Run(() => web.GetPackage(packageIdentifier)).Result)
                    {
                        using (var filestream = new System.IO.FileStream(filepath, System.IO.FileMode.Create))
                        {
                            stream.CopyTo(filestream);
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Unable to download package {1} for Network: {0}.", ns.NetworkName, packageIdentifier), ex);
                throw ex;
            }
        }

        #endregion

        #region Private Methods

        public static HubRequestStatus ConvertModelRequestStatus(Lpp.Dns.DataMart.Model.RequestStatus status)
        {
            switch (status.Code)
            {
                case Lpp.Dns.DataMart.Model.RequestStatus.StatusCode.Complete:
                case Lpp.Dns.DataMart.Model.RequestStatus.StatusCode.CompleteWithMessage:
                    return new HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.AwaitingResponseApproval, status.Message);
                case Lpp.Dns.DataMart.Model.RequestStatus.StatusCode.InProgress:
                    return new HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Submitted, status.Message);
                case Lpp.Dns.DataMart.Model.RequestStatus.StatusCode.Pending:
                    return new HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.PendingUpload, status.Message);
                case Lpp.Dns.DataMart.Model.RequestStatus.StatusCode.Canceled:
                    return new HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Canceled, status.Message);
                case Lpp.Dns.DataMart.Model.RequestStatus.StatusCode.Error:
                    return new HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Failed, status.Message);
                default:
                    throw new StatusConversionError(status);
            }
        }

        private static Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus ConvertComboBoxRequestStatus( int status )
        {
            switch ( status )
            {
                case 2: // Submitted
                    return Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Submitted;
                case 3: // Completed
                    return Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Completed;
                case 4: // Awaiting Approval
                    return Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.AwaitingResponseApproval;
                case 5: // Rejected
                    return Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.RequestRejected;
                case 6: // Canceled
                    return Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Canceled;
                case 99: // Failed
                    return Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Failed;
                case 1: // Pending
                case 7: // Deleted
                case 8: // Pending GOMA
                case 9: // In Progress
                default:
                    return Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Submitted;
            }
        }

        #endregion   

        private static X509Certificate2 FindCert( NetWorkSetting ns )
        {
            if ( ns.X509CertThumbprint.NullOrEmpty() ) return null;

            var store = new X509Store( StoreName.My, StoreLocation.CurrentUser );
            try
            {
                store.Open( OpenFlags.MaxAllowed | OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly );
                return store
                    .Certificates.Find( X509FindType.FindByThumbprint, ns.X509CertThumbprint ?? "", false )
                    .Cast<X509Certificate2>().FirstOrDefault();
            }
            finally
            {
                store.Close();
            }
        }

        
    }

    public enum HubRequestRights
    {
        Run        =   0x01,
        Hold       =   0x02,
        Reject     =   0x04,
        ModifyResults = 0x08,
        All        =   Run | Hold | Reject | ModifyResults
    }

    public class HubRequestStatus
    {
        static readonly IList<HubRequestStatus> _all;
        public static IList<HubRequestStatus> All
        {
            get
            {
                return _all;
            }
        }

        static readonly IList<string> _names;
        public static IList<string> Names
        {
            get
            {
                return _names;
            }
        }

        static HubRequestStatus()
        {
            _all = new List<HubRequestStatus> { 
                new HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Submitted),
                new HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.AwaitingResponseApproval),
                new HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Completed),
                //new HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.RequestRejected),
                new HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Hold),
                new HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Failed),
                new HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Canceled),
                new HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.PendingUpload),
                new HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.ResponseRejectedBeforeUpload),
                new HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.ResponseRejectedAfterUpload),
                new HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.ExaminedByInvestigator),
                new HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Resubmitted),
                new HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.ResultsModified)
            }.AsReadOnly();

            _names = new List<string> { 
                GetDescription(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Submitted),
                GetDescription(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.AwaitingResponseApproval),
                GetDescription(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Completed),
                //GetDescription(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.RequestRejected),
                GetDescription(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Hold),
                GetDescription(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Failed),
                GetDescription(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Canceled),
                GetDescription(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.PendingUpload),
                GetDescription(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.ResponseRejectedBeforeUpload),
                GetDescription(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.ResponseRejectedAfterUpload),
                GetDescription(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.ExaminedByInvestigator),
                GetDescription(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus.Resubmitted)            
            }.AsReadOnly();
        }

        public HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus code)
        {
            Code = code;
        }

        public HubRequestStatus(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus code, string message)
        {
            Code = code;
            Message = message;
        }
        
        public Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus Code
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public string Name
        {
            get
            {
                return GetDescription(Code);
            }
        }

        internal static string GetDescription(Lpp.Dns.DTO.DataMartClient.Enums.DMCRoutingStatus code)
        {
            try
            {
                var type = code.GetType();
                var memberInfo = type.GetMember(code.ToString());
                var attributes = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (attributes.Any())
                {
                    var description = ((System.ComponentModel.DescriptionAttribute)attributes[0]).Description;
                    return description;
                }
            }catch
            {
                return "Unable to determine: " + code;
            }

            return code.ToString();
        }
    }
}