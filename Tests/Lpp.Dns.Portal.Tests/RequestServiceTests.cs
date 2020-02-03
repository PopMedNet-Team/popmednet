using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Web.Mvc;
using Lpp.Composition;
using Lpp.Dns.Model;
using Lpp.Dns.Tests;
using Lpp.Security;
using Lpp.Tests;
//using Xunit;
//using Xunit.Extensions;
using Lpp.Utilities.Legacy;

namespace Lpp.Dns.Portal.Tests
{
    //public class RequestServiceTests : IUseFixture<CompositionFixture>
    //{
    //    [Fact]
    //    public void RequestService_CreateRequest()
    //    {
    //        Test( (scope,service, r) => {} );
    //    }

    //    [Fact]
    //    public void RequestService_UpdateRequest()
    //    {
    //        Test( ( scope, service, req ) =>
    //        {
    //                        var op = UpdateOp( service, req );
    //                            op.ProjectId = null;
    //            AssertFail( service.UpdateRequest( op ), "Project must be selected." );

    //            var res = service.UpdateRequest( UpdateOp( service, req ) );
    //            AssertSuccess( res );
    //            Assert.Equal( "test name", req.Name );
    //            Assert.Equal( "Request description", req.Description );
    //            Assert.NotNull( req.Activity );
    //            Assert.Equal( "Activity1", req.Activity.Name );
    //            Assert.Equal( "Activity description", req.ActivityDescription );
    //            Assert.Equal( (byte) DnsRequestPriority.High, req.Priority );
    //            Assert.Equal( new DateTime( 2001, 10, 01 ), req.DueDate );

    //            var p = TestPlugin( scope.Get<IPluginService>() );
    //            Assert.Equal( "xyz", p.LastPost.A );
    //            Assert.Equal( 555, p.LastPost.B );

    //            Assert.Equal( new[] { 1, 2 }, req.Routings.Select( d => d.DataMartId ).OrderBy( id => id ).ToArray() );
    //        } );
    //    }

    //    [Theory]
    //    [InlineData( null ), InlineData( "" )]
    //    public void RequestService_UpdateRequest_EmptyName( string targetName )
    //    {
    //        Test( ( scope, service, req ) =>
    //        {
    //            var res = service.UpdateRequest( UpdateOp( service, req, h => h.Name = targetName ) );
    //            Assert.False( res.IsSuccess );
    //            Assert.Equal( 1, res.ErrorMessages.Count() );
    //        } );
    //    }

    //    [Fact]
    //    public void RequestService_UpdateRequest_InvalidDate()
    //    {
    //        Test( ( scope, service, req ) =>
    //        {
    //            var now = DateTime.Now; req.DueDate = now;
    //            var res = service.UpdateRequest( UpdateOp( service, req, h => h.DueDate = "abcd" ) );
    //            AssertSuccess( res );
    //            Assert.Equal( now, req.DueDate );
    //        } );
    //    }

    //    [Fact]
    //    public void RequestService_UpdateRequest_InvalidActivity()
    //    {
    //        Test( ( scope, service, req ) =>
    //        {
    //            var res = service.UpdateRequest( UpdateOp( service, req, h => h.Activity = 5 ) );
    //            AssertSuccess( res );
    //            Assert.Null( req.Activity );
    //        } );
    //    }

    //    [Fact]
    //    public void RequestService_UpdateRequest_PluginFailure()
    //    {
    //        Test( ( scope, service, req ) =>
    //        {
    //            TestPlugin( scope.Get<IPluginService>() ).Fail = true;
    //            var res = service.UpdateRequest( UpdateOp( service, req ) );
    //            Assert.False( res.IsSuccess );
    //            Assert.Equal( new[] { "Error" }, res.ErrorMessages );
    //        } );
    //    }

    //    [Fact]
    //    public void RequestService_UpdateRequest_SubmittedRequest()
    //    {
    //        Test( ( scope, service, req ) =>
    //        {
    //            req.Submitted = DateTime.Now;
    //            var res = service.UpdateRequest( UpdateOp( service, req ) );
    //            Assert.False( res.IsSuccess );
    //            Assert.Equal( 1, res.ErrorMessages.Count() );
    //        } );
    //    }

