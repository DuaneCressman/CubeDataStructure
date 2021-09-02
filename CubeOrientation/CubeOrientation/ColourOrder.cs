using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CubeOrientation
{
    /// <summary>
    /// This class defines the colour order of the cube. 
    /// This is used to determine which colours are beside each other.
    /// </summary>
    public static class ColourOrder
    {
        public const int ROTATION_ORDER_LENGTH = 4;
        public static readonly char[] WRotationOrder = { 'R', 'G', 'O', 'B' };
        public static readonly char[] RRotationOrder = { 'W', 'B', 'Y', 'G' };
        public static readonly char[] BRotationOrder = { 'W', 'O', 'Y', 'R' };

        /// <summary>
        /// The base colours are arbitrarily chosen colours. None of the are opposite colours on the cube.
        /// These colours are always used first when referencing colours in the cube. This is mostly done 
        /// for consistency across the project.
        /// </summary>
        public static readonly char[] BASE_COLOURS = { 'W', 'R', 'B' };
        /// <summary>
        /// The secondary colours are the opposite colour to the <see cref="BASE_COLOURS"/> with the same index.
        /// These colours are always used second when referencing colours in the cube.
        /// </summary>
        public static readonly char[] SECONDARY_COLOURS = { 'Y', 'O', 'G' };

        /// <summary>
        /// Any time more that one colour is referenced, it should always be done in this order.
        /// This is for consistency across the project.
        /// </summary>
        public static readonly char[] COLOUR_ORDER = { 'W', 'Y', 'R', 'O', 'B', 'G' };

        /// <summary>
        /// Get a colour in a rotation order for a specific side of the cube.
        /// </summary>
        /// <param name="sideColour">The side that the colours are rotated around</param>
        /// <param name="start">The colour to be rotated.</param>
        /// <param name="offset">How far it should be rotated. Positive = Clockwise, Negative = Counter Clockwise.</param>
        /// <returns>The start colour after it has been rotated.</returns>
        public static char RotateColour(char sideColour, char start, int offset)
        {
            if (Math.Abs(offset) > ROTATION_ORDER_LENGTH)
            {
                throw new Exception("Offset must be less than the ROTATION_ORDER_LENGTH");
            }

            char[] rotationOrder;

            switch (sideColour)
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

            if (!"WRB".Contains(sideColour))
            {
                offset *= -1;
            }

            int index = rotationOrder.GetIndex(start);

            if (index == -1)
            {
                throw new Exception("The Rotation Order did not have the starting colour");
            }

            index += offset;

            //make sure that the offset is within the bounds of the array
            if (index < 0)
            {
                index += ROTATION_ORDER_LENGTH;
            }
            else
            {
                index %= ROTATION_ORDER_LENGTH;
            }

            return rotationOrder[index];
        }
    }
}
