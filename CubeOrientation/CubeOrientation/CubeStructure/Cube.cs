using System;
using System.Collections.Generic;
using static CubeOrientation.ColourOrder;
using static CubeOrientation.CubeStructure.CubeStructure;

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

        //public static readonly Dictionary<char, string> SLICE_MOVES = new Dictionary<char, string>()
        //{
        //    {'M', "" }
        //} 

        /// <summary>The data structure that holds all the segments.</summary>
        private CubeStructure structure;

        /// <summary>The data structure that holds all the segments.</summary>
        public CubeStructure Structure => structure;

        public char TopColour { get; private set; } = 'W';

        public char FrontColour { get; private set; } = 'G';

        /// <summary>If the cube is in a solved state.</summary>
        public bool Solved
        {
            get
            {
                return structure.GetSegments((s) => !s.Solved).Count == 0;
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

            foreach (char[] pathName in GetAllPathNames())
            {
                structure.SetSegment(new Segment(pathName), pathName);
            }
        }

        #endregion

        #region Rotation

        /// <summary>
        /// Rotate multiple slices in order.
        /// The formate should be like "W R' B G W' W' O B Y". An ' is used to denote an counter clockwise rotation.
        /// </summary>
        public void MoveBySideColour(string input)
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

                Move(slice, clockwise);
            }
        }

        /// <summary>
        /// Rotate slices of the cube using <see cref="ColourOrder.DIRECTIONS"/>.
        /// </summary>
        /// <remarks>
        /// Only directions can be used in the input string. Side colours can not be mixed in.
        /// </remarks>
        /// <param name="input">The directions of sides to rotate.</param>
        public void Move(string input)
        {
            string sidesToRotate = string.Empty;

            char[] directions = input.Replace(" ", string.Empty).ToCharArray();

            for(int i = 0; i < directions.Length; i++)
            {
                if(directions[i] == '\'')
                {
                    sidesToRotate += '\'';
                    continue;
                }

                sidesToRotate += GetSideFromDirection(FrontColour, TopColour, directions[i]); 
            }

            MoveBySideColour(sidesToRotate);
        }

        /// <summary>
        /// Rotate all the segments on one side of the cube.
        /// </summary>
        /// <param name="layer">The layer of the cube to rotate.</param>
        /// <param name="clockwise">True -> Clockwise, False -> Counter Clockwise</param>
        public void Move(char layer, bool clockwise)
        {
            if (COLOUR_ORDER.GetIndex(layer) != -1)
            {
                RotateSideLayer(layer, clockwise);
                return;
            }

            if (SLICES.GetIndex(layer) != -1)
            {
                RotateSlice(layer, clockwise);
                return;
            }

            throw new Exception($"{layer} is not a valid slice");
        }

        /// <summary>
        /// Rotate a slice on the side of the cube .
        /// </summary>
        /// <param name="slice">The colour of the side that is rotating</param>
        /// <param name="clockwise">True: clockwise. False: counter clockwise</param>
        private void RotateSideLayer(char slice, bool clockwise)
        {
            foreach (Segment segment in structure.GetSlice(slice))
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
        /// <param name="slice">The slice to rotate (w, e, s)</param>
        /// <param name="prime">Direction to rotate views from the base colour slice in the plane.</param>
        private void RotateSlice(char slice, bool prime)
        {

            //THIS WILL CALL RotateSideLayer and RotateWholeCube with clockwise incorrectly


            if (slice == 'm')
            {
                RotateSideLayer(GetSideFromDirection(FrontColour, TopColour, 'l'), !prime);
                RotateSideLayer(GetSideFromDirection(FrontColour, TopColour, 'r'), prime);
                RotateWholeCube('x', !prime);
            }
            else if(slice == 'e')
            {
                RotateSideLayer(GetSideFromDirection(FrontColour, TopColour, 'u'), prime);
                RotateSideLayer(GetSideFromDirection(FrontColour, TopColour, 'd'), !prime);
                RotateWholeCube('y', !prime);
            }
            else if(slice == 's')
            {
                RotateSideLayer(GetSideFromDirection(FrontColour, TopColour, 'f'), !prime);
                RotateSideLayer(GetSideFromDirection(FrontColour, TopColour, 'b'), prime);
                RotateWholeCube('z', prime);
            }
        }

        public void RotateWholeCube(char reorientation, bool clockwise)
        {
            if(reorientation == 'x')
            {
                //rotate the entire cube on r

                char rightSide = GetSideFromDirection(FrontColour, TopColour, 'r');

                TopColour = RotateColour(rightSide, TopColour, clockwise ? -1 : 1);
                FrontColour = RotateColour(rightSide, FrontColour, clockwise ? -1 : 1);
            }
            else if(reorientation == 'y')
            {
                //rotate the entire cube on u
                FrontColour = RotateColour(TopColour, FrontColour, clockwise ? -1 : 1);
            }
            else if(reorientation == 'z')
            {
                //rotate the entire cube on F
                TopColour = RotateColour(FrontColour, TopColour, clockwise ? -1 : 1);
            }
            else
            {
                //error
            }
        }

        /// <summary>
        /// Manually set the orientation of the cube.
        /// </summary>
        /// <param name="frontColour"></param>
        /// <param name="topColour"></param>
        public void SetCubeOrientation(char frontColour, char topColour)
        {
            if(!IsValidSideColour(frontColour, topColour))
            {
                throw new Exception($"Either {frontColour} or {topColour} is not a valid side colour");
            }

            FrontColour = frontColour;
            TopColour = topColour;
        }

        #endregion

        #region Get Segments

        /// <summary>
        /// Get segments that have the specified colour.
        /// </summary>
        /// <param name="colour">The colour on the segment to look for.</param>
        /// <param name="subset">The type of segments to check.</param>
        public List<Segment> GetSegmentsByColour(char colour, SegmentSubSets subset = SegmentSubSets.All)
        {
            return structure.GetSegments((s) => { return s.HasColour(colour);}, subset);
        }

        /// <summary>
        /// Get segments that have the specified colour. Only check the segments on the specifed side.
        /// </summary>
        /// <param name="colour">The colour on the segment to look for.</param>
        /// <param name="side">The side of the cube to check</param>
        public List<Segment> GetSegmentsByColour(char colour, char side)
        {
            return structure.GetSegments((s) => { return s.HasColour(colour); }, side);
        }

        #endregion 

        #region Get Face Colours

        /// <summary>
        /// Get the colour of a face on a segment using <see cref="ColourOrder.DIRECTIONS"/>.
        /// </summary>
        /// <param name="colourRefrenceDirection">The colour refrence for the face using directions</param>
        /// <param name="front">The colour of the side at the front of the cube.</param>
        /// <param name="top">The colour of the side at the top of the cube.</param>
        /// <returns></returns>
        public char GetFaceColour(string colourRefrenceDirection, char front, char top)
        {
            return GetFaceColour(colourRefrenceDirection.ToCharArray(), front, top);
        }

        /// <summary>
        /// Get the colour of a face on a segment using <see cref="ColourOrder.DIRECTIONS"/>.
        /// </summary>
        /// <param name="colourRefrenceDirection">The colour refrence for the face using directions</param>
        /// <param name="front">The colour of the side at the front of the cube.</param>
        /// <param name="top">The colour of the side at the top of the cube.</param>
        /// <returns></returns>
        public char GetFaceColour(char[] colourRefrenceDirection, char front, char top)
        {
            char[] sides = new char[colourRefrenceDirection.Length];

            for(int i = 0; i < colourRefrenceDirection.Length; i++)
            {
                sides[i] = GetSideFromDirection(front, top, colourRefrenceDirection[i]);
            }

            return GetFaceColour(sides);
        }

        /// <summary>
        /// Get the colour of single face on the cube.
        /// The <param name="colourRefrence"> is used to find the segment on the cube.
        /// The first element in the <param name="colourRefrence"> determines which side of 
        /// segment is used.
        /// </summary>
        public char GetFaceColour(string colourRefrence)
        {
            return GetFaceColour(colourRefrence.ToCharArray());
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

            Segment segment = structure.GetSegment(colourRefrence);

            int index = segment.location.GetIndex(face);

            return segment.colours[index];
        }

        /// <summary>
        /// Get all the colours of the faces on a side of the cube.
        /// The order of the colours is random.
        /// </summary>
        /// <param name="side">The side of the cube to get the face colours of.</param>
        /// <returns>The colours of all the faces on the given side.</returns>
        public char[] GetFaceColoursOnSide(char side)
        {
            char[] output = new char[SEGMENTS_PER_SIDE];

            List<Segment> segmentsOnSide = structure.GetSegments(side);

            for(int i = 0; i < segmentsOnSide.Count; i++)
            {
                output[i] = segmentsOnSide[i].colours[segmentsOnSide[i].location.GetIndex(side)];
            }

            return output;
        }

        /// <summary>
        /// Get the all the colours on one side of cube.
        /// </summary>
        /// <remarks>
        /// This method should only be used for testing purposes. 
        /// </remarks>
        /// <param name="sideColour">The side of the cube to get the colours on.</param>
        /// <param name="topColour">The colour at the top of the cube.</param>
        /// <returns>All the colours on the face. (0,0) is bottom left, (2, 2) is top right.</returns>
        public char[,] GetFacesOnSideToPrint(char sideColour, char topColour)
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

            char[,] red = GetFacesOnSideToPrint('R', 'Y');

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
                GetFacesOnSideToPrint('B', 'R'),
                GetFacesOnSideToPrint('W', 'R'),
                GetFacesOnSideToPrint('G', 'R'),
                GetFacesOnSideToPrint('Y', 'R')
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


            char[,] orange = GetFacesOnSideToPrint('O', 'W');

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