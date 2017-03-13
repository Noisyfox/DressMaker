using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace DressMaker.core
{
    public static class Utils
    {
        public static string IPAddressToString(this IPAddress address)
        {
            if (address.AddressFamily == AddressFamily.InterNetworkV6)
            {
                return $"[{address}]";
            }
            return address.ToString();
        }

        public static int CompareTo(this IPAddress self, IPAddress other)
        {
            if (self.Equals(other))
            {
                return 0;
            }

            if (self.AddressFamily != other.AddressFamily)
            {
                return self.AddressFamily == AddressFamily.InterNetworkV6 ? 1 : -1;
            }

            var byte1 = self.GetAddressBytes();
            var byte2 = self.GetAddressBytes();

            Debug.Assert(byte1.Length == byte2.Length);

            return byte1.Zip(byte2, (b1, b2) => b1.CompareTo(b2)).FirstOrDefault(r => r != 0);
        }
    }
}
