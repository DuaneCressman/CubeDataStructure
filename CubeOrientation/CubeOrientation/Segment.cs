using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CubeOrientation
{
    /// <summary>
    /// This class hold one segment of the cube. It is able to handle being rotated on its own.
    /// </summary>
    public class Segment
    {
        private static readonly char[] WRotationOrder = { 'R', 'G', 'O', 'B' };
        private static readonly char[] RRotationOrder = { 'W', 'B', 'Y', 'G' };
        private static readonly char[] BRotationOrder = { 'W', 'O', 'Y', 'R' };
        private const int ROTATION_ORDER_LENGTH = 4;

        /// <summary>
        /// The location of the segment on the cube.
        /// This is held by 2 chars for an edge, and 3 chars for a corner.
        /// </summary>
        private char[] location;

        /// <summary>
        /// The colours that are on the segment.
        /// </summary>
        private char[] colours;

        /// <summary>
        /// If the segment is in the correct location.
        /// </summary>
        public bool isCorrect
        {
            get
            {
                for (int i = 0; i < location.Length; i++)
                {
                    if (location[i] != colours[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public SpaceTypes SpaceType
        {
            get
            {
                return location.Length switch
                {
                    1 => SpaceTypes.Center,
                    2 => SpaceTypes.Edge,
                    3 => SpaceTypes.Corner,
                    _ => throw new Exception("This tags colours where invalid.")
                };
            }
        }

        public Segment(char[] location, char[] colours)
        {
            this.location = location;
            this.colours = colours;
        }

        /// <summary>
        /// Creates the segment in the correct location.
        /// </summary>
        public Segment(params char[] location)
        {
            this.location = new char[location.Length];
            colours = new char[location.Length];

            location.CopyTo(this.location, 0);
            location.CopyTo(this.colours, 0);
        }

        /// <summary>
        /// If the segment is on a side of the cube.
        /// This can be determined by if the location array contains
        /// the side colour.
        /// </summary>
        /// <param name="sideColour">The colour of the side to check.</param>
        public bool IsOnSide(char sideColour)
        {
            return GetIndex(sideColour, location) != -1;
        }

        /// <summary>
        /// Rotate the segment. 
        /// The colours array will stay the same.
        /// The location array will be updated.
        /// </summary>
        /// <param name="sideRotating">The side that is being rotated.</param>
        /// <param name="clockwise">If the side is being rotated clockwise.</param>
        public void Rotate(char sideRotating, bool clockwise)
        {
            if (!IsOnSide(sideRotating))
            {
                return;
            }

            //get the order that the colours will need to rotate
            char[] rotationOrder;
            switch (sideRotating)
            {
                case 'W':
                case 'Y':
                    rotationOrder = WRotationOrder;
                    break;

                case 'R':
                case 'O':
                    rotationOrder = RRotationOrder;
                    break;

                case 'B':
                case 'G':
                    rotationOrder = BRotationOrder;
                    break;

                default:
                    throw new Exception("The side rotating was invalid");
            }

            //if the side rotating is Yellow, Orange, or Green, we can just use White, Red, or Blue rotation orders in revers.  
            if (!"WRB".Contains(sideRotating))
            {
                clockwise = !clockwise;
            }

            for (int i = 0; i < location.Length; i++)
            {
                //The side that is on the cube doesn't change
                if (location[i] == sideRotating)
                {
                    continue;
                }

                int index = GetIndex(location[i], rotationOrder);

                if (index == -1)
                {
                    throw new Exception("Unable to find this colour in the rotation order");
                }

                if (clockwise)
                {
                    location[i] = rotationOrder[index == ROTATION_ORDER_LENGTH - 1 ? 0 : index + 1];
                }
                else
                {
                    location[i] = rotationOrder[index == 0 ? ROTATION_ORDER_LENGTH - 1 : index - 1];
                }
            }
        }

        /// <summary>
        /// Get the index of an element in an array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int GetIndex<T>(T value, T[] array)
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

        public override string ToString()
        {
            return $"\n" +
                   $"   Piece: {new string(colours)}\n" +
                   $"Location: {new string(location)}";
        }
    }
}
