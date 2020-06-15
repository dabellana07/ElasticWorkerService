using System.Collections.Generic;
using ElasticSearchWorkerService.Models;

namespace ElasticSearchWorkerService.Contracts
{
    public interface IExternalLogReader
    {
        List<LogEvent> ReadLogs();
    }
}
