﻿using Multilarr.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multilarr.Common.Interfaces.Logger
{
    public interface ILoggerDatabase
    {
        #region Logs
        
        Task<List<Log>> GetLogsAsync();
        Task<Log> GetLogAsync(int id);
        Task<int> SaveLogAsync(Log item);
        Task<int> DeleteLogAsync(Log item);

        #endregion

        #region Notifications

        Task<List<NotificationLog>> GetNotificationLogsAsync();
        Task<NotificationLog> GetNotificationLogAsync(int id);
        Task<int> SaveLogAsync(NotificationLog item);
        Task<int> DeleteLogAsync(NotificationLog item);

        #endregion
    }
}