    //    [Fact]
    //    public void RequestService_SubmitRequest()
    //    {
    //        Test( ( scope, service, req ) =>
    //        {
    //            AssertSuccess( service.SubmitRequest( service.GetRequestContext( req.Id ) ) );
    //            Assert.NotNull( req.Submitted );
    //            Assert.True( req.Routings.All( d => d.RequestStatus == RoutingStatuses.Submitted ) );
    //        } );
    //    }

    //    [Fact]
    //    public void RequestService_SubmitRequest_NoProject()
    //    {
    //        Test( ( scope, service, req ) =>
    //        {
    //            req.Project = null;
    //            AssertFail( service.SubmitRequest( service.GetRequestContext( req.Id ) ), "Cannot submit a request outside of a Project context. Please select a Project." );
    //            Assert.Null( req.Submitted );
    //            Assert.True( req.Routings.All( d => d.RequestStatus != RoutingStatuses.Submitted ) );
    //        } );
    //    }

    //    [Fact]
    //    public void RequestService_SubmitRequest_NoPermission()
    //    {
    //        Test( ( scope, service, req ) =>
    //        {
    //            var sec = scope.Get<ISecurityService<DnsDomain>>() as MoqSec;
    //            sec.AllowSubmit = false;

    //            var res = service.SubmitRequest( service.GetRequestContext( req.Id ) );
    //            Assert.False( res.IsSuccess );
    //            Assert.Contains( "permission", string.Join( " ", res.ErrorMessages.EmptyIfNull() ), StringComparison.InvariantCultureIgnoreCase );
    //        } );
    //    }

    //    // Validation
    //    // Separate test suites for validators

    //    [Fact]
    //    public void RequestService_SubmitRequest_ApprovalRequired()
    //    {
    //        Test( ( scope, service, req ) =>
    //        {
    //            var sec = scope.Get<ISecurityService<DnsDomain>>() as MoqSec;
    //            sec.RequireApproval = true;
    //            var ctx = service.GetRequestContext( req.Id );

    //            AssertSuccess(  service.SubmitRequest( ctx ) );
    //            Assert.NotNull( req.Submitted );
    //            Assert.True( req.Routings.All( d => d.RequestStatus == RoutingStatuses.AwaitingRequestApproval ) );

    //            AssertSuccess( service.ApproveRequest( ctx ) );
    //            Assert.NotNull( req.Submitted );
    //            Assert.True( req.Routings.All( d => d.RequestStatus == RoutingStatuses.Submitted ) );
    //        } );
    //    }

    //    [Fact]
    //    public void RequestService_RemoveAddDatamarts()
    //    {
    //        Test( ( scope, service, req ) =>
    //        {
    //            var dmRep = scope.Get<IRepository<DnsDomain, DataMart>>();
    //            var dmrRep = scope.Get<IRepository<DnsDomain, RequestRouting>>();
    //            var org = scope.Get<IRepository<DnsDomain, Model.Organization>>().All.First();
    //            var sec = scope.Get<ISecurityService<DnsDomain>>() as MoqSec;
    //            dmRep.Add( new DataMart { Id = 3, Name = "DataMart3", InstalledModels = { new DataMartInstalledModel { ModelId = TestReqType( scope.Get<IPluginService>() ).Model.Id } }, Organization = org } );
    //            dmRep.Add( new DataMart { Id = 4, Name = "DataMart4", InstalledModels = { new DataMartInstalledModel { ModelId = TestReqType( scope.Get<IPluginService>() ).Model.Id } }, Organization = org } );
    //            sec.DataMarts = dmRep.All;
    //            var ctx = service.GetRequestContext( req.Id );
    //            var dctx = ctx as IDnsRequestContext;
    //            var dm1 = dctx.DataMarts.Where( d => d.Id == 1 );
    //            var dm3 = dctx.DataMarts.Where( d => d.Id == 3 );
    //            var dm4 = dctx.DataMarts.Where( d => d.Id == 4 );
    //            req.Routings.ForEach( d => { d.RequestId = req.Id; scope.Get<IRepository<DnsDomain, RequestRouting>>().Add( d ); } );
    //            (service as RequestService).Validators = new IDnsRequestValidator[] { };

