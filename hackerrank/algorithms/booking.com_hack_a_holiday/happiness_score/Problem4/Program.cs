using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem4
{
    class Solution
    {
        static int n;
        static HashSet<int> set = new HashSet<int>();
        static BitArray primes;
        static void Main(string[] args)
        {
            bool console = true;
            input(console);
            solve();

            if (!console) Console.ReadKey();
        }

        static void solve()
        {
            int max = set.Max();
            ComputePrimes(max + 1);
            int k = 0;
            foreach (int b in set)
            {
                if (primes[b]) k++;
            }
            Console.WriteLine(k);
        }

        public static void ComputePrimes(int limit)
        {
            // Sieve of Erathosthenes
            primes = new BitArray(limit);
            primes.SetAll(true);
            primes.Set(0, false);
            primes.Set(1, false);
            for (int i = 0; i * i < limit; i++)
            {
                if (primes.Get(i))
                {
                    for (int j = i * i; j < limit; j += i)
                    {
                        primes.Set(j, false);
                    }
                }
            }
        }

        static void input(bool console, string filename = "input.txt")
        {
            if (!console)
            {
                Console.SetIn(new StreamReader(filename));
            }
            n = Int32.Parse(Console.ReadLine());
            string[] line = Console.ReadLine().Split();
            for (int i = 0; i < n; i++)
            {
                int b = Int32.Parse(line[i]);
                HashSet<int> toAdd = new HashSet<int>();
                foreach (int j in set)
                {
                    toAdd.Add(b + j);
                }
                foreach (int j in toAdd)
                {
                    set.Add(j);
                }
                set.Add(b);
            }
        }

    }
}
