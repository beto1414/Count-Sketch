 using System.Numerics;
using System;
// using System.Math;

namespace random_bigints {
    public class Randomizer {
        public static BigInteger getRandom(int length)
        {
            Random random = new Random();
            byte[] data = new byte[length];
            random.NextBytes(data);
            return new BigInteger(data, true);
        }
    }
}