using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PacketDotNet;

namespace DressMaker.core.pipeline.tcp
{
    public class TcpCollector : IPacketCollector
    {
        private readonly Dictionary<TcpTuple, TcpSeq> _activeTcpSeqs = new Dictionary<TcpTuple, TcpSeq>();

        public Type RequiredPacketType { get; } = typeof(TcpPacket);

        public bool OnPacketArrival(TimePacket packet, ISet<object> changedSet)
        {
            var tcpPkt = (TcpPacket) packet.Packet;
            var ipPkt = tcpPkt.ParentPacket as IpPacket;
            if (ipPkt == null) // Only TCP/IP is supported
            {
                return false;
            }

            // Build SocketTuple
            var socketTuple = new TcpTuple(ipPkt.SourceAddress, tcpPkt.SourcePort, ipPkt.DestinationAddress,
                tcpPkt.DestinationPort);
            Debug.WriteLine(socketTuple);

            TcpSeq seq;
            if (!_activeTcpSeqs.TryGetValue(socketTuple, out seq))
            {
                seq = new TcpSeq(socketTuple);
                _activeTcpSeqs[socketTuple] = seq;
            }

            // TODO: Update TcpSeq

            changedSet.Add(seq);

            return false;
        }
    }

    public class TcpTuple : SocketTuple
    {
        public IPAddress AddressA { get; }

        public ushort PortA { get; }

        public IPAddress AddressB { get; }

        public ushort PortB { get; }

        public TcpTuple(IPAddress srcAddress, ushort srcPort, IPAddress dstAddress, ushort dstPort)
            : base(SocketType.Stream, srcAddress, srcPort, dstAddress, dstPort)
        {
            var cmp = srcAddress.CompareTo(dstAddress);
            if (cmp == 0)
            {
                cmp = srcPort.CompareTo(dstPort);
            }

            if (cmp > 0)
            {
                AddressA = dstAddress;
                PortA = dstPort;
                AddressB = srcAddress;
                PortB = srcPort;
            }
            else
            {
                AddressA = srcAddress;
                PortA = srcPort;
                AddressB = dstAddress;
                PortB = dstPort;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }

            var o = obj as TcpTuple;

            if (o == null)
            {
                return false;
            }

            return
                Type == o.Type
                && AddressA.Equals(o.AddressA)
                && AddressB.Equals(o.AddressB)
                && PortA == o.PortA
                && PortB == o.PortB;
        }

        public override int GetHashCode()
        {
            var hash = Type.GetHashCode();
            hash ^= AddressA?.GetHashCode() ?? 0;
            hash ^= AddressB?.GetHashCode() ?? 0;
            hash ^= PortA;
            hash ^= PortB << 16;

            return hash;
        }
    }
}