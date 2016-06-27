using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aegis.Core.Tests
{
    [TestClass]
    public class WhiteListItemTests
    {
        [TestMethod]
        public void WhiteListItemIsAnIPAddressRange()
        {
            var whiteListItem = new WhiteListItem
            {
                LowerIPAddress = IPAddress.Parse("195.27.14.130"),
                UpperIPAddress = IPAddress.Parse("195.27.14.131")
            };

            Assert.IsTrue(whiteListItem.IsRange);
        }

        [TestMethod]
        public void WhiteListItemIsNotAnIPAddressRange()
        {
            var whiteListItem = new WhiteListItem
            {
                LowerIPAddress = IPAddress.Parse("195.27.14.131")
            };

            Assert.IsFalse(whiteListItem.IsRange);

            whiteListItem = new WhiteListItem
            {
                UpperIPAddress = IPAddress.Parse("195.27.14.131")
            };

            Assert.IsFalse(whiteListItem.IsRange);
        }
    }
}