    //            AssertSuccess( service.RemoveDataMarts( ctx, new[] { 1 } ) );
    //            Assert.Equal( RoutingStatuses.Canceled, req.Routings.First( d => d.DataMart.Id == 1 ).RequestStatus );
    //            AssertSuccess( service.AddDataMarts( ctx, dm1 ) );
    //            Assert.Equal( RoutingStatuses.Draft, req.Routings.First( d => d.DataMart.Id == 1 ).RequestStatus );

    //            req.Submitted = DateTime.Now;
    //            AssertSuccess( service.RemoveDataMarts( ctx, new[] { 1 } ) );
    //            Assert.Equal( RoutingStatuses.Canceled, req.Routings.First( d => d.DataMart.Id == 1 ).RequestStatus );
    //            AssertSuccess( service.AddDataMarts( ctx, dm1 ) );
    //            Assert.Equal( RoutingStatuses.Submitted, req.Routings.First( d => d.DataMart.Id == 1 ).RequestStatus );
    //            AssertSuccess( service.AddDataMarts( ctx, dm3 ) );
    //            Assert.Equal( 3, req.Routings.Count );
    //            Assert.Equal( RoutingStatuses.Submitted, req.Routings.First( d => d.DataMart.Id == 3 ).RequestStatus );

    //            sec.RequireApproval = true;
    //            ctx = service.GetRequestContext( req.Id );

    //            AssertSuccess( service.RemoveDataMarts( ctx, new[] { 1 } ) );
    //            Assert.Equal( RoutingStatuses.Canceled, req.Routings.First( d => d.DataMart.Id == 1 ).RequestStatus );
    //            AssertSuccess( service.AddDataMarts( ctx, dm1 ) );
    //            Assert.Equal( RoutingStatuses.AwaitingRequestApproval, req.Routings.First( d => d.DataMart.Id == 1 ).RequestStatus );
    //            AssertSuccess( service.AddDataMarts( ctx, dm4 ) );
    //            Assert.Equal( 4, req.Routings.Count );
    //            Assert.Equal( RoutingStatuses.AwaitingRequestApproval, req.Routings.First( d => d.DataMart.Id == 4 ).RequestStatus );
    //        } );
    //    }

    //    [Fact]
    //    public void RequestService_Validation()
    //    {
    //        Test( ( scope, service, req ) =>
    //        {
    //            var ctx = service.GetRequestContext( req.Id );
    //            var val = new TestValidator { Result = DnsResult.Success };
    //            ( service as RequestService ).Validators = new[] { val };

    //            AssertSuccess( service.SubmitRequest( ctx ) );
    //            AssertSuccess( service.ApproveRequest( ctx ) );
    //            AssertSuccess( service.RemoveDataMarts( ctx, ( ctx as IDnsRequestContext ).DataMarts.Take(1).Select( d => d.Id ) ) );
    //            AssertSuccess( service.AddDataMarts( ctx, (ctx as IDnsRequestContext).DataMarts.Take( 1 ) ) );
    //        } );
    //    }

    //    [Fact]
    //    public void RequestService_Validation_Fail()
    //    {
    //        Test( ( scope, service, req ) =>
    //        {
    //            var ctx = service.GetRequestContext( req.Id );
    //            var val = new TestValidator { Result = DnsResult.Failed( "Error" ) };
    //            ( service as RequestService ).Validators = new[] { val };

    //            AssertFail( service.SubmitRequest( ctx ), "Error" );
    //            AssertFail( service.RemoveDataMarts( ctx, (ctx as IDnsRequestContext).DataMarts.Take( 1 ).Select( d => d.Id ) ), "Error" );

    //            val.Result = DnsResult.Success;
    //            req.Submitted = null;
    //            AssertSuccess( service.SubmitRequest( ctx ) );
    //            AssertSuccess( service.RemoveDataMarts( ctx, (ctx as IDnsRequestContext).DataMarts.Take( 1 ).Select( d => d.Id ) ) );

    //            val.Result = DnsResult.Failed( "Error" );
    //            AssertFail( service.ApproveRequest( ctx ), "Error" );
    //            AssertFail( service.AddDataMarts( ctx, (ctx as IDnsRequestContext).DataMarts.Take( 1 ) ), "Error" );
    //        } );
    //    }

