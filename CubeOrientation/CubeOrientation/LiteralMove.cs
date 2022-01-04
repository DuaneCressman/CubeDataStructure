using System;
using System.Collections.Generic;
using System.Text;
using CubeOrientation.Exceptions;
using CubeOrientation.Notation;

namespace CubeOrientation
{
    public class LiteralMove : Move
    {
        public FaceColours Face => face;
        private readonly FaceColours face;

        private readonly Dictionary<char, FaceColours> facesByChar = new Dictionary<char, FaceColours>()
        {
            {'W', FaceColours.W },
            {'Y', FaceColours.Y },
            {'R', FaceColours.R },
            {'O', FaceColours.O },
            {'B', FaceColours.B },
            {'G', FaceColours.G }
        };

        public LiteralMove(char notation, bool prime, bool halfTurn) : base(prime, halfTurn)
        {
            if(!facesByChar.ContainsKey(notation))
            {
                throw InvalidMoveNotationException.Build(notation, MoveClassifications.Literal);
            }

            face = facesByChar[notation];
        }
    }
}
