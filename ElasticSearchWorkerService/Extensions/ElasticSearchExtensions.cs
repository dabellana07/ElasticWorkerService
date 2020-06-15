using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace ElasticSearchWorkerService.Extensions
{
    public static class ElasticSearchExtensions
    {
        public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var uri = new Uri(configuration["ElasticSearchConfig:Url"]);
            var username = configuration["ElasticSearchConfig:Username"];
            var password = configuration["ElasticSearchConfig:Password"];

            var settings = new ConnectionSettings(uri);
            settings.BasicAuthentication(username, password);

            var client = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(client);
        }
    }
}
