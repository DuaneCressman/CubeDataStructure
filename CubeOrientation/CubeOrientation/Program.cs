using CubeOrientation.CubeStructure;
using System;
using System.Collections.Generic;

namespace CubeOrientation
{
    class Program
    {
        static void Main(string[] args)
        {
            TestRotateingMiddleSlices();

            TestGettingMiddleSlices();

            TestRotations();

            TestGettingAFullFace();
        }

        public static void TestRotateingMiddleSlices()
        {
            Cube cube = new Cube();

            cube.RotateMiddleSlice('x', true);
            cube.RotateMiddleSlice('x', true);
            cube.RotateMiddleSlice('y', true);
            cube.RotateMiddleSlice('y', true);
            cube.RotateMiddleSlice('z', true);
            cube.RotateMiddleSlice('z', true);

            Console.WriteLine(cube);
        }

        public static void TestGettingMiddleSlices()
        {
            Cube cube = new Cube();

            cube.RotateSideSlice('O', false);
            cube.RotateSideSlice('R', true);
            cube.RotateSideSlice('G', true);
            cube.RotateSideSlice('G', true);
            cube.RotateSideSlice('B', true);
            cube.RotateSideSlice('O', false);
            cube.RotateSideSlice('O', false);

            foreach (Segment segment in cube.Structure.GetSlice('y'))
            {
                Console.WriteLine(segment);
            }
        }

        public static void TestGettingAFullFace()
        {
            Cube cube = new Cube();

            cube.RotateSideSlice('O', false);
            cube.RotateSideSlice('R', true);
            cube.RotateSideSlice('G', true);
            cube.RotateSideSlice('G', true);
            cube.RotateSideSlice('B', true);
            cube.RotateSideSlice('O', false);
            cube.RotateSideSlice('O', false);

            char[] colours = "GRBOWY".ToCharArray();

            for (int i = 0; i < 6; i++)
            {
                char[,] faceColours = cube.GetFacesOnSide(colours[i], i < 4 ? 'Y' : 'R');

                for (int y = 2; y >= 0; y--)
                {
                    string s = string.Empty;

                    for (int x = 0; x < 3; x++)
                    {
                        s += faceColours[x, y];
                    }

                    Console.WriteLine(s);
                }

                Console.WriteLine();
            }
        }

        public static void TestRotations()
        {
            Cube cube;
            char[] slices = "WYROBG".ToCharArray();
            Random random = new Random();
            int moveCount = 1000;
            int testCount = 1000;
            Tuple<char, bool>[] moves;

            int correctSolves = 0;
            int incorrectSolves = 0;

            for (int j = 0; j < testCount; j++)
            {
                cube = new Cube();

                moves = new Tuple<char, bool>[moveCount];

                for (int i = 0; i < moveCount; i++)
                {
                    char slice = slices[random.Next(0, slices.Length)];
                    bool clockwise = random.Next(0, 2) == 0;

                    moves[i] = new Tuple<char, bool>(slice, clockwise);

                    cube.RotateSideSlice(slice, clockwise);
                }

                for (int i = moveCount - 1; i >= 0; i--)
                {
                    cube.RotateSideSlice(moves[i].Item1, !moves[i].Item2);
                }

                if (j % 500 == 0)
                {
                    Console.WriteLine($"{j} solves have been done");
                }

                if (cube.Solved)
                {
                    correctSolves++;
                }
                else
                {
                    incorrectSolves++;
                }
            }

            Console.WriteLine($"Correct solves: {correctSolves}\nIncorrect solves: {incorrectSolves}");

            Console.ReadLine();
        }
    }
}
