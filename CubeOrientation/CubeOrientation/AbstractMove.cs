using System;
using System.Collections.Generic;
using System.Text;
using CubeOrientation.Exceptions;
using static CubeOrientation.Notation;

namespace CubeOrientation
{
    /// <summary>
    /// An abstract move is a move on the cube that is based on direction.
    /// There moves mean nothing unless the orientation of the cube is known.
    /// </summary>
    public sealed class AbstractMove : Move
    {
        public enum MoveTypes
        {
            /// <summary>
            /// Rotate the entire cube
            /// </summary>
            WholeCubeRotation,

            /// <summary>
            /// Rotate one of the middle layers of the cube
            /// </summary>
            Slice,

            /// <summary>
            /// Rotate one of the side layers of the cube
            /// </summary>
            SideLayer
        }

        public MoveTypes MoveType => MoveType;
        private readonly MoveTypes moveType;

        public AbstractMoveNotation Move => move;
        private readonly AbstractMoveNotation move;

        public AbstractMove Reversed => new AbstractMove(move, FlipModifierPrime(Modifier));

        public AbstractMove(char notation, Modifiers modifier) : base(modifier)
        {
            if(!MovesByChar.ContainsKey(notation))
            {
                throw InvalidMoveNotationException.Build(notation, MoveClassifications.Abstract);
            }

            move = MovesByChar[notation];
            moveType = GetMoveType(move);
        }

        public AbstractMove(AbstractMoveNotation move, Modifiers modifier) : base(modifier)
        {
            this.move = move;
            moveType = GetMoveType(move);
        }

        /// <summary>
        /// Get the <see cref="MoveTypes"/> of a <see cref="AbstractMoveNotation"/>
        /// </summary>
        public static MoveTypes GetMoveType(AbstractMoveNotation move)
        {
            if (ColourOrder.ABSTRACT_DIRECTIONS.GetIndex(move) != -1)
            {
                return MoveTypes.SideLayer;
            }
            else if (ColourOrder.SLICES.GetIndex(move) != -1)
            {
                return MoveTypes.Slice;
            }
            else if (ColourOrder.WHOLE_CUBE_ROTATIONS.GetIndex(move) != -1)
            {
                return MoveTypes.WholeCubeRotation;
            }
            else
            {
                throw new Exception($"Abstract notation does not have a move type: {move}");
            }
        }
    }
}
