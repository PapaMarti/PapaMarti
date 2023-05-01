using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PapaMarti
{
    //this is used to decide which hardcoded rectangles to use, but the correct outline & cut dough texture needs to be passed in manually
    public enum PizzaShape
    {
        Circle,
        Bomb
    }

    class CuttingScreen : CookingStage
    {
        PizzaShape shape;
        Rectangle screenRect;
        Rectangle uncutDoughRect;
        Texture2D whitePixel;
        Texture2D dough;
        Texture2D outline;
        Texture2D cutDough;
        private bool mouseIsPressed;
        List<Point> mouseLocations;
        List<double> accuracies;
        List<Rectangle> outlineRects;
        Rectangle outlineTextRect;
        bool done;

        //please never make the screen width less than its height. you will break my stuff
        public CuttingScreen(Pizza type, Rectangle screenRectangle, Texture2D dough, Texture2D outline, Texture2D cutDough, Texture2D white) : base(type)
        {
            shape = this.type.shape;
            screenRect = screenRectangle;
            int uncutHeight = (int)(screenRect.Height * 0.8);
            uncutDoughRect = new Rectangle((screenRect.Width - uncutHeight) / 2 + screenRect.X, (screenRect.Height - uncutHeight) / 2 + screenRect.Y, uncutHeight, uncutHeight);
            int outlineHeight = (int)(uncutHeight * 0.8);
            outlineTextRect = new Rectangle((uncutHeight - outlineHeight) / 2 + uncutDoughRect.X, (uncutHeight - outlineHeight) / 2 + uncutDoughRect.Y, outlineHeight, outlineHeight);

            this.dough = dough;
            this.outline = outline;
            this.cutDough = cutDough;
            whitePixel = white;

            mouseIsPressed = false;
            done = false;
            accuracies = new List<double>();
            mouseLocations = new List<Point>();
            outlineRects = getOutlineRects();

            //for testing delete later
            //for (int r = 0; r < screenRect.Width; r++)
            //{
            //    for (int c = screenRect.Y; c < screenRect.Height + screenRect.Y; c++)
            //    {
            //        mouseLocations.Add(new Point(r, c));
            //        accuracies.Add(getMousePointAccuracy(mouseLocations[mouseLocations.Count - 1]));
            //    }
            //}
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            if (done)
            {
                spriteBatch.Draw(cutDough, outlineTextRect, Color.White);
            }
            else
            {
                spriteBatch.Draw(dough, uncutDoughRect, Color.White);
                spriteBatch.Draw(outline, outlineTextRect, Color.White);

                //testing by drawing rectangles used for accuracy
                //for (int i = 0; i < outlineRects.Count; i++)
                //{
                //    spriteBatch.Draw(whitePixel, outlineRects[i], Color.Red);
                //}

                Rectangle pixel = new Rectangle(0, 0, 2, 2);
                for (int i = 0; i < mouseLocations.Count; i++)
                {
                    //makes the color anywhere from green to red depending on how accurate the point is
                    Color accuracyColor = new Color((float)(1.0 - accuracies[i]), (float)accuracies[i], 0.0f);
                    pixel.X = mouseLocations[i].X;
                    pixel.Y = mouseLocations[i].Y;
                    spriteBatch.Draw(whitePixel, pixel, accuracyColor);
                }
            }
            //might need to print accuracy rectangles for testing
        }
        public override void update(GameTime time)
        {
            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed && !done)
            {
                Point mousePosition = new Point(mouse.X, mouse.Y);
                //the point of this whole if statement is to prevent gaps in the line it draws as the mouse moves
                int previousIndex = mouseLocations.Count - 1;
                if (mouseIsPressed)
                {
                    Point previous = mouseLocations[previousIndex];
                    int xDiff = mousePosition.X - previous.X;
                    int yDiff = mousePosition.Y - previous.Y;
                    int diff = yDiff;
                    if (Math.Abs(xDiff) > Math.Abs(yDiff))
                        diff = xDiff;
                    int step = 2;
                    if (diff < 0)
                        step *= -1;
                    double xStep = (double)xDiff / diff;
                    double yStep = (double)yDiff / diff;
                    for (int i = step; Math.Abs(i) < Math.Abs(diff); i += step)
                    {
                        mouseLocations.Add(new Point(previous.X + (int)(i * xStep), previous.Y + (int)(i * yStep)));
                        accuracies.Add(getMousePointAccuracy(mouseLocations[mouseLocations.Count - 1]));
                    }
                }
                mouseLocations.Add(mousePosition);
                accuracies.Add(getMousePointAccuracy(mouseLocations[mouseLocations.Count - 1]));
                mouseIsPressed = true;
            }
            else if (mouseIsPressed)
            {
                mouseIsPressed = false;

                if (!done)
                {
                    bool isDone = true;
                    bool[] checkingRectangles = new bool[outlineRects.Count];

                    for(int i = 0; i < outlineRects.Count; i++)
                    {
                        for(int j = 0; j < mouseLocations.Count; j++)
                        {
                            if (outlineRects[i].Contains(mouseLocations[j]))
                            {
                                checkingRectangles[i] = true;
                                break;
                            }
                        }
                        if(i != 0 && i != 1)
                        {
                            if (!checkingRectangles[i] && !checkingRectangles[i - 1] && !checkingRectangles[i - 2])
                            {
                                isDone = false;
                                break;
                            }
                        }
                    }
                    if (isDone)
                        done = true;
                }

                if (!done)
                {
                    mouseLocations = new List<Point>();
                    accuracies = new List<double>();
                }
            }
        }
        public override bool isDone()
        {
            //if (Keyboard.GetState().IsKeyDown(Keys.X))
            //    return true;
            return done;
        }
        public override double getAccuracy()
        {
            if (accuracies.Count == 0)
                return 0.0;
            double average = 0.0;
            for (int i = 0; i < accuracies.Count; i++)
            {
                average += accuracies[i];
            }
            return average / accuracies.Count;
        }
        private List<Rectangle> getOutlineRects()
        {
            List<Rectangle> rects = new List<Rectangle>();
            //for now this is an enum i force you to give me in the constructor
            if (shape == PizzaShape.Circle || shape == PizzaShape.Bomb)
            {
                int oX = outlineTextRect.X;
                int oY = outlineTextRect.Y;
                int oW = outlineTextRect.Width;
                int oH = outlineTextRect.Height;
                //might need to go back in later and add a buffer zone to these rectangles so it isnt impossible to get 100 percent
                //i dont remember if these need to be ints or doubles so if they need to be int just cast them but after double calculations have been done
                //i hate rectangles
                // X: 9 to 15, Y: 1
                rects.Add(new Rectangle((int)(oX + (8.0 * (oW / 23.0))), oY, (int)(7.0 * (oW / 23.0)), (int)(oH / 23.0)));
                // X: 16 to 17, Y: 2
                rects.Add(new Rectangle((int)(oX + (15.0 * (oW / 23.0))), (int)(oY + (oH / 23.0)), (int)(2.0 * (oW / 23.0)), (int)(oH / 23.0)));
                // X: 18, Y: 3
                rects.Add(new Rectangle((int)(oX + (17.0 * (oW / 23.0))), (int)(oY + (2.0 * (oH / 23.0))), (int)(oW / 23.0), (int)(oH / 23.0)));
                // X: 19 to 20, Y: 4
                rects.Add(new Rectangle((int)(oX + (18.0 * (oW / 23.0))), (int)(oY + (3.0 * (oH / 23.0))), (int)(2.0 * (oW / 23.0)), (int)(oH / 23.0)));
                // X: 20, Y: 5
                rects.Add(new Rectangle((int)(oX + (19.0 * (oW / 23.0))), (int)(oY + (4.0 * (oH / 23.0))), (int)(oW / 23.0), (int)(oH / 23.0)));
                // X: 21, Y: 6
                rects.Add(new Rectangle((int)(oX + (20.0 * (oW / 23.0))), (int)(oY + (5.0 * (oH / 23.0))), (int)(oW / 23.0), (int)(oH / 23.0)));
                // X: 22, Y: 7 to 8
                rects.Add(new Rectangle((int)(oX + (21.0 * (oW / 23.0))), (int)(oY + (6.0 * (oH / 23.0))), (int)(oW / 23.0), (int)(2.0 * (oH / 23.0))));
                // X: 7 to 8, Y: 2
                rects.Add(new Rectangle((int)(oX + (6.0 * (oW / 23.0))), (int)(oY + (oH / 23.0)), (int)(2.0 * (oW / 23.0)), (int)(oH / 23.0)));
                // X: 6, Y: 3
                rects.Add(new Rectangle((int)(oX + (5.0 * (oW / 23.0))), (int)(oY + (2.0 * (oH / 23.0))), (int)(oW / 23.0), (int)(oH / 23.0)));
                // X: 4 to 5, Y: 4
                rects.Add(new Rectangle((int)(oX + (3.0 * (oW / 23.0))), (int)(oY + (3.0 * (oH / 23.0))), (int)(2.0 * (oW / 23.0)), (int)(oH / 23.0)));
                // X: 4, Y: 5
                rects.Add(new Rectangle((int)(oX + (3.0 * (oW / 23.0))), (int)(oY + (4.0 * (oH / 23.0))), (int)(oW / 23.0), (int)(oH / 23.0)));
                // X: 3, Y: 6
                rects.Add(new Rectangle((int)(oX + (2.0 * (oW / 23.0))), (int)(oY + (5.0 * (oH / 23.0))), (int)(oW / 23.0), (int)(oH / 23.0)));
                // X: 2, Y: 7 to 8
                rects.Add(new Rectangle((int)(oX + (oW / 23.0)), (int)(oY + (6.0 * (oH / 23.0))), (int)(oW / 23.0), (int)(2.0 * (oH / 23.0))));
                // X: 1, Y: 9 to 15
                rects.Add(new Rectangle((int)(oX), (int)(oY + (8.0 * (oH / 23.0))), (int)(oW / 23.0), (int)(7.0 * (oH / 23.0))));
                // X: 23, Y: 9 to 15
                rects.Add(new Rectangle((int)(oX + (22.0 * (oW / 23.0))), (int)(oY + (8.0 * (oH / 23.0))), (int)(oW / 23.0), (int)(7.0 * (oH / 23.0))));
                // X: 2, Y: 16 to 17
                rects.Add(new Rectangle((int)(oX + (oW / 23.0)), (int)(oY + (15.0 * (oH / 23.0))), (int)(oW / 23.0), (int)(2.0 * (oH / 23.0))));
                // X: 22, Y: 16 to 17
                rects.Add(new Rectangle((int)(oX + (21.0 * (oW / 23.0))), (int)(oY + (15.0 * (oH / 23.0))), (int)(oW / 23.0), (int)(2.0 * (oH / 23.0))));
                // X: 3, Y: 18
                rects.Add(new Rectangle((int)(oX + (2.0 * (oW / 23.0))), (int)(oY + (17.0 * (oH / 23.0))), (int)(oW / 23.0), (int)(oH / 23.0)));
                // X: 21, Y: 18
                rects.Add(new Rectangle((int)(oX + (20.0 * (oW / 23.0))), (int)(oY + (17.0 * (oH / 23.0))), (int)(oW / 23.0), (int)(oH / 23.0)));
                // X: 4, Y: 19
                rects.Add(new Rectangle((int)(oX + (3.0 * (oW / 23.0))), (int)(oY + (18.0 * (oH / 23.0))), (int)(oW / 23.0), (int)(oH / 23.0)));
                // X: 20, Y: 19
                rects.Add(new Rectangle((int)(oX + (19.0 * (oW / 23.0))), (int)(oY + (18.0 * (oH / 23.0))), (int)(oW / 23.0), (int)(oH / 23.0)));
                // X: 4 to 5, Y: 20
                rects.Add(new Rectangle((int)(oX + (3.0 * (oW / 23.0))), (int)(oY + (19.0 * (oH / 23.0))), (int)(2.0 * (oW / 23.0)), (int)(oH / 23.0)));
                // X: 19 to 20, Y: 20
                rects.Add(new Rectangle((int)(oX + (18.0 * (oW / 23.0))), (int)(oY + (19.0 * (oH / 23.0))), (int)(2.0 * (oW / 23.0)), (int)(oH / 23.0)));
                // X: 6, Y: 21
                rects.Add(new Rectangle((int)(oX + (5.0 * (oW / 23.0))), (int)(oY + (20.0 * (oH / 23.0))), (int)(oW / 23.0), (int)(oH / 23.0)));
                // X: 18, Y: 21
                rects.Add(new Rectangle((int)(oX + (17.0 * (oW / 23.0))), (int)(oY + (20.0 * (oH / 23.0))), (int)(oW / 23.0), (int)(oH / 23.0)));
                // X: 7 to 8, Y: 22
                rects.Add(new Rectangle((int)(oX + (6.0 * (oW / 23.0))), (int)(oY + (21.0 * (oH / 23.0))), (int)(2.0 * (oW / 23.0)), (int)(oH / 23.0)));
                // X: 16 to 17, Y: 22
                rects.Add(new Rectangle((int)(oX + (15.0 * (oW / 23.0))), (int)(oY + (21.0 * (oH / 23.0))), (int)(2.0 * (oW / 23.0)), (int)(oH / 23.0)));
                // X: 9 to 15, Y: 23
                rects.Add(new Rectangle((int)(oX + (8.0 * (oW / 23.0))), (int)(oY + (22.0 * (oH / 23.0))), (int)(7.0 * (oW / 23.0)), (int)(oH / 23.0)));
            }
            else
            { //if we add more shapes the rectangles for those can go here, just change this to else if

            }
            return rects;
        }
        private double getMousePointAccuracy(Point point)
        {
            double bestAccuracy = 0;
            int buffer = 3;
            double range = 30.0;
            for (int i = 0; i < outlineRects.Count; i++)
            {
                double currentAccuracy = 0;
                double xDistance, yDistance;
                if (point.X <= outlineRects[i].Width + outlineRects[i].X + buffer && point.X >= outlineRects[i].X - buffer && point.Y >= outlineRects[i].Y - buffer && point.Y <= outlineRects[i].Height + outlineRects[i].Y + buffer)
                {
                    bestAccuracy = 1.0;
                    break;
                }
                if (Math.Abs(outlineRects[i].Width + outlineRects[i].X - point.X) < Math.Abs(outlineRects[i].X - point.X))
                {
                    xDistance = Math.Abs(outlineRects[i].Width + outlineRects[i].X - point.X) - buffer;
                }
                else
                {
                    xDistance = Math.Abs(outlineRects[i].X - point.X) - buffer;
                }
                if (Math.Abs(outlineRects[i].Height + outlineRects[i].Y - point.Y) < Math.Abs(outlineRects[i].Y - point.Y))
                {
                    yDistance = Math.Abs(outlineRects[i].Height + outlineRects[i].Y - point.Y) - buffer;
                }
                else
                {
                    yDistance = Math.Abs(outlineRects[i].Y - point.Y) - buffer;
                }
                double distance = 40;
                if(point.X <= outlineRects[i].Width + outlineRects[i].X + buffer && point.X >= outlineRects[i].X - buffer)
                {
                    distance = yDistance;
                }
                else if (point.Y >= outlineRects[i].Y - buffer && point.Y <= outlineRects[i].Height + outlineRects[i].Y + buffer)
                {
                    distance = xDistance;
                }
                else
                {
                    distance = Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2));
                }
                //double distance = Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2));
                if (distance > 30)
                {
                    currentAccuracy = 0.0;
                }
                else
                {
                    currentAccuracy = (range - distance) / range;
                }
                if (currentAccuracy > bestAccuracy)
                    bestAccuracy = currentAccuracy;
            }
            return bestAccuracy;
        }

        public override CookStage getStage()
        {
            return CookStage.Cutting;
        }
    }
}
