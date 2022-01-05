using System;
using System.Collections.Generic;
using static CubeOrientation.ColourOrder;
using static CubeOrientation.CubeStructure.CubeStructure;
using static CubeOrientation.Notation;

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

        /// <summary>The data structure that holds all the segments.</summary>
        private CubeStructure structure;

        /// <summary>The data structure that holds all the segments.</summary>
        public CubeStructure Structure => structure;

        public CubeOrientation Orientation => orientation;
        private CubeOrientation orientation = new CubeOrientation(FaceColours.G, FaceColours.W);


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

            foreach (FaceColours[] pathName in GetAllPathNames())
            {
                structure.SetSegment(new Segment(pathName), pathName);
            }
        }

        #endregion

        #region Rotation

        /// <summary>
        /// Execute multiple <see cref="AbstractMove"/>
        /// </summary>
        public void MoveAbstract(params AbstractMove[] moves)
        {
            foreach(AbstractMove move in moves)
            {
                MoveAbstract(move);
            }
        }

        /// <summary>
        /// Execute a single <see cref="AbstractMove"/>
        /// </summary>
        public void MoveAbstract(AbstractMove move)
        {
            switch (move.MoveType)
            {
                case AbstractMove.MoveTypes.SideLayer:
                    MoveAbstractSide(move);
                    break;

                case AbstractMove.MoveTypes.WholeCubeRotation:
                    MoveAbstractWholeCube(move);
                    break;

                case AbstractMove.MoveTypes.Slice:
                    MoveAbstractSlice(move);
                    break;
            }
        }

        /// <summary>
        /// Execute a abstract move with <see cref="AbstractMove.MoveTypes.SideLayer"/>
        /// </summary>
        private void MoveAbstractSide(AbstractMove move)
        {
            MoveLiteral(MoveFactory.AbstractToLiteral(orientation, move));
        }

        /// <summary>
        /// Execute an <see cref="AbstractMove"/> with <see cref="AbstractMove.MoveTypes.WholeCubeRotation"/>.
        /// </summary>
        /// <remarks>This is done by changing the <see cref="orientation"/> of the cube.</remarks>
        private void MoveAbstractWholeCube(AbstractMove move)
        {
            switch (move.Move)
            {
                case AbstractMoveNotation.x:
                {
                    //rotate the entire cube on r
                    FaceColours rightSide = GetSideFromDirection(orientation, move.Move);

                    LiteralMove literal = new LiteralMove(rightSide, move.Modifier);
                    int rotationOffset = ColourOrder.GetRotationOffset(literal) * -1;

                    orientation.Top = RotateColour(rightSide, orientation.Top, rotationOffset);
                    orientation.Front = RotateColour(rightSide, orientation.Front, rotationOffset);
                    break;
                }
     
                case AbstractMoveNotation.y:
                {
                    LiteralMove literal = new LiteralMove(orientation.Top, move.Modifier);
                    int rotationOffset = ColourOrder.GetRotationOffset(literal) * -1;

                    //rotate the entire cube on u
                    orientation.Front = RotateColour(orientation.Top, orientation.Front, rotationOffset);
                    break;
                }

                case AbstractMoveNotation.z:
                {
                    LiteralMove literal = new LiteralMove(orientation.Front, move.Modifier);
                    int rotationOffset = ColourOrder.GetRotationOffset(literal) * -1;

                    //rotate the entire cube on F
                    orientation.Top = RotateColour(orientation.Top, orientation.Front, rotationOffset);
                    break;
                }

                default:
                    throw new Exception($"\"{move.Move}\" is not a valid notation for rotating the entire cube.");
            }
        }

        /// <summary>
        /// Execute an <see cref="AbstractMove"/> with <see cref="AbstractMove.MoveTypes.Slice"/>.
        /// </summary>
        /// <remarks>This is done by rotating the 2 side layers beside the slice, and then
        /// changing the <see cref="orientation"/> of the cube.</remarks>
        private void MoveAbstractSlice(AbstractMove move)
        {
            switch (move.Move)
            {
                case AbstractMoveNotation.m:
                    MoveLiteral(new LiteralMove(GetSideFromDirection(orientation, AbstractMoveNotation.l), Move.FlipModifierPrime(move.Modifier)));
                    MoveLiteral(new LiteralMove(GetSideFromDirection(orientation, AbstractMoveNotation.r), move.Modifier));
                    MoveAbstractWholeCube(new AbstractMove(AbstractMoveNotation.x, Move.FlipModifierPrime(move.Modifier)));
                    break;

                case AbstractMoveNotation.e:
                    MoveLiteral(new LiteralMove(orientation.Top, move.Modifier));
                    MoveLiteral(new LiteralMove(GetSideFromDirection(orientation, AbstractMoveNotation.d), Move.FlipModifierPrime(move.Modifier)));
                    MoveAbstractWholeCube(new AbstractMove(AbstractMoveNotation.y, Move.FlipModifierPrime(move.Modifier)));
                    break;

                case AbstractMoveNotation.s:
                    MoveLiteral(new LiteralMove(orientation.Front, Move.FlipModifierPrime(move.Modifier)));
                    MoveLiteral(new LiteralMove(GetSideFromDirection(orientation, AbstractMoveNotation.b), move.Modifier));
                    MoveAbstractWholeCube(new AbstractMove(AbstractMoveNotation.z, move.Modifier));
                    break;

                default:
                    throw new Exception($"Invalid slice move notation: {move.Move}");
            }
        }

        /// <summary>
        /// Execute multiple <see cref="LiteralMove"/>.
        /// </summary>
        public void MoveLiteral(params LiteralMove[] moves)
        {
            foreach (LiteralMove move in moves)
            {
                MoveLiteral(move);
            }
        }

        /// <summary>
        /// Execute a <see cref="LiteralMove"/>.
        /// </summary>
        public void MoveLiteral(LiteralMove move)
        {
            foreach(Segment segment in structure.GetSideLayer(move.Face))
            {
                segment.Rotate(move);
                structure.SetSegment(segment, segment.location);
            }
        }

        /// <summary>
        /// Manually set the orientation of the cube.
        /// </summary>
        /// <param name="frontColour"></param>
        /// <param name="topColour"></param>
        public void SetCubeOrientation(FaceColours frontColour, FaceColours topColour)
        {
            orientation.Front = frontColour;
            orientation.Top = topColour;
        }

        #endregion

        #region Get Segments

        /// <summary>
        /// Get segments that have the specified colour.
        /// </summary>
        /// <param name="colour">The colour on the segment to look for.</param>
        /// <param name="subset">The type of segments to check.</param>
        public List<Segment> GetSegmentsByColour(FaceColours colour, SegmentSubSets subset = SegmentSubSets.All)
        {
            return structure.GetSegments((s) => s.HasColour(colour), subset);
        }

        /// <summary>
        /// Get segments that have the specified colour. Only check the segments on the specified side.
        /// </summary>
        /// <param name="colour">The colour on the segment to look for.</param>
        /// <param name="side">The side of the cube to check</param>
        public List<Segment> GetSegmentsByColour(FaceColours colour, FaceColours side)
        {
            return structure.GetSegments((s) => s.HasColour(colour), side);
        }

        #endregion 

        #region Get Face Colours

        /// <summary>
        /// Get the colour of a face on a segment using <see cref="ColourOrder.ABSTRACT_DIRECTIONS"/>.
        /// </summary>
        /// <param name="colourRefrenceDirection">The colour reference for the face using directions</param>
        public FaceColours GetSegmentColourAbstract(AbstractMoveNotation[] colourRefrenceDirection)
        {
            FaceColours[] sides = new FaceColours[colourRefrenceDirection.Length];

            for(int i = 0; i < colourRefrenceDirection.Length; i++)
            {
                if(ABSTRACT_DIRECTIONS.GetIndex(colourRefrenceDirection[i]) == -1)
                {
                    throw new Exception($"Only {AbstractMove.MoveTypes.SideLayer} abstract notations can be used to get face colour");
                }

                sides[i] = GetSideFromDirection(orientation, colourRefrenceDirection[i]);
            }

            return GetSegmentColourLiteral(sides);
        }

        /// <summary>
        /// Get the colour of single face on the cube.
        /// The <param name="colourRefrence"> is used to find the segment on the cube.
        /// The first element in the <param name="colourRefrence"> determines which side of 
        /// segment is used.
        /// </summary>
        public FaceColours GetSegmentColourLiteral(FaceColours[] colourRefrence)
        {
            FaceColours face = colourRefrence[0];

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
        public FaceColours[] GetFaceColoursOnSide(FaceColours side)
        {
            FaceColours[] output = new FaceColours[SEGMENTS_PER_SIDE];

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
        public char[,] GetFacesOnSideToPrint(FaceColours sideColour, FaceColours topColour)
        {
            //get the colours that will be used to define the colour references
            FaceColours[] adjacentColours = new FaceColours[ROTATION_ORDER_LENGTH];

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

                    List<FaceColours> colourReference = new List<FaceColours>() { sideColour };

                    if (xOffset == 1)
                    {
                        colourReference.Add(adjacentColours[(int)Directions.Right]);
                    }
                    else if (xOffset == -1)
                    {
                        colourReference.Add(adjacentColours[(int)Directions.Left]);
                    }

                    if (yOffset == 1)
                    {
                        colourReference.Add(adjacentColours[(int)Directions.Up]);
                    }
                    else if (yOffset == -1)
                    {
                        colourReference.Add(adjacentColours[(int)Directions.Down]);
                    }

                    output[x, y] = (GetSegmentColourLiteral(colourReference.ToArray()).ToString())[0];
                }
            }

            return output;
        }

        #endregion

        public override string ToString()
        {
            string fourSpaces = "    ";

            string output = string.Empty;

            char[,] red = GetFacesOnSideToPrint(orientation.Top, GetSideFromDirection(orientation, AbstractMoveNotation.b));

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
                GetFacesOnSideToPrint(GetSideFromDirection(orientation, AbstractMoveNotation.l), orientation.Top),
                GetFacesOnSideToPrint(orientation.Front, orientation.Top),
                GetFacesOnSideToPrint(GetSideFromDirection(orientation, AbstractMoveNotation.r), orientation.Top),
                GetFacesOnSideToPrint(GetSideFromDirection(orientation, AbstractMoveNotation.b), orientation.Top)
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


            char[,] orange = GetFacesOnSideToPrint(GetSideFromDirection(orientation, AbstractMoveNotation.d), orientation.Front);

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