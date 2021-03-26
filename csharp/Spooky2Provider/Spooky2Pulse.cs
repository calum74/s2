using HidSharp;
using System;
using GeneratorLib;
using System.IO;

namespace Spooky2
{
    sealed class HidPulse : IHeartRateMonitor
    {
        public string Name => "Spooky² Pulse";

        public override string ToString() => Name;

        readonly HidDevice hid;

        DeviceStream stream;

        public HidPulse(HidSharp.HidDevice device)
        {
            hid = device;
        }

        public void Dispose()
        {
            stream?.Dispose();
            stream = null;
        }

        public TimeSpan Read()
        {
            if (stream is null)
            {
                try
                {
                    stream = hid.Open();
                }
                catch(IOException ex)
                {
                    throw new HardwareException(DeviceType.Hrm, ErrorCode.CommunicationError, "HRM is unplugged", "Plug the HRM back in");
                }
            }

            byte[] buffer = new byte[1024];
            try
            {
                int x = stream.Read(buffer, 0, 1024);
            }
            catch(IOException ex)
            {
                stream.Close();
                stream = null;
                throw new HardwareException(DeviceType.Hrm, ErrorCode.CommunicationError, "HRM is unplugged", "Plug the HRM back in");
            }
            catch(TimeoutException)
            {
                stream.Close();
                stream = null;
                throw new HardwareException(DeviceType.Hrm, ErrorCode.Timeout, "Unable to detect heart rate", "Attach the sensor correctly");

            }
            catch
            {
                stream.Close();
                stream = null;
                throw new HardwareException(DeviceType.Hrm, ErrorCode.OtherError, "General error", "Plug the device back in");
            }
            var b0 = buffer[3];
            var b1 = buffer[4];
            var b2 = buffer[5];
            long ticks = b0 << 16 | b1 << 8 | b2;
            ticks *= 40;
            return new TimeSpan(ticks);
        }

        public override int GetHashCode()
        {
            return hid.DevicePath.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is HidPulse p && p.hid.DevicePath == hid.DevicePath;
        }
    }
}
