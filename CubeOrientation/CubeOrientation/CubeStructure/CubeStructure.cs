using CubeOrientation.Notation;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CubeOrientation.CubeStructure
{
    /// <summary>
    /// This class holds the 3x3x3 array needed to store all the 
    /// <see cref="Segment"/>s that make up the cube.
    /// </summary>
    public class CubeStructure
    {
        public const int TOTAL_SEGMENTS = 26;
        public const int TOTAL_CORNER_SEGMENTS = 8;
        public const int TOTAL_EDGE_SEGMENTS = 12;

        public const int SEGMENTS_PER_SIDE = 9;

        private static readonly Dictionary<FaceColours, V3Int> offsetByFaceColour = new Dictionary<FaceColours, V3Int>()
        {
            { FaceColours.W, new V3Int(1, 0, 0) },
            { FaceColours.Y, new V3Int(-1, 0, 0) },
            { FaceColours.R, new V3Int(0, 1, 0) },
            { FaceColours.O, new V3Int(0, -1, 0) },
            { FaceColours.B, new V3Int(0, 0, 1) },
            { FaceColours.G, new V3Int(0, 0, -1) }
        };

        private enum Plane
        {
            /// <summary>Going from 'W' to 'Y'<summary>
            X,
            /// <summary>Going from 'R' to 'O'<summary>
            Y,
            /// <summary>Going from 'B' to 'G'<summary>
            Z
        };

        public enum SegmentSubSets
        {
            All,
            Edges,
            Corners
        }

        public delegate bool segmentChecked(Segment segment);

        private Segment[,,] structure;

        public CubeStructure()
        {
            structure = new Segment[3, 3, 3];
        }

        #region Get Segments

        /// <summary>
        /// Get all the segments that are on the side of the cube that is passed in.
        /// </summary>
        public List<Segment> GetSegments(FaceColours side)
        {
            return GetSegments((_) => { return true; }, side);
        }

        /// <summary>
        /// Get all the segments on the side passed in that pass delegate passed in.
        /// </summary>
        /// <param name="segmentChecked">The condition for if a segments should be returned.</param>
        /// <param name="side">This side of the cube check the segments on.</param>
        public List<Segment> GetSegments(segmentChecked segmentChecked, FaceColours side)
        {
            List<Segment> output = new List<Segment>();

            V3Int offset = offsetByFaceColour[side];
            V3Int start = offset + offset.Simplified;
            V3Int max = new V3Int(Cube.SIZE, Cube.SIZE, Cube.SIZE);

            if(ColourOrder.IsSecondaryColour(side))
            {
                max += offset.Simplified * -2;
            }
        
            for (int x = start.x; x < max.x; x++)
            {
                for (int y = start.y; y < max.y; y++)
                {
                    for (int z = start.z; z < max.z; z++)
                    {
                        if (segmentChecked(structure[x, y, z]))
                        {
                            output.Add(structure[x, y, z]);
                        }
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Get segments based on their type. (corner segment, edge segment, or just all the segments)
        /// </summary>
        public List<Segment> GetSegments(SegmentSubSets subset = SegmentSubSets.All)
        {
            return GetSegments((_) => true, subset);
        }

        /// <summary>
        /// Get segments base on their type that pass the delegate passed in.
        /// </summary>
        /// <param name="segmentChecked">The condition for if a segments should be returned.</param>
        /// <param name="subset">The type of segments to check</param>
        public List<Segment> GetSegments(segmentChecked segmentChecked, SegmentSubSets subset = SegmentSubSets.All)
        {
            List<Segment> output = new List<Segment>();

            int itarator = subset == SegmentSubSets.Corners ? 2 : 1;

            for (int x = 0; x < Cube.SIZE; x += itarator)
            {
                for (int y = 0; y < Cube.SIZE; y += itarator)
                {
                    for (int z = 0; z < Cube.SIZE; z += itarator)
                    {
                        if (subset != SegmentSubSets.Corners)
                        {
                            if (x == 1 && y == 1 && z == 1)
                            {
                                continue;
                            }

                            if(subset == SegmentSubSets.Edges && (x + y + z) % 2 == 0)
                            {
                                continue;
                            }
                        }

                        if (segmentChecked(structure[x, y, z]))
                        {
                            output.Add(structure[x, y, z]);
                        }
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Get the <see cref="Segment"/> at a specified location in the cube.
        /// </summary>
        /// <param name="pathName">The path name for the segment.</param>
        /// <returns>The segment at the specified location.</returns>
        public Segment GetSegment(FaceColours[] pathName)
        {
            V3Int coordinates = GetPath(pathName);

            return structure[coordinates.x + 1, coordinates.y + 1, coordinates.z + 1];
        }

        #endregion

        /// <summary>
        /// Set the <see cref="Segment"/> that is at a specific location in the cube.
        /// </summary>
        /// <param name="segment">The segment to place in the cube</param>
        /// <param name="pathName">The path name for the location in the cube.</param>
        public void SetSegment(Segment segment, params FaceColours[] pathName)
        {
            V3Int coordinates = GetPath(pathName);

            structure[coordinates.x + 1, coordinates.y + 1, coordinates.z + 1] = segment;
        }

        /// <summary>
        /// Get all the possible path names for a 3x3x3 cube. This should only be used for 
        /// setting up the cube.
        /// </summary>
        public static FaceColours[][] GetAllPathNames()
        {
            FaceColours[][] output = new FaceColours[TOTAL_SEGMENTS][];

            int index = 0;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        if (x == 0 && y == 0 && z == 0)
                        {
                            continue;
                        }

                        output[index] = GetPathName(new V3Int(x, y, z));

                        index++;
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Get the path name for a coordinate path.
        /// </summary>
        /// <param coordinates="x">The coordinates</param>
        /// <returns>The path to this location in the cube.</returns>
        private static FaceColours[] GetPathName(V3Int coordinates)
        {
            List<FaceColours> faces = new List<FaceColours>();

            foreach(FaceColours colour in offsetByFaceColour.Keys)
            {
                V3Int current = offsetByFaceColour[colour];

                if(coordinates.x == current.x ||
                   coordinates.y == current.y ||
                   coordinates.z == current.z )
                {
                    faces.Add(colour);
                }

                if(faces.Count == 3)
                {
                    break;
                }
            }

            return faces.ToArray();
        }

        /// <summary>
        /// Get the path coordinates for a given path name.
        /// </summary>
        /// <param name="pathName">The path name of location.</param>
        /// <returns>The path coordinates for the given path name.</returns>
        private static V3Int GetPath(FaceColours[] pathName)
        {
            V3Int coordinates = new V3Int();

            for (int i = 0; i < pathName.Length; i++)
            {
                coordinates += offsetByFaceColour[pathName[i]];
            }

            return coordinates;
        }

        /// <summary>
        /// Get all the segments from a slice in the cube.
        /// The slices on the sides can be referenced using the colour
        /// of the center on that side.
        /// The middle slices can be gotten using:
        /// 'x' => The middle slice without the 'W' and 'Y' centers.
        /// 'y' => The middle slice without the 'R' and 'O' centers.
        /// 'z' => The middle slice without the 'B' and 'G' centers.
        /// </summary>
        public List<Segment> GetSlice(char slice)
        {
            int offset = 0;
            Plane plane;

            switch (slice)
            {
                case ('W'):
                    offset = 1;
                    plane = Plane.X;
                    break;

                case ('Y'):
                    offset = -1;
                    plane = Plane.X;
                    break;

                case ('R'):
                    offset = 1;
                    plane = Plane.Y;
                    break;

                case ('O'):
                    offset = -1;
                    plane = Plane.Y;
                    break;

                case ('B'):
                    offset = 1;
                    plane = Plane.Z;
                    break;

                case ('G'):
                    offset = -1;
                    plane = Plane.Z;
                    break;

                case ('x'):
                    plane = Plane.X;
                    break;

                case ('y'):
                    plane = Plane.Y;
                    break;

                case ('z'):
                    plane = Plane.Z;
                    break;

                default:
                    throw new Exception($"The slice {slice} could not be found.");
            }

            return GetSegmentsInSlice(plane, offset);
        }

        /// <summary>
        /// Get all the segments in a slice.
        /// The 'x' plane goes from 'Y' to 'W'.
        /// The 'y' plane goes from 'R' to 'O'.
        /// The 'z' plane goes from 'B' to 'G'.
        /// To get the middle slices, leave <param name="offset"> = 0.
        /// To get the colour slices, use the offset. +1 for the base colours.
        /// -1 for the secondary colours.
        /// </summary>
        /// <param name="plane">The plane that the slice is in.</param>
        /// <param name="offset">Which slice in the plane.</param>
        /// <returns></returns>
        private List<Segment> GetSegmentsInSlice(Plane plane, int offset)
        {
            List<Segment> output = new List<Segment>();

            for (int a = -1; a <= 1; a++)
            {
                for (int b = -1; b <= 1; b++)
                {
                    int x = 0;
                    int y = 0;
                    int z = 0;

                    switch (plane)
                    {
                        case Plane.X:
                            x = offset;
                            y = a;
                            z = b;
                            break;

                        case Plane.Y:
                            x = a;
                            y = offset;
                            z = b;
                            break;

                        case Plane.Z:
                            x = a;
                            y = b;
                            z = offset;
                            break;
                    }

                    if (x == 0 && y == 0 && z == 0)
                    {
                        continue;
                    }

                    output.Add(structure[x + 1, y + 1, z + 1]);
                }
            }

            return output;
        }
    }
}
