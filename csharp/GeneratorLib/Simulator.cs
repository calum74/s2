using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace GeneratorLib
{
    public class DummyChannel : IChannel, IOpenChannel
    {
        public bool Relay { get; set; }
        public double Frequency { get; set; }
        public double Amplitude { get; set; }
        public double VoltageOffset { get; set; }
        public double DutyCycle { get; set; }
        public Waveform Waveform { get; set; }

        public double MinFrequency => throw new NotImplementedException();

        public double MaxFrequency => throw new NotImplementedException();

        public double MaxAmplitude => throw new NotImplementedException();

        public bool Available => throw new NotImplementedException();

        public string Name => "Channel";

        public void Close()
        {
        }

        public void Dispose()
        {
        }

        public IOpenChannel TryOpen() => this;

        public override string ToString() => "Test";
    }

    public class DummyGenerator : ISignalGenerator
    {
        public string Name => "Test";

        public string Port => "None";

        IChannel[] channels = new DummyChannel[3];

        public IEnumerable<IChannel> Channels => channels;

        public void Dispose()
        {
        }

        public bool GetRelay(int channel)
        {
            return false;
        }

        public void LoadCustomWaveform(int channel, int[] values)
        {
        }

        public void SetFrequency(int channel, double value)
        {
        }

        public void SetRelay(int channel, bool value)
        {
        }
    }

    public class DummyPulse : IHeartRateMonitor
    {
        public string Name => "Test";

        public void Dispose()
        {
        }

        public TimeSpan Read()
        {
            Thread.Sleep(1000);
            return new TimeSpan(0, 0, 1);
        }
    }
}