    //    [Fact]
    //    public void ResponseService_Resubmit()
    //    {
    //        Test( ( scope, service, req ) =>
    //        {
    //            var ctx = service.GetRequestContext( req.Id );
    //            var resp = scope.Get<IResponseService>();
    //            var dms = scope.Compose( new DataMartClientService() );

    //            AssertSuccess( service.SubmitRequest( ctx ) );

    //            dms.SetRequestStatus( new DnsCredentials { Username = "user", Password = "password" }, new DnsRequestId( req.Id ),
    //                new DnsDataMartId( req.Routings.First().DataMartId ), DnsRequestStatus.Completed, "", null, null );
    //            Assert.Equal( RoutingStatuses.AwaitingResponseApproval, req.Routings.First().RequestStatus );

    //            AssertSuccess( resp.ApproveResponses( req.Routings.First().Instances ) );

    //            AssertSuccess( resp.ResubmitResponses( req.Routings.SelectMany( r => r.Instances ), "Msg" ) );
    //            Assert.Equal( RoutingStatuses.Resubmitted, req.Routings.First().RequestStatus );

    //            req.Routings.Skip( 1 ).ForEach( r => Assert.Equal( RoutingStatuses.Submitted, r.RequestStatus ) );
    //        } );
    //    }

    //    RequestUpdateOperation UpdateOp( IRequestService service, Request req, Action<Models.RequestHeader> alterHeader = null )
    //    {
    //        var ctx = service.GetRequestContext( req.Id );
    //        return new RequestUpdateOperation
    //        {
    //            Context = ctx,
    //            Header = Header( alterHeader ),
    //                            ProjectId = req.Project.SID,
    //            Post = new PostContext( new DictionaryValueProvider<object>(
    //                new SortedList<string,object> { { "A", "xyz" }, { "B", "555" } },
    //                Thread.CurrentThread.CurrentCulture ) ),
    //            AssignedDataMarts = (ctx as IDnsRequestContext).DataMarts
    //        };
    //    }

    //    Models.RequestHeader Header( Action<Models.RequestHeader> alter = null )
    //    {
    //        var res = new Models.RequestHeader
    //        {
    //            Name = "test name",
    //            Description = "Request description",
    //            DueDate = "10/01/2001",
    //            Priority = DnsRequestPriority.High,
    //            Activity = 1,
    //            ActivityDescription = "Activity description"
    //        };
    //        if ( alter != null ) alter( res );
    //        return res;
    //    }

    //    void Test( Action<CompositionContainer, IRequestService, Request> doTest )
    //    {
    //        using ( var scope = MockExports( Composition.MockScope() ).Build() )
    //        {
    //            scope.GetMany<Lpp.Audit.AuditEventKind>(); // Make sure event kinds get loaded and registered
    //            var service = scope.Get<IRequestService>();
    //            var type = TestReqType( scope.Get<IPluginService>() );
    //            scope.Entities<DataMart>().ForEach( d => d.InstalledModels = new[] { new DataMartInstalledModel { ModelId = type.Model.Id } } );

    //            var r = service.CreateRequest( scope.Get<IRepository<DnsDomain, Project>>().All.First(), type ).Request;
    //            r.Id = 5;
    //            foreach ( var ri in r.Routings.SelectMany( rt => rt.Instances.Select( i => new { rt, i } ) ) )
    //            {
    //                ri.i.Routing = ri.rt;
    //                ri.rt.Request = r;
    //                ri.i.DataMartId = ri.rt.DataMart.Id;
    //                ri.i.Routing.DataMartId = ri.rt.DataMart.Id;
    //                ri.i.RequestId = r.Id;
    //                ri.i.Routing.RequestId = r.Id;
    //                scope.Repo<DnsDomain, RequestRouting>().Add( ri.rt );
    //                scope.Repo<DnsDomain, RequestRoutingInstance>().Add( ri.i );
    //            }

