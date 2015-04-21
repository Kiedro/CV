using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sortowanie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortowanieTests
{
    [TestClass]
    public class CountigSortTests
    {
        [TestMethod]
        public void TestInts()
        {
            int[] toSort = new int[] { 3, 2, 1 };
            int[] sorted = new int[] { 1, 2, 3 };

            Sort.CountigSort(toSort);

            CollectionAssert.AreEqual(toSort, sorted);
        }

        [TestMethod]
        public void TestNegatives()
        {
            int[] toSort = new int[] { 3, -2, 1 };
            int[] sorted = new int[] { 1, -2, 3 };

            Sort.CountigSort(new int[] { 1, -2, 3 });

            CollectionAssert.AreEqual(toSort, sorted);
        }

    }
}
