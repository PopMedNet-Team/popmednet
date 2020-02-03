using Lpp.Data.Composition;
using Lpp.Security.Data.Tuples;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Model
{
    public class DataContext : ComposableDbContext<DnsDomain>
    {
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestRoutingInstance> RoutingInstances { get; set; }
        public DbSet<RequestRouting> Routings { get; set; }

        public DbSet<Document> Documents { get; set; }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<Group> Groups { get; set; }
        public DbSet<Registry> Registries { get; set; }
        public DbSet<DataMart> DataMarts { get; set; }
        public DbSet<Project> Projects { get; set; }

        public DbSet<Organization> Organizations { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Demographic> Demographics { get; set; }

        public DbSet<Tuple1> SecurityTuple1s { get; set; }
        public DbSet<Tuple2> SecurityTuple2s { get; set; }
        public DbSet<Tuple3> SecurityTuple3s { get; set; }
        public DbSet<Tuple4> SecurityTuple4s { get; set; }

        public DataContext()
            : base("Lpp.Dns.Model.DnsDomain", null)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = true;
            var objectContext = (this as IObjectContextAdapter).ObjectContext;
            objectContext.CommandTimeout = 999;
        }

        public DataContext(bool enableLazyLoading) : this()
        {
            this.Configuration.LazyLoadingEnabled = enableLazyLoading;
            this.Configuration.ProxyCreationEnabled = enableLazyLoading;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Note this overrides the default on the composabledbcontext.

            //Loads all of the configuration classes to define the joins. These are stored in the same file as the originating class
            //Fluent should only be used for relationships that are not easily describable using attributes
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes().Where(
                type => type.GetInterfaces().Any(t => t == typeof(Lpp.Data.Composition.IPersistenceDefinition<DnsDomain>))).ToList();
            foreach (IPersistenceDefinition<DnsDomain> configurationInstance in typesToRegister.Select(Activator.CreateInstance))
            {
                configurationInstance.BuildModel(modelBuilder);
            }
        }

        public override int SaveChanges()
        {
            int count = 0;
        retry:

            try
            {
                return base.SaveChanges();
            }
            catch (CommitFailedException ce)
            {
                count++;
                if (count <= 5)
                {
                    System.Threading.Thread.Sleep(300 * count);
                    goto retry;
                }

                throw ce;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public override Task<int> SaveChangesAsync()
        //{
        //    int count = 0;
        //retry:

        //    try
        //    {
        //        return base.SaveChangesAsync();
        //    }
        //    catch (CommitFailedException ce)
        //    {
        //        count++;
        //        if (count <= 5)
        //        {
        //            System.Threading.Thread.Sleep(300 * count);
        //            goto retry;
        //        }

        //        throw ce;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public override Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        //{
        //    int count = 0;
        //retry:

        //    try
        //    {
        //        return base.SaveChangesAsync(cancellationToken);
        //    }
        //    catch (CommitFailedException ce)
        //    {
        //        count++;
        //        if (count <= 5)
        //        {
        //            System.Threading.Thread.Sleep(300 * count);
        //            goto retry;
        //        }

        //        throw ce;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
