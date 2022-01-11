using CubeOrientation.CubeStructure;
using System;
using System.Collections.Generic;

namespace CubeOrientation
{
    class Program
    {
        static void Main(string[] args)
        {
            //Example();

            Cube cube = new Cube();

            Console.WriteLine(cube);

            while (true)
            {
                string move = Console.ReadLine();
                AbstractMove[] moves;

                try
                {
                    moves = MoveFactory.BuildAbstractMoves(move);
                    cube.MoveAbstract(moves);
                    Console.WriteLine(cube);

                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid");
                }

                    
            }

            //char[,] colours = cube.GetFacesOnSideToPrint(Notation.FaceColours.R, Notation.FaceColours.W);


            //for(int x = 0; x < 3; x++)
            //{
            //    for(int y = 0; y < 3; y++)
            //    {
            //        Console.Write(colours[x, y]);
            //    }

            //    Console.WriteLine();
            //}

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
