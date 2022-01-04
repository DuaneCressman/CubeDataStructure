using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CubeOrientation
{
    public class Move
    {
        public enum MoveClassifications
        {
            Abstract,
            Literal
        }

        private const string VALID_ABSTRACT_LETTERS = "udlrfbmsexyz2\'";
        private const string VALID_LITERAL_LETTERS = "wyrgbg2\'";

        public const char PRIME_NOTATION = '\'';
        public const char HALF_TURN_NOTATION = '2';

        public bool Prime { get; protected set; }
        public bool HalfTurn { get; protected set; }

        public Move(bool prime, bool halfTurn)
        {
            Prime = prime;
            HalfTurn = halfTurn;
        }

        #region Static Validation

        public static bool ValidAbstractMove(char move)
        {
            return ValidAbstractMove(move.ToString());
        }

        public static bool ValidAbstractMove(string move)
        {
            return Regex.Match(move.Trim().Replace(" ", string.Empty), $"^[{VALID_ABSTRACT_LETTERS}]$").Success;
        }

        public static bool ValidateLiteralMove(char move)
        {
            return ValidateLiteralMove(move.ToString());
        }

        public static bool ValidateLiteralMove(string move)
        {
            return Regex.Match(move.Trim().Replace(" ", string.Empty), $"^[{VALID_LITERAL_LETTERS}]$").Success;
        }

        #endregion
    }
}
