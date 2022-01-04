using System;
using System.Collections.Generic;
using System.Text;
using CubeOrientation.Exceptions;

namespace CubeOrientation.CubeStructure
{
    public static class MoveFactory
    {
        public static AbstractMove BuildAbstractMove(char notation, bool prime = false, bool halfTurn = false)
        {
            if(!Move.ValidAbstractMove(notation))
            {
                throw InvalidMoveNotationException.Build(notation, Move.MoveClassifications.Abstract);
            }

            return new AbstractMove(notation, prime, halfTurn);
        }

        public static List<AbstractMove> BuildAbstractMoves(string notation)
        {
            if (!Move.ValidAbstractMove(notation))
            {
                throw InvalidMoveNotationException.Build(notation, Move.MoveClassifications.Abstract);
            }

            List<AbstractMove> moves = new List<AbstractMove>();

            notation = notation.Trim().Replace(" ", string.Empty);

            for(int i = 0; i < notation.Length; i++)
            {
                char note = notation[i];
                bool prime = false, halfTurn = false;

                if(i < notation.Length - 2)
                {
                    if (notation[i + 1] == Move.PRIME_NOTATION)
                    {
                        prime = true;
                        i++;
                    }
                    else if(notation[i + 1] == Move.HALF_TURN_NOTATION)
                    {
                        halfTurn = true;
                        i++;
                    }
                }

                moves.Add(new AbstractMove(note, prime, halfTurn));
            }

            return moves;
        }


    }
}
