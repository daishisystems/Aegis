using System.Net;
using System.Net.Sockets;

namespace Aegis.Monitor.Core
{
    /// <summary>
    ///     <see cref="IPAddressRange" /> consists of 2 <see cref="IPAddress" />
    ///     instances, from which an IP4/6 range is implied.
    /// </summary>
    public class IPAddressRange
    {
        private readonly AddressFamily _addressFamily;
        private readonly byte[] _lowerBytes;
        private readonly byte[] _upperBytes;

        /// <summary>
        ///     <see cref="IPAddressRange" /> constructor.
        /// </summary>
        /// <param name="lower">The beginning of the <see cref="IPAddress" /> range.</param>
        /// <param name="upper">The end of the <see cref="IPAddress" /> range.</param>
        public IPAddressRange(IPAddress lower, IPAddress upper)
        {
            _addressFamily = lower.AddressFamily;
            _lowerBytes = lower.GetAddressBytes();
            _upperBytes = upper.GetAddressBytes();
        }

        /// <summary>
        ///     <see cref="IsInRange" /> determines whether <see cref="ipAddress" /> falls
        ///     within the IP-range encapsulated by this instance.
        /// </summary>
        /// <param name="ipAddress">The <see cref="IPAddress" /> to validate.</param>
        /// <returns>
        ///     <c>True</c>, if <see cref="ipAddress" /> falls within the IP-range
        ///     encapsulated by this instance.
        /// </returns>
        public bool IsInRange(IPAddress ipAddress)
        {
            if (ipAddress.AddressFamily != _addressFamily)
            {
                return false;
            }

            var addressBytes = ipAddress.GetAddressBytes();

            bool lowerBoundary = true, upperBoundary = true;

            for (var i = 0;
                i < _lowerBytes.Length &&
                (lowerBoundary || upperBoundary);
                i++)
            {
                if ((lowerBoundary && addressBytes[i] < _lowerBytes[i]) ||
                    (upperBoundary && addressBytes[i] > _upperBytes[i]))
                {
                    return false;
                }

                lowerBoundary &= addressBytes[i] == _lowerBytes[i];
                upperBoundary &= addressBytes[i] == _upperBytes[i];
            }

            return true;
        }
    }
}