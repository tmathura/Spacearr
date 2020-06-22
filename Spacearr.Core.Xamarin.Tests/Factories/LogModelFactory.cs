using Spacearr.Common.Enums;
using Spacearr.Common.Models;
using System;
using System.Collections.Generic;

namespace Spacearr.Core.Xamarin.Tests.Factories
{
    public static class LogModelFactory
    {
        public static List<LogModel> CreateLogModels(int total)
        {
            var logModels = new List<LogModel>();

            for (var i = 0; i < total; i++)
            {
                var logType = Enum.GetValues(typeof(LogType));
                var random = new Random();
                var randomLogType = (LogType)logType.GetValue(random.Next(logType.Length));

                logModels.Add(new LogModel
                {
                    Id = i,
                    LogMessage = $"Log Message {i}",
                    LogStackTrace = $"Log Stack Trace {i}",
                    LogType = randomLogType,
                    LogDate = DateTime.Now.AddDays(i)
                });
            }

            return logModels;
        }
    }
}