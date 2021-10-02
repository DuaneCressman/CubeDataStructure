using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CubeOrientation.CubeStructure;

namespace CubeOrientation
{
    public static class EdgeSorter
    {
        /// <summary>
        /// Gets all edges that have the edgeColour on then them.
        /// They are sorted by how long it takes for each edge to be solved to the faceColour passed in.
        /// </summary>
        public static Dictionary<Segment, int> SortEdges(Cube cube, char edgeFaceColour, char correctFaceColour)
        {
            Dictionary<Segment, int> output = new Dictionary<Segment, int>();

            char[] beltColours = ColourOrder.GetBeltColours(correctFaceColour);

            //if the edge face is already on the correct face
            foreach (char beltColour in beltColours)
            {
                (char foundFaceColour, Segment foundSegment) = cube.GetSegmentByFaceColour(correctFaceColour, beltColour);

                if (foundFaceColour == edgeFaceColour)
                {
                    output.Add(foundSegment, 0);
                }
            }

            //Get the segments that are on the middle slice
            foreach(Segment segment in cube.Structure.GetBeltSlice(correctFaceColour))
            {
                if(segment.HasFaceColour(edgeFaceColour))
                {
                    output.Add(segment, 1);
                }
            }

            if (output.Count == 4)
            {
                return output;
            }

            foreach (char beltColour in beltColours)
            {
                (char foundFaceColour, Segment foundSegment) = cube.GetSegmentByFaceColour(beltColour, correctFaceColour);

                if (foundFaceColour == edgeFaceColour)
                {
                    output.Add(foundSegment, 2);
                }
            }

            if (output.Count == 4)
            {
                return output;
            }

            foreach (char beltColour in beltColours)
            {
                (char foundFaceColour, Segment foundSegment) = cube.GetSegmentByFaceColour(beltColour, ColourOrder.GetOppositeColour(correctFaceColour));

                if (foundFaceColour == edgeFaceColour)
                {
                    output.Add(foundSegment, 2);
                }
            }

            if (output.Count == 4)
            {
                return output;
            }

            foreach (char beltColour in beltColours)
            {
                (char foundFaceColour, Segment foundSegment) = cube.GetSegmentByFaceColour(ColourOrder.GetOppositeColour(correctFaceColour), beltColour);

                if (foundFaceColour == edgeFaceColour)
                {
                    output.Add(foundSegment, 2);
                }
            }

            if (output.Count == 4)
            {
                return output;
            }

            throw new Exception("Not all 4 of the edges were found");
        }
    }
}
