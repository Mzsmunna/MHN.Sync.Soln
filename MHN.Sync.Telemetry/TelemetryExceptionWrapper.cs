using MHN.Sync.Entity.Enum;
using MHN.Sync.Entity.MongoEntity;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MHN.Sync.Telemetry
{
    public class TelemetryExceptionWrapper : Exception
    {
        private Log log;
        private LogType logtype = LogType.ERROR;

        public TelemetryExceptionWrapper(Exception ex)
        {
            var message = new StringBuilder();
            log = new Log
            {
                ExceptionMessage = ex.Message
            };
            var tmpStuckTrace = new StackTrace(ex, true);
            for(var i = tmpStuckTrace.FrameCount - 1; i >= 0; i--)
            {
                var Frame = tmpStuckTrace.GetFrame(i);
                message.Append(Path.GetFileName(Frame.GetFileName()) + "  --->  " + Frame.GetFileLineNumber() + Environment.NewLine);
            }
            var tmpFrame = tmpStuckTrace.GetFrame(0);
            log.LineNumber = tmpFrame.GetFileLineNumber();
            log.SourceMethodName = tmpFrame.GetMethod().Name;
            log.SourceClassName = Path.GetFileName(tmpFrame.GetFileName());
            log.ProcessName = ex.Data["ProcessName"] != null ? ex.Data["ProcessName"].ToString() : "";
            log.ProcessFileName = ex.Data["ProcessFileName"] != null ? ex.Data["ProcessFileName"].ToString() : "";
            log.ProcessFileLineNumber = ex.Data["ProcessFileLineNumber"] != null ? Convert.ToInt32(ex.Data["ProcessFileLineNumber"]) : 0;
            log.ExceptionToLog = ex;
            log.StackTrace = message.ToString();
        }

        public IEntity Handle()
        {
            if (log.ExceptionToLog is FileNotFoundException ||
                log.ExceptionToLog is DivideByZeroException ||
                log.ExceptionToLog is ArgumentException ||
                log.ExceptionToLog is IndexOutOfRangeException ||
                log.ExceptionToLog is FormatException
                )
            {
                logtype = LogType.ERROR;
            }
            else if (log.ExceptionToLog is DriveNotFoundException ||
                log.ExceptionToLog is InsufficientMemoryException ||
                log.ExceptionToLog is InsufficientMemoryException ||
                log.ExceptionToLog is AccessViolationException)
            {
                logtype = LogType.FATAL;
            }
            else
            {
                logtype = LogType.INFO;
            }
            return log;
        }
    }
}
