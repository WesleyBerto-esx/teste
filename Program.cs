using FunctionApp2.Services;
using functions.Data.Repository;
using functions.Data;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using YourNamespace.Services;
using financeiroFunctions.Entidades;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        var configuration = new ConfigurationBuilder()
               .AddEnvironmentVariables()
               .Build();

        string? sqlConnectionString = configuration.GetConnectionString("SQL_ConnectionString");
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.Configure<KestrelServerOptions>(options =>
        {
            options.AllowSynchronousIO = true;
        });

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(sqlConnectionString));

        services.AddTransient<PdfRepository>();
        services.AddTransient<IExtractFromPdf, ExtractFromPdf>();
        //services.AddTransient<PdfService>();
        services.AddTransient<FileService>();
        services.AddTransient<ValidationService>();
        services.AddHttpClient<OpenAiService>();
        services.AddScoped<OpenAiService>();
    })
    .Build();

host.Run();