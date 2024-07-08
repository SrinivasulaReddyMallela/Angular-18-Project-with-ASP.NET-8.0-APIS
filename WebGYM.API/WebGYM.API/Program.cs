using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebGYM.API.Models;
using WebGYM.Concrete;
using WebGYM.Interface;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebGYM.API.Common;
using NLog.Web;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Rewrite;
using WebGYM.API.Mappings;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Migrations;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.

#region Nlog Changes
builder.Logging.ClearProviders(); // Clears default logging providers.
builder.Host.UseNLog(); // Use NLog as the logging provider.
var logger = NLog.LogManager.GetCurrentClassLogger();
#endregion


builder.Services.AddControllers().AddJsonOptions(jsonOptions =>
{
    jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

#region MyRegion

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("DatabaseConnection"),builder =>
    {
        builder.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromMinutes(100), errorNumbersToAdd: null);
    });
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
var appSettingsSection = configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
builder.Services.AddSingleton<IConfiguration>(configuration);
builder.Services.AddTransient<ISchemeMaster, SchemeMasterConcrete>();
builder.Services.AddTransient<IPlanMaster, PlanMasterConcrete>();
builder.Services.AddTransient<IPeriodMaster, PeriodMasterConcrete>();
builder.Services.AddTransient<IRole, RoleConcrete>();
builder.Services.AddTransient<IMemberRegistration, MemberRegistrationConcrete>();
builder.Services.AddTransient<IUsers, UsersConcrete>();
builder.Services.AddTransient<IUsersInRoles, UsersInRolesConcrete>();
builder.Services.AddTransient<IPaymentDetails, PaymentDetailsConcrete>();
builder.Services.AddTransient<IRenewal, RenewalConcrete>();
builder.Services.AddTransient<IReports, ReportsMaster>();
builder.Services.AddTransient<IGenerateRecepit, GenerateRecepitConcrete>();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped<IUrlHelper>(implementationFactory =>
{
    var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
    return new UrlHelper(actionContext);
});
#endregion

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
           //.AllowCredentials()
           .WithExposedHeaders("X-Pagination")
           );
});
builder.Services.AddProblemDetails();
builder.Services.AddLogging();
#region swagger configuration
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Srinivasula Reddy Mallela API", Version = "v1" });
    // Add JWT Bearer token authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.\\n\\n Enter 'Bearer' [space] and then your token in the text input below.\\n\\nExample: \\\"Bearer 12345abcdef\\",
        Name = "Authorization",
        // Type = SecuritySchemeType.Http,
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});
#endregion

#region Swagger 

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,

        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
    #region Commented code
    //options.Events = new JwtBearerEvents()
    //{
    //    OnAuthenticationFailed = c =>
    //    {
    //        c.NoResult();
    //        c.Response.StatusCode = 500;
    //        c.Response.ContentType = "text/plain";
    //        return c.Response.WriteAsync(c.Exception.ToString());
    //    },
    //    OnChallenge = context =>
    //    {
    //        context.HandleResponse();
    //        if (context.Response.StatusCode == null)
    //            context.Response.StatusCode = 401;
    //        context.Response.ContentType = "application/json";
    //        var result = JsonConvert.SerializeObject(new WebGYM.API.Models.Response<string>("You are not Authorized"));
    //        return context.Response.WriteAsync(result);
    //    },
    //    OnForbidden = context =>
    //    {
    //        if (context.Response.StatusCode == null)
    //            context.Response.StatusCode = 403;
    //        context.Response.ContentType = "application/json";
    //        var result = JsonConvert.SerializeObject(new WebGYM.API.Models.Response<string>("You are not authorized to access this resource"));
    //        return context.Response.WriteAsync(result);
    //    },
    //};
    #endregion
});

#endregion

var app = builder.Build();

//logger.Error("Line:57", "Unhandled Exception.");
// Using below code data Migration will run.
var config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").AddUserSecrets<Program>().Build();
var DoYouwantsTorunMigration = config.GetSection("AppSettings:DoYouwantsTorunMigration");

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
                var Initializer = new DbInitializer(scope.ServiceProvider.GetRequiredService<ILogger<DbInitializer>>(), scope.ServiceProvider.GetRequiredService<IRole>(),
                    scope.ServiceProvider.GetRequiredService<IUsers>(), scope.ServiceProvider.GetRequiredService<IUsersInRoles>(), scope.ServiceProvider.GetRequiredService<IPeriodMaster>());
                Initializer.CreateRoles().Wait();
                Initializer.CreateDefaultUsers().Wait();
                Initializer.CreatePeriodTB().Wait();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
        if (config.GetSection("AppSettings:ScriptsPath").ToString() != "")
        {
            try
            {
                foreach (var sqlFile in Directory.GetFiles(/*config.GetSection("AppSettings:ScriptsPath")*/AppDomain.CurrentDomain.BaseDirectory + "/SQLScripts", "*.sql"))
                {
                    var scriptContent = File.ReadAllText(sqlFile);
                    db.Database.ExecuteSqlRawAsync(scriptContent).Wait();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
    }
}
// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
    /*options => // UseSwaggerUI is called only in Development.
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
}*/
    );
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//logger.Error("Line:85", "Unhandled Exception.");
var option = new RewriteOptions();
option.AddRedirect("^$", "swagger");
app.UseRewriter(option);
//logger.Error("Line:90", "Unhandled Exception.");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.Use((context, next) =>
{
    context.Response.Headers.Add("Referrer-Policy", "no-referrer");
    return next();
});
//app.UseMiddleware<EncryptionMiddleware>();
app.MapControllers();
app.UseCors("CorsPolicy");
app.Run();