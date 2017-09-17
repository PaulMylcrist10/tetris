using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace tetris
{
    class ForcedMovement
    {
        static int timePeriod;
        static int score = 0;
        static int level = 0;
        static int RowsCleared = 0;
        static int RowsPerLevel = 5;
        public static Bitmap ScoreSheet;
        public static readonly object ScoreSheetBatton=new object();
        public static bool[] isRow = new bool[Program.height];
        Shape newShape, myShape;
        public static bool LoadTexture;

        public ForcedMovement()
        {
            newShape = new Shape();
            myShape = new Shape();
            ScoreSheet = new Bitmap(1000,450);
            drawScoreSheet();
            timePeriod = 500;
            LoadTexture = true;
        }

        public void threadWork()
        { 
            while (Program.getKey != OpenTK.Input.Key.Escape)
            {
                bool isOK = false;
                bool isPrevOK = false;
                Thread.Sleep(timePeriod);
                for (int j = 0; j < isRow.Length; j++)
                {
                    if (isRow[j])
                    {
                        Thread.Sleep(500);
                        break;
                    }
                }
                scoring(); //checks if there was a score
                lock (Program.CurrentBat)
                {
                    newShape = new Shape(Program.getCurrentShape);
                    myShape = new Shape(Program.getCurrentShape);

                    isPrevOK = MatrixOperations.checkMatrix(myShape);
                    //lower the new shape
                    newShape.lowerShape();
                    isOK = MatrixOperations.checkMatrix(newShape);
                    if (isOK)
                    {
                       Program.getCurrentShape = newShape; //the shape is lowered
                    }
                    else if (isPrevOK)
                    {
                        //the shape has hit the bottom, so add the shape to shapes and make a new current shape
                        Program.Shapes.Add(myShape);
                        MatrixOperations.addToMatrix(myShape);
                        Program.getCurrentShape = new Shape(Program.rand.NextDouble());
                        //check if there is a row complete
                        isRow = MatrixOperations.checkForRow();
                    }
                } 
            }
        }

        static void drawScoreSheet()
        {
            lock (ScoreSheetBatton)
            {
                Graphics ScoreSheetDrawer = Graphics.FromImage(ScoreSheet);
                ScoreSheetDrawer.Clear(Color.Brown);
                string text = "Score " + score.ToString() + "\n" + "Level  " + level.ToString();
                Font font1 = new Font("Arial", 120, FontStyle.Bold, GraphicsUnit.Point);
                ScoreSheetDrawer.DrawString(text, font1, Brushes.Black, new PointF(5, 5));
                ScoreSheet.Save("BitmapScore.bmp");
            }  
        }
        void scoring()
        {
            int NumRowCleared = 0;
            for (int j = 0; j < isRow.Length; j++)
            {
                if (isRow[j])
                {
                    NumRowCleared++;
                    //delete the matrix row and shift down
                    MatrixOperations.deleteMatrixRow(j);
                    //Delete the shapes in that row and shift down
                    MatrixOperations.deleteShapesRow(j);
                    for (int i = j; i < Program.height - 1; i++)
                    {
                        isRow[i] = isRow[i + 1];
                    }
                    isRow[Program.height - 1] = false;

                    score += 50;
                    RowsCleared++;
                    drawScoreSheet();
                    LoadTexture = true;
                }
            }
            if (NumRowCleared > 1)
                score += 15;
            if (NumRowCleared > 2)
                score += 15;
            if (NumRowCleared > 3)
                score += 15;
            if (RowsCleared == RowsPerLevel)
            {
                ++level;
                if (timePeriod > 110)
                {
                    timePeriod -= 50;
                }
                RowsCleared = 0;
                drawScoreSheet();
            }
        }
    }
}
