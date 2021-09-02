using System;

namespace CubeOrientation
{
    class Program
    {
        static void Main(string[] args)
        {
            Cube cube;
            char[] slices = "WYROBG".ToCharArray();
            Random random = new Random();
            int moveCount = 1000;
            Tuple<char, bool>[] moves;

            int correctSolves = 0;
            int incorrectSolves = 0;

            for (int j = 0; j < 1000; j++)
            {
                cube = new Cube();

                moves = new Tuple<char, bool>[moveCount];

                for (int i = 0; i < moveCount; i++)
                {
                    char slice = slices[random.Next(0, slices.Length)];
                    bool clockwise = random.Next(0, 2) == 0;

                    moves[i] = new Tuple<char, bool>(slice, clockwise);

                    cube.RotateSlice(slice, clockwise);
                }

                for (int i = moveCount - 1; i >= 0; i--)
                {
                    cube.RotateSlice(moves[i].Item1, !moves[i].Item2);
                }


                if (cube.CubeIsSolved)
                {
                    correctSolves++;
                }
                else
                {
                    incorrectSolves++;
                }

                if (j % 500 == 0)
                {
                    Console.WriteLine($"{j} solves have been done");
                }
            }

            Console.WriteLine($"Correct solves: {correctSolves}\nIncorrect solves: {incorrectSolves}");

            Console.ReadLine();
        }
    }
}
