using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sortowanie
{
    public static class Utiliteis
    {
        /// <summary>
        /// Zwraca odczytaną z konsoli liczbę, domyślną wartością zwracaną jest 10
        /// </summary>
        /// <returns></returns>
        public static int ReadSize()
        {
            try
            {
                return Int32.Parse(Console.ReadLine());
            }
            catch
            {
                return 10;
            };
        }
        
        /// <summary>
        /// Tworzy tablicę elementów odczytancych z pliku.
        /// Pierwszy wiersz pliku musi zawierać liczbę elementów do odczytania, drugi elementy rozdzielone spacjami. 
        /// </summary>
        /// <typeparam name="T">Typ elementów tworzących tabelę</typeparam>
        /// <param name="filePath">Ścieżka do pliku</param>
        /// <param name="array">Zwracana tablica</param>
        /// <returns>true - powodzenie odczytu, false - niepowodzenie</returns>
        public static bool ArrayRead<T>(string filePath, out T[] array)
        {
            Contract.Requires<ArgumentNullException>(!String.IsNullOrEmpty(filePath));

            array = null;
            try
            {
                using (StreamReader stream = new StreamReader(filePath))
                {
                        string line = stream.ReadLine();
                        int counter;
                        if (!String.IsNullOrEmpty(line))
                            counter = Int32.Parse(line);
                        else
                            return false;

                        if (counter > 0)
                            array = new T[counter];

                        else
                            return false;

                        line = stream.ReadLine();
                        var elemtens = line.Split(' ');
                        if(elemtens.Length != counter)
                            throw new Exception();
                        TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

                        for (int i = 0; i < counter; i++)
                        {
                            array[i] = (T)converter.ConvertFromString(elemtens[i]);
                        }

                        return true;
                }
            }
            catch
            {
                Console.WriteLine("Nieudany odczyt z pliku");
                    array = null;
                    return false;
            }
        }

        /// <summary>
        /// Tworzy tabelę int o warościach losowych z zakresu [0,size]
        /// </summary>
        /// <param name="size">Żądana liczba elementów</param>
        /// <returns></returns>
        public static int[] CreateRandomIntTable(int size)
        {
            Contract.Requires<ArgumentOutOfRangeException>(size > 0);

            Random random = new Random();
            return Enumerable.Range(0, size).Select(x => x = random.Next(size + 1)).ToArray();  // tablica z elementami z zakresu [0-size]
        }

        public static void WriteList<T>(IEnumerable<T> collection)
        {
            Contract.Requires<ArgumentNullException>(collection != null);

            Console.WriteLine("[{0}]", string.Join(", ", collection));
        }
    }
}
