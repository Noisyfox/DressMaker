using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressMaker.core.pipeline.tcp
{
    public class TcpSeq
    {
        public TcpTuple SocketIdent { get; }

        public TcpSeq(TcpTuple socketIdent)
        {
            SocketIdent = socketIdent;
        }
    }
}
