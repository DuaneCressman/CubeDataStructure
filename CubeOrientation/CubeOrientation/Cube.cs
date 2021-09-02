using CubeOrientation.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CubeOrientation
{
    public class Cube
    {
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

        public char GetFaceColour(char[] colourRefrence)
        {
            char face = colourRefrence[0];

            Segment segment = locationTree.GetSegment(colourRefrence);

            int index = segment.location.GetIndex(face);

            return segment.colours[index];
        }
    }
}