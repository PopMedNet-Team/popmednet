using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
//using Lpp.Dns.Model;
using Lpp.Security;
using System.ComponentModel.Composition;
//using Lpp.Data;
using Lpp.Utilities.Legacy;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal
{
    class RequestContext : IRequestContext
    {
        private readonly RequestService _service;
        private readonly Request _request;
        private readonly PluginRequestType _rtype;
        private readonly IQueryable<DataMart> _dataMarts;
        private readonly List<IDnsDataMart> _dnsDataMarts;
        private readonly IEnumerable<IDnsActivity> _activities;
        private bool _closed = false;
        private readonly bool _isNew;
        private IDnsActivity _activity;
        private IEnumerable<Document> _documents;
        private readonly LocalThreadMemoizer _memoizer = new LocalThreadMemoizer();

        public Guid RequestID
        {
            get
            {
                AssertNotClosed();
                return _request.ID;
            }
        }

        public Request Request { 
            get
            {
                AssertNotClosed();
                return _request; 
            } 
        }

        public PluginRequestType PluginReqeustType
        {
            get
            {
                AssertNotClosed();
                return _rtype;
            }
        }

        public IDnsModelPlugin Plugin 
        {
            get
            {
                AssertNotClosed(); 
                return _rtype.Plugin; 
            }
        }

        public Func<SecurityPrivilege, bool> Can
        {
            get
            {
                throw new Lpp.Utilities.CodeToBeUpdatedException();
            }
        }



        public IDnsModel Model 
        { 
            get 
            { 
                AssertNotClosed(); 
                return _rtype.Model; 
            } 
        }
        
        public IDnsRequestType RequestType 
        { 
            get 
            { 
                AssertNotClosed(); 
                return _rtype.RequestType; 
            } 
        }
        
        public IEnumerable<Document> Documents 
        { 
            get 
            { 
                AssertNotClosed(); 
                return _documents; 
            } 
        }
        
        public IQueryable<DataMart> DataMarts 
        { 
            get 
            { 
                AssertNotClosed(); 
                return _dataMarts; 
            } 
        }
        
        IEnumerable<IDnsDataMart> IDnsRequestContext.DataMarts 
        { 
            get 
            { 
                AssertNotClosed(); 
                return _dnsDataMarts; 
            } 
        }
        
        public IEnumerable<IDnsActivity> Activities 
        { 
            get 
            { 
                AssertNotClosed(); 
                return _activities; 
            } 
        }

        public DnsRequestHeader Header
        {
            get
            {
                AssertNotClosed();
                return new DnsRequestHeader
                {
                    Name = _request.Name,
                    Description = _request.Description,
                    Activity = _activity,
                    ActivityDescription = _request.ActivityDescription,
                    DueDate = _request.DueDate,
                    Priority = _request.Priority,

                    Submitted = _request.SubmittedOn,
                    AuthorName = _request.CreatedBy == null ? null : string.Format("{0} {1}", _request.CreatedBy.FirstName, _request.CreatedBy.LastName).Trim(),
                    AuthorEmail = _request.CreatedBy == null ? null : _request.CreatedBy.Email,
                    Organization = _request.Organization == null ? null : _request.Organization.Name
                };
            }
        }

        public RequestContext(RequestService srv, Request req, PluginRequestType rt, bool isNew = false)
        {
            _service = srv;
            _request = req;
            _rtype = rt;
            _isNew = isNew;

            _dataMarts = _service.GetGrantedDataMarts(req.Project, _rtype);

            var allMetadataRequests = Model.Requests.Where(r => r.IsMetadataRequest).Select(r => r.ID).ToArray();
            _dnsDataMarts = _dataMarts.ToArray().Select(m => new DnsDataMart
                {
                    ID = m.ID,
                    Name = m.Name,
                    Organization = m.Organization.Name,
                    MetadataDocuments = EnumerableEx.Defer(() =>
                    {
                        var lastMetadataResponse = (
                                from rsp in _service.DataContext.RequestDataMarts
                                from i in rsp.Responses
                                where rsp.Request.SubmittedOn.HasValue && rsp.DataMartID == m.ID && allMetadataRequests.Contains(rsp.Request.RequestTypeID)
                                orderby rsp.Request.SubmittedOn
                                select i
                            ).FirstOrDefault();

                        return lastMetadataResponse == null ? Enumerable.Empty<Document>() : _service.DataContext.Documents.Where( d => d.ItemID == lastMetadataResponse.ID ).ToList();
                    })
                })
                .Cast<IDnsDataMart>()
                .ToList();

            _activities = _service.DataContext.Activities.Select(a => new DnsActivity { ID = a.ID, Name = a.Name }).ToList();

            Reset();
        }

        public void Reset()
        {
            _documents = _service.DataContext.Documents.Where(d => d.ItemID == _request.ID).ToArray();
            _activity = _request.Activity == null ? null : _activities.FirstOrDefault(a => a.ID == _request.ActivityID);
        }

        public void Close() 
        { 
            _closed = true; 
        }

        void AssertNotClosed() 
        { 
            if (_closed) 
                throw new InvalidOperationException("Cannot operate on a closed request context"); 
        }

        public void ModifyMetadata(DnsRequestMetadata md)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException("Missing DueDate from Request class.");

            //Can.Demand(SecPrivileges.Crud.Edit);

            //AssertNotClosed();
            //_request.Name = md.Header.Name;
            //_request.Description = md.Header.Description;
            //_request.ActivityDescription = md.Header.ActivityDescription;
            //_request.Activity = md.Header.Activity == null ? null : _service.Activities.Find(md.Header.Activity.Id);
            ////_request.DueDate = md.Header.DueDate;
            //_request.Priority = (byte)md.Header.Priority;

            //if (md.DataMartFilter != null)
            //{
            //    var dmsToDelete = from d in _request.Routings
            //                      join dm in _dnsDataMarts.Value on d.DataMartId equals dm.Id
            //                      where dm == null || !md.DataMartFilter(dm)
            //                      select d;

            //    foreach (var dm in dmsToDelete.ToList())
            //    {
            //        _service.Routings.Remove(dm);
            //    }
            //}

            //if (!_isNew) _service.UnitOfWork.Commit();
            //Reset();
        }

        internal class DnsDataMart : IDnsDataMart
        {
            public Guid ID { get; set; }
            public string Name { get; set; }
            public string Organization { get; set; }
            public IEnumerable<Document> MetadataDocuments { get; set; }
            public DTO.Enums.Priorities Priority { get; set; }
            public DateTime? DueDate { get; set; }
        }

        class DnsActivity : IDnsActivity
        {
            public Guid ID { get; set; }
            public string Name { get; set; }
        }
    }

    class DataMartResponse : IDnsDataMartResponse
    {
        private Lazy<IDnsDataMart> _dataMart;

        public DataMartResponse(Func<IDnsDataMart> dataMart)
        {
            _dataMart = new Lazy<IDnsDataMart>(dataMart);
        }

        public IDnsDataMart DataMart 
        { 
            get 
            { 
                return _dataMart.Value; 
            } 
        }

        public IEnumerable<Document> Documents { get; set; }
    }
}