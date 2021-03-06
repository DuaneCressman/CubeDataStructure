using System;

namespace CubeOrientation
{
    /// <summary>
    /// A single move that is made on the cube
    /// </summary>
    public abstract class Move
    {
        public enum MoveClassifications
        {
            Abstract,
            Literal
        }

        public enum Modifiers
        {
            /// <summary>
            /// A regular clockwise move.
            /// </summary>
            None,

            /// <summary>
            /// A counter Clockwise move.
            /// </summary>
            Prime,

            /// <summary>
            /// A 180 degree turn.
            /// </summary>
            HalfTurn
        }

        public const int MODIFIER_COUNT = 3;

        public Modifiers Modifier { get; }

        public Move(Modifiers modifier)
        {
            Modifier = modifier;
        }

        /// <summary>
        /// Flip the prime modifier.
        /// </summary>
        public static Modifiers FlipModifierPrime(Modifiers modifier) => modifier switch
        {
            Modifiers.None => Modifiers.Prime,
            Modifiers.Prime => Modifiers.None,
            Modifiers.HalfTurn => Modifiers.HalfTurn,
            _ => throw new Exception($"Invalid Modifier case: {modifier}")
        };
    }
}
