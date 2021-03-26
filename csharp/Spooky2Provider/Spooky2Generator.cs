using HidSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using GeneratorLib;
using System.IO;

namespace Spooky2
{
    class Spooky2Generator : ISignalGenerator
    {
        SerialDevice device;
        SerialStream stream;
        IChannel combined;
        IChannel channel1, channel2;

        public Spooky2Generator(SerialDevice sd)
        {
            device = sd;
            combined = new CombinedChannel(this);
            channel1 = new SingleChannel(0, this);
            channel2 = new SingleChannel(1, this);
        }

        bool[] open = new bool[2];

        void CheckChannel(int channel)
        {
            if (channel < 0 || channel > 1) throw new ArgumentException("Invalid channel");
        }

        public bool Open(int channel)
        {
            CheckChannel(channel);
            if(Available(channel))
            {
                if (stream is null)
                {
                    try
                    {
                        stream = device.Open();
                    }
                    catch(IOException)
                    {
                        throw new HardwareException(DeviceType.Generator, ErrorCode.CommunicationError, "Generator unavailable", "Connect the generator");
                    }
                    stream.BaudRate = 57600;
                    stream.ReadTimeout = 500;
                    stream.WriteTimeout = 500;
                }
                open[channel] = true;
                return true;
            }
            else
                return false;
        }

        public void Close(int channel)
        {
            CheckChannel(channel);

            if (Available(channel)) throw new Exception("Closing a closed channel");
            open[channel] = false;

            if (!open[0] && !open[1])
                CloseAll();
        }

        void CloseAll()
        {
            stream.Close();
            stream = null;
        }

        public bool Available(int channel)
        {
            CheckChannel(channel);

            return !open[channel];
        }

        public IEnumerable<IChannel> Channels
        {
            get
            {
                yield return combined;
                yield return channel1;
                yield return channel2;
            }
        }

        public void Dispose()
        {
            CloseAll();
        }

        void Send(string cmd)
        {
            try
            {
                stream.WriteLine(cmd);
                var response = stream.ReadLine();
                if (response != "ok")
                    throw new HardwareException(DeviceType.Generator, ErrorCode.CommunicationError, "Communication error", "");
            }
            catch(IOException)
            {
                throw new HardwareException(DeviceType.Generator, ErrorCode.CommunicationError, "Communication error", "Connect the generator");
            }
            catch (TimeoutException)
            {
                throw new HardwareException(DeviceType.Generator, ErrorCode.CommunicationError, "Communication error", "Switch the generator on");
            }
        }


        public override int GetHashCode() => device.DevicePath.GetHashCode();

        public override bool Equals(object obj)
        {
            return obj is Spooky2Generator gen && device.DevicePath.Equals(gen.device.DevicePath);
        }

        public string Name => device.GetFileSystemName(); // GetFriendlyName();
        public string Port => device.DevicePath;

        public void LoadCustomWaveform(int channel, int[] values)
        {
            throw new NotImplementedException();
        }

        bool[] relay = new bool[2];

        enum Command
        {
            set_waveform_1 = 21,
            set_waveform_2 = 22,
            set_frequency_1 = 23,   // e.g. :w2336456000 = 364.450000 Hz
            set_frequency_2 = 24,   // e.g. :w2336456000 = 364.450000 Hz
            set_voltage_1 = 25,  // e.g. :w251005  Set channel 1 to 10.05 V
            set_voltage_2 = 26,  // e.g. :w251005  Set channel 1 to 10.05 V
            set_offset_1 = 27,
            set_offset_2 = 28,
            set_duty_1 = 29,    // e.g. :w29500 = 50%
            set_duty_2 = 30,
            set_phase_1 = 31,
            set_phase_2 = 32,   // e.g. :w32180 = 180degrees
            set_relay_1 = 61,       // e.g. :w621	Set output 2 on
            set_relay_2 = 62,
            set_freq_scale_1 = 63,
            set_freq_scale_2 = 64,
            set_sync = 68   // Channel 2 copies freq from Channel 1 e.g. :w681 set sync on
        };


