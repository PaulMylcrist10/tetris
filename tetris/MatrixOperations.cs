using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;

namespace tetris
{
    static public class MatrixOperations
    {
        static bool[,] Matrix;
        public static readonly object MatrixBat = new object();

        static MatrixOperations()
        {
            Matrix = new bool[Program.width, Program.height];
            initializeMatrix();
        }
        public static void deleteShapesRow(int j)
        {
            Vector2[] CoOrds = new Vector2[4];
            Vector2 Centre = new Vector2();
            foreach (var shape in Program.Shapes)
            {
                CoOrds = shape.getMatrixCoOrds;
                Centre = shape.getMatrixCentre;
                for (int i = 0; i < 4; i++)
                {
                    if (CoOrds[i].Y + Centre.Y == j)
                    {
                        shape.deleteCoOrd(i);
                    }
                    else if (CoOrds[i].Y + Centre.Y > j)
                    {
                        shape.shiftCoOrdDown(i);
                    }
                }
            }
        }
        public static void deleteMatrixRow(int j)
        {
            //shift all other rows down
            for (j = j; j < Program.height - 1; j++)
            {
                for (int i = 0; i < Program.width; i++)
                {
                    Matrix[i, j] = Matrix[i, j + 1];
                }
            }
            //the top row is empty
            for (int i = 0; i < Program.width; i++)
            {
                Matrix[i, Program.height - 1] = false;
            }
        }

        public static bool checkMatrix(Shape newShape)
        {
            Vector2[] CoOrds = newShape.getMatrixCoOrds;
            Vector2 CoOrd = new Vector2();
            bool check = true;
            for (int i = 0; i < 4; i++)
            {
                CoOrd = Vector2.Add(CoOrds[i], newShape.getMatrixCentre);
                int x = (int)CoOrd.X, y = (int)CoOrd.Y;
                if (y < 0 || x < 0 || x > 9)
                {
                    check = false;
                    break;
                }
                else
                {
                    if (Matrix[x, y])
                    {
                        check = false;
                        break;
                    }
                }
            }
            return check;
        }

        public static bool[] checkForRow()
        {
            bool[] isRow = new bool[Program.height];

            for (int j = 0; j < Program.height; j++)
            {
                isRow[j] = true;
                for (int i = 0; i < Program.width; i++)
                {
                    if (!Matrix[i, j])
                    {
                        isRow[j] = false;
                        break;
                    }

                }
            }
            return isRow;
        }
        public static void addToMatrix(Shape myShape)
        {
            Vector2[] CoOrds = myShape.getMatrixCoOrds;
            Vector2 CoOrd = new Vector2();
            for (int i = 0; i < 4; i++)
            {
                CoOrd = Vector2.Add(CoOrds[i], myShape.getMatrixCentre);
                int x = (int)CoOrd.X, y = (int)CoOrd.Y;
                Matrix[x, y] = true;
            }
        }

        public static void initializeMatrix()
        {
            for (int i = 0; i < Program.width; i++)
            {
                for (int j = 0; j < Program.height; j++)
                {
                    Matrix[i, j] = false;
                }
            }
        }
    }
}
