using Application.Commands.Book;
using Application.Services.Book;
using Domain.Infrastructure.Context;
using Domain.Infrastructure.Repositories.Book;
using MediatR;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using System.Data.Common;
using System.Reflection;
using Serilog.Sinks.File;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory()
});


Log.Logger = new LoggerConfiguration()
       .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 5)
       .CreateLogger();

builder.Services.AddSerilog();

builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue; // Tamaño límite de los datos enviados en megabytes
    options.MultipartBodyLengthLimit = long.MaxValue; // Tamaño límite de todo el cuerpo de la solicitud en bytes
    options.MemoryBufferThreshold = int.MaxValue;
});

//var loggerFactory = new LoggerFactory(new[] { new SerilogProvider(Log.Logger) });

//var serviceProvider = new ServiceCollection()
//    .AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true))
//    .AddDbContext<MyDbContext>(options => options.UseNpgsql("Host=myhost;Username=myuser;Password=mypassword;Database=mydatabase"))
//    .BuildServiceProvider();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});




// Add services to the container.



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHttpContextAccessor();
//builder.Services.AddTransient<ApiKeyMiddleware>();

builder.Services
    .AddCustomMvc()
    .AddCustomDbContext(builder.Configuration)
    .AddCustomSwagger(builder.Configuration)
    .AddCustomIC(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();
//app.UseMiddleware<ApiKeyMiddleware>();

app.UseCors("AllowAllOrigins");
app.MapControllers();

try
{
    Log.Information("Applying migrations ({ApplicationContext})...", Program.AppName);
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<BookMarketContext>();
    var env = app.Services.GetService<IWebHostEnvironment>();
    await context.Database.MigrateAsync();


    Log.Information("Starting web host ({ApplicationContext})...", Program.AppName);
    await app.RunAsync();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", Program.AppName);
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program
{

    public static string Namespace = typeof(Program).Assembly.GetName().Name;
    public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
}

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _apiKey;

    public ApiKeyMiddleware(RequestDelegate next, string apiKey)
    {
        _next = next;
        _apiKey = apiKey;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("X-Api-Key", out var apiKeyValues))
        {
            var providedApiKey = apiKeyValues.FirstOrDefault();
            if (providedApiKey == _apiKey)
            {
                await _next(context);
                return;
            }
        }

        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Unauthorized");
    }
}

static class CustomExtensionsMethods
{



    public static IServiceCollection AddCustomMvc(this IServiceCollection services)
    {
        // Add framework services.
        services.AddControllers();
  

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .WithExposedHeaders("X-Api-Key"); 
                });
        });



        return services;
    }



    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BookMarketContext>(options => {
            options.UseNpgsql(configuration["Data:DefaultConnection:ConnectionString"],
                npgsqlOptionsAction: sqlOptions => {
                    sqlOptions.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name);
                });
        },
                   ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
               );



        return services;
    }

    public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "CustomSoft - BookMarket - José Angel Loredo Hernandez",
                Version = "v1",
                Description = "HTTP API."
            });
            //options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme {
            //  Type = SecuritySchemeType.OAuth2,
            //  Flows = new OpenApiOAuthFlows() {
            //    Implicit = new OpenApiOAuthFlow() {
            //      AuthorizationUrl = new Uri($"{configuration.GetValue<string>("IdentityUrlExternal")}/connect/authorize"),
            //      TokenUrl = new Uri($"{configuration.GetValue<string>("IdentityUrlExternal")}/connect/token"),
            //      Scopes = new Dictionary<string, string>()
            //            {
            //                      { "eFarms", "eFarms API" }
            //                  }
            //    }
            //  }
            //});
            //options.OperationFilter<AuthorizeCheckOperationFilter>();
        });

    

        return services;
    }


    public static IServiceCollection AddCustomIC(this IServiceCollection services, IConfiguration configuration)
    {

      

        // Agrega la cadena de conexión al contenedor de inyección de dependencias
        services.AddSingleton(configuration["Data:DefaultConnection:ConnectionString"]);


        ///// Repositories
        /////
       services.AddScoped<IBookRepository, BookRepository>();

        /////Servces
        /////

        services.AddScoped<IBookServices, BookServices>();


        return services;
    }


}