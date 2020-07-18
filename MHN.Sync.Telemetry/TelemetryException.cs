using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;

namespace MHN.Sync.Telemetry
{
    internal class TelemetryException : TelemetryWrapper
    {
        public override void Handle(Object ex)
        {
            var telemetry = new TelemetryClient();

            try
            {
                var result = new TelemetryExceptionWrapper(ex as Exception).Handle() as Log;
                var properties = new Dictionary<string, string>();
                typeof(Log).GetProperties().ToList().ForEach(x =>
                {
                    properties.Add(x.Name, Convert.ToString(x.GetValue(result, null)));
                });

                telemetry.TrackException(ex as Exception, properties);
            }
            catch(Exception exx)
            {
                telemetry.TrackException(exx);
            }

            telemetry.Flush();
            Task.Delay(5000).Wait();
        }
    }
}
