using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Threading;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Text;

namespace tetris
{
    class Program
    {
        public const int height = 23; public const int width = 10;

        static OpenTK.Input.Key lastKey;
        public static readonly object keyBat=new object();
        static Shape CurrentShape;
        public static readonly object CurrentBat=new object();
        public static Random rand=new Random();
        // public static object randBat = new object();
        public static List<Shape> Shapes ;
        public static readonly object ShapesBat=new object();

        static GameWindow myWindow;
        static game myGame;

        

        static void Main(string[] args)
        {
            lastKey = new OpenTK.Input.Key();
            CurrentShape = new Shape(rand.NextDouble());
            // public static object randBat = new object();
            Shapes = new List<Shape>();


            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE); //hide console

            ForcedMovement Worker = new ForcedMovement();
            Thread myThread = new Thread(new ThreadStart( Worker.threadWork));
            myThread.Start();

            myWindow = new GameWindow(500,500);
            myGame = new game(myWindow);

            myThread.Join();
        }


        

        public static OpenTK.Input.Key getKey
        {
            get
            {
                lock (keyBat)
                {
                    return lastKey;
                }
            }
            set
            {
                lock (keyBat)
                {
                    lastKey = value;
                }
            }
        }



        public static List<Shape> getShapes
        {
            get
            {
                lock (ShapesBat)
                {
                    return Shapes;
                }
            }
            set
            {
                lock (ShapesBat)
                {
                    Shapes = value;
                }
            }
        }

        public static Shape getCurrentShape
        {
            get
            {
                lock (CurrentBat)
                {
                    return CurrentShape;
                } 
            }
            set
            {
                lock (CurrentBat)
                {
                    CurrentShape=value;
                }
            }
        }
        

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

    }
}
