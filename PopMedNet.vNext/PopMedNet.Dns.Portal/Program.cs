using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie( options => {
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(builder.Configuration.GetValue<int>("SessionExpirationMinutes"));
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Forbidden/";
        options.ReturnUrlParameter = "returnUrl";
    });

builder.Services.AddHostedService<PopMedNet.Utilities.WebSites.BackgroundWorkerService>();
builder.Services.AddSingleton<PopMedNet.Utilities.WebSites.BackgroundWorkerQueue>();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<PopMedNet.Dns.Portal.Code.CommonPageValuesActionFilter>();
});
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();    
}

app.UseHttpsRedirection();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    //register the folders that contain the ts files to be accessible as static content.
    //This allows for the browser to be able to request the ts files when debugging.
    app.UseFileServer(
            new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "js")),
                RequestPath = new PathString("/js")
            }
            );

    app.UseFileServer(
            new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "scripts")),
                RequestPath = new PathString("/scripts")
            }
            );
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<PopMedNet.Dns.Portal.Hubs.SessionHub>("SessionHub");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
