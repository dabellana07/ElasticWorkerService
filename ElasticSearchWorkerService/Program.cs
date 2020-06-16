using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElasticSearchWorkerService.Contracts;
using ElasticSearchWorkerService.ElasticSearch.Services;
using ElasticSearchWorkerService.Extensions;
using ElasticSearchWorkerService.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nest;

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
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddElasticSearch(hostContext.Configuration);
                    services.AddSingleton<IExternalLogReader, JsonLogFileReader>();
                    services.AddSingleton<ILogDumperService, LogElasticService>();
                    AddElasticSearch(services, hostContext.Configuration);
                });

        private static void AddElasticSearch(
            IServiceCollection services,
            IConfiguration configuration)
        {
            var uri = new Uri(configuration["ElasticSearchConfig:Url"]);
            var username = configuration["ElasticSearchConfig:Username"];
            var password = configuration["ElasticSearchConfig:Password"];

            var settings = new ConnectionSettings(uri);
            settings.BasicAuthentication(username, password);
            settings.DisableDirectStreaming();
            settings.OnRequestCompleted(call =>
            {
                Debug.WriteLine("Endpoint Called: " + call.Uri);

                if (call.RequestBodyInBytes != null)
                {
                    Debug.WriteLine("Request Body: " + Encoding.UTF8.GetString(call.RequestBodyInBytes));
                }

                if (call.ResponseBodyInBytes != null)
                {
                    Debug.WriteLine("Response Body: " + Encoding.UTF8.GetString(call.ResponseBodyInBytes));
                }

                if (call.ResponseMimeType != null)
                {
                    Debug.WriteLine("Response Mime: " + call.ResponseMimeType);
                }
            });

            var client = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(client);
        }
    }
}