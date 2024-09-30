using IdentityStandaloneMfa.Common;
using IdentityStandaloneMfa.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

#region Nlog Changes
builder.Logging.ClearProviders(); // Clears default logging providers.
builder.Host.UseNLog(); // Use NLog as the logging provider.
var logger = NLog.LogManager.GetCurrentClassLogger();
#endregion

#region MyRegion

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("DatabaseConnection"), builder =>
    {
        builder.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromMinutes(100), errorNumbersToAdd: null);
    });
});
builder.Services.AddIdentity<IdentityUser, IdentityRole>(
    options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<DatabaseContext>()
    .AddDefaultTokenProviders();

var appSettingsSection = configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
builder.Services.AddSingleton<IConfiguration>(configuration);

builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<IdentityUser>, AdditionalUserClaimsPrincipalFactory>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("TwoFactorEnabled",
        x => x.RequireClaim("amr", "mfa")
    );
});
builder.Services.AddProblemDetails();
builder.Services.AddLogging();
#endregion
// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

IdentityModelEventSource.ShowPII = true;
JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();
var config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").AddUserSecrets<Program>().Build();
var DoYouwantsTorunMigration = config.GetSection("AppSettings:DoYouwantsTorunMigration");
//dotnet tool install --global dotnet-ef
//dotnet ef
//dotnet ef migrations add InitialMigration


if (config.GetSection("AppSettings:DoYouwantsTorunDBScripts").Value != null
    && !config.GetSection("AppSettings:DoYouwantsTorunDBScripts").Value.IsNullOrEmpty()
    && (config.GetSection("AppSettings:DoYouwantsTorunDBScripts").Value.ToLower() == "true"
        || config.GetSection("AppSettings:DoYouwantsTorunDBScripts").Value.ToLower() == "1"))
{
    try
    {
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            if (Directory.Exists(config.GetSection("AppSettings:ScriptsPath").Value))
            {
                foreach (var sqlFile in Directory.GetFiles(config.GetSection("AppSettings:ScriptsPath").Value, "*.sql"))
                {
                    var scriptContent = File.ReadAllText(sqlFile);
                    db.Database.ExecuteSqlRawAsync(scriptContent).Wait();
                }
            }
        }
    }
    catch (Exception ex)
    {
        logger.Error(ex);
    }
}

if (DoYouwantsTorunMigration.Value != null
    && !DoYouwantsTorunMigration.Value.IsNullOrEmpty()
    && (DoYouwantsTorunMigration.Value.ToLower() == "true"
        || DoYouwantsTorunMigration.Value.ToLower() == "1"))
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        if (db.Database.GetPendingMigrations().Any())
        {
            try
            {
                db.Database.Migrate();
                //Creating default Users and Roles..
                //var Initializer = new DbInitializer(scope.ServiceProvider.GetRequiredService<ILogger<DbInitializer>>(), scope.ServiceProvider.GetRequiredService<IRole>(),
                //    scope.ServiceProvider.GetRequiredService<IUsers>(), scope.ServiceProvider.GetRequiredService<IUsersInRoles>(), scope.ServiceProvider.GetRequiredService<IPeriodMaster>());
                //Initializer.CreateRoles().Wait();
                //Initializer.CreateDefaultUsers().Wait();
                 
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
