using System;
using System.Collections.Generic;
using System.Text;
using CubeOrientation.Exceptions;
using static CubeOrientation.Notation;

namespace CubeOrientation.CubeStructure
{
    public static class MoveFactory
    {
        public static LiteralMove[] BuildLiteralMoves(string notation)
        {
            if (!Notation.ValidateLiteralMove(notation))
            {
                throw InvalidMoveNotationException.Build(notation, Move.MoveClassifications.Literal);
            }

            List<(char note, Move.Modifiers modifier)> moves = ParseNotation(notation);

            LiteralMove[] output = new LiteralMove[moves.Count];

            for(int i = 0; i < moves.Count; i++)
            {
                output[i] = new LiteralMove(moves[i].note, moves[i].modifier);
            }

            return output;
        }

        public static AbstractMove[] BuildAbstractMoves(string notation)
        {
            if (!Notation.ValidAbstractMove(notation))
            {
                throw InvalidMoveNotationException.Build(notation, Move.MoveClassifications.Abstract);
            }

            List<(char note, Move.Modifiers modifier)> moves = ParseNotation(notation);
            AbstractMove[] output = new AbstractMove[moves.Count];

            for (int i = 0; i < moves.Count; i++)
            {
                output[i] = new AbstractMove(moves[i].note, moves[i].modifier);
            }

            return output;
        }

        private static List<(char notation, Move.Modifiers modifier)> ParseNotation(string notation)
        {
            List<(char notation, Move.Modifiers modifier)> output = new List<(char notation, Move.Modifiers modifier)>();

            for (int i = 0; i < notation.Length; i++)
            {
                char note = notation[i];
                Move.Modifiers modifier = Move.Modifiers.None;

                if (i < notation.Length - 2)
                {
                    if (notation[i + 1] == Notation.PRIME_NOTATION)
                    {
                        modifier = Move.Modifiers.Prime;
                        i++;
                    }
                    else if (notation[i + 1] == Notation.HALF_TURN_NOTATION)
                    {
                        modifier = Move.Modifiers.HalfTurn;
                        i++;
                    }
                }

                output.Add((note, modifier));
            }

            return output;
        }

        public static LiteralMove AbstractToLiteral(CubeOrientation orientation, AbstractMove abstractMove)
        {
            if(abstractMove.MoveType != AbstractMove.MoveTypes.SideLayer)
            {
                throw new Exception("Only Side Layer moves can be converted to literal moves");
            }

            return new LiteralMove(ColourOrder.GetSideFromDirection(orientation, abstractMove.Move), abstractMove.Modifier);
        }

        public static AbstractMove[] GenerateRandomAbstractMoves(int amount)
        {
            AbstractMove[] output = new AbstractMove[amount];

            Random random = new Random();

            for(int i = 0; i < amount; i++)
            {
                int move = random.Next(0, ABSTRACT_LETTER_COUNT);
                int modifier = random.Next(0, Move.MODIFIER_COUNT);

                output[i] = new AbstractMove((AbstractMoveNotation)move, (Move.Modifiers)modifier);
            }

            return output;
        }

    }
}
