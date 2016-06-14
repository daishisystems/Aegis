using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aegis.Monitor.Core.Tests
{
    [TestClass]
    public class MemoryCacheTests
    {
        [TestMethod]
        public void SimpleAddAndRemove()
        {
            var self = new MemoryCache<int>(5);
            Assert.AreEqual(false, self.Add(1));
            Assert.AreEqual(false, self.Add(2));

            this.TestListAux(self, 5, new int[] { 1, 2 }, true);

            self.Process(
                5,
                x =>
                    {
                        Assert.Fail("Cache should be empty");
                        return true;
                    });
        }

        [TestMethod]
        public void FewBatches()
        {
            var testData = this.GetTestData(16);

            var self = new MemoryCache<int>(100);
            this.AddItems(self, testData, false);

            this.TestListAux(self, 5, testData.Take(5), true);
            this.TestListAux(self, 5, testData.Skip(5).Take(5), true);
            this.TestListAux(self, 5, testData.Skip(10).Take(5), true);
            this.TestListAux(self, 5, testData.Skip(15).Take(5), true);

            self.Process(
                5,
                x =>
                {
                    Assert.Fail("Cache should be empty");
                    return true;
                });
        }

        [TestMethod]
        public void AddAndProcess()
        {
            var testData = this.GetTestData(40);

            var self = new MemoryCache<int>(100);

            this.AddItems(self, testData.Take(10), false);
            this.TestListAux(self, 10, testData.Take(10), true);

            this.AddItems(self, testData.Skip(10).Take(10), false);
            this.TestListAux(self, 10, testData.Skip(10).Take(10), true);

            this.AddItems(self, testData.Skip(20).Take(10), false);
            this.TestListAux(self, 10, testData.Skip(20).Take(10), true);

            this.AddItems(self, testData.Skip(30).Take(10), false);
            this.TestListAux(self, 10, testData.Skip(30).Take(10), true);

            self.Process(
                10,
                x =>
                {
                    Assert.Fail("Cache should be empty");
                    return true;
                });
        }

        [TestMethod]
        public void CapacityLimit()
        {
            var testData = this.GetTestData(32);

            var self = new MemoryCache<int>(10);

            // adding with space available
            this.AddItems(self, testData.Take(10), false);

            // adding with NO space available
            this.AddItems(self, testData.Skip(10), true);

            this.TestListAux(self, 50, testData.Skip(22), true);

            self.Process(
                5,
                x =>
                    {
                        Assert.Fail("Cache should be empty");
                        return true;
                    });
        }

        [TestMethod]
        public void ProcessError()
        {
            var testData = this.GetTestData(40);

            var self = new MemoryCache<int>(100);

            this.AddItems(self, testData, false);

            this.TestListAux(self, 10, testData.Take(10), true);
            this.TestListAux(self, 10, testData.Skip(10).Take(10), false);
            this.TestListAux(self, 10, testData.Skip(10).Take(10), false);
            this.TestListAux(self, 10, testData.Skip(10).Take(10), true);
            this.TestListAux(self, 10, testData.Skip(20).Take(10), true);
            this.TestListAux(self, 10, testData.Skip(30).Take(10), false);
            this.TestListAux(self, 10, testData.Skip(30).Take(10), true);
        }

        private List<int> GetTestData(int count)
        {
            var testData = new List<int>();
            for (var index = 0; index < count; index++)
            {
                testData.Add(index);
            }

            return testData;
        }

        private void AddItems<T>(MemoryCache<T> container, IEnumerable<T> items, bool expectedResult)
        {
            foreach (var item in items)
            {
                Assert.AreEqual(expectedResult, container.Add(item));
            }
        }

        private void TestListAux<T>(MemoryCache<T> self, int batchCount, IEnumerable<T> expected, bool result)
        {
            self.Process(
                batchCount,
                x =>
                {
                    CollectionAssert.AreEqual(expected.ToList(), x);
                    return result;
                });
        }
    }
}
