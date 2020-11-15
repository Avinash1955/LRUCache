using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LRUCache;

namespace CachingUnitTest
{
    [TestClass]
    public class LRUTest
    {
        private const int capacity = 5;
        private LruCache<int,string> lruCache = new LruCache<int,string>(capacity);

        [TestMethod]
        public void AddandRetrieveValue()
        {
            lruCache.Add(0, "Test0");
            string value;
            bool result = lruCache.TryGetValue(0, out value);
            Assert.IsTrue(result);
            Assert.AreEqual(value, "Test0");
        }

        [TestMethod]
        public void UpdateValue()
        {
            lruCache.Add(0, "Test0");
            lruCache.Add(1, "Test1");
            lruCache.Add(1, "Test11");
            string value;
            bool result = lruCache.TryGetValue(1, out value);
            Assert.IsTrue(result);
            Assert.AreEqual(value, "Test11");

        }

        [TestMethod]
        public void GetValueKeyNotPresent()
        {
            lruCache.Add(0, "Test0");
            lruCache.Add(1, "Test1");
            string value;
            bool result = lruCache.TryGetValue(2, out value);
            Assert.IsFalse(result);
            Assert.AreEqual(value,null);

        }

        [TestMethod]
        public void MaxCapacityAndRemoveLRUTest()
        {
            for(int i=0;i<capacity;i++)
            {
                lruCache.Add(i,"Item"+i);
            }
            lruCache.Add(capacity, "Test"+ capacity);
            string value;
            bool result = lruCache.TryGetValue(0, out value);
            Assert.IsFalse(result);
            result = lruCache.TryGetValue(capacity, out value);
            Assert.IsTrue(result);
            Assert.AreEqual(value, "Test" + capacity);

        }

        [TestMethod]
        public void RemoveElementTest()
        {
            string value;
            lruCache.Add(20, "Test20");
            bool result = lruCache.TryGetValue(20, out value);
            Assert.IsTrue(result);
            lruCache.Remove(20);
            result = lruCache.TryGetValue(20, out value);
            Assert.IsFalse(result);

        }


    }
}
