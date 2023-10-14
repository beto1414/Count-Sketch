using System;
using System.Numerics;
using System.Diagnostics;
using System.Collections.Generic;
// using Utilities;


namespace hashing 
{
    public class Hash_function 
    {
        // ******** MULTIPLY SHIFT  *********      
        public ulong a_ms = 0b0001000110111000101100010100101111000001110111101010010011101001;
        
        public ulong mul_shift(ulong x,int l)
        {
            ulong retVal = (a_ms*x)>>(64-l);
            return retVal;
        }

        //******** MULTIPLY MOD PRIME MOD 2^L*******

        BigInteger p_mmp = ((BigInteger)1<<89)-1;
        BigInteger a_mmp = BigInteger.Parse("258409117785417240037739775");
        BigInteger b_mmp = BigInteger.Parse("310120584644151575600621");

        public ulong mul_mod_prime(ulong x, int l)
        {
            ulong l_ = (ulong)Math.Pow(2,l);

            BigInteger y = a_mmp*x+b_mmp;
            y = (y&p_mmp)+(y>>89);
            if(y>=p_mmp)
            {
                y-=p_mmp;
            }
            
            ulong z = (ulong)(y&(l_-1));
            // ulong z = (ulong)(y % l_);
            if(z>=l_)
            {
                z-=l_;
            } 
            return z;
        }
    }



    public class H_table_MS
    {
        public int l;
        Hash_function fun;
        public List<Tuple<ulong,int>>[] hashTable;

        public H_table_MS(int l_)
        {
            l = l_;
            fun = new Hash_function();
            hashTable = new List<Tuple<ulong,int>>[(1<<l)];
        }

        public int get(ulong x)
        {
            bool isEmpty = hashTable[(ulong)fun.mul_shift(x,l)]?.Any() != true;
            if(isEmpty)
            {
                return 0;
            }
            else if (hashTable[(ulong)fun.mul_shift(x,l)].Any(a => a.Item1 == x))
            {
                return hashTable[(ulong)fun.mul_shift(x,l)].Find(a => a.Item1 == x).Item2;
            }
            else
            {
                return 0;
            }
        }

        public void set(ulong x, int v)
        {
            bool isEmpty = hashTable[(ulong)fun.mul_shift(x,l)]?.Any() != true;
            if(isEmpty)
            {
                var variable = Tuple.Create(x,v);
                hashTable[(ulong)fun.mul_shift(x,l)] = new List<Tuple<ulong,int>>
                {
                    variable
                };

            }
            else if  (hashTable[(ulong)fun.mul_shift(x,l)].Any(a => a.Item1 == x))
            {
                var variable = Tuple.Create(x,v);
                hashTable[(ulong)fun.mul_shift(x,l)][hashTable[(ulong)fun.mul_shift(x,l)].FindIndex(a => a.Item1 == x)] = variable;
            }
            else
            {
                var variable = Tuple.Create(x,v);
                hashTable[(ulong)fun.mul_shift(x,l)].Add(variable);
            }
        }

        public void increment (ulong x, int d)
        {
            if (get(x)== 0)
            {
                set(x,d);
            }
            else
            {
                set(x, (get(x) + d));
            }
        }
    }


    public class H_table_MMP
    {
        public int l;
        Hash_function fun;
        public List<Tuple<ulong,int>>[] hashTable;

        public H_table_MMP(int l_)
        {
            l = l_;
            fun = new Hash_function();
            hashTable = new List<Tuple<ulong,int>>[(1<<l)];
        }

        public int get(ulong x)
        {
            bool isEmpty = hashTable[(ulong)fun.mul_mod_prime(x,l)]?.Any() != true;
            if(isEmpty)
            {
                return 0;
            }
            else if (hashTable[(ulong)fun.mul_mod_prime(x,l)].Any(a => a.Item1 == x))
            {
                return hashTable[(ulong)fun.mul_mod_prime(x,l)].Find(a => a.Item1 == x).Item2;
            }
            else
            {
                return 0;
            }
        }

        public void set(ulong x, int v)
        {
            bool isEmpty = hashTable[(ulong)fun.mul_mod_prime(x,l)]?.Any() != true;
            if(isEmpty)
            {
                var variable = Tuple.Create(x,v);
                hashTable[(ulong)fun.mul_mod_prime(x,l)] = new List<Tuple<ulong,int>>
                {
                    variable
                };

            }
            else if  (hashTable[(ulong)fun.mul_mod_prime(x,l)].Any(a => a.Item1 == x))
            {
                var variable = Tuple.Create(x,v);
                hashTable[(ulong)fun.mul_mod_prime(x,l)][hashTable[(ulong)fun.mul_mod_prime(x,l)].FindIndex(a => a.Item1 == x)] = variable;
            }
            else
            {
                var variable = Tuple.Create(x,v);
                hashTable[(ulong)fun.mul_mod_prime(x,l)].Add(variable);
            }
        }

        public void increment (ulong x, int d)
        {
            if (get(x)== 0)
            {
                set(x,d);
            }
            else
            {
                set(x, (get(x) + d));
            }
        }
    }


    public class SquareSum
    {
        private int l;
        public SquareSum(int l_)
        {
            l = l_;
        }
        public ulong MulShift (IEnumerable<Tuple<ulong,int>> pairs)
        {
            H_table_MS table_MulShift = new H_table_MS(l);
            ulong sum = 0;
            foreach(Tuple<ulong,int> item in pairs)
            {
                table_MulShift.increment(item.Item1, item.Item2);
            }
            
            for(int i = 0; i<table_MulShift.hashTable.Length;i++)
            {
                if(table_MulShift.hashTable[i] != null)
                {
                    foreach(Tuple<ulong,int> item in table_MulShift.hashTable[i])
                    {
                        sum += (ulong)Math.Pow(item.Item2,2);
                    }
                }
            }
            return sum;
        }



        public ulong ModPrime (IEnumerable<Tuple<ulong,int>> pairs)
        {
            ulong sum = 0;
            H_table_MMP table_ModPrime = new H_table_MMP(l);

            foreach(Tuple<ulong,int> item in pairs)
            {
                table_ModPrime.increment(item.Item1, item.Item2);
            }
            
            for(int i = 0; i<table_ModPrime.hashTable.Length;i++)
            {
                if(table_ModPrime.hashTable[i] != null)
                {
                    foreach(Tuple<ulong,int> item in table_ModPrime.hashTable[i])
                    {
                        sum += (ulong)Math.Pow(item.Item2,2);
                    }
                }
            }
            return sum;


        }
    }
}
