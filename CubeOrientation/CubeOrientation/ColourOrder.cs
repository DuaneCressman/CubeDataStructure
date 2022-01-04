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
        /// These are the "directions" that can be used to specify a side. For these
        /// directions to be used, the orientation of the cube must be known. The front
        /// and top side are given.
        /// </summary>
        public static readonly char[] DIRECTIONS = { 'f', 'b', 'r', 'l', 'u', 'd' };

        /// <summary>
        /// The notation for slice moves.
        /// </summary>
        public static readonly char[] SLICES = { 'm', 'e', 's' };

        /// <summary>
        /// The notation for rotating the entire cube.
        /// </summary>
        public static readonly char[] WHOLE_CUBE_ROTATIONS = { 'x', 'y', 'z' };

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

        /// <summary>
        /// Get the side colour based on a direction.
        /// The orientation of the cube must be given.
        /// </summary>
        /// <param name="front">The colour of the side at the front of the cube.</param>
        /// <param name="top">The colour of the side at the top of the cube.</param>
        /// <param name="direction">The direction of the side you want. See <see cref="DIRECTIONS"/> for valid directions.</param>
        /// <returns>The colour of the side on the direction passed in.</returns>
        public static char GetSideFromDirection(char front, char top, char direction)
        {
            return direction switch
            {
                'f' => front,
                'b' => GetOppositeColour(front),
                'r' => RotateColour(front, top, 1),
                'l' => RotateColour(front, top, -1),
                'u' => top,
                'd' => RotateColour(front, top, 2),
                _ => throw new Exception($"{direction} is not a valid direction"),
            };
        }

        /// <summary>
        /// Get the colour on the opposite side of the cube of the side passed in.
        /// </summary>
        /// <param name="colour">The colour you want the opposite colour of.</param>
        /// <returns>The opposite colour of the colour passes in.</returns>
        public static char GetOppositeColour(char colour)
        {
            int index = COLOUR_ORDER.GetIndex(colour);

            if(index == -1)
            {
                throw new Exception($"{colour} is invalid");
            }

            int offset = index % 2 == 0 ? 1 : -1;

            return COLOUR_ORDER[index + offset];
        }

        /// <summary>
        /// Returns is a side colour is valid.
        /// Valid colours are defined as colours within the <see cref="COLOUR_ORDER"/>
        /// </summary>
        public static bool IsValidSideColour(params char[] colours)
        {
            foreach(char c in colours)
            {
                if(COLOUR_ORDER.GetIndex(c) == -1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
