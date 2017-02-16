using Aegis.Pumps.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aegis.Pumps.Tests
{
    [TestClass]
    public class ActionsUtilsTests
    {
        [TestMethod]
        public void GetTopDomain()
        {
            Assert.IsNull(ActionsUtils.GetTopDomain(null));
            Assert.IsNull(ActionsUtils.GetTopDomain(""));
            Assert.AreEqual("bla.com", ActionsUtils.GetTopDomain("bla.com"));
            Assert.AreEqual("bla.com", ActionsUtils.GetTopDomain("www.bla.com"));
            Assert.AreEqual("bla.com", ActionsUtils.GetTopDomain("WWW.BLA.COM"));
            Assert.AreEqual("bla.com", ActionsUtils.GetTopDomain("http://www.bla.com"));
            Assert.AreEqual("bla.com", ActionsUtils.GetTopDomain("https://www.bla.com"));
            Assert.AreEqual("abc-bla.org", ActionsUtils.GetTopDomain("www.abc-bla.org"));
            Assert.AreEqual("abc-bla.pl", ActionsUtils.GetTopDomain("www.abc-bla.pl"));
            Assert.AreEqual("abc-bla.pl", ActionsUtils.GetTopDomain("abc-bla.pl"));
            Assert.AreEqual("bbc.co.uk", ActionsUtils.GetTopDomain("bbc.co.uk"));
            Assert.AreEqual("bbc.co.uk", ActionsUtils.GetTopDomain("bla.bbc.co.uk"));
            Assert.AreEqual("bbc.co.uk", ActionsUtils.GetTopDomain("ccc.bla.bbc.co.uk"));
            Assert.AreEqual("bbc.co.uk", ActionsUtils.GetTopDomain("ddd.ccc.bla.bbc.co.uk"));
            Assert.AreEqual("boom.com", ActionsUtils.GetTopDomain("bla.boom.com"));
            Assert.AreEqual("boom.pl", ActionsUtils.GetTopDomain("bla.boom.pl"));
            Assert.AreEqual("bla.org.uk", ActionsUtils.GetTopDomain("zzz.bla.org.uk"));
        }
    }
}
