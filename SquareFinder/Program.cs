using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace SquareFinder
{
    public class Program
    {
        //static entry point and shit
        readonly static Color[] colors = { Color.Red, Color.Blue, Color.Yellow, Color.Green, Color.Purple, Color.Turquoise };

        public static void Main(string[] args)
        {
            Console.WriteLine("Please Enter the width and height of the Rectangle (pixels): <W> <H>");
            string[] input = Console.ReadLine().Split(' ');
            Program solver;
            //error handling when inputting the wrong shit.
            try
            {
                solver = new Program(int.Parse(input[0]), int.Parse(input[1]));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed parsing input.");
                Console.WriteLine(ex.Source);
                Console.ReadKey();
                return;
            }
            //Start the damn thing.
            solver.Start();
            //wait for a key before closing immediately.
            solver.Save();
            Console.ReadKey();
        }

        //Program instance stuff.
        Bitmap image;
        int w, h;


        public Program(int width, int height)
        {
            image = new Bitmap(width, height);
            //image.Save("path");
            w = width;
            h = height;
        }

        public void Start()
        {
            Solve(new Vector2(0, 0), new Vector2(w, h), 0);
        }

        public void Save()
        {
            string path = Path.GetFullPath($"{w}_{h}.png");
            image.Save(path);
            Console.WriteLine($"Image saved at path: {path}");
        }

        /// <summary>
        /// Solve the rectangle into squares
        /// </summary>
        /// <param name="globalOffset">the global offset due to previous operations</param>
        /// <param name="bounds">current bounds of the rectangle</param>
        /// <param name="iteration">how many steps needed to get here, also color.</param>
        void Solve(Vector2 globalOffset, Vector2 bounds, int iteration)
        {
            //get the max size.
            int rectSize = 0;
            Vector2 localOffset = new Vector2();
            if(bounds.y > bounds.x)
            {
                //_y is bigger, take x
                rectSize = bounds.x;
                localOffset.y = rectSize;
            }
            else
            {
                //_x is bigger so take y
                rectSize = bounds.y;
                localOffset.x = rectSize;
            }
            //"subtract" the rect size from the bounds
            //draw the current square onto the bitmap image
            Color color = colors[iteration % colors.Length];
            for(int dx = 0; dx < rectSize; dx++)
            {
                for(int dy = 0; dy <rectSize; dy++)
                {
                    //draw
                    image.SetPixel(globalOffset.x + dx, globalOffset.y + dy, color);
                }
            }
            //adjust the global offset.
            globalOffset += localOffset;
            //subtract the rectangle from the bounds.
            bounds -= localOffset;
            iteration++;
            //write the square information.
            Console.WriteLine($"Found Square #{iteration} with size {rectSize}x{rectSize} at {globalOffset.x}/{globalOffset.y}");
            //do we need to continue??
            if (bounds.y <= 0 || bounds.x <= 0)
            {
                Console.WriteLine($"Stopped Solving at bounds of {bounds.x}/{bounds.y}");
                return;
            }
            Solve(globalOffset, bounds, iteration);
        }
    }

    public struct Vector2
    {
        public int x, y;
        public Vector2(int a, int b)
        {
            x = a; y = b;
        }

        public static Vector2 operator +(Vector2 left, Vector2 right)
        {
            return new Vector2(left.x + right.x, left.y + right.y);
        }
        public static Vector2 operator -(Vector2 left, Vector2 right)
        {
            return new Vector2(left.x - right.x, left.y - right.y);
        }
    }
}
