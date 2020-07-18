using System;
using Microsoft.ApplicationInsights;

namespace MHN.Sync.Telemetry
{
    internal class TelemetryDependency : TelemetryWrapper
    {
        public override void Handle(Object ex)
        {
            var telemetry = new TelemetryClient();
            //telemetry.TrackDependency(ex as Exception);
        }
    }
}
