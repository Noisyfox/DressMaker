using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PacketDotNet;

namespace DressMaker.core.pipeline.tcp
{
    public class TcpCollector : IPacketCollector
    {
        private readonly Dictionary<SocketTuple, TcpSeq> _activeTcpSeqs = new Dictionary<SocketTuple, TcpSeq>();

        public Type RequiredPacketType { get; } = typeof(TcpPacket);

        public bool OnPacketArrival(TimePacket packet, ISet<object> changedSet)
        {
            var tcpPkt = (TcpPacket)packet.Packet;
            var ipPkt = tcpPkt.ParentPacket as IpPacket;
            if (ipPkt == null) // Only TCP/IP is supported
            {
                return false;
            }

            // Build SocketTuple
            var socketTuple = new SocketTuple(SocketType.Stream, ipPkt.SourceAddress, tcpPkt.SourcePort, ipPkt.DestinationAddress, tcpPkt.DestinationPort);

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
}
