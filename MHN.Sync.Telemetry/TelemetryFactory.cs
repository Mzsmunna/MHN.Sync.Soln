
namespace MHN.Sync.Telemetry
{
    public enum TelemetryType
    {
        Exception,
        Trace,
        CustomEvent
    }

    public static class TelemetryFactory
    {
        public static TelemetryWrapper GetInstance(TelemetryType type)
        {
            switch (type.ToString())
            {
                case "Exception":
                    return new TelemetryException();
                case "Trace":
                    return new TelemetryDependency();
                case "CustomEvent":
                    return new TelemetryCustomEvent();
                default:
                    return new TelemetryException();
            }
        }
    }
}
