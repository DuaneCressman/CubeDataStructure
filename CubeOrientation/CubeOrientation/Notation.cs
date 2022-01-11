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
        public const string VALID_LITERAL_LETTERS = "wyrobg";

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
            move = move.CleanNotation();
            string rx = $"^[{VALID_ABSTRACT_LETTERS + PRIME_NOTATION + HALF_TURN_NOTATION}]+$";

            return Regex.Match(move, rx).Success;
        }

        public static bool ValidateLiteralMove(char move)
        {
            return ValidateLiteralMove(move.ToString().ToLower());
        }

        public static bool ValidateLiteralMove(string move)
        {
            return Regex.Match(CleanNotation(move), $"^[{VALID_LITERAL_LETTERS + PRIME_NOTATION + HALF_TURN_NOTATION}]+$").Success;
        }

        public static FaceColours ParseFaceColours(char notation)
        {
            if (FacesByChar.TryGetValue(notation, out FaceColours face))
            {
                return face;
            }
            else
            {
                throw InvalidMoveNotationException.Build(notation, Move.MoveClassifications.Literal);
            }
        }

        public static FaceColours[] ParseFaceColours(string notation)
        {
            notation = notation.CleanNotation();

            if (!Regex.Match(notation, $"^[{VALID_LITERAL_LETTERS}]+$").Success)
            {
                throw InvalidMoveNotationException.Build(notation, Move.MoveClassifications.Literal);
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
            if (!Regex.Match(notation.CleanNotation(), $"^[{VALID_ABSTRACT_LETTERS}]+$").Success)
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

        public static string CleanNotation(this string s)
        {
            return s.Trim().Replace(" ", string.Empty).ToLower();
        }

        public static char ToChar(this FaceColours faceColour)
        {
            return faceColour.ToString()[0];
        }
    }
}
