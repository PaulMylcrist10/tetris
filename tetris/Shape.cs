using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;


namespace tetris
{
    public class Shape
    {
        public Shape()
        {
            colour = new Color();
            CoOrds = new Vector2[4];
            Centre = new Vector2();
        }
        public Shape(Shape oldShape)
        {
            colour = oldShape.colour;
            CoOrds = new Vector2[4];
            for (int i = 0; i < 4; i++)
            {
                CoOrds[i] = new Vector2(oldShape.CoOrds[i]);
            }
            Centre = oldShape.Centre;
        }
        public Shape(double r)
        {
            CoOrds = new Vector2[4];
            colour = new Color();
            Centre = new Vector2(5,20);
            if (r < 1.0 / 7.0) //Green
            {
                colour = Color.Green;

                CoOrds[0] = new Vector2(-1, 0);
                CoOrds[1] = new Vector2(0, 0);
                CoOrds[2] = new Vector2(0, 1);
                CoOrds[3] = new Vector2(1, 1);
            }
            else if (r < 2.0 / 7.0) //red
            {
                colour = Color.Red;

                CoOrds[0] = new Vector2(-1, 1);
                CoOrds[1] = new Vector2(0, 1);
                CoOrds[2] = new Vector2(0, 0);
                CoOrds[3] = new Vector2(1, 0);
            }
            else if (r < 3.0 / 7.0) //LB
            {
                colour = Color.LightBlue;

                CoOrds[0] = new Vector2(0, 2);
                CoOrds[1] = new Vector2(0, 1);
                CoOrds[2] = new Vector2(0, 0);
                CoOrds[3] = new Vector2(0, -1);
            }
            else if (r < 4.0 / 7.0) //orange
            {
                colour = Color.Orange;

                CoOrds[0] = new Vector2(-1, 0);
                CoOrds[1] = new Vector2(0, 0);
                CoOrds[2] = new Vector2(1, 0);
                CoOrds[3] = new Vector2(1, 1);
            }
            else if (r < 5.0 / 7.0)
            {
                colour = Color.Blue;

                CoOrds[0] = new Vector2(-1, 0);
                CoOrds[1] = new Vector2(0, 0);
                CoOrds[2] = new Vector2(1, 0);
                CoOrds[3] = new Vector2(-1, 1);
            }
            else if (r < 6.0 / 7.0)
            {
                colour = Color.Purple;

                CoOrds[0] = new Vector2(-1, 0);
                CoOrds[1] = new Vector2(0, 0);
                CoOrds[2] = new Vector2(1, 0);
                CoOrds[3] = new Vector2(0, 1);
            }
            else
            {
                colour = Color.Yellow;

                CoOrds[0] = new Vector2(0, 1);
                CoOrds[1] = new Vector2(1, 1);
                CoOrds[2] = new Vector2(0, 0);
                CoOrds[3] = new Vector2(1, 0);
            }
        }

        Color colour;
        Vector2[] CoOrds;
        Vector2 Centre;

        public Color getColour
        {
            get { return colour; }
        }
        public Vector2 getMatrixCentre
        {
            get { return Centre; }
        }
        public Vector2[] getMatrixCoOrds
        {
            get { return CoOrds; }
        }

        public void lowerShape()
        {
            Centre= Vector2.Add(Centre, new Vector2(0, -1));
        }
        public void moveLeft()
        {
            Centre = Vector2.Add(Centre, new Vector2(-1, 0));
        }
        public void moveRight()
        {
            Centre = Vector2.Add(Centre, new Vector2(1, 0));
        }
        public void rotate()
        {
            for (int i = 0; i < 4; i++)
            {
                float x, y;
                Vector2 v = CoOrds[i];
                x = -v.Y;
                y =v.X;
                CoOrds[i] = new Vector2(x,y);
            }
        }
        public void deleteCoOrd(int i)
        {
            CoOrds[i] = new Vector2(-Program.width-2, -Program.height-2);
        }
        public void shiftCoOrdDown(int i)
        {
            CoOrds[i] = Vector2.Add(CoOrds[i], new Vector2(0, -1));
        }
    }
}
