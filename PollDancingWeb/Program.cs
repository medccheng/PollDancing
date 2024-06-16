using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using PollDancingLibrary.Data;

var builder = WebApplication.CreateBuilder(args);

var logger = LogManager.Setup()
    .LoadConfigurationFromAppSettings()
    .GetCurrentClassLogger();

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();

builder.Services.AddHttpClient();
// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Configure DbContext with SQL Server
//builder.Services.AddDbContext<CongressDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("CongressDBConnection")));

// Configure DbContext with SQL Server
//var server = Environment.GetEnvironmentVariable("server");
//var port = Environment.GetEnvironmentVariable("port");
//var database = Environment.GetEnvironmentVariable("database");
//var username = Environment.GetEnvironmentVariable("user");
//var password = Environment.GetEnvironmentVariable("password");

var connectionString = builder.Configuration.GetConnectionString("CongressDBConnection");
//    .Replace("{server}", server)
//    .Replace("{port}", port)
//    .Replace("{database}", database)
//    .Replace("{username}", username)
//    .Replace("{password}", password);

Console.WriteLine($"Connecting with: {connectionString}");
builder.Services.AddDbContext<CongressDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(60),
            errorNumbersToAdd: null);
    }).UseLazyLoadingProxies());


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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
