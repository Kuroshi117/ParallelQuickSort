using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace ParallelQuickSort
{
    class Program
    {
        public static List<Tuple<int, Guid, double>> SortTuple = new List<Tuple<int, Guid, double>>();
        static void Main(string[] args)
        {
            ReadText(@"C:\workspace\FakeData.csv");
            Stopwatch sw = Stopwatch.StartNew();

            //SortByDoubleUsingQuickSort(SortTuple, 0, SortTuple.Count - 1);
            ParallelQuickSortDouble(SortTuple, 0, SortTuple.Count - 1);
            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds + " ms");
            for (int i = 0; i < SortTuple.Count; i++)
            {
                Console.Write(i.ToString() + ": ");
                Console.WriteLine(SortTuple[i].ToString());
            }
            Console.ReadKey();
        }

        public static void ReadText(string path)
        {
            string line;
            string[] words = new string[3];
            using (StreamReader sr = new StreamReader(path))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    words = line.Split(',');
                    SortTuple.Add(Tuple.Create(Convert.ToInt32(words[0]), Guid.Parse(words[1]), Convert.ToDouble(words[2])));

                }

            }
        }

        public static void SortByDoubleUsingQuickSort(List<Tuple<int, Guid, double>> Sort, int low, int high)
        {
            if(low<high)
            {
                int p=Partition(Sort, low, high);
                SortByDoubleUsingQuickSort(Sort, low, p - 1);
                SortByDoubleUsingQuickSort(Sort, p + 1, high);
            }

        }

        public static void ParallelQuickSortDouble(List<Tuple<int, Guid, double>> Sort, int low, int high)
        {   
            const int threshold = 2048;
            if (high <= low) return;
            if (low < high)
            {
                if(high-low<threshold)
                {
                    SortByDoubleUsingQuickSort(SortTuple,low, high);
                }
                else
                {
                    int p = Partition(Sort, low, high);
                    Parallel.Invoke(
                        () => ParallelQuickSortDouble(Sort, low, p - 1),
                        () => ParallelQuickSortDouble(Sort, p + 1, high)
                        );

                }
            }

        }

        public static int Partition(List<Tuple<int, Guid, double>> Sort, int low, int high)
        {
            var pivot = Sort[low].Item3;
            while(true)
            {
                while(Sort[low].Item3<pivot)
                {
                    low++;
                }
                while(Sort[high].Item3>pivot)
                {
                    high--;
                }
                if(low<high)
                {
                    if(Sort[low].Item3==Sort[high].Item3)
                    {
                        return high;
                    }
                    Tuple<int, Guid, double> temp = Sort[low];
                    Sort[low] = Sort[high];
                    Sort[high] = temp;
                }
                else
                {
                    return high;
                }
            }
            
        }
    }
}
