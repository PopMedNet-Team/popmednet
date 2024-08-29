using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Serilog;

var AllowedSpecificOrigins = "_allowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfiguration) => { 
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console();
});

// Add services to the container.
builder.Services.AddCors( options => {
    options.AddPolicy(name: AllowedSpecificOrigins,
        policy =>
        {
            policy.AllowAnyHeader()
                  .AllowAnyOrigin()
                  .AllowAnyMethod();
        });
});


builder.Services.AddDbContext<PopMedNet.Dns.Data.DataContext>( (serviceProvider, options) =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DataContext"));
});

builder.Services.AddAutoMapper((config) => {
            config.CreateMap<DateTime, DateTimeOffset?>();
        }
    ,typeof(PopMedNet.Dns.Data.DataContext).Assembly);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

builder.Services.AddAuthentication(
    options => options.DefaultScheme = PopMedNet.Utilities.WebSites.Security.AuthSchemeConstants.Scheme)
    .AddScheme<PopMedNet.Utilities.WebSites.Security.PopMedNetAuthenticationSchemeOptions, PopMedNet.Utilities.WebSites.Security.PopMedNetAuthenticationHandler>(
        PopMedNet.Utilities.WebSites.Security.AuthSchemeConstants.Scheme, options => { });

builder.Services.AddScoped<PopMedNet.Utilities.Security.IPopMedNetAuthenticationManager, PopMedNet.Dns.Data.DataContext>();

builder.Services.AddControllers( options =>
    { 
        options.Filters.Add<PopMedNet.Dns.Api.HttpResponseExceptionFilter>();
    })
    .AddOData(options => options.Select().Filter().OrderBy().SetMaxTop(null).Count())    
    .AddJsonOptions(options => {
        //removing the camel casing policy from json serialization
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        //add custom serialization formatter to format all dates as UTC. Conversion to local should be done at the client.
        options.JsonSerializerOptions.Converters.Add(new PopMedNet.Utilities.WebSites.Formatters.DateTimeConverterFactory());
    });

builder.Services.AddResponseCaching();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors(AllowedSpecificOrigins);

app.UseResponseCaching();

app.UseAuthorization();

app.MapControllers();

app.Run();
