using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace PopMedNet.DMCS.Tests
{
    [TestClass]
    public class SyncTests
    {
        static IConfigurationRoot _config;
        readonly IServiceProvider _serviceProvider;
        static ILogger _log;
        const string TestEntityPrefix = "DMCS-Test-";

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            _log = Log.Logger;
        }

        public SyncTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<Data.Model.ModelContext>(builder => {
                var connectionString = _config.GetSection("ConnectionStrings").GetValue<string>("ModelContext");

                builder.UseSqlServer(connectionString, opt => {
                    opt.EnableRetryOnFailure();
                });
            });

            _serviceProvider = services.BuildServiceProvider();
        }

        

        [TestMethod]
        public async Task DeleteDatamart()
        {
            Guid dmID = Guid.NewGuid();
            Guid userID = Guid.NewGuid();
            Guid requestID = Guid.NewGuid();

            try
            {
                //create a datamart, user, and association
                using (var scope = _serviceProvider.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<Data.Model.ModelContext>();
                    _log.Information($"Database context ID: {db.ContextId}");

                    var dm = new Data.Model.DataMart {
                        ID = dmID,
                        Name = TestEntityPrefix + "01",
                        Acronym = "TEP-01",
                        Adapter = "Test Model Adapter",
                        AdapterID = Guid.Empty,
                        AutoProcess = Data.Enums.AutoProcesses.None,
                        CacheDays = 30,
                        EnableExplictCacheRemoval = true,
                        EncryptCache = false,
                        PmnTimestamp = BitConverter.GetBytes(DateTime.UtcNow.Ticks)
                    };

                    db.DataMarts.Add(dm);

                    var user = new Data.Model.User
                    {
                        ID = userID,
                        UserName = TestEntityPrefix + "01",
                        Email = TestEntityPrefix + "01@testing.local"
                    };
                    db.Users.Add(user);

                    db.UserDataMarts.Add(new Data.Model.UserDataMart { 
                        DataMartID = dm.ID,
                        UserID = user.ID
                    });


                    var request = new Data.Model.Request
                    {
                        ID = requestID,
                        Name = TestEntityPrefix + "01",
                        Identifier = DateTime.Now.Ticks,
                        MSRequestID = TestEntityPrefix + "01",
                        Description = string.Empty,
                        AdditionalInstructions = string.Empty,
                        Activity = "Not Selected",
                        RequestType = "[All Models] File Distribution",
                        SubmittedOn = DateTime.UtcNow,
                        SubmittedBy = TestEntityPrefix + "01",
                        Project = "DMCS Tests",
                        TaskOrder = "Not Selected",
                        ActivityProject = "Not Selected",
                        SourceActivity = "Not Selected",
                        SourceActivityProject = "Not Selected",
                        SourceTaskOrder = "Not Selected"
                    };

                    db.Requests.Add(request);

                    var route = new Data.Model.RequestDataMart
                    {
                        ID = Guid.NewGuid(),
                        DataMartID = dmID,
                        RequestID = request.ID,
                        ModelID = Guid.Parse("85EE982E-F017-4BC4-9ACD-EE6EE55D2446"),
                        ModelText = "PCORnet CDM",
                        Status = Data.Enums.RoutingStatus.Submitted,
                        Priority = Data.Enums.Priorities.Low,
                        UpdatedOn = DateTime.UtcNow
                    };
                    db.RequestDataMarts.Add(route);

                    await db.SaveChangesAsync();

                }

                //attempt to delete the datamart
                using (var scope = _serviceProvider.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<Data.Model.ModelContext>();
                    _log.Information($"Database context ID: {db.ContextId}");

                    //Assert.IsNotNull(db.DataMarts.FirstOrDefault(dm => dm.Name == TestEntityPrefix + "01"));

                    var dm = await db.DataMarts.FindAsync(dmID);
                    db.UserDataMarts.RemoveRange(db.UserDataMarts.Where(udm => udm.DataMartID == dm.ID));

                    db.DataMarts.Remove(dm);

                    db.SaveChanges();

                }
            }
            finally
            {
                using(var scope = _serviceProvider.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<Data.Model.ModelContext>();
                    db.UserDataMarts.RemoveRange(db.UserDataMarts.Where(udm => udm.DataMart.Name.StartsWith(TestEntityPrefix)));
                    db.Users.RemoveRange(db.Users.Where(u => u.UserName.StartsWith(TestEntityPrefix)));
                    db.DataMarts.RemoveRange(db.DataMarts.Where(dm => dm.Name.StartsWith(TestEntityPrefix)));

                    await db.SaveChangesAsync();

                    Assert.IsNull(db.DataMarts.FirstOrDefault(dm => dm.Name == TestEntityPrefix + "01"));
                }
            }
        }
    }
}
