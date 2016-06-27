using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aegis.Core.Tests
{
    [TestClass]
    public class BlackListManagerTests
    {
        [TestMethod]
        public void BlackListsAreSegmentedByCountry()
        {
            var ipAddress = "195.27.14.131";

            Func<List<BlackListItem>> getBlackListItems =
                () => new List<BlackListItem>
                {
                    new BlackListItem
                    {
                        IPAddress = IPAddress.Parse(ipAddress)
                    }
                };

            var blackListsByCountry =
                BlackListManager.SegmentBlackListByCountry(getBlackListItems, string.Empty, null,
                    new Dictionary<string, IPAddressGeoLocation>
                    {
                        {
                            ipAddress, new IPAddressGeoLocation
                            {
                                CountryName = "Ireland"
                            }
                        }
                    });

            List<BlackListItem> irishBlackListItems;
            var irishListExists =
                blackListsByCountry.TryGetValue("ireland",
                    out irishBlackListItems);

            Assert.IsTrue(irishListExists);
            Assert.AreEqual(1, irishBlackListItems.Count);
        }
    }
}