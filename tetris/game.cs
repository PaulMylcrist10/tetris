using System;
using System.Drawing;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Threading;


namespace tetris
{
    public class game
    {
        GameWindow window;
        int texture;
        public game (GameWindow window)
        {
            this.window = window;
            window.Load += Window_Load;
            window.Resize += Window_Resize;
            window.RenderFrame += Window_RenderFrame;
            window.KeyDown += Window_KeyDown;
            window.Closed += Window_Closed;
            window.Run(1.0 / 60.0); //sets frame rate
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Program.getKey = OpenTK.Input.Key.Escape;
        }

        private void Window_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            lock (Program.CurrentBat)
            {
                OpenTK.Input.Key lastKey = e.Key;
                Program.getKey = lastKey;
                Shape CurrentShape = Program.getCurrentShape;
                Shape newShape;
                bool isOk = false;
                newShape = new Shape(CurrentShape);


                if (lastKey == OpenTK.Input.Key.Right)
                {
                    newShape.moveRight();
                    isOk = MatrixOperations.checkMatrix(newShape);
                }
                else if (lastKey == OpenTK.Input.Key.Left)
                {
                    newShape.moveLeft();
                    isOk = MatrixOperations.checkMatrix(newShape);
                }
                else if (lastKey == OpenTK.Input.Key.Up)
                {
                    newShape.rotate();
                    isOk = MatrixOperations.checkMatrix(newShape);
                }
                else if (lastKey == OpenTK.Input.Key.Down)
                {
                    newShape.lowerShape();
                    isOk = MatrixOperations.checkMatrix(newShape);
                }
                if (isOk)
                    Program.getCurrentShape = newShape;
            }
        }

