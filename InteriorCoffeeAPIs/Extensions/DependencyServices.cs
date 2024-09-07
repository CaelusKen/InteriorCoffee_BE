using InteriorCoffee.Infrastructure.Repositories.Interfaces;
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

namespace InteriorCoffeeAPIs.Extensions
{
    public static class DependencyServices
    {
        /*        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
                {
                    services.Configure<MongoDBContext>(config.GetSection("MongoDbSection"));
                    return services;
                }

                public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
                {
                    #region Other
                    services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
                    services.AddScoped<IMongoClient>(sp =>
                    {
                        var getContext = sp.GetRequiredService<IOptions<MongoDBContext>>();
                        IMongoClient client = new MongoClient(getContext.Value.ConnectionURI);
                        return client;
                    });
                    #endregion*/

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
            #endregion

            #region Service Scope
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ICampaignProductsService, CampaignProductsService>();
            services.AddScoped<IChatSessionService, ChatSessionService>();
            services.AddScoped<IDesignService, DesignService>();
            services.AddScoped<IMerchantService, MerchantService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductCategoryService, ProductCategoryService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ISaleCampaignService, SaleCampaignService>();
            services.AddScoped<IStyleService, StyleService>();
            services.AddScoped<ITemplateService, TemplateService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IVoucherService, VoucherService>();
            services.AddScoped<IVoucherTypeService, VoucherTypeService>();
            #endregion

            #region Repository Scope
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICampaignProductsRepository, CampaignProductsRepository>();
            services.AddScoped<IChatSessionRepository, ChatSessionRepository>();
            services.AddScoped<IDesignRepository, DesignRepository>();
            services.AddScoped<IMerchantRepository, MerchantRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ISaleCampaignRepository, SaleCampaignRepository>();
            services.AddScoped<IStyleRepository, StyleRepository>();
            services.AddScoped<ITemplateRepository, TemplateRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();
            services.AddScoped<IVoucherTypeRepository, VoucherTypeRepository>();
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
            });
            return services;
        }
    }
}
