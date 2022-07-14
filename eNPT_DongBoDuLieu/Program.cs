using eNPT_DongBoDuLieu.Models;
using eNPT_DongBoDuLieu.Models.DataBases.EVNNPT;
using eNPT_DongBoDuLieu.Models.GISSystemInfor;
using eNPT_DongBoDuLieu.Services;
using eNPT_DongBoDuLieu.Services.DataBases;
using eNPT_DongBoDuLieu.Services.Datas;
using eNPT_DongBoDuLieu.Services.Portals;
using CommandLine;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace eNPT_DongBoDuLieu
{
    public class Program
    {
        static async Task<int> Main(string[] args)
        {
            return await Parser.Default.ParseArguments<CommandLineOptions>(args)
                .MapResult(async (opts) =>
                {
                    await CreateHostBuilder(args, opts).Build().RunAsync();
                    return 0;
                },
                errs => Task.FromResult(-1)); // Invalid arguments
        }

        private static IHostBuilder CreateHostBuilder(string[] args, CommandLineOptions opts)
        => Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {
                    Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(hostContext.Configuration).CreateLogger();

                    services.Configure<AppSettings>(hostContext.Configuration.GetSection("AppSettings"));
                    services.Configure<GISSystemInfor>(hostContext.Configuration.GetSection("GISSystemInfor"));

                    services.AddHttpClient("HttpClientFactory")
                    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = ValidateServerCertificattion
                    });

                    services.AddDbContext<ModelContext>
                    (options => options.UseOracle(hostContext.Configuration.GetConnectionString("EVNNPT_Database")), ServiceLifetime.Singleton);

                    services.AddHostedService<Worker>();

                    services.AddSingleton(opts);
                    services.AddSingleton<IDataServices, DataServices>();
                    services.AddSingleton<IPortalServices, PortalServices>();
                    services.AddSingleton<IDataBaseServices, DataBaseServices>();

                });

        private static bool ValidateServerCertificattion(HttpRequestMessage requestMessage, X509Certificate2 certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            Console.WriteLine("- ValidateServerCertificattion:");
            Console.WriteLine($"    + Requested URI: {requestMessage.RequestUri}");
            Console.WriteLine($"    + RequEfective date: {certificate.GetEffectiveDateString()}");
            Console.WriteLine($"    + Exp date: {certificate.Issuer}");
            Console.WriteLine($"    + Subject: {certificate.Subject}");
            return true;
        }

    }
}
