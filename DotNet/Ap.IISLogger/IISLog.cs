using System;

namespace Ap.IISLogger
{
    public class IisLog
    {
        public string LogFilename { get; set; }
        public int RowNumber { get; set; }
        public DateTime EntryTime { get; set; }
        public string SiteName { get; set; }
        public string ServerName { get; set; }
        public string ServerIpAddress { get; set; }
        public string Method { get; set; }
        public string UriStem { get; set; }
        public string UriQuery { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string ClientIpAddress { get; set; }
        public string HttpVersion { get; set; }
        public string UserAgent { get; set; }
        public string Cookie { get; set; }
        public string Referrer { get; set; }

        public string Hostname { get; set; }
        public int HttpStatus { get; set; }
        public int HttpSubstatus { get; set; }
        public int Win32Status { get; set; }
        public int BytesFromServerToClient { get; set; }
        public int BytesFromClientToServer { get; set; }
        public string TimeTaken { get; set; }
    }
}