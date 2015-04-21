using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sortowanie;
using System.Collections.Generic;

namespace SortowanieTests
{
    [TestClass]
    public class InsertSortTest
    {
        [TestMethod]
        public void TestInts()
        {
            int[] toSort = new int[] { 3, 2, 1 };
            int[] sorted = new int[] { 1, 2, 3 };

            Sort.InsertSort(toSort);

            CollectionAssert.AreEqual(toSort, sorted);
        }

        [TestMethod]
        public void TestFloats()
        {
            double[] toSort = new double[] { 3.2, 2.9, 1.1 };
            double[] sorted = new double[] { 1.1, 2.9, 3.2 };

            Sort.InsertSort(toSort);

            CollectionAssert.AreEqual(toSort, sorted);
        }

        [TestMethod]
        public void TestNegatives()
        {
            double[] toSort = new double[] { -3.2, 2, -8 };
            double[] sorted = new double[] { -8, -3.2, 2 };

            Sort.InsertSort(toSort);

            CollectionAssert.AreEqual(toSort,sorted);
        }
    }
}
