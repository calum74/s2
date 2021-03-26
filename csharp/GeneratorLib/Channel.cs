using System;

namespace GeneratorLib
{
    public interface IOpenChannel : IDisposable
    {
        void Close();

        /// <summary>
        /// Enables the output relay.
        /// true = output enabled.
        /// </summary>
        bool Relay { get; set; }

        /// <summary>
        /// Sets the frequency of the output, in Hz.
        /// </summary>
        double Frequency { get; set; }

        /// <summary>
        /// Sets the amplitude of the output, in V.
        /// This is probably the peak amplitude.
        /// (half of peak-peak amplitude, not RMS amplitude).
        /// </summary>
        double Amplitude { get; set; }

        /// <summary>
        /// Sets the voltage offset of the channel.
        /// </summary>
        double VoltageOffset { get; set; }

        /// <summary>
        /// Sets the duty cycle. Applies to triangle and square waves.
        /// </summary>
        double DutyCycle { get; set; }

        /// <summary>
        /// Sets the waveform.
        /// </summary>
        Waveform Waveform { get; set; }
    }

    /// <summary>
    /// A single channel of a frequency generator.
    /// 
    /// Values out of range throw OutOfRangeException.
    /// Values not supported throw NotSupportedException.
    /// </summary>
    public interface IChannel
    {
        /// <summary>
        /// Display name of this channel.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The lowest programmable frequency.
        /// </summary>
        double MinFrequency { get; }

        /// <summary>
        /// The highest programmable frequency.
        /// </summary>
        double MaxFrequency { get; }

        /// <summary>
        /// The largest programmable amplitude.
        /// The unit is probably Volts, but it depends on the signal generator.
        /// </summary>
        double MaxAmplitude { get; }

        /// <summary>
        /// Whether this channel is currently available for use.
        /// The device could in theory be unplugged, disabled or in use.
        /// </summary>
        bool Available { get; }

        IOpenChannel TryOpen();
    }

    /// <summary>
    /// A wrapper for a channel that stores the previously set values.
    /// </summary>
    public class CachedChannel : IOpenChannel
    {
        public CachedChannel(IOpenChannel channel)
        {
            UnderlyingChannel = channel;
        }

        public void Close() => UnderlyingChannel.Close();

        bool relay;
        public bool Relay { get => relay; set => UnderlyingChannel.Relay = relay = value; }

        double frequency;
        public double Frequency { get => frequency; set => UnderlyingChannel.Frequency = frequency = value;  }

        double amplitude;
        public double Amplitude { get => amplitude; set => UnderlyingChannel.Amplitude = amplitude = value; }

        double offset;
        public double VoltageOffset { get => offset; set => UnderlyingChannel.VoltageOffset = offset = value; }

        double dutyCycle;

        public double DutyCycle { get => dutyCycle; set => UnderlyingChannel.DutyCycle = dutyCycle = value; }

        Waveform waveform;
        public Waveform Waveform { get => waveform; set => UnderlyingChannel.Waveform = waveform = value; }

        public IOpenChannel UnderlyingChannel { get; private set; }

        public void Dispose()
        {
            UnderlyingChannel.Dispose();
        }

        public override string ToString() => UnderlyingChannel.ToString();
    }
}