    //            Assert.Equal( type.RequestType.Id, r.RequestTypeId );
    //            Assert.Equal( null, r.Submitted );
    //            Assert.Equal( scope.Get<IRepository<DnsDomain, User>>().All.First(), r.CreatedByUser );
    //            scope.Get<IRepository<DnsDomain, Request>>().Add( r );
    //            r.Routings.ForEach( d => d.DataMartId = d.DataMart.Id );
    //            doTest( scope, service , r );
    //        }
    //    }

    //    MockScopeBuilder MockExports( MockScopeBuilder scope )
    //    {
    //        var data = Mock.Data<DnsDomain>();
    //        var user = new User
    //        {
    //            Id = 1,
    //            Username = "user",
    //            Password = Password.ComputeHash( "password" ),
    //            Organization = new Organization { Id = 1, Name = "Org1" }
    //        };
    //        var dms = new[]
    //        {
    //            new DataMart { Id = 1, Name = "DataMart1", Organization = user.Organization }, 
    //            new DataMart { Id = 2, Name = "DataMart2", Organization = user.Organization }
    //        };
    //        var reqs = data.Repository<Request>( r => r.Id );
    //        var proj = new Project { DataMarts = dms.ToList(), IsActive = true };
    //        var sec = new MoqSec( proj, user, dms, reqs.All );

    //        return scope
    //            .Override( data.UnitOfWork() )
    //            .Override( reqs )
    //            .Override( data.Repository( p => p.SID, proj ) )
    //            .Override( data.Repository<Document>( s => s.ID ) )
    //            .Override( data.Repository<RequestRouting>( s => new { s.DataMartId, s.RequestId } ) )
    //            .Override( data.Repository<RequestRoutingInstance>( s => s.Id ) )
    //            .Override( data.Repository( s => s.Id, dms ) )
    //            .Override( data.Repository( u => u.Id, user ) )
    //            .Override( data.Repository( a => a.Id, new Activity { Id = 1, Name = "Activity1" }, new Activity { Id = 2, Name = "Activity2" } ) )
    //            .Override( data.Repository( r => r.Id, user.Organization ) )
    //            .Override<IDnsModelPlugin>( new TestModelPlugin() )
    //            .Override<ISecurityService<DnsDomain>>( sec )
    //            .Override<ISecurityObjectHierarchyService<DnsDomain>>( sec )
    //            .Override( DnsMock.HttpContext() )
    //            .Override( DnsMock.Auth( user ) )
    //            .Override( DnsMock.Audit() );
    //    }

    //    PluginRequestType TestReqType( IPluginService s )
    //    {
    //        return s.GetPluginRequestType( TestModelPlugin.RequestTypeId );
    //    }

    //    TestModelPlugin TestPlugin( IPluginService s )
    //    {
    //        return TestReqType( s ).Plugin as TestModelPlugin;
    //    }

    //    CompositionFixture Composition;
    //    public void SetFixture( CompositionFixture data )
    //    {
    //        Composition = data;
    //    }

    //    void AssertSuccess( DnsResult res )
    //    {
    //        Assert.True( res.IsSuccess, string.Join( Environment.NewLine, res.ErrorMessages.EmptyIfNull() ) );
    //    }

    //    void AssertFail( DnsResult res, params string[] errMsgs )
    //    {
    //        Assert.False( res.IsSuccess );
    //        Assert.Equal( errMsgs, res.ErrorMessages.EmptyIfNull().ToArray() );
    //    }

    //    class TestModelPlugin : IDnsModelPlugin
    //    {
    //        public TestPostModel LastPost { get; set; }
    //        public bool Fail { get; set; }
    //        public Func<DnsRequestTransaction> OnEditPost { get; set; }
    //        public static readonly Guid ModelId = new Guid( "{85099DF3-E5D6-42D6-AB8D-0B6B264320A1}" );
    //        public static readonly Guid RequestTypeId = new Guid( "{C51BF490-747B-421F-A0E4-21F41353B908}" );
    //        public static readonly Guid MdRequestTypeId = new Guid( "{249CFC0D-DB62-494C-8246-C45D79FC0E65}" );

    //        public IEnumerable<IDnsModel> Models
    //        {
    //            get
    //            {
    //                return new[] { Dns.Model( ModelId, new Guid( "{184C9847-EBDC-45F9-A081-4BB3AC118259}" ),
    //                    "Test Model", 
    //                    Dns.RequestType( RequestTypeId, "Test Request", "" ),
    //                    Dns.RequestType( MdRequestTypeId, "Test MD Request", "", isMetadataRequest: true ) ) };
    //            }
    //        }

