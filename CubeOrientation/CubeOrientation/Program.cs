using CubeOrientation.CubeStructure;
using System;
using System.Collections.Generic;

namespace CubeOrientation
{
    class Program
    {
        static void Main(string[] args)
        {
            Cube cube = new Cube();

            string rotationtions = "W R' B G W' W' O B Y";

            Console.WriteLine($"Aplying rotations {rotationtions}\n");

            cube.RotateSlices(rotationtions);

            Console.WriteLine(cube);
        }
    }
}
