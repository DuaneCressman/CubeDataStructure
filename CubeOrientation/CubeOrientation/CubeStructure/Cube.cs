using System;
using System.Collections.Generic;
using static CubeOrientation.ColourOrder;

namespace CubeOrientation.CubeStructure
{

    /// <summary>
    /// This class has functionality to hold, rotates, and get face colours for 
    /// a cube. The actual data is stored in a <see cref="CubeStructure"/>.
    /// </summary>
    public class Cube
    {
        public const int SIZE = 3;

        public enum Directions
        {
            Up = 0,
            Right = 1,
            Down = 2,
            Left = 3
        }

        public static readonly char[] PLANES = { 'x', 'y', 'z' };

        /// <summary>The data structure that holds all the segments.</summary>
        private CubeStructure structure;

        /// <summary>The data structure that holds all the segments.</summary>
        public CubeStructure Structure => structure;

        /// <summary>If the cube is in a solved state.</summary>
        public bool Solved
        {
            get
            {
                CubeStructure.StructureDelegate segmentIsSolved = (seg) => { return seg.Solved; };

                int unsolvedSegments = structure.CheckSegmentReturnValue(segmentIsSolved, false);

                return unsolvedSegments == 0;
            }
        }

        #region Initialization

        public Cube()
        {
            BuildCube();
        }

        /// <summary>
        /// Creates all the segments that make up the cube. The segments are stored in a <see cref="CubeStructure"/>. 
        /// The cube will be in the solved position.
        /// </summary>
        public void BuildCube()
        {
            structure = new CubeStructure();

            foreach (char[] pathName in CubeStructure.GetAllPathNames())
            {
                structure.SetSegment(new Segment(pathName), pathName);
            }
        }

        #endregion

        #region Rotation

        public void RotateSlices(string input)
        {
            input = input.Replace(" ", string.Empty);

            for (int i = 0; i < input.Length; i++)
            {
                char slice = input[i];
                bool clockwise = true;

                if (i + 1 < input.Length && input[i + 1] == '\'')
                {
                    i++;
                    clockwise = false;
                }

                RotateSlice(slice, clockwise);
            }
        }

        public void RotateSlice(char slice, bool clockwise)
        {
            if (COLOUR_ORDER.GetIndex(slice) != -1)
            {
                RotateSideSlice(slice, clockwise);
                return;
            }

            if (PLANES.GetIndex(slice) != -1)
            {
                RotateMiddleSlice(slice, clockwise);
                return;
            }

            throw new Exception($"{slice} is not a valid slice");
        }

        /// <summary>
        /// Rotate a slice on the side of the cube the cube.
        /// </summary>
        /// <param name="slice">The colour of the side that is rotating</param>
        /// <param name="clockwise">True: clockwise. False: counter clockwise</param>
        private void RotateSideSlice(char slice, bool clockwise)
        {
            List<Segment> segments = structure.GetSlice(slice);

            foreach (Segment segment in segments)
            {
                segment.Rotate(slice, clockwise);
                structure.SetSegment(segment, segment.location);
            }
        }

        /// <summary>
        /// Rotate one of the middle slices in the cube.
        /// This is done by rotating the side slices in the same plane so that
        /// the cube is in the same orientation as if the middle slice was rotated.
        /// </summary>
        /// <param name="slice">The slice to rotate ('x', 'y', 'z')</param>
        /// <param name="clockwise">Direction to rotate views from the base colour slice in the plane.</param>
        private void RotateMiddleSlice(char slice, bool clockwise)
        {
            char a, b;

            switch (slice)
            {
                case 'x':
                    a = 'W';
                    b = 'Y';
                    break;

                case 'y':
                    a = 'R';
                    b = 'O';
                    break;

                case 'z':
                    a = 'B';
                    b = 'G';
                    break;

                default:
                    throw new Exception($"{slice} is not a valid middle slice");
            }

            RotateSideSlice(a, !clockwise);
            RotateSideSlice(b, clockwise);
        }

        #endregion

        #region Face Colours

        /// <summary>
        /// Get the colour of single face on the cube.
        /// The <param name="colourRefrence"> is used to find the segment on the cube.
        /// The first element in the <param name="colourRefrence"> determines which side of 
        /// segment is used.
        /// </summary>
        public char GetFaceColour(char[] colourRefrence)
        {
            char face = colourRefrence[0];

            Segment segment = structure.GetSegment(colourRefrence);

            int index = segment.location.GetIndex(face);

            return segment.colours[index];
        }

        /// <summary>
        /// Get the all the colours on one side of cube.
        /// </summary>
        /// <param name="sideColour">The side of the cube to get the colours on.</param>
        /// <param name="topColour">The colour at the top of the cube.</param>
        /// <returns>All the colours on the face. (0,0) is bottom left, (2, 2) is top right.</returns>
        public char[,] GetFacesOnSide(char sideColour, char topColour)
        {
            //get the colours that will be used to define the colour references
            char[] adjacentColours = new char[ROTATION_ORDER_LENGTH];

            for (int i = 0; i < ROTATION_ORDER_LENGTH; i++)
            {
                adjacentColours[i] = RotateColour(sideColour, topColour, i);
            }

            char[,] output = new char[3, 3];

            for (int y = 0; y < SIZE; y++)
            {
                for (int x = 0; x < SIZE; x++)
                {
                    //determine the colours that are needed to reference this side.

                    int xOffset = x - 1;
                    int yOffset = y - 1;

                    char[] colourReference = new char[3] { sideColour, '\0', '\0' };

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

                    output[x, y] = GetFaceColour(colourReference);
                }
            }

            return output;
        }

        #endregion

        public override string ToString()
        {
            string fourSpaces = "    ";

            string output = string.Empty;

            char[,] red = GetFacesOnSide('R', 'Y');

            for (int y = SIZE - 1; y >= 0; y--)
            {
                output += fourSpaces;

                for (int x = 0; x < SIZE; x++)
                {
                    output += red[x, y];
                }

                output += "\n";
            }

            output += "\n";

            List<char[,]> middleColours = new List<char[,]>()
            {
                GetFacesOnSide('B', 'R'),
                GetFacesOnSide('W', 'R'),
                GetFacesOnSide('G', 'R'),
                GetFacesOnSide('Y', 'R')
            };

            for (int y = SIZE - 1; y >= 0; y--)
            {
                for (int m = 0; m < 4; m++)
                {
                    for (int x = 0; x < SIZE; x++)
                    {
                        output += middleColours[m][x, y];
                    }

                    output += " ";
                }

                output += "\n";
            }

            output += "\n";


            char[,] orange = GetFacesOnSide('O', 'W');

            for (int y = SIZE - 1; y >= 0; y--)
            {
                output += fourSpaces;

                for (int x = 0; x < SIZE; x++)
                {
                    output += orange[x, y];
                }

                output += "\n";
            }

            return output;
        }
    }
}