    //        public Func<System.Web.Mvc.HtmlHelper, System.Web.IHtmlString> DisplayRequest( IDnsRequestContext context )
    //        {
    //            throw new NotImplementedException();
    //        }

    //        public Func<System.Web.Mvc.HtmlHelper, System.Web.IHtmlString> DisplayConfigurationForm( IDnsModel model, Dictionary<string, string> properties )
    //        {
    //            return null;
    //        }

    //        public IEnumerable<string> ValidateConfig(ArrayList config)
    //        {
    //            return null;
    //        }

    //        public Func<System.Web.Mvc.HtmlHelper, System.Web.IHtmlString> DisplayResponse( IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode )
    //        {
    //            throw new NotImplementedException();
    //        }

    //        public Func<System.Web.Mvc.HtmlHelper, System.Web.IHtmlString> EditRequestView( IDnsRequestContext context )
    //        {
    //            throw new NotImplementedException();
    //        }

    //        public Func<HtmlHelper, System.Web.IHtmlString> EditRequestReDisplay( IDnsRequestContext request, IDnsPostContext post )
    //        {
    //            throw new NotImplementedException();
    //        }

    //        public DnsRequestTransaction EditRequestPost( IDnsRequestContext request, IDnsPostContext post )
    //        {
    //            LastPost = post.GetModel<TestPostModel>();
    //            if ( OnEditPost != null ) return OnEditPost();
    //            return Fail ? DnsRequestTransaction.Failed( "Error" ) : new DnsRequestTransaction();
    //        }

    //        public void CacheMetadataResponse( int requestId, IDnsDataMartResponse response )
    //        {
    //        }

    //        public IEnumerable<IDnsResponseExportFormat> GetExportFormats( IDnsResponseContext context )
    //        {
    //            return null;
    //        }

    //        public IDnsDocument ExportResponse( IDnsResponseContext context, IDnsResponseAggregationMode aggregationMode, IDnsResponseExportFormat format, string args )
    //        {
    //            return null;
    //        }

    //        public DnsResult ValidateForSubmission( IDnsRequestContext context )
    //        {
    //            return DnsResult.Success;
    //        }

    //        public IEnumerable<IDnsResponseAggregationMode> GetAggregationModes( IDnsRequestContext context )
    //        {
    //            return null;
    //        }

    //        public DnsRequestTransaction TimeShift( IDnsRequestContext ctx, TimeSpan timeDifference )
    //        {
    //            return new DnsRequestTransaction();
    //        }

    //        public DnsResponseTransaction ExecuteRequest(IDnsRequestContext context)
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    class TestPostModel
    //    {
    //        public string A { get; set; }
    //        public int B { get; set; }
    //    }

    //    class TestValidator : IDnsRequestValidator
    //    {
    //        public DnsResult Result { get; set; }
    //        public DnsResult Validate( IDnsRequestContext context ) { return Result; }
    //    }

    //    class MoqSec : ISecurityService<DnsDomain>, ISecurityObjectHierarchyService<DnsDomain>
    //    {
    //        private User _user;
    //        private Project _project;
    //        public IEnumerable<DataMart> DataMarts { get; set; }
    //        public IEnumerable<Request> Requests { get; set; }
    //        public bool AllowSubmit { get; set; }
    //        public bool RequireApproval { get; set; }

    //        public MoqSec( Project project, User user, DataMart[] dms, IEnumerable<Request> rs )
    //        {
    //            _project = project;
    //            _user = user;
    //            DataMarts = dms;
    //            Requests = rs;
    //            AllowSubmit = true;
    //        }

    //        public IQueryable<BigTuple<Guid>> AllGrantedTargets( ISecuritySubject subject, Expression<Func<Guid, bool>> privilegeFilter, int arity )
    //        {
    //            if ( ( arity == 4 && AllowSubmit ) || arity == 3 )
    //            {
    //                return (from d in DataMarts
    //                        from t in new[] { TestModelPlugin.MdRequestTypeId, TestModelPlugin.RequestTypeId }
    //                        select arity == 4 
    //                            ? BigTuple.Create( _project.SID, d.Organization.SID, d.SID, t )
    //                            : BigTuple.Create( _project.SID, d.Organization.SID, d.SID )
    //                       )
    //                       .AsQueryable();
    //            }

