using CubeOrientation.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace CubeOrientation
{
    public class Cube
    {
        private static readonly char[] WRotationOrder = { 'R', 'G', 'O', 'B' };
        private static readonly char[] RRotationOrder = { 'W', 'B', 'Y', 'G' };
        private static readonly char[] BRotationOrder = { 'W', 'O', 'Y', 'R' };
        private const int ROTATION_ORDER_LENGTH = 4;
        public const int CUBE_SIZE = 3;

        public enum Directions
        {
            Up = 0,
            Right = 1,
            Down = 2,
            Left = 3
        }


        public LocationTree locationTree { get; private set; }

        public bool CubeSolved
        {
            get
            {
                LocationTree.SegmentDelegate segmentIsSolved = (seg) => { return seg.isCorrect; };

                int unsolvedSegments = locationTree.CheckSegmentsReturnValue(segmentIsSolved, false);

                return unsolvedSegments == 0;
            }
        }


        public Cube()
        {
            locationTree = new LocationTree();
            BuildCube();
        }

        /// <summary>
        /// Creates all the segments that make up the cube. The cube will be in the 
        /// solved position.
        /// </summary>
        public void BuildCube()
        {
            char[] ROBG = { 'R', 'O', 'B', 'G' };

            for (int i = 0; i < 2; i++)
            {
                char WY = i == 0 ? 'W' : 'Y';

                //add the edges with 'W' and 'Y'
                foreach (char c in ROBG)
                {
                    locationTree.SetSegment(new Segment(WY, c), WY, c);
                }

                //add the corners
                for (int j = 0; j < 2; j++)
                {
                    char RO = j == 0 ? 'R' : 'O';

                    locationTree.SetSegment(new Segment(WY, RO, 'B'), WY, RO, 'B');
                    locationTree.SetSegment(new Segment(WY, RO, 'G'), WY, RO, 'G');
                }
            }

            //add the ROBG edges
            for (int i = 0; i < 2; i++)
            {
                char RO = i == 0 ? 'R' : 'O';

                locationTree.SetSegment(new Segment(RO, 'B'), RO, 'B');
                locationTree.SetSegment(new Segment(RO, 'G'), RO, 'G');
            }
        }

        /// <summary>
        /// Rotate a side on the cube.
        /// </summary>
        /// <param name="sideColour">The colour of the side that is rotating</param>
        /// <param name="clockwise">True: clockwise. False: counter clockwise</param>
        public void RotateSlice(char sideColour, bool clockwise)
        {
            List<Segment> segments = locationTree.GetSegmentsByColour(sideColour);

            foreach (Segment segment in segments)
            {
                segment.Rotate(sideColour, clockwise);
                locationTree.SetSegment(segment, segment.location);
            }
        }

        /// <summary>
        /// Get the colour of single face on the cube.
        /// The <param name="colourRefrence"> is used to find the segment on the cube.
        /// The first element in the <param name="colourRefrence"> determines which side of 
        /// segment is used.
        /// </summary>
        public char GetFaceColour(char[] colourRefrence)
        {
            char face = colourRefrence[0];

            Segment segment = locationTree.GetSegment(colourRefrence);

            int index = segment.location.GetIndex(face);

            return segment.colours[index];
        }

        /// <summary>
        /// Get a colour in a rotation order for a specific side of the cube.
        /// </summary>
        /// <param name="sideColour">The side that the colours are rotated around</param>
        /// <param name="start">The colour to be rotated.</param>
        /// <param name="offset">How far it should be rotated. Positive = Clockwise, Negative = Counter Clockwise.</param>
        /// <returns>The start colour after it has been rotated.</returns>
        public static char RotateColour(char sideColour, char start, int offset)
        {
            if (Math.Abs(offset) > ROTATION_ORDER_LENGTH)
            {
                throw new Exception("Offset must be less than the ROTATION_ORDER_LENGTH");
            }

            char[] rotationOrder;

            switch (sideColour)
            {
                case 'W':
                case 'Y':
                    rotationOrder = WRotationOrder;
                    break;

                case 'R':
                case 'O':
                    rotationOrder = RRotationOrder;
                    break;

                case 'B':
                case 'G':
                    rotationOrder = BRotationOrder;
                    break;

                default:
                    throw new Exception("The side rotating was invalid");
            }

            if (!"WRB".Contains(sideColour))
            {
                offset *= -1;
            }

            int index = rotationOrder.GetIndex(start);

            if (index == -1)
            {
                throw new Exception("The Rotation Order did not have the starting colour");
            }

            index += offset;

            //make sure that the offset is within the bounds of the array
            if (index < 0)
            {
                index += ROTATION_ORDER_LENGTH;
            }
            else
            {
                index %= ROTATION_ORDER_LENGTH;
            }

            return rotationOrder[index];
        }

        /// <summary>
        /// Get the all the colours on one side of cube.
        /// </summary>
        /// <param name="sideColour">The side of the cube to get the colours on.</param>
        /// <param name="topColour">The colour at the top of the cube.</param>
        /// <returns>All the colours on the face. (0,0) is bottom left, (2, 2) is top right.</returns>
        public char[][] GetSideColours(char sideColour, char topColour)
        {
            //get the colours that will be used to define the colour references
            char[] adjacentColours = new char[ROTATION_ORDER_LENGTH];

            for (int i = 0; i < ROTATION_ORDER_LENGTH; i++)
            {
                adjacentColours[i] = RotateColour(sideColour, topColour, i);
            }

            char[][] output = new char[3][]
            {
                new char[3],
                new char[3],
                new char[3]
            };

            for (int y = 0; y < CUBE_SIZE; y++)
            {
                for (int x = 0; x < CUBE_SIZE; x++)
                {
                    //determine the colours that are needed to reference this side.

                    int xOffset = x - 1;
                    int yOffset = y - 1;

                    if (xOffset == 0 && yOffset == 0)
                    {
                        //we are on the center face
                        output[x][y] = sideColour;
                        continue;
                    }

                    char[] colourReference = new char[3] { sideColour, '\0', '\0' }; //colour Reference

                    if (xOffset == 1)
                    {
                        colourReference[1] = adjacentColours[(int)Directions.Right];
                    }
                    else if (xOffset == -1)
                    {
                        colourReference[1] = adjacentColours[(int)Directions.Left];
                    }

                    if (yOffset == 1)
                    {
                        colourReference[2] = adjacentColours[(int)Directions.Up];
                    }
                    else if (yOffset == -1)
                    {
                        colourReference[2] = adjacentColours[(int)Directions.Down];
                    }

                    output[x][y] = GetFaceColour(colourReference);
                }
            }

            return output;
        }
    }
}