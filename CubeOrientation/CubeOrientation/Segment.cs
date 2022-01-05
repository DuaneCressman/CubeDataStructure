using CubeOrientation.CubeStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CubeOrientation.Notation;

namespace CubeOrientation
{
    /// <summary>
    /// This class hold one segment of the cube. It is able to handle being rotated on its own.
    /// </summary>
    public class Segment
    {
        /// <summary>
        /// The location of the segment on the cube.
        /// This is held by 2 chars for an edge, and 3 chars for a corner.
        /// </summary>
        public FaceColours[] location { get; private set; }

        /// <summary>
        /// The colours that are on the segment.
        /// </summary>
        public FaceColours[] colours { get; private set; }

        /// <summary>
        /// If the segment is in the correct location.
        /// </summary>
        public bool Solved
        {
            get
            {
                for (int i = 0; i < location.Length; i++)
                {
                    if (location[i] != colours[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public Segment(FaceColours[] location, FaceColours[] colours)
        {
            this.location = location;
            this.colours = colours;
        }

        /// <summary>
        /// Creates the segment in the correct location on the cube.
        /// </summary>
        public Segment(params FaceColours[] location)
        {
            this.location = new FaceColours[location.Length];
            colours = new FaceColours[location.Length];

            location.CopyTo(this.location, 0);
            location.CopyTo(colours, 0);
        }

        /// <summary>
        /// If the segment is on a side of the cube.
        /// This can be determined by if the location array contains
        /// the side colour.
        /// </summary>
        /// <param name="sideColour">The colour of the side to check.</param>
        public bool IsOnSide(FaceColours sideColour)
        {
            return location.GetIndex(sideColour) != -1;
        }

        /// <summary>
        /// If the segment has the colour passed in.
        /// </summary>
        public bool HasColour(FaceColours colour)
        {
            return colours.GetIndex(colour) != -1;
        }

        /// <summary>
        /// Rotate the segment. 
        /// The colours array will stay the same.
        /// The location array will be updated.
        /// </summary>
        /// <param name="move">The move that is rotating the segment</param>
        public void Rotate(LiteralMove move)
        {
            if (!IsOnSide(move.Face))
            {
                return;
            }

            int rotationOffset = ColourOrder.GetRotationOffset(move);

            for (int i = 0; i < location.Length; i++)
            {
                //The side that is on the cube doesn't change
                if (location[i] == move.Face)
                {
                    continue;
                }

                location[i] = ColourOrder.RotateColour(move.Face, location[i], rotationOffset);
            }
        }

        public override string ToString()
        {
            string colours = string.Empty, location = string.Empty;

            for (int i = 0; i < colours.Length; i++)
            {
                colours += this.colours[i];
                location += this.location[i];
            }

            return "\n" +
                   $"   Piece: {colours} \n" +
                   $"Location: {location}";
        }
    }
}
