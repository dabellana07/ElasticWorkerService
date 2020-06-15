using System;
using System.Threading;
using System.Threading.Tasks;
using ElasticSearchWorkerService.Contracts;
using ElasticSearchWorkerService.Utils;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ElasticSearchWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly IExternalLogReader _externalLogReader;
        private readonly ILogDumperService _logDumperService;
        private readonly ILogger<Worker> _logger;

        public Worker(
            IExternalLogReader logReader,
            ILogDumperService logDumperService,
            ILogger<Worker> logger
        )
        {
            _externalLogReader = logReader;
            _logDumperService = logDumperService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                try
                {
                    ReadAndWriteLogs();
                }
                catch (Exception e)
                {
                    _logger.LogError("AN ERROR OCURRED: " + e.ToString());
                }

                await Task.Delay(10000, stoppingToken);
            }
        }

        private void ReadAndWriteLogs()
        {
            var logs = _externalLogReader.ReadLogs();

            _logger.LogInformation("Number of logs: " + logs.Count);

            _logDumperService.BulkAdd(logs);
        }
    }
}