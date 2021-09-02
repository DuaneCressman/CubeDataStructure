using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CubeOrientation
{
    public class Cube
    {
        /// <summary>
        /// The segments that make up the cube
        /// </summary>
        private List<Segment> segments;

        /// <summary>
        /// If all the segments are correct.
        /// </summary>
        public bool CubeIsSolved
        {
            get
            {
                return segments.Where(x => !x.isCorrect).ToList().Count == 0;
            }
        }

        public Cube()
        {
            BuildCube();
        }

        /// <summary>
        /// Creates all the segments that make up the cube. The cube will be in the 
        /// solved position.
        /// </summary>
        public void BuildCube()
        {
            segments = new List<Segment>();

            char[] ROBG = { 'R', 'O', 'B', 'G' };

            for (int i = 0; i < 2; i++)
            {
                char WY = i == 0 ? 'W' : 'Y';

                //add the edges with 'W' and 'Y'
                foreach (char c in ROBG)
                {
                    segments.Add(new Segment(WY, c));
                }

                //add the corners
                for (int j = 0; j < 2; j++)
                {
                    char RO = j == 0 ? 'R' : 'O';

                    segments.Add(new Segment(WY, RO, 'B'));
                    segments.Add(new Segment(WY, RO, 'G'));
                }
            }

            //add the ROBG edges
            for (int i = 0; i < 2; i++)
            {
                char RO = i == 0 ? 'R' : 'O';

                segments.Add(new Segment(RO, 'B'));
                segments.Add(new Segment(RO, 'G'));
            }
        }

        /// <summary>
        /// Rotate a side on the cube.
        /// </summary>
        /// <param name="sideColour">The colour of the side that is rotating</param>
        /// <param name="clockwise">True: clockwise. False: counter clockwise</param>
        public void RotateSlice(char sideColour, bool clockwise)
        {
            segments.Where(x => x.IsOnSide(sideColour)).ToList().ForEach(x => x.Rotate(sideColour, clockwise));
        }

        public override string ToString()
        {
            string output = string.Empty;

            output += "\nEdges:\n";
            segments.Where(x => x.SpaceType == SpaceTypes.Edge).ToList().ForEach((x) => output += x.ToString() + "\n");
            output += "\nCorners:\n";
            segments.Where(x => x.SpaceType == SpaceTypes.Corner).ToList().ForEach((x) => output += x.ToString() + "\n");

            output += $"\n\nTotal Locations: {segments.Count}";

            return output;
        }

        public string GetColourName(char colour) => colour switch
        {
            'W' => "White",
            'Y' => "Yellow",
            'R' => "Red",
            'O' => "Orange",
            'B' => "Blue",
            'G' => "Green",
            _ => "Invalid Colour!"
        };
    }
}