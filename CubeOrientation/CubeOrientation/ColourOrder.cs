using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CubeOrientation.Notation;

using L = CubeOrientation.Notation.FaceColours;
using A = CubeOrientation.Notation.AbstractMoveNotation;

namespace CubeOrientation
{
    /// <summary>
    /// This class defines the colour order of the cube. 
    /// This is used to determine which colours are beside each other.
    /// </summary>
    public static class ColourOrder
    {
        public const int ROTATION_ORDER_LENGTH = 4;
        public static readonly FaceColours[] WRotationOrder = { L.R, L.G, L.O, L.B };
        public static readonly FaceColours[] RRotationOrder = { L.W, L.B, L.Y, L.G };
        public static readonly FaceColours[] BRotationOrder = { L.W, L.O, L.Y, L.R };

        private static readonly Dictionary<FaceColours, FaceColours[]> rotationOrderByFace = new Dictionary<L, L[]>()
        {
            {L.W, WRotationOrder },
            {L.Y, WRotationOrder },
            {L.R, RRotationOrder },
            {L.O, RRotationOrder },
            {L.B, BRotationOrder },
            {L.G, BRotationOrder }
        };

        private static readonly List<FaceColours> inverseRotationFaces = new List<FaceColours>()
        {
            L.Y, L.O, L.G
        };

        /// <summary>
        /// The base colours are arbitrarily chosen colours. None of the are opposite colours on the cube.
        /// These colours are always used first when referencing colours in the cube. This is mostly done 
        /// for consistency across the project.
        /// </summary>
        public static readonly FaceColours[] BASE_COLOURS = { L.W, L.R, L.B };
        
        /// <summary>
        /// The secondary colours are the opposite colour to the <see cref="BASE_COLOURS"/> with the same index.
        /// These colours are always used second when referencing colours in the cube.
        /// </summary>
        public static readonly FaceColours[] SECONDARY_COLOURS = { L.Y, L.O, L.G };

        /// <summary>
        /// Any time more that one colour is referenced, it should always be done in this order.
        /// This is for consistency across the project.
        /// </summary>
        public static readonly FaceColours[] COLOUR_ORDER = { L.W, L.Y, L.R, L.O, L.B, L.G };

        /// <summary>
        /// These are the "directions" that can be used to specify a side. For these
        /// directions to be used, the orientation of the cube must be known. The front
        /// and top side are given.
        /// </summary>
        public static readonly AbstractMoveNotation[] DIRECTIONS = { A.f, A.b, A.r, A.l, A.u, A.d };

        /// <summary>
        /// Get a colour in a rotation order for a specific side of the cube.
        /// </summary>
        /// <param name="sideColour">The side that the colours are rotated around</param>
        /// <param name="start">The colour to be rotated.</param>
        /// <param name="offset">How far it should be rotated. Positive = Clockwise, Negative = Counter Clockwise.</param>
        /// <returns>The start colour after it has been rotated.</returns>
        public static FaceColours RotateColour(FaceColours sideColour, FaceColours start, int offset)
        {
            if (Math.Abs(offset) > ROTATION_ORDER_LENGTH)
            {
                throw new Exception("Offset must be less than the ROTATION_ORDER_LENGTH");
            }

            FaceColours[] rotationOrder = rotationOrderByFace[sideColour];

            if (inverseRotationFaces.Contains(sideColour))
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
        public static FaceColours GetSideFromDirection(CubeOrientation orientation, AbstractMoveNotation direction)
        {
            return direction switch
            {
                AbstractMoveNotation.f => orientation.Front,
                AbstractMoveNotation.b => GetOppositeColour(orientation.Front),
                AbstractMoveNotation.r => RotateColour(orientation.Front, orientation.Top, 1),
                AbstractMoveNotation.l => RotateColour(orientation.Front, orientation.Top, -1),
                AbstractMoveNotation.u => orientation.Top,
                AbstractMoveNotation.d => RotateColour(orientation.Front, orientation.Top, 2),
                _ => throw new Exception($"{direction} is not a valid direction"),
            };
        }

        /// <summary>
        /// Get the colour on the oposit side of the cube of the side passed in.
        /// </summary>
        /// <param name="colour">The colour you want the oposit colour of.</param>
        /// <returns>The oposit colour of the colour passes in.</returns>
        public static FaceColours GetOppositeColour(FaceColours colour)
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
        /// Checks if a colour is a <see cref="SECONDARY_COLOURS"/>
        /// </summary>
        public static bool IsSecondaryColour(FaceColours colour)
        {
            return SECONDARY_COLOURS.GetIndex(colour) != -1;
        }
    }
}
