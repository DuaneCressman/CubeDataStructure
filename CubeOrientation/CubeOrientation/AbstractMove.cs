using System;
using System.Collections.Generic;
using System.Text;
using CubeOrientation.Exceptions;
using CubeOrientation.Notation;

namespace CubeOrientation
{
    /// <summary>
    /// An abstract move is a move on the cube that is based on direction.
    /// There moves mean nothing unless the orientation of the cube is known.
    /// </summary>
    public class AbstractMove : Move
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

        private readonly static Dictionary<char, (AbstractMoveNotation move, MoveTypes type)> movesByChar = new Dictionary<char, (AbstractMoveNotation move, MoveTypes type)>()
        {
            { 'u', (AbstractMoveNotation.u, MoveTypes.SideLayer) },
            { 'd', (AbstractMoveNotation.d, MoveTypes.SideLayer) },
            { 'l', (AbstractMoveNotation.l, MoveTypes.SideLayer) },
            { 'r', (AbstractMoveNotation.r, MoveTypes.SideLayer) },
            { 'f', (AbstractMoveNotation.f, MoveTypes.SideLayer) },
            { 'b', (AbstractMoveNotation.b, MoveTypes.SideLayer) },
            { 'm', (AbstractMoveNotation.m, MoveTypes.Slice) },
            { 'e', (AbstractMoveNotation.e, MoveTypes.Slice) },
            { 's', (AbstractMoveNotation.s, MoveTypes.Slice) },
            { 'x', (AbstractMoveNotation.s, MoveTypes.WholeCubeRotation) },
            { 'y', (AbstractMoveNotation.s, MoveTypes.WholeCubeRotation) },
            { 'z', (AbstractMoveNotation.s, MoveTypes.WholeCubeRotation) }
        };

        public AbstractMove(char notation, bool prime, bool halfTurn) : base(prime, halfTurn)
        {
            if(!movesByChar.ContainsKey(notation))
            {
                throw InvalidMoveNotationException.Build(notation, MoveClassifications.Abstract);
            }

            (move, moveType) = movesByChar[notation];
        }
    }
}
