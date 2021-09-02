using System;
using System.Collections.Generic;
using System.Text;

namespace CubeOrientation
{
    public static class Helpers
    {
        public static int GetIndex<T>(this T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (Equals(value, array[i]))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
