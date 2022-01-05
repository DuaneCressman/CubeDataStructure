using CubeOrientation.CubeStructure;
using System;
using System.Collections.Generic;

namespace CubeOrientation
{
    class Program
    {
        static void Main(string[] args)
        {
            Example();

        }

        public static void Example()
        {
            Cube cube = new Cube();

            string rotationtions = "W R' B G W' W' O B Y";

            Console.WriteLine($"Applying rotations {rotationtions}\n");

            cube.MoveLiteral(MoveFactory.BuildLiteralMoves(rotationtions));

            Console.WriteLine(cube);
        }
    }
}
