using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressMaker.core.pipeline.tcp
{
    public class TcpSeq
    {
        public SocketTuple SocketIdent { get; }

        public TcpSeq(SocketTuple socketIdent)
        {
            SocketIdent = socketIdent;
        }
    }
}
