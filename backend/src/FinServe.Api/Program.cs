using Admin.Infrastructure.Module;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Auth.Infrastructure.Module;
using FinServe.Api.Configurations;
using FinServe.Api.ConfigureOptions;
using FinServe.Api.Extensions;
using FinServe.Api.Services;
using Location.Infrastructure.Module;
using Lookup.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Notification.Infrastructure.Module;
using Serilog;
using Serilog.Debugging;
using Shared.Application.Results;
using Shared.Common;
using Shared.Common.Helpers;
using Shared.Common.Utils;
using Shared.Common.Validators;
using Shared.Infrastructure.Extensions;
using Shared.Logging;
using Shared.Security;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using System.Runtime;
using System.Text.Json.Serialization;
using Users.Infrastructure.Module;
using static FinServe.Api.Extensions.SwaggerExtensions;
using ILogger = Serilog.ILogger;

namespace FinServe.Api;

internal sealed class Program
{
    private static string _appName = "API";
    private const string _stopCommand = "STOP";
    private static ILogger? _logger;
    public static async Task Main(string[] args)
    {
#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            var mainThreadName = "Main Thread";
            Thread.CurrentThread.Name = mainThreadName;

            var builder = WebApplication.CreateBuilder(args);

            var processPath = Path.GetDirectoryName(Environment.ProcessPath) ?? throw new InvalidOperationException("Unable to determine process path");

            Directory.SetCurrentDirectory(processPath);

            //Adding Json Files
            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

            //Adding Configuration Options into DI
            builder.Services.AddOptions<AppConfig>().BindConfiguration(AppConfig.SectionName).ValidateOnStart();
            

            var appConfig = builder.Configuration.GetSection(AppConfig.SectionName).Get<AppConfig>() ?? throw new InvalidOperationException("AppConfig section is not defined.");

            GCSettings.LatencyMode = appConfig.GCLatencyMode;
            _appName = $"{appConfig.Branding.AppName} API";

            SelfLog.Enable(msg => Console.Error.WriteLine($"Serilog SelfLog: {msg}"));

            builder.Host.UseSerilog((ctx, lc) => lc
                .ReadFrom.Configuration(ctx.Configuration));

            builder.WebHost.CaptureStartupErrors(true);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // -----------------------------------------------
            // API Versioning + API Explorer
            // -----------------------------------------------   
            builder.Services
                .AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ReportApiVersions = true;

                    // URL segment + header + query string support
                    options.ApiVersionReader = ApiVersionReader.Combine(
                        new UrlSegmentApiVersionReader(),
                        new HeaderApiVersionReader("x-api-version"),
                        new QueryStringApiVersionReader("api-version")
                    );
                })
                .AddApiExplorer(options =>
                {
                    // e.g. v1, v1.1, v2
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

            // -----------------------------------------------       
            // Register Controllers & Swagger
            // -----------------------------------------------
            builder.Services.AddControllers
                (options => options.Filters.Add<ModelValidationFilter>())
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    //o.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
                    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {                        var errors = context.ModelState
                        .Where(kvp => kvp.Value != null && kvp.Value.Errors.Any())
                        .SelectMany(kvp => kvp.Value is null ? [] : kvp.Value.Errors
                        .Select(e =>
                        new ValidationError(kvp.Key, e.ErrorMessage)))
                        .ToList();

                        var result = Result.Validation(errors);

                        return new ObjectResult(result) { StatusCode = (int)HttpStatusCode.BadRequest, };
                    };
                })                
                .AddApplicationPart(typeof(Auth.Api.AssemblyReference).Assembly)
                .AddApplicationPart(typeof(Location.Api.AssemblyReference).Assembly)
                .AddApplicationPart(typeof(Users.Api.AssemblyReference).Assembly)
                .AddApplicationPart(typeof(Lookup.Api.AssemblyReference).Assembly)
                .AddApplicationPart(typeof(Admin.Api.AssemblyReference).Assembly)
                .AddApplicationPart(typeof(Notification.Api.AssemblyReference).Assembly)
                ;
            ;

            // -------------------------------------------------------------
            // Add Swagger + JWT Support
            // -------------------------------------------------------------
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddStandardApiResponses();
                c.OperationFilter<ApiResponseOperationFilter>();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
            });
            // Per-version swagger docs
            builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            // -----------------------------------------------
            // Register Module Services
            // -----------------------------------------------
            builder.Services
                .AddSharedInfrastructure(AppConfig.SectionName)
                .AddSharedCommonModule()
                .AddSharedLoggingModule()
                .AddSharedSecurityModule(AppConfig.SectionName)
                .AddLocationModule(builder.Configuration)
                .AddUserModule(builder.Configuration)
                .AddAuthModule(AppConfig.SectionName, builder.Configuration)
                .AddAdminModule()
                .AddLookupApplication()
                .AddNotificationModule(AppConfig.SectionName, builder.Configuration)
                ;

            builder.Services.AddMemoryCache();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSingleton<IHostLifetime, CustomConsoleLiftime>();

            // -------------------------------------------------------------
            // Authentication + Authorization
            // -------------------------------------------------------------
            builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

            builder.Services.AddAuthorization();

            // Read CORS config from appsettings.json
            var corsSettings = builder.Configuration.GetSection("Cors");
            var allowedOrigins = corsSettings.GetSection("AllowedOrigins").Get<string[]>();
            var allowCredentials = corsSettings.GetValue<bool>("AllowCredentials");

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AppCorsPolicy", policy =>
                {
                    policy.SetIsOriginAllowed(origin =>
                        allowedOrigins.Contains(origin)
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod();

                    if (allowCredentials)
                        policy.AllowCredentials();
                });
            });

            // -------------------------------------------------------------
            // Build app
            // -------------------------------------------------------------
            var app = builder.Build();

            //Register middlewares
            app.UseMiddlewares();

            //Get Required Services
            T GetService<T>() where T : notnull
            {
                return app.Services.GetRequiredService<T>();
            }

            _logger = GetService<ILogger>().ForContext<Program>();

            _logger?.Information("Application Starting. {_appName} at {Now}.", _appName, DateTimeUtil.Now);

            // -------------------------------------------------------------
            // Apply Migrations Automatically (Optional)
            // -------------------------------------------------------------
            // AUTO-MIGRATION FOR ALL MODULE DB CONTEXTS
            app.AddLocationMigrations();
            app.AddUserMigrations();
            app.AddAuthMigrations();
            app.AddAdminMigrations();
            app.AddNotificationMigrations();

            //if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                // -----------------------------------------------
                // Configure Swagger for API versions
                // -----------------------------------------------
                var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                app.UseSwaggerUI(options =>
                {
                    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"/swagger/{description.GroupName}/swagger.json",
                            $"{appConfig.Branding.AppName} API {description.GroupName.ToUpperInvariant()}"
                        );
                    }
                });
            }

            // -----------------------------------------------
            // Middleware
            // -----------------------------------------------
            app.UseMiddleware<RequestLoggingMiddleware>();

            // -------------------------------------------------------------
            // Logging (as early as possible)
            // -------------------------------------------------------------
            app.UseSerilogRequestLogging();

            // -------------------------------------------------------------
            // HTTPS should be before routing & auth
            // -------------------------------------------------------------
            app.UseHttpsRedirection();

            // -------------------------------------------------------------
            // Routing
            // -------------------------------------------------------------
            app.UseRouting();

            // -------------------------------------------------------------
            // CORS must be BETWEEN UseRouting and Auth
            // -------------------------------------------------------------
            app.UseCors("AppCorsPolicy");

            // -------------------------------------------------------------
            // Security
            // -------------------------------------------------------------
            app.UseAuthentication();
            app.UseAuthorization();

            // -------------------------------------------------------------
            // Endpoints
            // -------------------------------------------------------------
            app.MapGet("/", () => Results.Redirect("/swagger"));
            app.MapControllers();

            app.Lifetime.ApplicationStarted.Register(() => OnApplicationStarted(app));
            app.Lifetime.ApplicationStopping.Register(OnApplicationStopping);
            app.Lifetime.ApplicationStopped.Register(OnApplicationStopped);

            await app.RunAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger?.Fatal(ex, "Application start-up failed");
            Console.WriteLine(ex.Message);
            Console.Read();
        }
        finally
        {
            OnApplicationStopping();
            OnApplicationStopped();
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var exception = e.ExceptionObject as Exception;

        if (exception is not null)
        {
            _logger?.Fatal(exception, "Unhandled exception");
            Console.WriteLine(exception.Message);
        }
        else
        {
            Console.WriteLine($"Unhandled exception occurred, but it could not be cast to Exception type. {e}");
        }
        Log.CloseAndFlush();
    }

    private static void OnApplicationStarted(WebApplication app)
    {
        _logger?.Information("Now listening on:{0}", app.Urls);
        _logger?.Information("Hosting environment:{0}", app.Environment.EnvironmentName);
        _logger?.Information("Content root path:{0}", app.Environment.ContentRootPath);
        _logger?.Information("Application started. {_appName} at {Now} Enter {_stopCommand} to shut down.", _appName, DateTimeUtil.Now, _stopCommand);

        //Console.CancelKeyPress += OnCancelKeyPressed;

        var isAppStopped = ConsoleHelper.WaitConsoleForUserCommand(_stopCommand);

        if (isAppStopped)
        {
            _logger?.Information("{0} triggered. Stopping and Exiting application", _stopCommand);
            app.Lifetime.StopApplication();
        }
    }

    private static void OnApplicationStopping()
    {
        _logger?.Information("Application stopping. {_appName} at {Now}.", _appName, DateTimeUtil.Now);
        HandleCleanupBeforeExit();
    }

    private static void OnApplicationStopped()
    {
        _logger?.Information("Application stopped. {_appName} at {Now}.", _appName, DateTimeUtil.Now);
        ExitApplication();
    }

    private static void ExitApplication()
    {
        Environment.Exit(0);
    }

    private static void HandleCleanupBeforeExit()
    {
        Log.CloseAndFlush();
    }
}