        private void Send(Command command, int channel, int value)
        {
            CheckChannel(channel);

            Send($":w{(int)command + channel}{value}");
        }

        public void SetRelay(int channel, bool value)
        {
            CheckChannel(channel);

            Send(Command.set_relay_1, channel, value ? 1 : 0);
        }

        enum FrequencyScale { High, Low };

        FrequencyScale[] frequencyScaling = new FrequencyScale[2];

        private void FrequencyScaling(int channel, FrequencyScale fs)
        {
            Send(Command.set_freq_scale_1, channel, fs == FrequencyScale.High ? 0 : 1);
            frequencyScaling[channel] = fs;
        }

        public void SetFrequency(int channel, double value)
        {
            CheckChannel(channel);

            if (value < 600.0 && frequencyScaling[channel] != FrequencyScale.Low)
            {
                FrequencyScaling(channel, FrequencyScale.Low);
            }
            else if (frequencyScaling[channel] != FrequencyScale.High)
            {
                FrequencyScaling(channel, FrequencyScale.High);
            }
            Send(Command.set_frequency_1, channel, frequencyScaling[channel] == FrequencyScale.High ? (int)(value * 100) : (int)(value * 100000));
        }

        public void SetAmplitude(int channel, double value)
        {
            CheckChannel(channel);

            Send(Command.set_voltage_1, channel, (int)(value * 100));
        }

        public void SetSync(bool value)
        {
            Send(Command.set_sync, 0, value ? 1 : 0);
        }

        public void SetWaveform(int channel, Waveform value)
        {
            CheckChannel(channel);
 
            // !! Set 
            int wf = 0;
            Send(Command.set_waveform_1, channel, wf);
        }
    }


    class HardwareList : IDisposable
    {
        DeviceList list;
        public delegate void GeneratorChangedDel(ISignalGenerator g);
        public delegate void HrmChangedDel(IHeartRateMonitor g);

        public event GeneratorChangedDel GeneratorAdded;
        public event GeneratorChangedDel GeneratorRemoved;

        public event HrmChangedDel MonitorAdded;
        public event HrmChangedDel MonitorRemoved;

        public HardwareList()
        {
            list = DeviceList.Local;
            generators = InternalGenerators.ToArray();
            monitors = InternalHeartRateMonitors.ToArray();
            list.Changed += DeviceChanged;
        }

        public void Dispose()
        {
            list.Changed -= DeviceChanged;
        }

        void DeviceChanged(object o, DeviceListChangedEventArgs changed)
        {
            // Sadly we don't get any info on which device changed.
            // So we need to manually check
            var newGenerators = InternalGenerators.ToArray();

            foreach (var added in newGenerators.Except(generators))
                GeneratorAdded(added);
            foreach (var removed in generators.Except(newGenerators))
                GeneratorRemoved(removed);

            generators = generators.Intersect(newGenerators).Union(newGenerators).ToArray();

            var newPulses = InternalHeartRateMonitors.ToArray();
            foreach (var added in newPulses.Except(monitors))
                MonitorAdded(added);
            foreach (var removed in monitors.Except(newPulses))
                MonitorRemoved(removed);

            monitors = monitors.Intersect(newPulses).Union(newPulses).ToArray();

        }

        ISignalGenerator[] generators;
        IHeartRateMonitor[] monitors;

        private IEnumerable<ISignalGenerator> InternalGenerators => list.GetSerialDevices().Select(sd => new Spooky2Generator(sd));

        public IEnumerable<IHeartRateMonitor> InternalHeartRateMonitors =>
            list.GetHidDevices(1602, 7).Select(p => new HidPulse(p));


        public IEnumerable<ISignalGenerator> Generators => generators;

        public IEnumerable<IHeartRateMonitor> HeartRateMonitors => monitors;
    }

    class OpenSingleChannel : IOpenChannel
    {
        public OpenSingleChannel(int channel, Spooky2Generator gen)
        {
            Generator = gen;
            Channel = channel;
        }

        Spooky2Generator Generator { get; }
        int Channel { get; }


