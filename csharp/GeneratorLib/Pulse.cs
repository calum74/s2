using System;

namespace GeneratorLib
{
    public interface IHeartRateMonitor : IDisposable
    {
        TimeSpan Read();

        string Name { get; }
    }

    public static class TimeSpanExtensions
    {
        public static double AsBpm(this TimeSpan ts) => 60.0 / ts.TotalSeconds;
    }

    delegate void PulseUnitAdded(IHeartRateMonitor p);
    delegate void PulseUnitRemoved(IHeartRateMonitor p);

}