    //            return Enumerable.Empty<BigTuple<Guid>>().AsQueryable();
    //        }

    //        public void SetAcl( SecurityTarget target, IEnumerable<AclEntry> entries )
    //        {
    //        }

    //        public IQueryable<SecurityTargetAcl> GetAllAcls( int arity )
    //        {
    //            if ( arity == 4 && AllowSubmit )
    //            {
    //                var entries = new[] { new UnresolvedAclEntry { SubjectId = _user.SID, PrivilegeId = SecPrivileges.RequestType.SubmitManual.SID, Allow = AllowSubmit, IsInherited = false } };
    //                return AllGrantedTargets( null, null, 4 )
    //                        .Select( t => new SecurityTargetAcl { TargetId = t, Entries = entries } )
    //                        .AsQueryable();
    //            }
    //            else if ( arity == 3 )
    //            {
    //                var privs = new[] 
    //                            { 
    //                                typeof( SecPrivileges.Crud ).GetFields().Select( p => p.GetValue( null ) ),
    //                                typeof( RequestPrivileges ).GetFields().Select( p => p.GetValue( null ) )
    //                            }
    //                            .SelectMany( xx => xx )
    //                            .Where( p => !RequireApproval || p != RequestPrivileges.SkipSubmissionApproval )
    //                            .OfType<SecurityPrivilege>();
    //                return (from r in Requests
    //                        group r by new { r.Project, r.Organization, r.CreatedByUser } into rr
    //                        let r = rr.Key
    //                        from p in privs
    //                        select new SecurityTargetAcl
    //                        {
    //                            TargetId = BigTuple.Create( r.Project != null ? r.Project.SID : VirtualSecObjects.AllProjects.SID, r.Organization.SID, r.CreatedByUser.SID ),
    //                            Entries = new[] { new UnresolvedAclEntry { SubjectId = _user.SID, PrivilegeId = p.SID, Allow = true } }
    //                        }
    //                       ).AsQueryable();
    //            }

    //            return Enumerable.Empty<SecurityTargetAcl>().AsQueryable();
    //        }

    //        public IEnumerable<SecurityTargetKind> KindsFor( SecurityTarget target )
    //        {
    //            throw new NotImplementedException();
    //        }

    //        public IEnumerable<SecurityPrivilege> PrivilegesFor( SecurityTargetKind targetKind )
    //        {
    //            throw new NotImplementedException();
    //        }

    //        public SecurityTarget ResolveTarget( BigTuple<Guid> id, SecurityTargetKind kind )
    //        {
    //            throw new NotImplementedException();
    //        }

    //        public AnnotatedAclEntry ResolveAclEntry( UnresolvedAclEntry e, SecurityTargetKind targetKind )
    //        {
    //            throw new NotImplementedException();
    //        }

    //        public void SetObjectInheritanceParent( ISecurityObject obj, ISecurityObject parent )
    //        {
    //        }

    //        public IDictionary<Guid, SecurityPrivilege> AllPrivileges
    //        {
    //            get { throw new NotImplementedException(); }
    //        }

    //        public IEnumerable<SecurityTargetKind> AllTargetKinds
    //        {
    //            get { throw new NotImplementedException(); }
    //        }


    //        public IQueryable<Guid> GetObjectChildren( ISecurityObject obj, bool includeSelf )
    //        {
    //            throw new NotImplementedException();
    //        }

    //        public IQueryable<Guid> GetObjectTransitiveChildren( ISecurityObject obj, bool includeSelf )
    //        {
    //            throw new NotImplementedException();
    //        }

    //        public System.Linq.Expressions.Expression<Func<Guid, IQueryable<Guid>>> GetObjectChildren( bool includeSelf )
    //        {
    //            throw new NotImplementedException();
    //        }

    //        public System.Linq.Expressions.Expression<Func<Guid, IQueryable<Guid>>> GetObjectTransitiveChildren( bool includeSelf )
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }
    //}
}