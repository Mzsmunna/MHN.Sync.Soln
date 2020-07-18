using MHN.Sync.Entity.MongoEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.Telemetry
{
    public class Log : IEntity
    {
        public string Id { get; set; }

        public string ExceptionMessage { get; set; }

        public string StackTrace { get; set; }

        public string SourceMethodName { get; set; }

        public string SourceClassName { get; set; }

        public string ProcessName { get; set; }

        public string ProcessFileName { get; set; }

        public int? LineNumber { get; set; }

        public int? ProcessFileLineNumber { get; set; }

        public Exception ExceptionToLog { get; set; }
    }
}
