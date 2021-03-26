using System;
using System.Collections.Generic;

namespace GeneratorLib
{
    /// <summary>
    /// A signal generator is a hardware device providing a number
    /// of channels. Each channel can be programmed independently.
    /// However, not all channels may be available at the same time
    /// for example if one of the channels is ann abstraction for a "combined
    /// output" where one of the outputs is an invert and sync.
    /// </summary>
    public interface ISignalGenerator : IDisposable
    {
        /// <summary>
        /// A user-friendly name identifying the device.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The hardware device name of the signal generator,
        /// for example "COM3"
        /// </summary>
        string Port { get; }

        IEnumerable<IChannel> Channels { get;  }

        void LoadCustomWaveform(int channel, int[] values);
    }
}
