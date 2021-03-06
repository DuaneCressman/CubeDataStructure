using Microsoft.VisualStudio.TestTools.UnitTesting;
using CubeOrientation.CubeStructure;
using System;
using System.Collections.Generic;
using System.Text;
using static CubeOrientation.Notation;

using FC = CubeOrientation.Notation.FaceColours;

namespace CubeOrientation.CubeStructure.Tests
{
    [TestClass]
    public class CubeTests
    {
        [TestMethod]
        public void AbstractMoves()
        {
            List<AbstractMove> moves = new List<AbstractMove>();

            foreach(char c in VALID_ABSTRACT_LETTERS)
            {
                moves.Add(new AbstractMove(c));
                moves.Add(new AbstractMove(c, Move.Modifiers.Prime));
                moves.Add(new AbstractMove(c, Move.Modifiers.HalfTurn));
            }

            Cube cube = new Cube();

            cube.MoveAbstract(moves.ToArray());

            Assert.IsTrue(true);
        }

        /// <summary>
        /// Test that the cube can be rotated many times without loosing track of any
        /// of the segments.
        /// </summary>
        [TestMethod]
        public void CubeTracksRotations()
        {
            int moveCount = 100;
            int testCount = 100;

            bool allTestsPassed = true;

            for (int t = 0; t < testCount; t++)
            {
                Cube cube = new Cube();

                AbstractMove[] moves = MoveFactory.GenerateRandomAbstractMoves(moveCount);

                cube.MoveAbstract(moves);

                for (int i = moveCount - 1; i >= 0; i--)
                {
                    cube.MoveAbstract(moves[i].Reversed);
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
        [TestMethod]
        public void FaceColoursAreCorrect()
        {
            Cube cube = new Cube();

            cube.MoveLiteral(MoveFactory.BuildLiteralMoves("R2 G' Y' W"));

            char[,] redFaces = cube.GetFacesOnSideToPrint(FC.R, FC.W);
            char[,] whiteFaces = cube.GetFacesOnSideToPrint(FC.W, FC.B);

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
        [TestMethod]
        public void GetIndividualFaces()
        {
            Cube cube = new Cube();

            LiteralMove[] moves = MoveFactory.BuildLiteralMoves("W B2 W' G Y' W");

            cube.MoveLiteral(moves);

            bool errorFound = false;

            List<(string seg, FaceColours colour)> answers = new List<(string seg, FC colour)>()
            {
                ("WRG", FC.W),
                ("OB", FC.R),
                ("RBY", FC.O),
                ("BW", FC.B)
            };

            foreach ((string seg, FaceColours colour) in answers)
            {
                if (cube.GetSegmentColourLiteral(ParseFaceColours(seg)) != colour)
                {
                    errorFound = true;
                    break;
                }
            }

            Assert.IsFalse(errorFound);
        }

        [TestMethod]
        public void GettingAllSegments()
        {
            Cube cube = new Cube();

            List<Segment> all = cube.Structure.GetSegments();

            Assert.IsTrue(all.Count == CubeStructure.TOTAL_SEGMENTS);
        }

        [TestMethod]
        public void GettingEdgeSegments()
        {
            Cube cube = new Cube();

            List<Segment> edges = cube.Structure.GetSegments(CubeStructure.SegmentSubSets.Edges);

            bool allCorrect = true;

            allCorrect = edges.Count == CubeStructure.TOTAL_EDGE_SEGMENTS;

            foreach(Segment segment in edges)
            {
                if(segment.location.Length != 2)
                {
                    allCorrect = false;
                    break;
                }
            }

            Assert.IsTrue(allCorrect);
        }

        public void GettingCornersSegments()
        {
            Cube cube = new Cube();

            List<Segment> corners = cube.Structure.GetSegments(CubeStructure.SegmentSubSets.Corners);
            
            bool allCorrect = corners.Count == CubeStructure.TOTAL_CORNER_SEGMENTS;
            
            foreach (Segment segment in corners)
            {
                if (segment.location.Length != 3)
                {
                    allCorrect = false;
                    break;
                }
            }

            Assert.IsTrue(allCorrect);
        }

        [TestMethod]
        public void GettingSegmentsBySide()
        {
            Cube cube = new Cube();

            bool allCorrect = true;

            for (int i = 0; i < ColourOrder.COLOUR_ORDER.Length; i++)
            {
                List<Segment> segs = cube.Structure.GetSegments(ColourOrder.COLOUR_ORDER[i]);
                
                if(segs.Count != CubeStructure.SEGMENTS_PER_SIDE)
                {
                    allCorrect = false;
                    break;
                }

                foreach(Segment segment in segs)
                {
                    if(!segment.IsOnSide(ColourOrder.COLOUR_ORDER[i]))
                    {
                        allCorrect = false;
                        break;
                    }
                }
            }

            Assert.IsTrue(allCorrect);
        }

        [TestMethod]
        public void DirectionsAreCorrect()
        {
            bool allCorrect = true;

            FaceColours[] correctSides = ParseFaceColours("ROBGWY");

            CubeOrientation orientation = new CubeOrientation(FaceColours.R, FaceColours.W);

            //ABSTRACT_DIRECTIONS = F B R L U D

            for (int i = 0; i < ColourOrder.ABSTRACT_DIRECTIONS.Length; i++)
            {
                if(ColourOrder.GetSideFromDirection(orientation, ColourOrder.ABSTRACT_DIRECTIONS[i]) != correctSides[i])
                {
                    allCorrect = false;
                    break;
                }
            }

            Assert.IsTrue(allCorrect);
        }

        [TestMethod]
        public void GettingFacesByDirections()
        {
            Cube cube = new Cube();
            cube.SetCubeOrientation(FaceColours.R, FaceColours.W);

            cube.MoveAbstract(MoveFactory.BuildAbstractMoves("f u l' u b d' r r f'"));

            string[] faces = { "ubr", "fl", "dfr", "bd" };
            FaceColours[] correctColours = { FaceColours.R, FaceColours.O, FaceColours.R, FaceColours.G };

            bool allCorrect = true;
            
            for (int i = 0; i < faces.Length; i++)
            {
                if(cube.GetSegmentColourAbstract(ParseAbstractNotation(faces[i])) != correctColours[i])
                {
                    allCorrect = false;
                    break;
                }
            }

            Assert.IsTrue(allCorrect);
        }

        [TestMethod]
        public void RotateEntireCube()
        {
            FaceColours correctTop = FaceColours.G;
            FaceColours correctFront = FaceColours.O;

            Cube cube = new Cube();

            AbstractMove[] cubeRotations = new AbstractMove[]
            {
                new AbstractMove(AbstractMoveNotation.x, Move.Modifiers.None),
                new AbstractMove(AbstractMoveNotation.z, Move.Modifiers.None),
                new AbstractMove(AbstractMoveNotation.y, Move.Modifiers.Prime),
                new AbstractMove(AbstractMoveNotation.x, Move.Modifiers.Prime)
            };

            cube.MoveAbstract(cubeRotations);

            Assert.IsTrue(cube.Orientation.Front == correctFront && cube.Orientation.Top == correctTop);
        }
    }
}