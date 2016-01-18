using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Tests
{
    [TestClass]
    public class VectorTest
    {
        [TestMethod]
        public void TestPush()
        {
            var vk = new Vector<int>(1);
            vk.Push(1);
            vk.Push(2);
            vk.Push(3);
            vk.Push(4);
            Assert.AreEqual(1, vk.Data[0]);
            Assert.AreEqual(4, vk.Count);
            Assert.AreEqual(4, vk.Capacity);
        }

        [TestMethod]
        public void TestPop()
        {
            var vk = new Vector<int>(4);
            vk.Push(1);
            vk.Push(2);
            vk.Push(3);
            vk.Push(4);
            Assert.AreEqual(4, vk.Pop());
            Assert.AreEqual(3, vk.Count);
        }

        [TestMethod]
        public void TestEnsureCapacity()
        {
            var vk = new Vector<int>();
            vk.EnsureCapacity(50);
            Assert.AreEqual(50, vk.Capacity);
            vk.Count = 100;
            Assert.AreEqual(100, vk.Capacity);
        }
        [TestMethod]
        public void TestCapacity()
        {
            var vk = new Vector<int>();
            vk.EnsureCapacity(50);
            for (int i = 0; i <= 50; i++)
            {
                vk.Push(i);
            }
            vk.Capacity = 25;
            Assert.AreEqual(25, vk.Capacity);
        }

        [TestMethod]
        public void TestGetSetItem()
        {
            var vk = new Vector<int>(3);
            vk[0] = 1;
            vk[1] = 2;
            vk[2] = 3;
            Assert.AreEqual(1, vk[0]);
            Assert.AreEqual(2, vk[1]);
            Assert.AreEqual(3, vk[2]);
        }
    }
}

