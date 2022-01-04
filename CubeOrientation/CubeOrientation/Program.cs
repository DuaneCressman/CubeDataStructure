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

            Console.WriteLine($"Front = {cube.FrontColour}, Top = {cube.TopColour}");

            cube.RotateWholeCube('x', false);
            cube.RotateWholeCube('z', false);
            cube.RotateWholeCube('y', true);
            cube.RotateWholeCube('x', true);




            Console.WriteLine($"Front = {cube.FrontColour}, Top = {cube.TopColour}");

            


            //cube.RotateSlices("f u l' u b d' r r f'", 'R', 'W');

            //string[] faces = { "ubr", "fl", "dfr", "bd" };
            //char[] correctColours = { 'O', 'R', 'R', 'O' };

            //for(int i = 0; i < faces.Length; i++)
            //{
            //    Console.WriteLine($"{faces[i]} = {cube.GetFaceColour(faces[i], 'W', 'R')}");
            //}

        }

        public static void Example()
        {
            Cube cube = new Cube();

            string rotationtions = "W R' B G W' W' O B Y";

            Console.WriteLine($"Applying rotations {rotationtions}\n");

            cube.MoveBySideColour(rotationtions);

            Console.WriteLine(cube);
        }
    }
}
