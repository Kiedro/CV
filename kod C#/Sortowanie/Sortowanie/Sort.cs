using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Sortowanie
{
    public static class Sort
    {
        public static void BubbleSort<T>(IList<T> ar) where T: IComparable
        {
            int tempSize = ar.Count;
            bool sorted;
            do
            {
                sorted = true;
                for (int j = 0; j < tempSize - 1 ; j++)
                {
                    if (ar[j].CompareTo(ar[j + 1]) == 1) 
                    {
                        Swap(ar, j, j + 1);
                        sorted = false;
                    }
                }
                tempSize--;
            } while (!sorted);
        }

        public static void InsertSort<T>(IList<T> ar) where T: IComparable<T>
        {
            Contract.Requires(ar != null);

            int size = ar.Count;
            for (int i = 1; i < size; i++)
            {
                T key = ar[i];
                int j = i - 1;
                while ((j >= 0) && (ar[j].CompareTo(key) == 1))
                {
                    ar[j + 1] = ar[j];
                    j--;
                }
                ar[j + 1] = key;
            }
        }

        public static void QuickSort<T>(IList<T> ar) where T : IComparable
        {
            Contract.Requires(ar != null);

            int r = ar.Count - 1;    // ostatni index
            if (0 < r)
            {
                int q = DivideTable(ar, 0, r);
                QuickSort(ar, 0, q - 1);
                QuickSort(ar, q + 1, r);
            }
        }

        public static void QuickSort<T>(IList<T> ar,int l, int r) where T: IComparable
        {
            if(l<r)
            {
                int q = DivideTable(ar, l, r);
                QuickSort(ar, l, q - 1);
                QuickSort(ar, q + 1, r);
            }
        }

        private static int DivideTable<T>(IList<T> A, int l, int r) where T: IComparable
        {
         
            T pivot = A[r];
            int i = l;
            for (int j = l; j < r; j++)
            {
                if(A[j].CompareTo(pivot) != 1)
                {
                    Swap(A, i, j);
                    i++;
                }
            }
            Swap(A, i, r);
            return i;
        }

        /// <summary>
        /// Sortowanie przez zliczanie dla liczb całowitych nieujemnych
        /// </summary>
        /// <param name="ar"></param>
        public static void CountigSort(IList<int> ar)
        {
            Contract.Requires<ArgumentNullException>(ar != null);
            Contract.Requires<ArgumentOutOfRangeException>(Contract.ForAll<int>(ar, p => p >= 0), "Argumenty muszą być nieujemne.");

            int length = ar.Count;
            int maxValue = ar.Max();
            int[] copyAr = ar.ToArray();                 // kopia tablicy wejściowej, umożliwia wpisanie posortowanych wartości bezpośrednio do tablicy ar
            int[] countigTable = new int[maxValue+1];          // tablica tymczasowa, od 0 do maxValue w ar

            // zliczanie elementów
            for (int i = 0; i < length; i++)
            {
                countigTable[copyAr[i]]++;
            }
            // sumowanie pozycji
            for (int i = 1; i <= maxValue; i++)
            {
                countigTable[i] += countigTable[i - 1];
            }
            for (int i = length - 1; i >= 0; i--)
            {
                ar[--countigTable[copyAr[i]]] = copyAr[i];
            }
            
        }

        public static void BucketSort(IList<double> ar)
        {
            Contract.Requires(ar != null);
            Contract.Requires<ArgumentOutOfRangeException>(Contract.ForAll<double>(ar, p => p >= 0 && p < 1), "Argumenty spoza przedziału [0;1)");

            int length = ar.Count;
           // List<double>[] B = new List<double>[length];

            var B = Enumerable.Range(0, length).Select((i) => new List<double>()).ToArray();

            for (int i = 0; i < length; i++)  // podział na kubełi
            {
                int index = (int)Math.Floor(length * ar[i]);
                B[index].Add(ar[i]);
            }
            foreach (var item in B)        // sortowanie kubełków za pomocą InsertSort
            {
                InsertSort<double>(item);
            }
            for (int i = 1; i < length; i++)
            {
                B[0].AddRange(B[i]);
            }
            ar = B[0];
            for (int i = 0; i < length; i++)
            {
                ar[i] = B[0].ElementAt(i);
            }
        }

        private static void Swap<T>(IList<T> ar, int first, int second)
        {
            T hold = ar[first];
            ar[first] = ar[second];
            ar[second] = hold;
        }
    }
}