        private void Window_Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, window.Width, window.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            
            if (window.Height!=0)
                GL.Ortho(0.0,20*window.Width/window.Height,0.0, 20.0, -1.0, 1.0);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        private void Window_RenderFrame(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            Shape current;
            List<Shape> shapes;
            lock (Program.CurrentBat)
            {
                current = new Shape(Program.getCurrentShape);
            }
            lock (Program.ShapesBat)
            {
                shapes = new List<Shape>(Program.getShapes);
            }
            if (current != null)
            {
                GL.Begin(PrimitiveType.Quads);
                {
                    //draw all the old shapes
                    foreach (var shape in shapes)
                    {
                        GL.Color3(shape.getColour);
                        Vector2[] CoOrds = shape.getMatrixCoOrds;
                        Vector2 CoOrd = new Vector2();
                        for(int i=0;i<4;i++)
                        {
                            CoOrd=Vector2.Add(CoOrds[i], shape.getMatrixCentre);
                            CoOrd = Vector2.Add(CoOrd, new Vector2(5, 0.5f));

                            GL.Vertex2(Vector2.Add(CoOrd, new Vector2(-0.5f, -0.5f)));
                            GL.Vertex2(Vector2.Add(CoOrd, new Vector2(-0.5f, 0.5f)));
                            GL.Vertex2(Vector2.Add(CoOrd, new Vector2(0.5f, 0.5f)));
                            GL.Vertex2(Vector2.Add(CoOrd, new Vector2(0.5f, -0.5f)));
                        }
                    }
                    // draw the current shape
                    {
                        GL.Color3(current.getColour);
                        Vector2[] CoOrds = current.getMatrixCoOrds;
                        Vector2 CoOrd = new Vector2();
                        for (int i = 0; i < 4; i++)
                        {
                            CoOrd = Vector2.Add(CoOrds[i], current.getMatrixCentre);
                            CoOrd = Vector2.Add(CoOrd, new Vector2(5, 0.5f));

                            GL.Vertex2(Vector2.Add(CoOrd, new Vector2(-0.5f, -0.5f)));
                            GL.Vertex2(Vector2.Add(CoOrd, new Vector2(-0.5f, 0.5f)));
                            GL.Vertex2(Vector2.Add(CoOrd, new Vector2(0.5f, 0.5f)));
                            GL.Vertex2(Vector2.Add(CoOrd, new Vector2(0.5f, -0.5f)));
                        }
                    }
                    
                    //draw the walls
                    GL.Color3(Color.Brown);
                    GL.Vertex2(0, 0); GL.Vertex2(0, 20); GL.Vertex2(4.5, 20); GL.Vertex2(4.5, 0);
                    GL.Vertex2(14.5, 0); GL.Vertex2(14.5, 20); GL.Vertex2(20, 20); GL.Vertex2(20, 0);
                }
                GL.End();
                GL.Begin(PrimitiveType.Lines);
                {
                    GL.Color3(Color.Black);
                    //draw all the old shapes
                    foreach (var shape in shapes)
                    {
                        Vector2[] CoOrds = shape.getMatrixCoOrds;
                        Vector2 CoOrd = new Vector2();
                        for (int i = 0; i < 4; i++)
                        {
                            CoOrd = Vector2.Add(CoOrds[i], shape.getMatrixCentre);
                            CoOrd = Vector2.Add(CoOrd, new Vector2(5, 0.5f));

                            GL.Vertex2(Vector2.Add(CoOrd, new Vector2(-0.5f, -0.5f)));
                            GL.Vertex2(Vector2.Add(CoOrd, new Vector2(-0.5f, 0.5f)));
                            GL.Vertex2(Vector2.Add(CoOrd, new Vector2(-0.5f, 0.5f)));
                            GL.Vertex2(Vector2.Add(CoOrd, new Vector2(0.5f, 0.5f)));
                            GL.Vertex2(Vector2.Add(CoOrd, new Vector2(0.5f, 0.5f)));
                            GL.Vertex2(Vector2.Add(CoOrd, new Vector2(0.5f, -0.5f)));
                            GL.Vertex2(Vector2.Add(CoOrd, new Vector2(0.5f, -0.5f)));
                            GL.Vertex2(Vector2.Add(CoOrd, new Vector2(-0.5f, -0.5f)));

                        }
                    }
                    // draw the current shape
                    {
                        Vector2[] CoOrds = current.getMatrixCoOrds;
                        Vector2 CoOrd = new Vector2();
                        for (int i = 0; i < 4; i++)
                        {
                            CoOrd = Vector2.Add(CoOrds[i], current.getMatrixCentre);
                            CoOrd = Vector2.Add(CoOrd, new Vector2(5, 0.5f));

                            GL.Vertex2(Vector2.Add(CoOrd, new Vector2(-0.5f, -0.5f)));
                            GL.Vertex2(Vector2.Add(CoOrd, new Vector2(-0.5f, 0.5f)));
                            GL.Vertex2(Vector2.Add(CoOrd, new Vector2(-0.5f, 0.5f)));
                            GL.Vertex2(Vector2.Add(CoOrd, new Vector2(0.5f, 0.5f)));
                            GL.Vertex2(Vector2.Add(CoOrd, new Vector2(0.5f, 0.5f)));
                            GL.Vertex2(Vector2.Add(CoOrd, new Vector2(0.5f, -0.5f)));
                            GL.Vertex2(Vector2.Add(CoOrd, new Vector2(0.5f, -0.5f)));
                            GL.Vertex2(Vector2.Add(CoOrd, new Vector2(-0.5f, -0.5f)));
                        }
                    }

                    //draw the walls
                    GL.Vertex2(0, 0); GL.Vertex2(0, 20); GL.Vertex2(4.5, 20); GL.Vertex2(4.5, 0);
                    GL.Vertex2(14.5, 0); GL.Vertex2(14.5, 20); GL.Vertex2(20, 20); GL.Vertex2(20, 0);
                }
                GL.End();

                GL.Begin(PrimitiveType.Quads);
                {
                    Random rand = new Random();
                    byte[] buffer = new byte[3];
                    rand.NextBytes(buffer);
                    GL.Color3(buffer); 
                    for (int i=0; i< ForcedMovement.isRow.Length; i++)
                    {
                        if (ForcedMovement.isRow[i])
                        {
                            GL.Vertex2(new Vector2(4.5f, (float)i));
                            GL.Vertex2(new Vector2(4.5f, (float)i + 1.0f));
                            GL.Vertex2(new Vector2(14.5f, (float)i + 1.0f));
                            GL.Vertex2(new Vector2(14.5f, (float)i ));
                        }
                    }
                }
                GL.End();
                if (ForcedMovement.LoadTexture)
                {
                    texture = LoadTexture(false, true);
                    ForcedMovement.LoadTexture = false;
                }
                GL.BindTexture(TextureTarget.Texture2D, texture);
                GL.Begin(PrimitiveType.Quads);
                {
                    GL.Color3(Color.White);
                    GL.TexCoord2(0, 0);
                    GL.Vertex2(1, 15.0);

                    GL.TexCoord2(0, 1);
                    GL.Vertex2(1, 18);

                    GL.TexCoord2(1, 1);
                    GL.Vertex2(4, 18);

                    GL.TexCoord2(1, 0);
                    GL.Vertex2(4, 15);
                }
                GL.End();
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }

            window.SwapBuffers();
        }

        private void Window_Load(object sender, EventArgs e)
        {
            GL.ClearColor(0.0f,0.0f,0.0f,0.0f);
            GL.Enable(EnableCap.Texture2D);
            texture=LoadTexture(false, true);
        }

        private int LoadTexture(bool repeat = true, bool flip_y = false)
        {
            Bitmap bitmap;
            lock (ForcedMovement.ScoreSheetBatton)
            {
                bitmap = new Bitmap(ForcedMovement.ScoreSheet);
            }

            //Flip the image
            if (flip_y)
                bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);

            //Generate a new texture target in gl
            int texture = GL.GenTexture();

            //Will bind the texture newly/empty created with GL.GenTexture
            //All gl texture methods targeting Texture2D will relate to this texture
            GL.BindTexture(TextureTarget.Texture2D, texture);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
                    
            

            if (repeat)
            {
                //This will repeat the texture past its bounds set by TexImage2D
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.Repeat);
            }
            else
            {
                //This will clamp the texture to the edge, so manipulation will result in skewing
                //It can also be useful for getting rid of repeating texture bits at the borders
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.ClampToEdge);
            }

            //Creates a definition of a texture object in opengl
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);

            //Load the data from are loaded image into virtual memory so it can be read at runtime
            System.Drawing.Imaging.BitmapData bitmap_data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, bitmap.Width, bitmap.Height, PixelFormat.Bgra, PixelType.UnsignedByte, bitmap_data.Scan0);

            bitmap.UnlockBits(bitmap_data);
            
            bitmap.Dispose();

            GL.BindTexture(TextureTarget.Texture2D, 0);

            return texture;
        }
    }
}
