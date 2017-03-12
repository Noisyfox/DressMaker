using System.Collections.Generic;
using SharpPcap;
using SharpPcap.LibPcap;

namespace DressMaker.core.input
{
    public class MultiFileInput : IPacketInput
    {
        private readonly Dictionary<string, DeviceWrapper> _devices = new Dictionary<string, DeviceWrapper>();

        public MultiFileInput(IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                var inputDevice = new CaptureFileReaderDevice(file);
                inputDevice.Open();
                var wrapper = new DeviceWrapper(inputDevice);
                _devices[wrapper.Name] = wrapper;
            }
        }
        
        public RawCapture NextPacket()
        {
            PosixTimeval timestamp = null;
            DeviceWrapper wrapper = null;

            List<string> devNeedClose = null;

            foreach (var device in _devices.Values)
            {
                var currentPkt = device.CurrentPacket;
                if (currentPkt == null)
                {
                    if (devNeedClose == null)
                    {
                        devNeedClose = new List<string>();
                    }
                    devNeedClose.Add(device.Name);
                }
                else if (timestamp == null || currentPkt.Timeval < timestamp)
                {
                    timestamp = currentPkt.Timeval;
                    wrapper = device;
                }
            }

            if (devNeedClose != null)
            {
                foreach (var name in devNeedClose)
                {
                    var d = _devices[name];
                    _devices.Remove(name);
                    d.Device.Close();
                }
            }

            if (wrapper == null)
            {
                return null;
            }

            var pkt = wrapper.CurrentPacket;
            wrapper.NextPacket();

            return pkt;
        }

        public void Close()
        {
            foreach (var device in _devices.Values)
            {
                device.Device.Close();
            }

            _devices.Clear();
        }

        private class DeviceWrapper
        {
            public CaptureFileReaderDevice Device { get; }

            public RawCapture CurrentPacket { get; private set; }

            public string Name => Device.Name;

            public DeviceWrapper(CaptureFileReaderDevice device)
            {
                Device = device;
                CurrentPacket = device.GetNextPacket();
            }

            public void NextPacket()
            {
                CurrentPacket = Device.GetNextPacket();
            }
        }
    }
}
