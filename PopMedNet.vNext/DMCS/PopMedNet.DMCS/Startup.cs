using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PopMedNet.DMCS.Code;
using PopMedNet.DMCS.Utils;
using Serilog;
using System;
using System.IO;

namespace PopMedNet.DMCS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Log.Logger.ForContext("SourceContext", typeof(Startup)).Information("Started DMCS Application");

            if (string.IsNullOrEmpty(Configuration.GetValue<string>("Settings:DMCSIdentifier")))
            {
                Log.Logger.ForContext("SourceContext", typeof(Startup)).Information("DMCS Identifier was not found in the config.  Regenerating a new one.");
                
                JObject o = JObject.Parse(File.ReadAllText("appsettings.json"));

                o["Settings"]["DMCSIdentifier"] = Guid.NewGuid().ToString("D");

                File.WriteAllText("appsettings.json", JsonConvert.SerializeObject(o));
            }


            services.AddDbContext<Data.Identity.IdentityContext>(options => {
                options.UseSqlServer(
                    Configuration.GetConnectionString("DBContextConnection"),
                    x => x.MigrationsAssembly("PopMedNet.DMCS.Data"));
            });
            services.AddDbContext<Data.Model.ModelContext>(options => {
                options.UseSqlServer(
                    Configuration.GetConnectionString("DBContextConnection"),
                    x => x.MigrationsAssembly("PopMedNet.DMCS.Data"));
            });

            //by adding the password hasher before the default identity call it will replace the default implementation.
            services.AddScoped<IPasswordHasher<Data.Identity.IdentityUser>, Data.Identity.PasswordHasher>();

            var applicationParameters = new ApplicationParameters();
            applicationParameters.SyncServiceInterval = TimeSpan.Parse(Configuration.GetValue<string>("Settings:SyncServiceInterval"));
            services.AddSingleton(applicationParameters);

            services.AddHostedService<DbMigrator>();
            services.AddTransient<Code.Sync.RouteSyncPersister>();
            services.AddSingleton<SyncService>();
            services.AddTransient<Code.UserSync>();
            services.AddHostedService<BackgroundJobs>();
            services.AddSingleton<ThemeManager>();
            services.AddTransient<Code.Authorization.RequestHeaderBasicAuthorizationFilter>();
            services.AddHostedService<Services.BackgroundWorkerService>();
            services.AddSingleton<Services.BackgroundWorkerQueue>();

            services.Configure<DMCSConfiguration>(Configuration);

            services.AddIdentity<Data.Identity.IdentityUser, IdentityRole<Guid>>()
                .AddUserManager<Data.Identity.DMCSUserManager>()
                .AddEntityFrameworkStores<Data.Identity.IdentityContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opts =>
            {
                opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(Configuration.GetSection("Application").GetValue<int>("LockoutTime"));
                opts.Lockout.MaxFailedAccessAttempts = Configuration.GetSection("Application").GetValue<int>("LockoutCount");
                opts.Lockout.AllowedForNewUsers = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(Configuration.GetSection("Application").GetValue<int>("SessionDurationMinutes"));

                options.LoginPath = "/login";
                options.SlidingExpiration = true;
            });

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            services.AddSignalR();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/home/error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<RequestHub>("RequestHub");
                endpoints.MapHub<LogHub>("LogHub");
                endpoints.MapHub<SessionHub>("SessionHub");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
