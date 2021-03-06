using System;
using System.Security.Cryptography;

namespace Util
{
    public static class RNG
    {
        private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();
        private const double RNG_MULT = 1d / 255d;

        public static int Generate(int i)
        {
            return Generate(0, i);
        }

        public static int Generate(int i, int j)
        {
            byte[] n = new byte[1];
            _generator.GetBytes(n);
            double mult = (Convert.ToDouble(n[0]) * RNG_MULT) - 0.00000000001d;
            return mult > 0 ? (i + (int)(Math.Floor(mult * (j - i)))) : i;
        }

        public static T TakeRandom<T>(T[] list)
        {
            return list.Length > 0 ? list[Generate(list.Length)] : default(T);
        }

        public static T TakeRandomExcept<T>(T[] list, T except)
        {
            if (list.Length == 0) return default(T);
            T result = list[Generate(1, list.Length)];
            return result.Equals(except) ? list[0] : result;
        }
    }
}
