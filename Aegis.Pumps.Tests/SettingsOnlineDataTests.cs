using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aegis.Pumps.Tests
{
    [TestClass]
    public class SettingsOnlineDataTests
    {
        [TestMethod]
        public void DeserializeEmpty()
        {
            var data = SettingsOnlineData.Deserialize(string.Empty);
            Assert.IsNull(data.AegisEventsDisabled);
        }

        [TestMethod]
        public void Serialization()
        {
            var data = new SettingsOnlineData();
            data.Blacklist = new SettingsOnlineData.BlackListData();
            data.Blacklist.CountriesBlock = new HashSet<string>() { "a", "b" };
            data.Blacklist.CountriesSimulate = new HashSet<string>() { "aaa", "bbb" };
            data.Experiments = new SettingsOnlineData.ExperimentData[]
                                   {
                                       new SettingsOnlineData.ExperimentData
                                           {
                                               ExperimentId = 1,
                                               DateStart = DateTime.Parse("2015-05-01"),
                                               DateEnd = DateTime.Parse("2015-06-30"),
                                               IpMask = "AQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAIA="
                                           }, 
                                   };

            var dataStr = data.Serialize();
            var dataNew = SettingsOnlineData.Deserialize(dataStr);

            CollectionAssert.AreEqual(data.Blacklist.CountriesBlock.ToArray(), dataNew.Blacklist.CountriesBlock.ToArray());
            CollectionAssert.AreEqual(data.Blacklist.CountriesSimulate.ToArray(), dataNew.Blacklist.CountriesSimulate.ToArray());
        }
    }
}
