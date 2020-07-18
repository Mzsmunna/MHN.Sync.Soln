using System;
using System.Collections.Generic;

namespace MHN.Sync.Telemetry
{
    public static class TelemetryLogger
    {
        public static void LogException(Exception ex)
        {
            var telemetryBase = TelemetryFactory.GetInstance(TelemetryType.Exception);
            telemetryBase.Handle(ex);
        }

        public static void LogCustomEvent(string eventName, TimeSpan elapsed, Dictionary<string, object> properties)
        {
            var telemetryBase = TelemetryFactory.GetInstance(TelemetryType.CustomEvent);
            telemetryBase.Handle(eventName, elapsed, properties);
        }
    }
}
