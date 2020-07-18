using System;
using System.Collections.Generic;
using MHN.Sync.Entity;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace MHN.Sync.Telemetry
{
    public abstract class TelemetryWrapper
    {
        public TelemetryWrapper()
        {
            TelemetryConfiguration.Active.InstrumentationKey = ApplicationConstants.InstrumentationKey;
        }

        public abstract void Handle(Object ex);

        public virtual void Handle(string eventName, TimeSpan elapsed, Dictionary<string, object> properties)
        {
            var telemetry = new TelemetryClient();
            var eventTelemetry = new EventTelemetry(eventName);
            eventTelemetry.Metrics.Add("Elapsed", elapsed.TotalMilliseconds);
            if(properties.Count > 0)
            {
                foreach(var property in properties)
                {
                    eventTelemetry.Properties.Add(property.Key, Convert.ToString(property.Value));
                }
            }

            telemetry.TrackEvent(eventTelemetry);
        }
    }
}
