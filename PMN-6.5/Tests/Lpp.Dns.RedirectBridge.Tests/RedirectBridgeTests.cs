using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Xml.Linq;
using Lpp.Composition;
using Lpp.Dns.Portal;
using Lpp.Dns.Tests;
using Lpp.Security;
using Lpp.Tests;
//using Xunit;

namespace Lpp.Dns.RedirectBridge.Tests
{
//    public class RedirectBridgeTests : IUseFixture<CompositionFixture>
//    {
//        [Fact]
//        public void Redirect_SOAP()
//        {
//            //Wcf.TestService<RedirectModelServices, IRequestService>( 
//            //    Composition, true, MockExports,
//            //    ( scope, address, chf ) =>
//            //    {
//            //        var sess = scope.Get<SessionService>().CreateSession( 1, "returnUrl", null );

//            //        var md = chf.GetSessionMetadata( sess.Id );
//            //        Assert.Equal( md.ReturnUrl, "returnUrl" );

//            //        chf.PostDocument( sess.Id, "doc", "text/plain", true, new byte[] { 1, 2, 3 } );
//            //        var doc = scope.Get<IRepository<RedirectDomain, PluginSessionDocument>>().All.Single();
//            //        Assert.Equal( "doc", doc.Name );
//            //        Assert.Equal( "text/plain", doc.MimeType );
//            //        Assert.Equal( true, doc.IsViewable );
//            //        Assert.Equal( new byte[] { 1, 2, 3 }, doc.Body );

//            //        var dms = chf.GetApplicableDataMarts( sess.Id );

//            //        chf.RequestCreated( sess.Id, new RequestHeader(), new[] { 1, 2, 3 } );
//            //    } );

//            //Wcf.TestService<RedirectModelServices, IResponseService>(
//            //    Composition, true, MockExports,
//            //    ( scope, address, chf ) =>
//            //    {
//            //        var sess = scope.Get<SessionService>().CreateSession( 1, "returnUrl", null );

//            //        var md = chf.GetSessionMetadata( sess.Id );
//            //        Assert.Equal( md.Session.ReturnUrl, "returnUrl" );
//            //    } );
//        }

//        [Fact]
//        public void Redirect_REST()
//        {
////            Wcf.TestService<RedirectModelServices, IRequestService>(
////                Composition, true, MockExports,
////                ( scope, address, chf ) =>
////                {
////                    var sess = scope.Get<SessionService>().CreateSession( 1, "returnUrl", null );
                    
////                    var mds = new WebClient().DownloadString( address.ToString() + "/" + sess.Id + "/Session" );
////                    AssertXmlEqual( @"
////                            <SessionMetadata xmlns=""http://lincolnpeak.com/schemas/DNS4/API"">
////                              <RequestId>*</RequestId>
////                              <ModelId>*</ModelId>
////                              <RequestTypeId>*</RequestTypeId>
////                              <ReturnUrl>returnUrl</ReturnUrl>
////                            </SessionMetadata>
////                        ", mds );

////                    var dms = new WebClient().DownloadString( address.ToString() + "/" + sess.Id + "/ApplicableDataMarts" );
////                    AssertXmlEqual( @"
////                        <DataMarts xmlns=""http://lincolnpeak.com/schemas/DNS4/API"">
////                          <DataMart>
////                            <Id>1</Id>
////                            <Name>DataMart1</Name>
////                            <Metadata />
////                          </DataMart>
////                          <DataMart>
////                            <Id>2</Id>
////                            <Name>DataMart2</Name>
////                            <Metadata />
////                          </DataMart>
////                        </DataMarts>", dms );

////                    PostOperation( sess, address, "Abort", "" );
////                    Assert.True( sess.IsAborted );

////                    sess = scope.Get<SessionService>().CreateSession( 1, "returnUrl", null );

////                    PostOperation( sess, address, "Document", @"
////                        <PostDocument xmlns=""http://lincolnpeak.com/schemas/DNS4/API"">
////                          <Name>document name</Name>
////                          <MimeType>some/type</MimeType>
////                          <Viewable>true</Viewable>
////                          <Body>" + Convert.ToBase64String( new byte[] { 11, 22, 33, 44 } ) + @"</Body>
////                        </PostDocument>
////                    " );
////                    var doc = scope.Get<IRepository<RedirectDomain, PluginSessionDocument>>().All.Single();
////                    Assert.Equal( "document name", doc.Name );
////                    Assert.Equal( "some/type", doc.MimeType );
////                    Assert.Equal( true, doc.IsViewable );
////                    Assert.Equal( new byte[] { 11, 22, 33, 44 }, doc.Body );

////                    PostOperation( sess, address, "Commit", @"
////                        <RequestCreated xmlns=""http://lincolnpeak.com/schemas/DNS4/API"">
////                          <Header>
////                            <Name>TheRequestName</Name>
////                            <Description>The request description</Description>
////                            <Activity>Some activity</Activity>
////                            <ActivityDescription>activity description</ActivityDescription>
////                            <DueDate>2012-01-01T12:30:55</DueDate>
////                            <Priority>Low</Priority>
////                          </Header>
////                          <ApplicableDataMarts>
////                            <int>1</int>
////                          </ApplicableDataMarts>
////                        </RequestCreated>
////                    " );
////                    Assert.True( sess.IsCommitted );
////                } );
//        }

//        [Fact]
//        public void Redirect_DocumentDownload()
//        {

//        }

//        void PostOperation( PluginSession sess, Uri baseAddress, string operation, string body )
//        {
//            //var req = WebRequest.Create( baseAddress.ToString() + "/" + sess.Id + "/" + operation ) as HttpWebRequest;
//            //req.Method = "POST";
//            //req.ContentType = "text/xml";
//            //using ( var s = req.GetRequestStream() )
//            //{
//            //    using ( var w = new StreamWriter( s ) )
//            //    {
//            //        w.Write( body );
//            //    }
//            //}
//            //req.GetResponse();
//        }

//        void AssertXmlEqual( string expected, string actual )
//        {
//            Assert.True( Compare( XElement.Parse( expected ), XElement.Parse( actual ) ), string.Format( "Expected: {1}{0}{1}{1}Actual: {1}{2}", expected, Environment.NewLine, actual ) );
//        }

//        private bool Compare( XElement expected, XElement actual )
//        {
//            if ( expected.Name != actual.Name ) return false;
//            if ( expected.Descendants().Any() != actual.Descendants().Any() ) return false;
//            if ( !expected.Descendants().Any() && expected.Value != "*" && expected.Value != actual.Value ) return false;

//            var exEs = expected.Elements().OrderBy( e => e.Name.LocalName ).ToList();
//            var acEs = actual.Elements().OrderBy( e => e.Name.LocalName ).ToList();
//            var allEqual = exEs.Zip( acEs, ( e, a ) => Compare( e, a ) ).All( c => c );
//            if ( !allEqual ) return false;
//            if ( exEs.Any() ) return true;

//            return expected.Value == "*" || expected.Value == actual.Value;
//        }

//        MockScopeBuilder MockExports( MockScopeBuilder scope )
//        {
//            //var data = Mock.Data<DnsDomain>();
//            //var org = new Organization { Id = 1, Name = "org" };
//            //var dms = data.Repository( s => s.Id, 
//            //    new Lpp.Dns.Model.DataMart { Id = 1, Name = "DataMart1", Organization = org, InstalledModels = { new DataMartInstalledModel { ModelId = GetAnyModelId() } } },
//            //    new Lpp.Dns.Model.DataMart { Id = 2, Name = "DataMart2", Organization = org, InstalledModels = { new DataMartInstalledModel { ModelId = GetAnyModelId() } } } );
//            //var user = new User { Id = 1, Username = "user", Organization = org };
//            //var reqs = data.Repository( r => r.Id, new Request { Id = 1, RequestTypeId = GetAnyRequestTypeId(), Organization = org, CreatedByUser = user, UpdatedByUser = user } );
//            //var sec = new MoqSec { Project = new Project(), User = user, DataMarts = dms.All, RequestTypeId = GetAnyRequestTypeId(), Requests = reqs.All };

//            //return scope
//            //    .Override( data.UnitOfWork() )
//            //    .Override( reqs )
//            //    .Override( data.Repository( p => p.SID, sec.Project ) )
//            //    .Override( data.Repository<Lpp.Dns.Model.Document>( s => s.ID ) )
//            //    .Override( data.Repository<Lpp.Dns.Model.Activity>( s => s.Id ) )
//            //    .Override( data.Repository<Lpp.Dns.Model.RequestRouting>( s => new { s.DataMartId, s.RequestId } ) )
//            //    .Override( data.Repository<Lpp.Dns.Model.RequestRoutingInstance>( i => i.Id ) )
//            //    .Override( dms )
//            //    .Override( data.Repository( u => u.Id, user ) )
//            //    .Override( data.Repository( o => o.Id, org ) )
//            //    .Override( Mock.Data<RedirectDomain>().Repository<Model>( m => m.Id ) )
//            //    .Override( Mock.Data<RedirectDomain>().Repository<PluginSession>( s => s.Id ) )
//            //    .Override( Mock.Data<RedirectDomain>().Repository<PluginSessionDocument>( s => s.ID ) )
//            //    .Override<ISecurityService<DnsDomain>>( sec )
//            //    .Override<ISecurityObjectHierarchyService<DnsDomain>>( sec )
//            //    .Override( DnsMock.HttpContext() )
//            //    .Override( DnsMock.Auth( user ) )
//            //    .Override( DnsMock.Audit() );
//        }

//        CompositionContainer MinimalComp()
//        {
//            //        var sec = new MoqSec();
//            //return Composition.MockScope()
//            //    .Override( DnsMock.HttpContext() )
//            //    .Override<ISecurityService<DnsDomain>>( sec )
//            //                    .Override<ISecurityObjectHierarchyService<DnsDomain>>( sec )
//            //                    .Module( Audit.Aud.Module<DnsDomain>() )
//            //    .Build();
//        }

//        Guid GetAnyModelId()
//        {
//            using ( var scope = MinimalComp() )
//                return 
//                    scope
//                    .GetMany<IDnsModelPlugin>()
//                                        .Where( p => !(p is RedirectModelPlugin) )
//                    .SelectMany( p => p.Models )
//                    .First().Id;
//        }

//        Guid GetAnyRequestTypeId()
//        {
//            using ( var scope = MinimalComp() )
//                return 
//                    scope
//                    .GetMany<IDnsModelPlugin>()
//                                        .Where( p => !(p is RedirectModelPlugin) )
//                                        .SelectMany( p => p.Models )
//                    .SelectMany( m => m.Requests )
//                    .First().Id;
//        }

//        CompositionFixture Composition;
//        public void SetFixture( CompositionFixture data )
//        {
//            Composition = data;
//        }

//        class MoqSec : ISecurityService<Lpp.Dns.Model.DnsDomain>, ISecurityObjectHierarchyService<Lpp.Dns.Model.DnsDomain>
//        {
//            public IEnumerable<Lpp.Dns.Model.DataMart> DataMarts { get; set; }
//            public IEnumerable<Lpp.Dns.Model.Request> Requests { get; set; }
//            public User User { get; set; }
//            public Project Project { get; set; }
//            public Guid RequestTypeId { get; set; }

//            public IQueryable<BigTuple<Guid>> AllGrantedTargets( ISecuritySubject subject, Expression<Func<Guid, bool>> privilegeFilter, int arity )
//            {
//                return (from d in DataMarts
//                        select BigTuple.Create( Project.SID, d.Organization.SID, d.SID, RequestTypeId )
//                       ).AsQueryable();
//            }

//            public void SetAcl( SecurityTarget target, IEnumerable<AclEntry> entries )
//            {
//                throw new NotImplementedException();
//            }

//            public IQueryable<SecurityTargetAcl> GetAllAcls( int arity )
//            {
//                if ( arity == 4 )
//                {
//                    var entries = new[] { new UnresolvedAclEntry { SubjectId = User.SID, PrivilegeId = Lpp.Dns.Portal.SecPrivileges.RequestType.SubmitManual.SID, Allow = true, IsInherited = false } };
//                    return DataMarts.Select( dm => new SecurityTargetAcl
//                    {
//                        TargetId = BigTuple.Create( Project.SID, dm.Organization.SID, dm.SID, RequestTypeId ),
//                        Entries = entries
//                    } )
//                    .AsQueryable();
//                }
//                else if ( arity == 3 )
//                {
//                    var privs = new[] { typeof( SecPrivileges.Crud ), typeof( RequestPrivileges ) }
//                                .SelectMany( xx => xx.GetFields().Select( p => p.GetValue( null ) ) )
//                                .OfType<SecurityPrivilege>();
//                    return (from r in Requests
//                            group r by new { r.Project, r.Organization, r.CreatedByUser } into rr
//                            let r = rr.Key
//                            from p in privs
//                            select new SecurityTargetAcl
//                            {
//                                TargetId = BigTuple.Create( r.Project != null ? r.Project.SID : VirtualSecObjects.AllProjects.SID, r.Organization.SID, r.CreatedByUser.SID ),
//                                Entries = new[] { new UnresolvedAclEntry { SubjectId = User.SID, PrivilegeId = p.SID, Allow = true } }
//                            }
//                           ).AsQueryable();
//                }

//                return Enumerable.Empty<SecurityTargetAcl>().AsQueryable();
//            }

//            public IEnumerable<SecurityTargetKind> KindsFor( SecurityTarget target )
//            {
//                throw new NotImplementedException();
//            }

//            public IEnumerable<SecurityPrivilege> PrivilegesFor( SecurityTargetKind targetKind )
//            {
//                throw new NotImplementedException();
//            }

//            public SecurityTarget ResolveTarget( BigTuple<Guid> id, SecurityTargetKind kind )
//            {
//                throw new NotImplementedException();
//            }

//            public AnnotatedAclEntry ResolveAclEntry( UnresolvedAclEntry e, SecurityTargetKind targetKind )
//            {
//                throw new NotImplementedException();
//            }

//            public void SetObjectInheritanceParent( ISecurityObject obj, ISecurityObject parent )
//            {
//            }


//            public IDictionary<Guid, SecurityPrivilege> AllPrivileges
//            {
//                get { throw new NotImplementedException(); }
//            }

//            public IEnumerable<SecurityTargetKind> AllTargetKinds
//            {
//                get { throw new NotImplementedException(); }
//            }


//            public IQueryable<Guid> GetObjectChildren( ISecurityObject obj, bool _ )
//            {
//                throw new NotImplementedException();
//            }

//            public IQueryable<Guid> GetObjectTransitiveChildren( ISecurityObject obj, bool _ )
//            {
//                throw new NotImplementedException();
//            }

//            public System.Linq.Expressions.Expression<Func<Guid, IQueryable<Guid>>> GetObjectChildren( bool _ )
//            {
//                throw new NotImplementedException();
//            }

//            public System.Linq.Expressions.Expression<Func<Guid, IQueryable<Guid>>> GetObjectTransitiveChildren( bool _ )
//            {
//                throw new NotImplementedException();
//            }
//        }
//    }
}