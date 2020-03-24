using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML1_Lib
{
    /// <summary>
    /// Helper class.
    /// </summary>
    public static class Misc
    {
        /// <summary>
        /// General, global rng.
        /// </summary>
        public static readonly Random rng;
        public static readonly char ESC = (char)27;
        static Misc()
        {
            rng = new Random((int)DateTime.Now.Ticks);
        }

        /// <summary>
        /// Swaps two values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void Swap<T>(ref T x, ref T y)
        {
            T t = x;
            x = y;
            y = t;
        }
    }
}
