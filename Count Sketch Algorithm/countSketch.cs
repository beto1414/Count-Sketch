 using System;
 using System.Numerics;
 using stream;
 using hashing;
 using random_bigints;
 // using System.Math;

namespace countsketch { // Opgave 4
    class CountSketch {
        BigInteger p = ((BigInteger)1<<89)-1;
        public BigInteger[] a =
        {Randomizer.getRandom(11),
            Randomizer.getRandom(11),
            Randomizer.getRandom(11),
            Randomizer.getRandom(11)
        };
 
        public BigInteger uni4_hash(ulong x) {
            // int k = 4;
            BigInteger y = a[3];
            for (int i = 2; i>-1; i--) 
            {
                y = y*x + a[i];
                y = (y&p)+(y>>89);
            }
            if (y>=p) 
            {
                y = y - p;
            }
            return y;  
        }

        public Tuple<int,int> Alg2(ulong x, int m) {
            BigInteger g = uni4_hash(x);
            
            int h = (int) (g&(m-1)); // Least log_2(m) significant bits
            int b = (int)(g>>88); // Most significant bit
            int s = 1-(2*b);
            return Tuple.Create(h,s);
        }

        public double CSketch(int t,  IEnumerable<Tuple<ulong, int>> stream) {
            int m = 1<<t;
            int[] C = new int[m];
            for (int i = 0; i<m; i++) {
                C[i] = new int();
            }
            foreach (Tuple<ulong, int> x in stream) {
                Tuple<int,int> temp = Alg2(x.Item1, m);
                int h = temp.Item1;
                int s = temp.Item2;
                C[h] = C[h]+s*x.Item2;
            }
            double sum = 0;
            for (int i = 0; i<m; i++) {
                sum += Math.Pow((double) C[i],2);
            }
            return sum;
        } 
    }
}

