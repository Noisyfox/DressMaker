using System.Net;
using System.Net.Sockets;

namespace DressMaker.core.pipeline
{
    public class SocketTuple
    {
        public SocketType Type { get; }

        public IPAddress SourceAddress { get; }

        public IPAddress DestinationAddress { get; }

        public ushort SourcePort { get; }

        public ushort DestinationPort { get; }

        public SocketTuple(SocketType type, IPAddress srcAddress, ushort srcPort, IPAddress dstAddress, ushort dstPort)
        {
            Type = type;
            SourceAddress = srcAddress;
            SourcePort = srcPort;
            DestinationAddress = dstAddress;
            DestinationPort = dstPort;
        }

        public override string ToString()
        {
            return $"{{{Type}, {SourceAddress.IPAddressToString()}:{SourcePort}->{DestinationAddress.IPAddressToString()}:{DestinationPort}}}";
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }

            var o = obj as SocketTuple;

            if (o == null)
            {
                return false;
            }

            return
                Type == o.Type
                && SourceAddress.Equals(o.SourceAddress)
                && DestinationAddress.Equals(o.DestinationAddress)
                && SourcePort == o.SourcePort
                && DestinationPort == o.DestinationPort;
        }

        public override int GetHashCode()
        {
            var hash = Type.GetHashCode();
            hash ^= SourceAddress?.GetHashCode() ?? 0;
            hash ^= DestinationAddress?.GetHashCode() ?? 0;
            hash ^= SourcePort;
            hash ^= DestinationPort << 16;

            return hash;
        }
    }
}