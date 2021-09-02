using System;
using System.Collections.Generic;
using System.Text;

namespace CubeOrientation.Tree
{
    internal class Node
    {
        internal Segment segment;

        internal Node[] outEdges;

        internal char[] edgeTitles;

        internal Node(char initalizer)
        {
            switch (initalizer)
            {
                case 'S':
                    edgeTitles = "WYRO".ToCharArray();
                    break;

                case 'W':
                case 'Y':
                    edgeTitles = "ROBG".ToCharArray();
                    break;

                case 'R':
                case 'O':
                    edgeTitles = "BG".ToCharArray();
                    break;

                case 'B':
                case 'G':
                    edgeTitles = new char[0];
                    break;
            }

            outEdges = new Node[edgeTitles.Length];

            for (int i = 0; i < edgeTitles.Length; i++)
            {
                outEdges[i] = new Node(edgeTitles[i]);
            }
        }
    }
}
