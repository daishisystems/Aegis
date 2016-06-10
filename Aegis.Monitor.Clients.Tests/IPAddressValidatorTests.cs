using System.Collections.Concurrent;
using Aegis.Monitor.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aegis.Monitor.Clients.Tests
{
    [TestClass]
    public class IPAddressValidatorTests
    {
        [TestMethod]
        public void IPAddressIsBlackListed()
        {
            const string ipAddress = "10.0.0.1";

            var blackList =
                new ConcurrentDictionary<string, BlackListItem>();

            blackList.TryAdd(ipAddress, new BlackListItem());

            BlackListItem blackListItem;
            var ipAddressIsValidated = IPAddressValidator.IPAddressIsBlacklisted(ipAddress,
                blackList, out blackListItem);

            Assert.IsTrue(ipAddressIsValidated);
        }
    }
}