        public void Close()
        {
            try
            {
                Relay = false;
            }
            finally
            {
                Generator.Close(Channel);
                disposed = true;
            }
        }

        bool disposed = false;
        public void Dispose()
        {
            if(!disposed)
            {
                Close();
                disposed = true;
            }
        }

        public bool Relay { get => throw new NotImplementedException(); set => Generator.SetRelay(Channel, value); }
        public double Frequency { get => throw new NotImplementedException(); set => Generator.SetFrequency(Channel, value); }
        public double Amplitude { get => throw new NotImplementedException(); set => Generator.SetAmplitude(Channel, value); }
        public double VoltageOffset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double DutyCycle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Waveform Waveform { get => throw new NotImplementedException(); set => Generator.SetWaveform(Channel, value); }
    }

    class SingleChannel : IChannel
    {
        Spooky2Generator Generator { get; }
        int Channel { get; }

        public string Name => Channel == 0 ? "Output 1" : "Output 2";

        public override string ToString() => $"{Generator.Name} Spooky²-XM channel {Channel + 1}";

        public SingleChannel(int channel, Spooky2Generator gen)
        {
            Generator = gen;
            Channel = channel;
        }

        public IOpenChannel TryOpen()
        {
            if(Generator.Open(Channel))
            {
                try
                {
                    Generator.SetSync(false);
                    return new CachedChannel(new OpenSingleChannel(Channel, Generator));
                }
                catch
                {
                    Generator.Close(Channel);
                }
            }
            return null;
        }

        public double MinFrequency => throw new NotImplementedException();

        public double MaxFrequency => throw new NotImplementedException();

        public double MaxAmplitude => throw new NotImplementedException();

        public bool Available => throw new NotImplementedException();

    }

    class OpenCombinedChannel : IOpenChannel
    {
        Spooky2Generator Generator;

        public OpenCombinedChannel(Spooky2Generator gen)
        {
            Generator = gen;
        }

        bool disposed = false;

        public void Close()
        {
            try
            {
                Generator.SetRelay(0, false);
                Generator.SetRelay(1, false);
            }
            finally
            {
                Generator.Close(0);
                Generator.Close(1);
                disposed = true;
            }
        }

        public void Dispose()
        {
            if(!disposed)
            {
                Close();
            }
        }

        public bool Relay
        {
            get => throw new NotImplementedException();
            set
            {
                Generator.SetRelay(0, value);
                Generator.SetRelay(1, value);
            }
        }
        public double Frequency
        {
            get => throw new NotImplementedException();
            set => Generator.SetFrequency(0, value);
        }

        public double Amplitude
        {
            get => throw new NotImplementedException();
            set => Generator.SetAmplitude(0, value);
        }

        public double VoltageOffset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double DutyCycle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Waveform Waveform { get => throw new NotImplementedException(); set => Generator.SetWaveform(0, value); }

    }

    class CombinedChannel : IChannel
    {
        Spooky2Generator Generator;

        public CombinedChannel(Spooky2Generator gen)
        {
            Generator = gen;
        }

        public IOpenChannel TryOpen()
        {
            if (!Generator.Available(0) || !Generator.Available(1))
                return null;

            bool close0 = false;
            bool close1 = false;

            try
            {
                if (Generator.Open(0))
                {
                    close0 = true;
                    Generator.SetRelay(0, true);
                }

                if (Generator.Open(1))
                {
                    close1 = true;
                    Generator.SetRelay(1, true);
                }

                Generator.SetSync(true);

                return new OpenCombinedChannel(Generator);
            }
            catch
            {
                if (close0) Generator.Close(0);
                if (close1) Generator.Close(1);
                throw;
            }
        }


        public string Name => "Combined";

        public override string ToString() => $"{Generator.Name} Spooky²-XM channel 1+2";

        public bool Available => Generator.Available(0) && Generator.Available(1);

        public double MinFrequency => throw new NotImplementedException();

        public double MaxFrequency => throw new NotImplementedException();

        public double MaxAmplitude => throw new NotImplementedException();

    }
}
