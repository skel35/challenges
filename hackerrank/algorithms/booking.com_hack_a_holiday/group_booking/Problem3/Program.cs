using System;
using System.IO;
using System.Collections.Generic;

namespace Problem3
{
    class Solution
    {
        /// <summary>
        ///  main idea - linear sorting, 
        ///  both on people's budgets and room's capacities
        /// </summary>
        static int g, r;
        const int MaxBudget = 10000;
        const int MaxCapacity = 100;
        static int[] people = new int[MaxBudget];
        static List<int>[] rms = new List<int>[MaxCapacity];
        
        static void Main(string[] args)
        {
            bool console = true;
            input(console, "input2.txt");
            solve();

            if (!console) Console.ReadKey();
        }

        static void solve()
        {
            int peopleDone = 0;
            int roomN = 0;
            Accomodate(ref peopleDone, ref roomN, true);
            if (peopleDone < g)
            {
                Accomodate(ref peopleDone, ref roomN, false);
            }
            Console.WriteLine(roomN);
        }

        static void Accomodate(ref int peopleDone, ref int roomN, bool toSkip = true)
        {
            int toAccomodate;
            for (int c = MaxCapacity; c >= 1; c--)
            {
                if (toSkip)
                {
                    toAccomodate = Math.Min(g - peopleDone, c);
                    c = toAccomodate;
                    if (toAccomodate <= 0)
                    {
                        return;
                    }
                }
                for (int room = 0; room < rms[c - 1].Count; room++)
                {
                    toAccomodate = Math.Min(g - peopleDone, c);
                    if (toAccomodate <= 0)
                    {
                        return;
                    }
                    bool success = RemoveRange(rms[c - 1][room], toAccomodate);
                    if (success)
                    {
                        roomN++;
                        peopleDone += toAccomodate;
                    }
                    else
                    {
                        if (c > 1)
                        {
                            rms[c - 2].Add(rms[c - 1][room]);
                        }
                    }
                }
            }
            
        }

        static bool RemoveRange(int fromPrice, int N)
        {
            int sum = 0;
            int i = fromPrice - 1;
            while (sum < N && i < MaxBudget)
            {
                sum += people[i++];
            }
            if (sum < N)
            {
                return false;
            }
            
            i = fromPrice - 1;
            
            sum = N;
            while (sum > 0)
            {
                sum -= people[i];
                people[i++] = 0;
            }
            people[i - 1] += - sum;

            return true;
        }

        static void input(bool console, string filename = "input.txt")
        {
            if (!console)
            {
                Console.SetIn(new StreamReader(filename));
            }
            string[] line = Console.ReadLine().Split();
            g = Int32.Parse(line[0]); r = Int32.Parse(line[1]);
            line = Console.ReadLine().Split();
           
            for (int i = 0; i < g; i++)
            {
                people[Int32.Parse(line[i]) - 1]++;
            }
            
            for (int i = 0; i < MaxCapacity; i++)
            {
                rms[i] = new List<int>();
            }
            for (int i = 0; i < r; i++)
            {
                line = Console.ReadLine().Split();
                short c = Int16.Parse(line[1]);
                int price = Int32.Parse(line[0]);
                if (price < MaxBudget + 1)
                {
                    rms[c - 1].Add(price);
                }
            }
        }

    }
}
