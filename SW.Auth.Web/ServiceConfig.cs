using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SW.CqApi;
using SW.CqApi.AuthOptions;
using SW.HttpExtensions;
using SW.PrimitiveTypes;

namespace SW.Auth.Web;

public static class ServiceConfig
{
    public static void ConfigureApp(this IServiceCollection services, IConfiguration configuration,
        string environmentName)
    {
        var instanceSettings = new InstanceSettings();
        configuration.Bind(nameof(InstanceSettings), instanceSettings);
        services.AddSingleton(instanceSettings);
        services.AddDbContext<SwDbContext>(c =>
        {
            c.UseSnakeCaseNamingConvention();
            c.UseNpgsql(configuration.GetConnectionString(SwDbContext.ConnectionString), b =>
            {
                b.MigrationsHistoryTable("_ef_migrations_history", SwDbContext.Schema);
                b.MigrationsAssembly(typeof(Program).Assembly.FullName);
                b.UseAdminDatabase("defaultdb");
            });
        });

        services.AddControllers(b => { })
            .AddJsonOptions(configure =>
            {
                configure.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        services.AddCqApi(options =>
        {
            options.UrlPrefix = "api";
            options.ProtectAll = false;
            options.AuthOptions = new CqApiAuthOptions
            {
                AuthType = AuthType.OAuth2
            };
        });

        services.AddAuthentication()
            .AddJwtBearer(configureOptions =>
            {
                configureOptions.RequireHttpsMetadata = false;
                configureOptions.SaveToken = true;
                configureOptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = configuration["Token:Issuer"],
                    ValidAudience = configuration["Token:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"]))
                };
            });


        services.AddJwtTokenParameters();
        services.AddScoped<RequestContext>();
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                builder =>
                {
                    builder.SetIsOriginAllowedToAllowWildcardSubdomains();
                    builder.AllowCredentials();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.WithOrigins(
                        "http://localhost:3000",
                        "https://localhost:7252",
                        "http://localhost:5192",
                        "https://*.sf9.io");
                });
        });
    }
}