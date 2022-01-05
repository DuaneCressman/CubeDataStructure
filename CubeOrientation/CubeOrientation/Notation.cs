using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using CubeOrientation.Exceptions;

namespace CubeOrientation
{
    public static class Notation
    {
        public const string VALID_ABSTRACT_LETTERS = "udlrfbmsexyz";
        public const string VALID_LITERAL_LETTERS = "wyrgbg";

        public const char PRIME_NOTATION = '\'';
        public const char HALF_TURN_NOTATION = '2';

        public enum FaceColours
        {
            /// <summary>
            /// White
            /// </summary>
            W,

            /// <summary>
            /// Yellow
            /// </summary>
            Y,

            /// <summary>
            /// Red
            /// </summary>
            R,

            /// <summary>
            /// Orange
            /// </summary>
            O,

            /// <summary>
            /// Blue
            /// </summary>
            B,

            /// <summary>
            /// Green
            /// </summary>
            G
        }

        public enum AbstractMoveNotation
        {
            /// <summary>
            /// Up
            /// </summary>
            u,

            /// <summary>
            /// Down
            /// </summary>
            d,

            /// <summary>
            /// Left
            /// </summary>
            l,

            /// <summary>
            /// Right
            /// </summary>
            r,

            /// <summary>
            /// Front
            /// </summary>
            f,

            /// <summary>
            /// Back
            /// </summary>
            b,

            /// <summary>
            /// Middle Slice with l
            /// </summary>
            m,

            /// <summary>
            /// Equatorial Slice with d
            /// </summary>
            e,

            /// <summary>
            /// Standing Slice with f
            /// </summary>
            s,

            /// <summary>
            /// Rotation on Right
            /// </summary>
            x,

            /// <summary>
            /// Rotation on Up
            /// </summary>
            y,

            /// <summary>
            /// Rotation on Front
            /// </summary>
            z
        }

        public const int ABSTRACT_LETTER_COUNT = 12;

        public static readonly Dictionary<char, FaceColours> FacesByChar = new Dictionary<char, FaceColours>()
        {
            {'w', FaceColours.W },
            {'y', FaceColours.Y },
            {'r', FaceColours.R },
            {'o', FaceColours.O },
            {'b', FaceColours.B },
            {'g', FaceColours.G }
        };

        public readonly static Dictionary<char, AbstractMoveNotation> MovesByChar = new Dictionary<char, AbstractMoveNotation>()
        {
            { 'u', AbstractMoveNotation.u },
            { 'd', AbstractMoveNotation.d },
            { 'l', AbstractMoveNotation.l },
            { 'r', AbstractMoveNotation.r },
            { 'f', AbstractMoveNotation.f },
            { 'b', AbstractMoveNotation.b },
            { 'm', AbstractMoveNotation.m },
            { 'e', AbstractMoveNotation.e },
            { 's', AbstractMoveNotation.s },
            { 'x', AbstractMoveNotation.x },
            { 'y', AbstractMoveNotation.y },
            { 'z', AbstractMoveNotation.z }
        };

        public static bool ValidAbstractMove(char move)
        {
            return ValidAbstractMove(move.ToString().ToLower());
        }

        public static bool ValidAbstractMove(string move)
        {
            return Regex.Match(move.Trim().Replace(" ", string.Empty).ToLower(), $"^[{VALID_ABSTRACT_LETTERS + PRIME_NOTATION + HALF_TURN_NOTATION}]$").Success;
        }

        public static bool ValidateLiteralMove(char move)
        {
            return ValidateLiteralMove(move.ToString().ToLower());
        }

        public static bool ValidateLiteralMove(string move)
        {
            return Regex.Match(move.Trim().Replace(" ", string.Empty).ToLower(), $"^[{VALID_LITERAL_LETTERS + PRIME_NOTATION + HALF_TURN_NOTATION}]$").Success;
        }

        public static FaceColours[] ParseFaceColours(string notation)
        {
            return ParseFaceColours(notation.ToLower().ToCharArray());
        }

        public static FaceColours[] ParseFaceColours(params char[] notation)
        {
            if(!Regex.Match(notation.ToString().ToLower(), $"^[{VALID_LITERAL_LETTERS}]$").Success)
            {
                throw InvalidMoveNotationException.Build(notation.ToString(), Move.MoveClassifications.Literal);
            }

            FaceColours[] output = new FaceColours[notation.Length];

            for(int i = 0; i < notation.Length; i++)
            {
                output[i] = FacesByChar[notation[i]];
            }

            return output;
        }

        public static AbstractMoveNotation[] ParseAbstractNotation(string notation)
        {
            return ParseAbstractNotation(notation.ToLower().ToCharArray());
        }

        public static AbstractMoveNotation[] ParseAbstractNotation(params char[] notation)
        {
            if (!Regex.Match(notation.ToString().ToLower(), $"^[{VALID_ABSTRACT_LETTERS}]$").Success)
            {
                throw InvalidMoveNotationException.Build(notation.ToString(), Move.MoveClassifications.Abstract);
            }

            AbstractMoveNotation[] output = new AbstractMoveNotation[notation.Length];

            for(int i = 0; i < notation.Length; i++)
            {
                output[i] = MovesByChar[notation[i]];
            }

            return output;
        }
    }
}
