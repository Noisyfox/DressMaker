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
    }
}
