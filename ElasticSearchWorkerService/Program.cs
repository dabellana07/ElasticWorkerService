using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElasticSearchWorkerService.Contracts;
using ElasticSearchWorkerService.ElasticSearch.Services;
using ElasticSearchWorkerService.Extensions;
using ElasticSearchWorkerService.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ElasticSearchWorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) => {
                    services.AddHostedService<Worker>();
                    services.AddElasticSearch(hostContext.Configuration);
                    services.AddSingleton<IExternalLogReader, JsonLogFileReader>();
                    services.AddSingleton<ILogDumperService, LogElasticService>();
                });
    }
}