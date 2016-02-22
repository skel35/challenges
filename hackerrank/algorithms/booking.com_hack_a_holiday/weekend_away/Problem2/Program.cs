using System;
using System.IO;
using System.Collections.Generic;

namespace Problem2
{
    class Solution
    {
        static int t, m, n;
        static List<Tuple<int, int, short>> roads;
        static void Main(string[] args)
        {
            bool console = true;
            inputT(console);
            while (t-- > 0)
            {
                input(console);
                solve();
            }

            if (!console) Console.ReadKey();
        }

        static int CompareTo(Tuple<int, int, short> t1, Tuple<int, int, short> t2)
        {
            return t1.Item3.CompareTo(t2.Item3);
        }

        static void solve()
        {
            roads.Sort(CompareTo);
            int maxM = m;
            int min = Int32.MaxValue;
            for (int i = 0; i < m - 1; i++)
            {
                for (int j = i + 1; j < maxM; j++)
                {
                    if (Neighbour(roads[i], roads[j]) &&
                        roads[i].Item3 + roads[j].Item3 < min)
                    {
                        min = roads[i].Item3 + roads[j].Item3;
                        maxM = j + 1;
                    }
                }
            }
            Console.WriteLine(min);
        }

        static bool Neighbour(Tuple<int, int, short> t1, Tuple<int, int, short> t2)
        {
            return t1.Item1 == t2.Item1 || t1.Item1 == t2.Item2 || t1.Item2 == t2.Item1 || t1.Item2 == t2.Item2;
        }

        static void input(bool console, string filename = "input.txt")
        {
            string[] line = Console.ReadLine().Split();
            n = Int32.Parse(line[0]); m = Int32.Parse(line[1]);
            roads = new List<Tuple<int, int, short>>();
            int v1, v2;
            short l;
            for (int i = 0; i < m; i++)
            {
                line = Console.ReadLine().Split();
                v1 = Int32.Parse(line[0]) - 1; v2 = Int32.Parse(line[1]) - 1;
                l = Int16.Parse(line[2]);
                roads.Add(Tuple.Create(v1, v2, l));
            }

        }

        static void inputT(bool console, string filename = "input.txt")
        {
            if (!console)
            {
                Console.SetIn(new StreamReader(filename));
            }
            t = Int32.Parse(Console.ReadLine());
        }

    }
}
