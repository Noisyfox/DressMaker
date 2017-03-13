using PacketDotNet;
using SharpPcap;

namespace DressMaker.core
{
    public class TimePacket
    {
        public Packet Packet { get; set; }

        public PosixTimeval Timestamp { get; set; }
    }
}
