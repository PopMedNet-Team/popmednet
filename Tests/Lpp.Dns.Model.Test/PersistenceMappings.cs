using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
//using Xunit;
using Lpp.Dns.Tests;
using Lpp.Composition;
using System.ComponentModel.Composition;

namespace Lpp.Dns.Model.Test
{
    //public class PersistenceMappings : TestBase
    //{
    //    [Fact]
    //    public void PersistenceMappings_Request()
    //    {
    //        TestEntity<Request>( ( repo, scope ) =>
    //            // This tests INSERT
    //            repo.Add( new Request
    //            {
    //                Project = new Project { Group = new Group() },
    //                RequestTypeId = Guid.NewGuid(),
    //                Created = DateTime.Now,
    //                CreatedByUser = new User { FirstName = "a", LastName = "b", Email = "c", Username = "x", Organization = new Organization { } }
    //            } )
    //        );

    //        TestEntity<Request>( repo =>
    //        {
    //            // This tests SELECT
    //            var req = repo.All.FirstOrDefault();

    //            // This tests lazy loading of navigation properties
    //            var dms = req.Routings;
    //        } );
    //    }

    //    [Fact]
    //    public void PersistenceMappings_DataMartRequest()
    //    {
    //        TestEntity<RequestRouting>( ( repo, scope ) => repo.Add( new RequestRouting
    //        {
    //            DataMart = new DataMart
    //            {
    //                Name = "a",
    //                Organization = new Organization { Name = "org" }
    //            },
    //            Request = new Request
    //            {
    //                Project = new Project { Group = new Group() },
    //                RequestTypeId = Guid.NewGuid(),
    //                CreatedByUser = new User { Username = "a", Organization = new Organization() },
    //                Organization = new Organization { Name = "org2" }
    //            }
    //        } ) );
    //    }

    //    [Fact]
    //    public void PersistenceMappings_Project()
    //    {
    //        TestEntity<Project>( r => r.Add( new Project { Group = new Group() } ) );
    //        TestEntity<Project>( r => r.All.First() );
    //        TestEntity<Project>( r => r.All.First().DataMarts.Add( new DataMart { Organization = new Organization() } ) );
    //        TestEntity<Project>( r => r.All.First().SecurityGroups.Add( new ProjectSecurityGroup() ) );
    //    }

    //    void TestEntity<TEntity>( Action<IRepository<DnsDomain, TEntity>> action )
    //        where TEntity : class
    //    {
    //        TestEntity<TEntity>( ( repo, svc ) => action( repo ) );
    //    }

    //    void TestEntity<TEntity>( Action<IRepository<DnsDomain,TEntity>, ICompositionService> action ) 
    //        where TEntity : class
    //    {
    //        using ( var scope = OpenScope() )
    //        {
    //            var repo = scope.GetExportedValue<IRepository<DnsDomain, TEntity>>();
    //            var uow = scope.GetExportedValue<IUnitOfWork<DnsDomain>>();
    //            action( repo, scope );
    //            uow.Commit();
    //        }
    //    }
    //}
}