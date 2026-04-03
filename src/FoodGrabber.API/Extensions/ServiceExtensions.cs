using FoodGrabber.Infrastructure.Data;
using FoodGrabber.Inventory.Extensions;
using FoodGrabber.Identity.Extensions;
using FoodGrabber.Menu.Extensions;
using FoodGrabber.Order.Extensions;
using FoodGrabber.Product.Extensions;
using FoodGrabber.Shared.Abstractions;
using FoodGrabber.Shared.Services;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace FoodGrabber.API.Extensions;

public static class ServiceExtensions
{
    private const string FrontendCorsPolicy = "FrontendClient";

    public static IServiceCollection AddApplicationModules(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FoodGrab;Integrated Security=True;Trust Server Certificate=True";

        services.AddIdentityModule<AppDbContext>(
            configuration,
            options => options.UseSqlServer(connectionString));
        services.AddFrontendCors(configuration);
        services.AddObjectStorage(configuration);
        services.AddOrderModule();
        services.AddMenuModule();
        services.AddProductModule();
        services.AddInventoryModule();
        return services;
    }

    public static IServiceCollection AddFrontendCors(this IServiceCollection services, IConfiguration configuration)
    {
        var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

        services.AddCors(options =>
        {
            options.AddPolicy(FrontendCorsPolicy, policy =>
            {
                policy
                    .WithOrigins((allowedOrigins is { Length: > 0 } ? allowedOrigins : ["http://localhost:3000", "http://localhost:5173"]).ToArray())
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }

    public static WebApplication UseFrontendCors(this WebApplication app)
    {
        app.UseCors(FrontendCorsPolicy);
        return app;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "FoodGrabber API",
                Version = "v1",
                Description = "FoodGrabber API Documentation"
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    public static IServiceCollection AddObjectStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var digitalOceanSection = configuration.GetSection("ObjectStorage:DigitalOcean");
        var endpoint = digitalOceanSection["Endpoint"];
        var accessKey = digitalOceanSection["AccessKey"];
        var secretKey = digitalOceanSection["SecretKey"];
        var bucketName = digitalOceanSection["BucketName"];
        var publicBaseUrl = digitalOceanSection["PublicBaseUrl"];

        var isDigitalOceanConfigured =
            !string.IsNullOrWhiteSpace(endpoint) &&
            !string.IsNullOrWhiteSpace(accessKey) &&
            !string.IsNullOrWhiteSpace(secretKey) &&
            !string.IsNullOrWhiteSpace(bucketName) &&
            !string.IsNullOrWhiteSpace(publicBaseUrl);

        if (isDigitalOceanConfigured)
        {
            services.AddSingleton<IAmazonS3>(_ =>
            {
                var credentials = new BasicAWSCredentials(accessKey, secretKey);
                var s3Config = new AmazonS3Config
                {
                    ServiceURL = endpoint,
                    ForcePathStyle = true,
                    AuthenticationRegion = RegionEndpoint.USEast1.SystemName
                };

                return new AmazonS3Client(credentials, s3Config);
            });

            services.AddSingleton<IObjectStorageService>(_ =>
                new DigitalOceanObjectStorageService(
                    _.GetRequiredService<IAmazonS3>(),
                    bucketName!,
                    publicBaseUrl!));

            return services;
        }

        var fileStorageSection = configuration.GetSection("ObjectStorage:File");
        var rootPath = fileStorageSection["RootPath"];
        var filePublicBaseUrl = fileStorageSection["PublicBaseUrl"];

        rootPath = string.IsNullOrWhiteSpace(rootPath)
            ? Path.Combine(AppContext.BaseDirectory, "uploads")
            : rootPath;

        filePublicBaseUrl = string.IsNullOrWhiteSpace(filePublicBaseUrl)
            ? "/uploads"
            : filePublicBaseUrl;

        services.AddSingleton<IObjectStorageService>(_ =>
            new ObjectStoregeFileSerive(rootPath, filePublicBaseUrl));

        return services;
    }
}
