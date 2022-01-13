using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CubeOrientation.Notation;

using L = CubeOrientation.Notation.FaceColours;
using AMN = CubeOrientation.Notation.AbstractMoveNotation;

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

        public static readonly AMN[] XRotationOrder = { AMN.u, AMN.b, AMN.d, AMN.f };
        public static readonly AMN[] YRotationOrder = { AMN.b, AMN.r, AMN.f, AMN.l };
        public static readonly AMN[] ZRotationOrder = { AMN.u, AMN.r, AMN.d, AMN.l };

        private static readonly Dictionary<AMN, AMN[]> rotationOrderByAxis = new Dictionary<AMN, AMN[]>()
        {
            {AMN.f, ZRotationOrder },
            {AMN.b, ZRotationOrder },
            {AMN.r, XRotationOrder },
            {AMN.l, XRotationOrder },
            {AMN.u, YRotationOrder },
            {AMN.d, YRotationOrder },
            {AMN.x, XRotationOrder },
            {AMN.y, YRotationOrder },
            {AMN.z, ZRotationOrder }
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
        public static readonly AbstractMoveNotation[] ABSTRACT_DIRECTIONS = { AMN.f, AMN.b, AMN.r, AMN.l, AMN.u, AMN.d };

        /// <summary>
        /// The notation for slice moves.
        /// </summary>
        public static readonly AbstractMoveNotation[] SLICES = { AMN.m, AMN.e, AMN.s };

        /// <summary>
        /// The notation for rotating the entire cube.
        /// </summary>
        public static readonly AbstractMoveNotation[] WHOLE_CUBE_ROTATIONS = { AMN.x, AMN.y, AMN.z };

        /// <summary>
        /// The directions that would be opposite the x, y, z rotations
        /// </summary>
        public static readonly AbstractMoveNotation[] SECONDARY_DIRECTIONS = { AMN.b, AMN.l, AMN.d };

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

            if (IsSecondaryColour(sideColour))
            {
                offset *= -1;
            }

            int index = rotationOrder.GetIndex(start);

            if (index == -1)
            {
                throw new Exception("The Rotation Order did not have the starting colour");
            }

            index = RotateIndex(index, offset);

            return rotationOrder[index];
        }

        /// <summary>
        /// Rotate a direction around an axis or other direction.
        /// </summary>
        /// <param name="axis">The axis to rotate around.</param>
        /// <param name="start">The starting direction to be rotated</param>
        /// <param name="offset">How far to rotate the direction</param>
        /// <returns>The starting direction after it has been rotated.</returns>
        /// <remarks>The <paramref name="axis"/> can be <see cref="ABSTRACT_DIRECTIONS"/> or <see cref="WHOLE_CUBE_ROTATIONS"/>
        /// The <paramref name="start"/> can ONLY be a <see cref="ABSTRACT_DIRECTIONS"/></remarks>
        public static AbstractMoveNotation RotateDirection(AbstractMoveNotation axis, AbstractMoveNotation start, int offset)
        {
            if(!rotationOrderByAxis.TryGetValue(axis, out AMN[] rotationOrder))
            {
                throw new Exception($"{axis} is not valid notation for rotating a direction");
            }

            int index = rotationOrder.GetIndex(start);

            if(index == -1)
            {
                throw new Exception($"{start} is not in the rotation order");
            }

            if(IsSecondaryDirection(axis))
            {
                offset *= -1;
            }

            index = RotateIndex(index, offset);

            return rotationOrder[index];
        }

        /// <summary>
        /// Move an index through an array wrapping at both ends.
        /// </summary>
        /// <param name="index">The index</param>
        /// <param name="offset">How far to move the index</param>
        /// <param name="rotationLength">How long the array is</param>
        /// <returns>The index with the offset wrapped</returns>
        private static int RotateIndex(int index, int offset, int rotationLength = ROTATION_ORDER_LENGTH)
        {
            index += offset;

            //make sure that the offset is within the bounds of the array
            if (index < 0)
            {
                index += rotationLength;
            }
            else
            {
                index %= rotationLength;
            }

            return index;
        }

        /// <summary>
        /// Get the side colour based on a direction.
        /// The orientation of the cube must be given.
        /// </summary>
        /// <param name="orientation">The orientation of the cube.</param>
        /// <param name="direction">The direction of the side you want. See <see cref="ABSTRACT_DIRECTIONS"/> for valid directions.</param>
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
        /// Get the colour on the opposite side of the cube of the side passed in.
        /// </summary>
        /// <param name="colour">The colour you want the opposite colour of.</param>
        /// <returns>The opposite colour of the colour passes in.</returns>
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

        /// <summary>
        /// Checks if a direction is a <see cref="SECONDARY_DIRECTIONS"/>
        /// </summary>
        public static bool IsSecondaryDirection(AbstractMoveNotation direction)
        {
            return SECONDARY_DIRECTIONS.GetIndex(direction) != -1;
        }

        /// <summary>
        /// Get how a colour should be rotated based on the <see cref="LiteralMove"/>
        /// </summary>
        /// <param name="move">The move being made</param>
        /// <returns>How many times the colour should be rotated and the direction.</returns>
        public static int GetRotationOffset(LiteralMove move)
        {
            if(move.Modifier == Move.Modifiers.HalfTurn)
            {
                return 2;
            }

            return move.Modifier == Move.Modifiers.Prime ? -1 : 1;
        }
    }
}
