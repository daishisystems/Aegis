using System.Collections.Generic;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aegis.Monitor.Core.Tests
{
    [TestClass]
    public class WhiteListManagerTests
    {
        [TestMethod]
        public void IPAddressIsWhitelisted()
        {
            var ipAddress = IPAddress.Parse("195.27.14.131");

            var whiteListedIPAddresses = new HashSet<string>
            {
                "195.27.14.131",
                "195.27.14.132"
            };

            var ipAddressIsWhiteListed =
                WhiteListManager.IPAddressIsWhiteListed(ipAddress,
                    whiteListedIPAddresses, new List<WhiteListItem>());

            Assert.IsTrue(ipAddressIsWhiteListed);
        }

        [TestMethod]
        public void IPAddressRangeIsWhiteListed()
        {
            var ipAddress = IPAddress.Parse("195.27.14.131");

            var whiteListedIPAddressRanges = new List<WhiteListItem>
            {
                new WhiteListItem
                {
                    LowerIPAddress = IPAddress.Parse("195.27.14.1"),
                    UpperIPAddress = IPAddress.Parse("195.27.14.255")
                }
            };

            var ipAddressIsWhiteListed =
                WhiteListManager.IPAddressIsWhiteListed(ipAddress,
                    new HashSet<string>(), whiteListedIPAddressRanges);

            Assert.IsTrue(ipAddressIsWhiteListed);
        }

        [TestMethod]
        public void IPAddressesAreSegmentedByType()
        {
            var singleIPAddressWhiteListItem = new WhiteListItem
            {
                LowerIPAddress = IPAddress.Parse("195.27.14.131"),
                UpperIPAddress = IPAddress.Parse("195.27.14.131")
            };

            var ipAddressRangeWhiteListItem = new WhiteListItem
            {
                LowerIPAddress = IPAddress.Parse("195.27.14.1"),
                UpperIPAddress = IPAddress.Parse("195.27.14.255")
            };

            HashSet<string> singleIPAddresses;
            List<WhiteListItem> ipAddressRanges;

            WhiteListManager.SegmentIPAddressesByType(out singleIPAddresses,
                out ipAddressRanges,
                () => new List<WhiteListItem>
                {
                    singleIPAddressWhiteListItem,
                    ipAddressRangeWhiteListItem
                });

            Assert.AreEqual(1, singleIPAddresses.Count);
            Assert.AreEqual(1, ipAddressRanges.Count);
        }
    }
}