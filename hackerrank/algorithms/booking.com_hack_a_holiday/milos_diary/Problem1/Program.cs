using System;
using System.IO;
using System.Collections.Generic;

namespace Problem1
{
    class Solution
    {
        static int m;
        static int[] a;
        static void Main(string[] args)
        {
            bool console = true; // false if debugging locally
            input(console);
            
            if (!console) Console.ReadKey();
        }


        static void input(bool console, string filename = "input.txt")
        {
            if (!console)
            {
                Console.SetIn(new StreamReader(filename));
            }
            m = Int32.Parse(Console.ReadLine());
            string[] line = Console.ReadLine().Split();
            a = new int[m];
            a[0] = Int32.Parse(line[0]);
            int max = a[0];
            HashSet<int> visitedSet = new HashSet<int>();
            visitedSet.Add(a[0]);
            for (int i = 1; i < m; i++)
            {
                a[i] = Int32.Parse(line[i]);
                if (a[i] > a[i - 1])
                {
                    if (a[i] > max) max = a[i];
                    else { Failure(); return; }
                }
                visitedSet.Add(a[i]);
            }
            if (visitedSet.Count != m) Failure();
            else Success();
        }

        static void Failure()
        {
            Console.WriteLine("NO");
        }

        static void Success()
        {
            Console.WriteLine("YES");
        }
    }
}
