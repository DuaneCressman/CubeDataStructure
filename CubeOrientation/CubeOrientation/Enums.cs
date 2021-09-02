using System;
using System.Collections.Generic;
using System.Text;

namespace CubeOrientation
{
    public enum TagTypes
    {
        Face = 1,
        Location = 2,
        Piece = 3
    }

    public enum SpaceTypes
    {
        Center,
        Edge,
        Corner
    }

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
