using Microsoft.VisualStudio.TestTools.UnitTesting;
using CubeOrientation.CubeStructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace CubeOrientation.CubeStructure.Tests
{
    [TestClass()]
    public class CubeTests
    {
        /// <summary>
        /// Test that the cube can be rotated many times without loosing track of any
        /// of the segments.
        /// </summary>
        [TestMethod()]
        public void CubeTracksRotations()
        {
            int moveCount = 100;
            int testCount = 100;

            Random random = new Random();

            char[] slices = "WYROBGxyz".ToCharArray();

            bool allTestsPassed = true;

            for (int t = 0; t < testCount; t++)
            {
                Cube cube = new Cube();

                Tuple<char, bool>[] moves = new Tuple<char, bool>[moveCount];

                for (int i = 0; i < moveCount; i++)
                {
                    char slice = slices[random.Next(0, slices.Length)];
                    bool clockwise = random.Next(0, 2) == 1;

                    moves[i] = new Tuple<char, bool>(slice, clockwise);

                    cube.RotateSlice(slice, clockwise);
                }

                for (int i = moveCount - 1; i >= 0; i--)
                {
                    cube.RotateSlice(moves[i].Item1, !moves[i].Item2);
                }

                if (!cube.Solved)
                {
                    allTestsPassed = false;
                }
            }

            Assert.IsTrue(allTestsPassed);
        }

        /// <summary>
        /// Test that getting an entire face of colours after rotations is accurate.
        /// </summary>
        [TestMethod()]
        public void FaceColoursAreCorrect()
        {
            Cube cube = new Cube();

            cube.RotateSlices("R R G' Y' W");

            char[,] redFaces = cube.GetFacesOnSide('R', 'W');
            char[,] whiteFaces = cube.GetFacesOnSide('W', 'B');

            char[,] correctRedFaces = new char[3, 3]
            {
                {'G', 'Y', 'G' },
                {'B', 'R', 'B' },
                {'B', 'R', 'B' }
            };

            char[,] correctWhiteFaces = new char[3, 3]
            {
                {'R', 'R', 'R' },
                {'Y', 'W', 'W' },
                {'Y', 'W', 'W' }
            };

            bool errorFound = false;

            for (int i = 0; i < 2; i++)
            {
                char[,] correctFaces = i == 0 ? correctRedFaces : correctWhiteFaces;
                char[,] facesToCheck = i == 0 ? redFaces : whiteFaces;

                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        if (facesToCheck[x, y] != correctFaces[x, y])
                        {
                            errorFound = true;
                            break;
                        }
                    }

                    if (errorFound)
                    {
                        break;
                    }
                }

                if (errorFound)
                {
                    break;
                }
            }

            Assert.IsFalse(errorFound);
        }

        /// <summary>
        /// Test that after rotations, the correct individual faces can be found.
        /// </summary>
        [TestMethod()]
        public void GetIndividualFaces()
        {
            Cube cube = new Cube();

            cube.RotateSlices("W Y W G G B' O' R B");

            bool errorFound = false;

            Tuple<string, char>[] answers = new Tuple<string, char>[]
            {
                new Tuple<string, char>("BO", 'B'),
                new Tuple<string, char>("OB", 'O'),
                new Tuple<string, char>("GWR", 'W'),
                new Tuple<string, char>("OG", 'B'),
                new Tuple<string, char>("Y", 'Y'),
                new Tuple<string, char>("BYR", 'B'),
                new Tuple<string, char>("YBO", 'G'),
                new Tuple<string, char>("YR", 'G')
            };

            foreach (Tuple<string, char> pair in answers)
            {
                if (cube.GetFaceColour(pair.Item1.ToCharArray()) != pair.Item2)
                {
                    errorFound = true;
                    break;
                }
            }

            Assert.IsFalse(errorFound);
        }
    }
}