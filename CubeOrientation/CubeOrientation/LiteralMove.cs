using System;
using System.Collections.Generic;
using System.Text;
using CubeOrientation.Exceptions;
using static CubeOrientation.Notation;


namespace CubeOrientation
{
    public sealed class LiteralMove : Move
    {
        public FaceColours Face { get; }

        public LiteralMove Reversed => new LiteralMove(Face, FlipModifierPrime(Modifier));

        public LiteralMove(char notation, Modifiers modifier) : base(modifier)
        {
            if(!FacesByChar.ContainsKey(notation))
            {
                throw InvalidMoveNotationException.Build(notation, MoveClassifications.Literal);
            }

            Face = FacesByChar[notation];
        }

        public LiteralMove(FaceColours face, Modifiers modifier) : base(modifier)
        {
            Face = face;
        }
    }
}
