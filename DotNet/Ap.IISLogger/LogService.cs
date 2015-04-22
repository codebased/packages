using System.Configuration;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using MSUtil;
using Ap.Common.Extensions;

namespace Ap.IISLogger
{

    public class LogService : ILogService
    {
        public List<IisLogCount> GetLogs(string fileName = null, string api = null)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = "{0}\\*.log".FormatMessage(ConfigurationManager.AppSettings["IISLOGPATH"]);
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(fileName);
            }

            string query;

            if (string.IsNullOrWhiteSpace(api))
            {
                query = @"
                SELECT date, cs-uri-stem, cs-method, count(cs-uri-stem) as requestcount from {0}
                WHERE STRLEN (cs-username ) > 0 and {1}
                GROUP BY date, cs-method, cs-uri-stem 
                ORDER BY date, cs-uri-stem, cs-method, count(cs-uri-stem) desc".FormatMessage(fileName, BetweenDates());
            }
            else
            {
                query = @"
            SELECT date, cs-uri-stem, cs-method, count(cs-uri-stem) as requestcount from {0}
                WHERE cs-uri-stem LIKE {1} and STRLEN (cs-username ) > 0 and {2}
                GROUP BY date, cs-method, cs-uri-stem 
                ORDER BY date, cs-uri-stem, cs-method, count(cs-uri-stem) desc".FormatMessage(fileName, " '%/api/{0}%' ".FormatMessage(api), BetweenDates());
            }

            var recordSet = ExecuteQuery(query);
            var records = new List<IisLogCount>();
            for (; !recordSet.atEnd(); recordSet.moveNext())
            {
                var record = recordSet.getRecord().toNativeString(",").Split(new[] { ',' });
                int hit;
                if (int.TryParse(record[3], out hit))
                {
                }
                else
                {
                    hit = 0;
                }

                records.Add(new IisLogCount { Hit = hit, EntryTime = Convert.ToDateTime(record[0]), Uri = record[1], Method = record[2] });
            }

