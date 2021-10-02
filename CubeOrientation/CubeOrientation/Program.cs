using CubeOrientation.CubeStructure;
using System;
using System.Collections.Generic;

namespace CubeOrientation
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 5; i++)
            {
                Cube cube = new Cube();

                string scramble = GenerateScramble(5, false);

                Console.WriteLine(scramble);

                cube.RotateSlices(scramble.ToString());

                Dictionary<Segment, int> sortedEdges = EdgeSorter.SortEdges(cube, 'W', 'Y');

                foreach (Segment segment in sortedEdges.Keys)
                {
                    Console.WriteLine($"{segment}\nMoves to Solve: {sortedEdges[segment]}");
                }

                Console.WriteLine("\n\n_____________________________\n\n");
            }

            
        }

        public static void Example()
        {
            Cube cube = new Cube();

            string rotationtions = "W R' B G W' W' O B Y";

            Console.WriteLine($"Aplying rotations {rotationtions}\n");

            cube.RotateSlices(rotationtions);

            Console.WriteLine(cube);
        }

        public static string GenerateScramble(int length, bool includeMiddleSlices)
        {
            List<string> allPossibleMoves = new List<string>();

            foreach(char c in ColourOrder.COLOUR_ORDER)
            {
                allPossibleMoves.Add(c.ToString()); 
                allPossibleMoves.Add(c + "\'");
            }

            if(includeMiddleSlices)
            {
                foreach (char c in Cube.PLANES)
                {
                    allPossibleMoves.Add(c.ToString());
                    allPossibleMoves.Add(c + "\'");
                }
            }

            Random random = new Random();
            string output = string.Empty;

            for(int i = 0; i < length; i++)
            {
                output += allPossibleMoves[random.Next() % allPossibleMoves.Count] + " ";
            }

            return output;
        }
    }
}
