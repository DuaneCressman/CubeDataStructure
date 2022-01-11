using System;
using System.Collections.Generic;
using System.Text;

namespace CubeOrientation
{
    /// <summary>
    /// This is just a stand in class until this project is moved to unity and Vector3Int can be used
    /// </summary>
    struct V3Int
    {
        public int x;
        public int y;
        public int z;

        public V3Int(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static V3Int operator +(V3Int a, V3Int b) => new V3Int(a.x + b.x, a.y + b.y, a.z + b.z);
        public static V3Int operator *(V3Int a, int b) => new V3Int(a.x * b, a.y * b, a.z * b);

        public int Sum => x + y + z;

        public V3Int Simplified
        {
            get
            {
                V3Int simplified = new V3Int();

                if(x != 0)
                {
                    simplified.x = 1;
                }

                if(y != 0)
                {
                    simplified.y = 1;
                }

                if(z != 0)
                {
                    simplified.z = 1;
                }

                return simplified;
            }
        }

        public override string ToString()
        {
            return $"({x}, {y}, {z})";
        }
    }
}
