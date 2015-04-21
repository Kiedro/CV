using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sortowanie;

namespace Sortowanie
{
    class Program
    {
        static void Main(string[] args)
        {
            //int size = Utiliteis.ReadSize();                               // tworzy tablicę o zadanym rozmiarze i rozkładzie jednostkowym
            //int[] randomTable = Utiliteis.CreateRandomIntTable(size);      // elementów z zakresu [0-size]

            double[] table;
            if (!Utiliteis.ArrayRead<double>(@"Resources\floats.txt", out table))
            {
                Console.ReadKey();
                return;
            }

            Console.WriteLine("[{0}]", string.Join(", ", table));
            Console.WriteLine();            
            Console.WriteLine();

            Sort.BubbleSort(table);
            //Sort.InsertSort(table);
            //Sort.QuickSort(randomTable);
            //Sort.BubbleSort(randomTable);
            //Sort.CountigSort(table);

            Utiliteis.WriteList(table);        
            Console.ReadKey();
        }
    }
}
