using System;
using System.Collections.Generic;
using System.Text;

namespace CubeOrientation.Exceptions
{
    class InvalidMoveNotationException : Exception
    {
        public InvalidMoveNotationException() : base()
        {
        }

        public InvalidMoveNotationException(string message) : base(message)
        {
        }

        public InvalidMoveNotationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public static InvalidMoveNotationException Build(char notation, Move.MoveClassifications classification)
        {
            return new InvalidMoveNotationException($"\"{notation}\" is not a valid character for a " +
                $"{classification} move");
        }

        public static InvalidMoveNotationException Build(string notation, Move.MoveClassifications classification)
        {
            return new InvalidMoveNotationException($"A character in {notation} is not valid for creating {classification} moves");
        }
    }
}
