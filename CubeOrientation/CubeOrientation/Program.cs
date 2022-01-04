using CubeOrientation.CubeStructure;
using System;
using System.Collections.Generic;

namespace CubeOrientation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Move.ValidAbstractMove("HI"));
            Console.WriteLine(Move.ValidAbstractMove("u"));
            Console.WriteLine(Move.ValidAbstractMove("r l u x q r'")); 

            //Cube cube = new Cube();

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

            Console.WriteLine($"Aplying rotations {rotationtions}\n");

            cube.RotateSlices(rotationtions);

            Console.WriteLine(cube);
        }
    }
}
