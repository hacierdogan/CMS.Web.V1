using System;

namespace CMS.Web.V1.Models.Entity
{
    public class LogGeneral
    {
        public int Id { get; set; }
        public LogGeneralErrorType LogType { get; set; }
        public string LogTypeStr { get { return LogType.ToString(); } }
        public ClientType Client { get; set; }
        public string ClientStr { get { return Client.ToString();} }
        public string Source { get; set; }
        public DateTime DateTime { get; set; }
        public string Message { get; set; }
        public string Explanation { get; set; }
        public string IpAddress { get; set; }
    }

    public enum LogGeneralErrorType
    {
        Error= 0,
        Information = 1
    }
    public enum ClientType
    {
        Admin=0,
        User=1,
        System=2,
    }
}