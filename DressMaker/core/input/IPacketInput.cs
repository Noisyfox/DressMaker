using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPcap;

namespace DressMaker.core.input
{
    public interface IPacketInput
    {
        RawCapture NextPacket();

        void Close();
    }
}
