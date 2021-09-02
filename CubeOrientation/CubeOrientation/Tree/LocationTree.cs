using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CubeOrientation.Tree
{
    public class LocationTree
    {
        /// <summary>
        /// Used in <see cref="CheckSegmentsReturnValue"/> to run a delegate on every segment.
        /// </summary>
        public delegate object SegmentDelegate(Segment segment);

        /// <summary>
        /// The node at the beginning of the tree.
        /// </summary>
        private Node startingNode;

        public LocationTree()
        {
            startingNode = new Node('S');
        }

        /// <summary>
        /// Run a delegate on all the <see cref="Segment"/>s in the tree.
        /// The delegate will return a value. The amount of times the value 
        /// equals the <param name="expectedValue"> is returned from this method.
        /// </summary>
        /// <param name="segmentDelegate">The delegate to run on each segment</param>
        /// <param name="expectedValue">The value that is being looked for</param>
        /// <returns></returns>
        public int CheckSegmentsReturnValue(SegmentDelegate segmentDelegate, object expectedValue)
        {
            int valuesFound = 0;

            RunFuncOnNodes(startingNode, segmentDelegate, expectedValue, ref valuesFound);

            return valuesFound;
        }

        /// <summary>
        /// This method runs recursively to go over all the nodes in the tree.
        /// On each node that has a segment, the <param name="segmentDelegate">
        /// will be run. If the value returned from the delegate matches the 
        /// <param name="expectedValue">, then <param name="valuesFound"> will 
        /// be incremented.
        /// </summary>
        /// <param name="currentNode">The current node</param>
        /// <param name="segmentDelegate">The delegate to be run on each segment in the tree</param>
        /// <param name="expectedValue">The expected return value of the delegate</param>
        /// <param name="valuesFound">How many times the <param name="expectedValue"> was returned</param>
        private void RunFuncOnNodes(Node currentNode, SegmentDelegate segmentDelegate, object expectedValue, ref int valuesFound)
        {
            if (currentNode.segment != null)
            {
                object value = segmentDelegate(currentNode.segment);

                if (Equals(value, expectedValue))
                {
                    valuesFound++;
                }
            }

            for (int i = 0; i < currentNode.outEdges.Length; i++)
            {
                RunFuncOnNodes(currentNode.outEdges[i], segmentDelegate, expectedValue, ref valuesFound);
            }
        }

        #region Search By Colour

        /// <summary>
        /// All the segments that have a location that is on the side 
        /// passed in. The piece that is on the segment has no effect on this method.
        /// </summary>
        /// <param name="sideColour">The side of the cube to get the segments on.</param>
        public List<Segment> GetSegmentsByColour(char sideColour)
        {
            return (from x in GetNodesByColour(sideColour) select x.segment).ToList();
        }

        /// <summary>
        /// Get all the nodes in the tree that has a segment that's location
        /// is on the side passed in.
        /// </summary>
        /// <param name="colour">The side of the cube to check</param>
        private List<Node> GetNodesByColour(char colour)
        {
            List<Node> output = new List<Node>();

            SearchByColour(startingNode, colour, false, output);

            return output;
        }

        /// <summary>
        /// This method will recursively look through the entire tree finding nodes
        /// that have a segment that would have a location on the side passed in.
        /// </summary>
        /// <param name="currentNode">The node being checked.</param>
        /// <param name="colour">The colour that is being looked for.</param>
        /// <param name="onCorrectPath">If the node being checked is on a branch of the tree that is a part of the right colour</param>
        /// <param name="foundNodes">The nodes that have been found</param>
        private void SearchByColour(Node currentNode, char colour, bool onCorrectPath, List<Node> foundNodes)
        {
            if (onCorrectPath && currentNode.segment != null)
            {
                foundNodes.Add(currentNode);
            }

            for (int i = 0; i < currentNode.outEdges.Length; i++)
            {
                bool correctPathFound = onCorrectPath || currentNode.edgeTitles[i] == colour;

                SearchByColour(currentNode.outEdges[i], colour, correctPathFound, foundNodes);
            }
        }

        #endregion

        /// <summary>
        /// Get a <see cref="Segment"/> at a specific location on the tree.
        /// </summary>
        /// <param name="locationTitle">The location on the tree to look for</param>
        public Segment GetSegment(char[] locationTitle)
        {
            return GetNode(locationTitle).segment;
        }

        /// <summary>
        /// Set the <see cref="Segment"/> for a specific location in the tree.
        /// </summary>
        /// <param name="segment">The segment to be placed in the tree</param>
        /// <param name="locationTitle">Where the segment should be placed</param>
        public void SetSegment(Segment segment, params char[] locationTitle)
        {
            GetNode(locationTitle).segment = segment;
        }

        /// <summary>
        /// Get a specific <see cref="Node"/> in the tree.
        /// </summary>
        /// <param name="locationTitle">The location of the node</param>
        /// <returns></returns>
        private Node GetNode(char[] locationTitle)
        {
            char[] location = Sort(locationTitle);

            Node current = startingNode;

            for (int i = 0; i < location.Length; i++)
            {
                if (location[i] == '\0')
                {
                    break;
                }

                current = current.outEdges[current.edgeTitles.GetIndex(location[i])];
            }

            return current;
        }

        /// <summary>
        /// Sort the elements in the char array that defines a node in the tree.
        /// </summary>
        /// <remarks>
        /// It is only possible to get to each point on the tree by one path.
        /// This means that the path name must be in the order that the tree can handle.
        /// </remarks>
        /// <param name="name">The name to sort</param>
        /// <returns>A sorted copy of the name passed in.</returns>
        private static char[] Sort(char[] name)
        {
            return name.OrderBy(x => GetColourOrder(x)).ToArray();
        }

        /// <summary>
        /// Get the priority of each colour in the cube.
        /// This is used to sort the names of location on the tree.
        /// </summary>
        private static int GetColourOrder(char c) => c switch
        {
            'W' => 0,
            'Y' => 1,
            'R' => 2,
            'O' => 3,
            'B' => 4,
            'G' => 5,
            '\0' => 6,
            _ => throw new Exception($"LocationTree can not handle the colour \'{c}\'")
        };
    }
}
