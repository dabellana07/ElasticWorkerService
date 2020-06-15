using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElasticSearchWorkerService.Models;

namespace ElasticSearchWorkerService.Contracts
{
    public interface ILogDumperService
    {
        Task AddLog(LogEvent logEvent);
        void BulkAdd(List<LogEvent> logEvents);
    }
}
