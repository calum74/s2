using System;
using System.Collections.Generic;
using System.Text;

namespace GeneratorLib
{
    public delegate void HardwareChangedDel();

    public interface IProvider
    {
        IProgramFolder Custom { get; }

        IProgramFolder RootPresetCollection { get; }

        IEnumerable<IHeartRateMonitor> HeartRateMonitors { get; }

        IEnumerable<ISignalGenerator> Generators { get; }

        IReverseLookup ReverseLookup { get; }

        event HardwareChangedDel OnHardwareChanged;
    }
}
