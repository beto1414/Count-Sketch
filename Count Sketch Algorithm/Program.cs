using stream;
using System;
using hashing;
using countsketch;
using System.Numerics;
using System.Collections.Generic;
using System.Diagnostics;

namespace hashImplementation {
    internal class Program {
        static void Main (string[] args) 
        {
            Stopwatch sw = new Stopwatch();
            CountSketch countSketch = new CountSketch();
            SquareSum calc = new SquareSum(15);
            IEnumerable<Tuple <ulong , int >> values = RandomStream.CreateStream((int)Math.Pow(2,18),15);
            ulong S = calc.ModPrime(values);
            double[] results = new double[100];
            int m = 15;
            sw.Start();

            double[] group_1 = new double[11];
            double[] group_2 = new double[11];
            double[] group_3 = new double[11];
            double[] group_4 = new double[11];
            double[] group_5 = new double[11];
            double[] group_6 = new double[11];
            double[] group_7 = new double[11];
            double[] group_8 = new double[11];
            double[] group_9 = new double[11];

            for (int i = 0; i<100; i++) 
            {
                results[i] = countSketch.CSketch(m,values);
            }
            for (int i = 0; i<11; i++) 
            {
                group_1[i] = results[i];
                group_2[i] = results[i+11];
                group_3[i] = results[i+22];
                group_4[i] = results[i+33];
                group_5[i] = results[i+44];
                group_6[i] = results[i+55];
                group_7[i] = results[i+66];
                group_8[i] = results[i+77];
                group_9[i] = results[i+88];
            }
            Array.Sort(group_1);
            Array.Sort(group_2);
            Array.Sort(group_3);
            Array.Sort(group_4);
            Array.Sort(group_5);
            Array.Sort(group_6);
            Array.Sort(group_7);
            Array.Sort(group_8);
            Array.Sort(group_9);
            
            Console.WriteLine("Median, group 1: {0}",group_1[5]);
            Console.WriteLine("Median, group 2: {0}",group_2[5]);
            Console.WriteLine("Median, group 3: {0}",group_3[5]);
            Console.WriteLine("Median, group 4: {0}",group_4[5]);
            Console.WriteLine("Median, group 5: {0}",group_5[5]);
            Console.WriteLine("Median, group 6: {0}",group_6[5]);
            Console.WriteLine("Median, group 7: {0}",group_7[5]);
            Console.WriteLine("Median, group 8: {0}",group_8[5]);
            Console.WriteLine("Median, group 9: {0}",group_9[5]);

            Array.Sort(results);
            Tuple<int,double>[] sortedResults = new Tuple<int,double>[100];

            double X_mean_sq_error = 0;
            double X_mean_value = 0;
            for (int i = 0;i<100;i++)
            {
                sortedResults[i] = Tuple.Create(i+1,results[i]);
                X_mean_sq_error += Math.Pow(sortedResults[i].Item2-S,2);
                X_mean_value += sortedResults[i].Item2;
                Console.WriteLine("Result {0}, value = {1}", sortedResults[i].Item1,sortedResults[i].Item2);

            }
            X_mean_value = X_mean_value/100;
            X_mean_sq_error = X_mean_sq_error/100;
            double S_mean_sq_error = (2*Math.Pow(S,2))/(1<<m);
            Console.WriteLine("Value of S: "+S);
            Console.WriteLine("Mean value X: {0}",X_mean_value);
            Console.WriteLine("S variance = {0}",S_mean_sq_error);
            Console.WriteLine("X variance: {0}",X_mean_sq_error);
            sw.Stop();
            Console.WriteLine("Elapsed time: {0}",sw.Elapsed);
        }
    }
}
