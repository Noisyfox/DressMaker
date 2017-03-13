using System;
using System.Collections.Generic;
using PacketDotNet;

namespace DressMaker.core.pipeline
{
    internal interface IPacketCollector
    {
        Type RequiredPacketType { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="changedSet">for output</param>
        /// <returns>true if <paramref name="packet"/> is consumed, false otherwise</returns>
        bool OnPacketArrival(TimePacket packet, ISet<object> changedSet);
    }
}