            return records;
        }

        public List<IisLog> GetLogDetails(string uri, int? status, string fileName = null)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = "{0}\\*.log".FormatMessage(ConfigurationManager.AppSettings["IISLOGPATH"]);
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(fileName);
            }

            string query;

            query = @"SELECT"
                    + " TO_TIMESTAMP(date, time) AS EntryTime"
                    + ", s-ip AS ServerIpAddress"
                    + ", cs-method AS Method"
                    + ", cs-uri-stem AS UriStem"
                    + ", cs-uri-query AS UriQuery"
                    + ", s-port AS Port"
                    + ", cs-username AS Username"
                    + ", c-ip AS ClientIpAddress"
                    + ", sc-status AS HttpStatus"
                    + ", sc-substatus AS HttpSubstatus"
                    + ", sc-win32-status AS Win32Status"
                    + ", time-taken AS TimeTaken"
                    +
                    " from {0} WHERE cs-uri-stem like '%{1}%' and {2} and STRLEN (cs-username ) > 0 ".FormatMessage(
                        fileName, uri, BetweenDates());

            if (status.HasValue)
            {
                query += " and sc-status = " + status.Value;
            }

            query += " ORDER BY EntryTime";

            var resultSet = ExecuteQuery(query);

            var records = new List<IisLog>();

            for (; !resultSet.atEnd(); resultSet.moveNext())
            {

                var record = resultSet.getRecord().toNativeString(",").Split(new[] { ',' });
                int httpStatus;
                int.TryParse(record[8], out httpStatus);
                int port;
                int.TryParse(record[5], out port);
                int httpSubStatus;
                int.TryParse(record[9], out httpSubStatus);
                records.Add(new IisLog
                {
                    EntryTime = Convert.ToDateTime(record[0]),
                    Method = record[2],
                    UriStem = record[3],
                    UriQuery = record[4],
                    Port = port,
                    Username = record[6],
                    HttpStatus = httpStatus,
                    HttpSubstatus = httpSubStatus,
                    TimeTaken = record[11]
                });

            }


            return records;

        }

        internal ILogRecordset ExecuteQuery(string query)
        {
            var logQuery = new LogQueryClass();

            var inputContext = new COMW3CInputContextClass();
            return logQuery.Execute(query, inputContext);
        }

        internal bool ExecuteBatch(string query, COMSQLOutputContextClass outputContext)
        {
            var logQuery = new LogQueryClass();
            
            var inputContext = new COMW3CInputContextClass();
            return logQuery.ExecuteBatch(query, inputContext, outputContext);
        }

        public List<CountStats> GetHttpStatusStats()
        {
            var fileName = "{0}\\*.log".FormatMessage(ConfigurationManager.AppSettings["IISLOGPATH"]);
            var betweenDates = BetweenDates();
            var query =
                "SELECT sc-status as Label, COUNT(*) as Value FROM {0} where {1} and  sc-status <> 200 and sc-status <> 304 GROUP BY sc-status".FormatMessage(
                    fileName, betweenDates);
            var recordSet = ExecuteQuery(query);
            var records = new List<CountStats>();

            for (; !recordSet.atEnd(); recordSet.moveNext())
            {
                var record = recordSet.getRecord().toNativeString(",").Split(new[] { ',' });
                records.Add(new CountStats { Label = record[0], Value = int.Parse(record[1]) });
            }

            return records;
        }

        private string BetweenDates()
        {
            return BetweenDates(DateTime.Now.Date.AddMonths(-1), DateTime.Now.Date);
        }

        private string BetweenDates(DateTime from, DateTime to)
        {
            return
                " to_timestamp(date,time) between timestamp('{0}', 'yyyy/MM/dd hh:mm:ss') and timestamp('{1}', 'yyyy/MM/dd hh:mm:ss') "
                    .FormatMessage(from.ToString("yyyy/MM/dd hh:mm:ss"),
                        to.ToString("yyyy/MM/dd hh:mm:ss"));
        }

        public List<IisLogCount> GetLogsWithDates(string api = null, DateTime? from = null, DateTime? to = null)
        {
            var fileName = "{0}\\*.log".FormatMessage(ConfigurationManager.AppSettings["IISLOGPATH"]);
            string query;

            if (string.IsNullOrWhiteSpace(api))
            {
                query = @"
                SELECT date, cs-uri-stem, cs-method, count(cs-uri-stem) as requestcount from {0}
                WHERE STRLEN (cs-username ) > 0 and to_timestamp(date) between timestap
                GROUP BY date, cs-method, cs-uri-stem 
                ORDER BY date, cs-uri-stem, cs-method, count(cs-uri-stem) desc".FormatMessage(fileName);
            }
            else
            {
                query = @"
            SELECT date, cs-uri-stem, cs-method, count(cs-uri-stem) as requestcount from {0}
                WHERE cs-uri-stem LIKE {1} and STRLEN (cs-username ) > 0 
                GROUP BY date, cs-method, cs-uri-stem 
                ORDER BY date, cs-uri-stem, cs-method, count(cs-uri-stem) desc".FormatMessage(fileName, " '%/api/{0}%' ".FormatMessage(api));
            }

            var recordSet = ExecuteQuery(query);
            var records = new List<IisLogCount>();
            int hit;
            for (; !recordSet.atEnd(); recordSet.moveNext())
            {
                var record = recordSet.getRecord().toNativeString(",").Split(new[] { ',' });
                if (int.TryParse(record[3], out hit))
                {

                }
                else
                {
                    hit = 0;
                }

                records.Add(new IisLogCount { Hit = hit, EntryTime = Convert.ToDateTime(record[0]), Uri = record[1], Method = record[2] });
            }

            return records;
        }

        public void Import2Sql(string fileName)
        {
            var connectionString =
                ConfigurationManager.ConnectionStrings.OfType<ConnectionStringSettings>().SingleOrDefault(connection => connection.Name.Equals("CEDSDbContext", StringComparison.OrdinalIgnoreCase));

            if (connectionString == null)
            {
                throw new ConfigurationErrorsException("Connection string is missing.");
            }
            
            var sqlConnection = new SqlConnectionStringBuilder(connectionString.ConnectionString);

            var outputContext = new COMSQLOutputContextClass
            {
                clearTable = false,
                createTable = true,
                transactionRowCount = -1,
                database = sqlConnection.InitialCatalog,
                server = sqlConnection.DataSource,
                username = sqlConnection.UserID,
                password = sqlConnection.Password,
                driver = "SQL Server"
            };

            //var query = @"SELECT * INTO iislogs FROM {0} where LogFilename <> '{1}'".FormatMessage(fileName, fileName.Replace("\\\\","\\"));
            var query = @"SELECT * INTO iislogs FROM {0} ".FormatMessage(fileName);

            ExecuteBatch(query, outputContext);
        }
    }
}