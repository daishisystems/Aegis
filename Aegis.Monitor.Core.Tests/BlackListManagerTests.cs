using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aegis.Monitor.Core.Tests
{
    [TestClass]
    public class BlackListManagerTests
    {
        [TestMethod]
        public void BlackListsAreSegmentedByCountry()
        {
            Func<List<BlackListItem>> getBlackListItems =
                () => new List<BlackListItem>
                {
                    new BlackListItem
                    {
                        Country = "Ireland",
                        IPAddress = IPAddress.Parse("195.27.14.131")
                    },
                    new BlackListItem
                    {
                        Country = "Ireland",
                        IPAddress = IPAddress.Parse("195.27.14.131")

                    },
                    new BlackListItem
                    {
                        Country = "United Kingdom",
                        IPAddress = IPAddress.Parse("195.27.14.131")

                    }
                };

            var blackListsByCountry = BlackListManager.SegmentBlackListByCountry(getBlackListItems);

            List<BlackListItem> irishBlackListItems;
            var irishListExists =
                blackListsByCountry.TryGetValue("ireland",
                    out irishBlackListItems);

            Assert.IsTrue(irishListExists);
            Assert.AreEqual(2, irishBlackListItems.Count);

            List<BlackListItem> ukBlackListItems;
            var ukListExists =
                blackListsByCountry.TryGetValue("united kingdom",
                    out ukBlackListItems);

            Assert.IsTrue(ukListExists);
            Assert.AreEqual(1, ukBlackListItems.Count);
        }
    }
}