using System;
using System.Collections.Generic;

namespace Ap.IISLogger
{
    public interface ILogService
    {
        List<IisLogCount> GetLogs(string fileName = null, string api = null);
        List<IisLogCount> GetLogsWithDates(string api = null, DateTime? from = null, DateTime? to = null);
        List<IisLog> GetLogDetails(string uri, int? status, string fileName = null);
        List<CountStats> GetHttpStatusStats();
        void Import2Sql(string fileName);
    }
}

