using CubeOrientation.CubeStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public char[] location { get; private set; }

        /// <summary>
        /// The colours that are on the segment.
        /// </summary>
        public char[] colours { get; private set; }

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

        public Segment(char[] location, char[] colours)
        {
            this.location = location;
            this.colours = colours;
        }

        /// <summary>
        /// Creates the segment in the correct location.
        /// </summary>
        public Segment(params char[] location)
        {
            this.location = new char[location.Length];
            colours = new char[location.Length];

            location.CopyTo(this.location, 0);
            location.CopyTo(colours, 0);
        }

        /// <summary>
        /// If the segment is on a side of the cube.
        /// This can be determined by if the location array contains
        /// the side colour.
        /// </summary>
        /// <param name="sideColour">The colour of the side to check.</param>
        public bool IsOnSide(char sideColour)
        {
            return location.GetIndex(sideColour) != -1;
        }

        /// <summary>
        /// If the segment has the colour passed in.
        /// </summary>
        public bool HasColour(char colour)
        {
            return colours.GetIndex(colour) != -1;
        }

        /// <summary>
        /// Rotate the segment. 
        /// The colours array will stay the same.
        /// The location array will be updated.
        /// </summary>
        /// <param name="sideRotating">The side that is being rotated.</param>
        /// <param name="clockwise">If the side is being rotated clockwise.</param>
        public void Rotate(char sideRotating, bool clockwise)
        {
            if (!IsOnSide(sideRotating))
            {
                return;
            }

            for (int i = 0; i < location.Length; i++)
            {
                //The side that is on the cube doesn't change
                if (location[i] == sideRotating)
                {
                    continue;
                }

                location[i] = ColourOrder.RotateColour(sideRotating, location[i], clockwise ? 1 : -1);
            }
        }

        public override string ToString()
        {
            return $"\n" +
                   $"   Piece: {new string(colours)} \n" +
                   $"Location: {new string(location)}";
        }
    }
}
