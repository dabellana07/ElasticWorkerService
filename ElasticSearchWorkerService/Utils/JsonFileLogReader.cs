﻿using System;
using System.Collections.Generic;
using System.IO;
using ElasticSearchWorkerService.Contracts;
using ElasticSearchWorkerService.Models;
using Newtonsoft.Json;

namespace ElasticSearchWorkerService.Utils
{
    public class JsonLogFileReader : IExternalLogReader
    {
        public JsonLogFileReader()
        {
        }

        public List<LogEvent> ReadLogs()
        {
            string filePath = String.Concat(Environment.CurrentDirectory, "/Data/log.json");
            using (var streamReader = File.OpenText(filePath))
            using (var jsonTextReader = new JsonTextReader(streamReader))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<List<LogEvent>>(jsonTextReader);
            }
        }
    }
}
