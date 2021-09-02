using CubeOrientation.Tree;
using System;
using System.Collections.Generic;

namespace CubeOrientation
{
    class Program
    {
        static void Main(string[] args)
        {
            TestGettingAFullFace();
        }

        public static void TestGettingAFullFace()
        {
            Cube cube = new Cube();

            cube.RotateSlice('O', false);
            cube.RotateSlice('R', true);
            cube.RotateSlice('G', true);
            cube.RotateSlice('G', true);
            cube.RotateSlice('B', true);
            cube.RotateSlice('O', false);
            cube.RotateSlice('O', false);

            char[] colours = "GRBOWY".ToCharArray();

            for (int i = 0; i < 6; i++)
            {
                char[][] faceColours = cube.GetSideColours(colours[i], i < 4 ? 'Y' : 'R');

                for (int y = 2; y >= 0; y--)
                {
                    string s = string.Empty;

                    for (int x = 0; x < 3; x++)
                    {
                        s += faceColours[x][y];
                    }

                    Console.WriteLine(s);
                }

                Console.WriteLine();
            }
        }

        public static void TestGettingFaces()
        {
            Cube cube = new Cube();

            cube.RotateSlice('R', true);
            cube.RotateSlice('B', true);
            cube.RotateSlice('Y', false);
            cube.RotateSlice('G', false);

            List<Segment> segments = cube.locationTree.GetSegmentsByColour('R');

            segments.ForEach(x => Console.WriteLine(x));

            string[] redFaceColours = new string[9]
            {
                "RWG",
                "RW",
                "RWB",
                "RG",
                "R",
                "RB",
                "RGY",
                "RY",
                "RYB"
            };

            string output = string.Empty;

            for (int i = 0; i < redFaceColours.Length; i++)
            {
                if (i % 3 == 0)
                {
                    Console.WriteLine(output);
                    output = string.Empty;
                }

                if (redFaceColours[i].Length == 1)
                {
                    output += redFaceColours[i];
                }
                else
                {
                    output += cube.GetFaceColour(redFaceColours[i].ToCharArray());
                }

            }

            Console.WriteLine(output);
        }

        public static void TestRotations()
        {
            Cube cube;
            char[] slices = "WYROBG".ToCharArray();
            Random random = new Random();
            int moveCount = 100;
            int testCount = 100;
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

                    cube.RotateSlice(slice, clockwise);
                }

                for (int i = moveCount - 1; i >= 0; i--)
                {
                    cube.RotateSlice(moves[i].Item1, !moves[i].Item2);
                }

                if (j % 500 == 0)
                {
                    Console.WriteLine($"{j} solves have been done");
                }

                if (cube.CubeSolved)
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
