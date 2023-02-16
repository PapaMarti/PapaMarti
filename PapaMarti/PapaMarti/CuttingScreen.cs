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
    enum PizzaShape
    {
        Circle
    }

    class CuttingScreen
    {
        PizzaShape shape;
        Rectangle screenRect;
        Rectangle tableRect;
        Rectangle uncutDoughRect; //this will always be a square
        Texture2D dough; //for testing purposes its always a white pixel i make different colors
        Texture2D outline;
        private bool mouseIsPressed;
        List<Point> mouseLocations;
        List<double> accuracies;
        List<Rectangle> outlineRects;
        Rectangle outlineTextRect;

        //please never make the screen width less than its height. you will break my stuff
        public CuttingScreen(PizzaShape shape, Rectangle screenRectangle, Texture2D dough, Texture2D outline)
        {
            this.shape = shape;
            screenRect = screenRectangle;
            int tableHeight = (int)(screenRect.Height * 0.6);
            tableRect = new Rectangle(screenRect.X, screenRect.Y + screenRect.Height - tableHeight, screenRect.Width, tableHeight);
            int uncutHeight = (int)(tableHeight * 0.95);
            uncutDoughRect = new Rectangle((tableRect.Width - uncutHeight) / 2 + tableRect.X, (tableHeight - uncutHeight) / 2 + tableRect.Y, uncutHeight, uncutHeight);
            int outlineHeight = (int)(uncutHeight * 0.8);
            outlineTextRect = new Rectangle((uncutHeight - outlineHeight) / 2 + uncutDoughRect.X, (uncutHeight - outlineHeight) / 2 + uncutDoughRect.Y, outlineHeight, outlineHeight);
            this.dough = dough;
            this.outline = outline;
            mouseIsPressed = false;
            accuracies = new List<double>();
            mouseLocations = new List<Point>();
            outlineRects = getOutlineRects();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(dough, tableRect, Color.Gray);
            spriteBatch.Draw(dough, uncutDoughRect, Color.Yellow);
            spriteBatch.Draw(outline, outlineTextRect, Color.Black);
            Rectangle pixel = new Rectangle(0, 0, 2, 2);
            for(int i = 0; i < mouseLocations.Count; i++)
            {
                pixel.X = mouseLocations[i].X;
                pixel.Y = mouseLocations[i].Y;
                spriteBatch.Draw(dough, pixel, Color.Blue);
            }
        }
        public void update(GameTime time)
        {
            MouseState mouse = Mouse.GetState();
            if(mouse.LeftButton == ButtonState.Pressed)
            {
                mouseLocations.Add(new Point(mouse.X, mouse.Y));
                mouseIsPressed = true;
            }
            else if (mouseIsPressed)
            {
                mouseIsPressed = false;
                mouseLocations = new List<Point>();
                accuracies = new List<double>();
            }
        }
        public bool isDone()
        {
            return false;
        }
        public double getAccuracy()
        {
            if (accuracies.Count == 0)
                return 0.0;
            double average = 0.0;
            for(int i = 0; i < accuracies.Count; i++)
            {
                average += accuracies[i];
            }
            return average / accuracies.Count;
        }
        private List<Rectangle> getOutlineRects()
        {
            List<Rectangle> rects = new List<Rectangle>();
            if(shape == PizzaShape.Circle)
            {

            }
            return rects;
        }
        private double getMousePointAccuracy(Point point)
        {
            double bestAccuracy = 0;
            for(int i = 0; i < outlineRects.Count; i++)
            {
                double currentAccuracy = 0;
                double xDistance, yDistance;
                if(point.X <= outlineRects[i].Width + outlineRects[i].X && point.X >= outlineRects[i].X && point.Y >= outlineRects[i].Y && point.Y <= outlineRects[i].Height + outlineRects[i].Y)
                {
                    bestAccuracy = 1.0;
                    break;
                }
                if(Math.Abs(outlineRects[i].Width + outlineRects[i].X - point.X) < Math.Abs(outlineRects[i].X - point.X))
                {
                    xDistance = Math.Abs(outlineRects[i].Width + outlineRects[i].X - point.X);
                }
                else
                {
                    xDistance = Math.Abs(outlineRects[i].X - point.X);
                }
                if (Math.Abs(outlineRects[i].Height + outlineRects[i].Y - point.Y) < Math.Abs(outlineRects[i].Y - point.Y))
                {
                    yDistance = Math.Abs(outlineRects[i].Height + outlineRects[i].Y - point.Y);
                }
                else
                {
                    yDistance = Math.Abs(outlineRects[i].Y - point.Y);
                }
                double distance = Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2));
                if(distance > 15)
                {
                    currentAccuracy = 0.0;
                }
                else
                {
                    currentAccuracy = (15 - distance) / 15;
                }
                if (currentAccuracy > bestAccuracy)
                    bestAccuracy = currentAccuracy;
            }
            return bestAccuracy;
        }
    }
}
