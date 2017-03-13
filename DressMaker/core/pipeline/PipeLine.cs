using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DressMaker.core.input;
using DressMaker.core.pipeline.tcp;
using PacketDotNet;
using SharpPcap;

namespace DressMaker.core.pipeline
{
    public class PipeLine
    {
        public IPacketInput PacketInput { get; set; }

        private readonly List<IPacketCollector> _collectors = new List<IPacketCollector>();

        public void Start()
        {
            _collectors.Add(new TcpCollector());

            RunPipeLine();
        }

        public void Stop()
        {
        }

        private void RunPipeLine()
        {
            RawCapture capture;
            var changedSet = new HashSet<object>();

            while ((capture = PacketInput.NextPacket()) != null)
            {
                var pkt = Packet.ParsePacket(capture.LinkLayerType, capture.Data);
                foreach (var collector in _collectors)
                {
                    var requiredPkt = pkt.Extract(collector.RequiredPacketType);
                    if (requiredPkt != null)
                    {
                        changedSet.Clear();

                        var result =
                            collector.OnPacketArrival(
                                new TimePacket {Packet = requiredPkt, Timestamp = capture.Timeval}, changedSet);

                        if (result)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}