﻿using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using InteriorCoffee.Application.Services.Implements;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Implements;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using InteriorCoffee.Application.Helpers;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Schema;
using InteriorCoffeeAPIs.Validate;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Hangfire.Mongo.Migration.Strategies;
using System.Text.Json;
using Interior.Infrastructure.Repositories.Interfaces;
using Interior.Infrastructure.Repositories.Implements;

namespace InteriorCoffeeAPIs.Extensions
{
    public static class DependencyServices
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IMongoClient, MongoClient>(sp =>
            {
                var connectionString = config.GetSection("MongoDbSection:ConnectionURI").Value;
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new ArgumentNullException(nameof(connectionString), "MongoDB connection string cannot be null or empty.");
                }
                return new MongoClient(connectionString);
            });

            services.AddScoped(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                var databaseName = config.GetSection("MongoDbSection:DatabaseName").Value;
                if (string.IsNullOrEmpty(databaseName))
                {
                    throw new ArgumentNullException(nameof(databaseName), "MongoDB database name cannot be null or empty.");
                }
                return client.GetDatabase(databaseName);
            });

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
        {
            #region Other
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IPaymentService, PaymentService>();
            services.AddSingleton(x => new PaypalClient(
                config["PaypalOptions:AppId"],
                config["PaypalOptions:AppSecret"],
                config["PaypalOptions:Mode"]
            ));

            services.AddSingleton<FirebaseService>();
            #endregion

            #region Service Scope
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IChatSessionService, ChatSessionService>();
            services.AddScoped<IDesignService, DesignService>();
            services.AddScoped<IFloorService, FloorService>();
            services.AddScoped<IMerchantService, MerchantService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductCategoryService, ProductCategoryService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<ISaleCampaignService, SaleCampaignService>();
            services.AddScoped<IStyleService, StyleService>();
            services.AddScoped<ITemplateService, TemplateService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IVoucherService, VoucherService>();
            #endregion

            #region Repository Scope
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IChatSessionRepository, ChatSessionRepository>();
            services.AddScoped<IDesignRepository, DesignRepository>();
            services.AddScoped<IFloorRepository, FloorRepository>();
            services.AddScoped<IMerchantRepository, MerchantRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<ISaleCampaignRepository, SaleCampaignRepository>();
            services.AddScoped<IStyleRepository, StyleRepository>();
            services.AddScoped<ITemplateRepository, TemplateRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();
            #endregion

            return services;
        }

        public static IServiceCollection AddJwtValidation(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
                };
            });
            return services;
        }

        public static IServiceCollection AddConfigSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo() { Title = "InteriorCoffee", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new string[] { }
                }
            });
                options.MapType<TimeOnly>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "time",
                    Example = OpenApiAnyFactory.CreateFromJson("\"13:45:42.0000000\"")
                });
                options.EnableAnnotations();

                options.MapType<JsonElement>(() => new OpenApiSchema { Type = "object" });
            });
            return services;
        }

        public static IServiceCollection AddJsonSchemaValidation(this IServiceCollection services, string schemaDirectoryPath)
        {
            var schemaFiles = Directory.GetFiles(schemaDirectoryPath, "*.json");
            var validationServices = new Dictionary<string, JsonValidationService>();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                var logger = serviceProvider.GetRequiredService<ILogger<JsonValidationService>>();

                foreach (var schemaFile in schemaFiles)
                {
                    try
                    {
                        var validationService = new JsonValidationService(schemaFile, logger);
                        var schemaName = Path.GetFileNameWithoutExtension(schemaFile);
                        validationServices[schemaName] = validationService;
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"Failed to load schema from file: {schemaFile}");
                    }
                }
            }

            services.AddSingleton<IDictionary<string, JsonValidationService>>(validationServices);

            return services;
        }

        public static IServiceCollection AddHangfire(this IServiceCollection services, IConfiguration config)
        {
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseMongoStorage(config.GetSection("MongoDbSection:ConnectionURI").Value, config.GetSection("MongoDbSection:DatabaseName").Value)
            );

            return services;
        }

        public static IServiceCollection AddHangfireServices(this IServiceCollection services, IConfiguration config)
        {
            var mongoConnectionString = config.GetSection("MongoDbSection:ConnectionURI").Value;
            var mongoDatabaseName = config.GetSection("MongoDbSection:DatabaseName").Value;
            services.AddHangfire(config => config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer().UseRecommendedSerializerSettings()
            .UseMongoStorage(mongoConnectionString, mongoDatabaseName, new MongoStorageOptions
            {
                MigrationOptions = new MongoMigrationOptions
                {
                    MigrationStrategy = new MigrateMongoMigrationStrategy(),
                    BackupStrategy = new CollectionMongoBackupStrategy()
                }
            }));
            services.AddHangfireServer();
            return services;
        }
    }
}
