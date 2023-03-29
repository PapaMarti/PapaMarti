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

enum Direction
{
    left,
    right,
    down,
    up
}
namespace PapaMarti
{
    class Car
    {
        int timer;
        Direction direction;//which way the car is facing
        int framesPerUpdate;

        public Car()
        {
            timer = 0;
            direction = Direction.right;
            framesPerUpdate = 15;//frames per update (yeah)
        }

        /// <param name="kb">The current keyboard state, used to calculate which way the car is suppposed to face</param>
        public void updateDirection(Direction direction)
        {
            this.direction = direction;
        }

        public void updateTime()
        {
            if (timer > framesPerUpdate * 2)
                timer = 0;
            else
                timer++;
        }

        public void draw(SpriteBatch spritebatch, Texture2D carTexture, Rectangle destinationRectangle)
        {
            int frame;
            if (timer < framesPerUpdate)
                frame = 0;
            else
                frame = 1;
            Rectangle source = new Rectangle(frame * 48, ((int)direction) * 32, 48, 32);
            spritebatch.Draw(carTexture, destinationRectangle, source, Color.White);
            //https://sketchfab.com/3d-models/1964-fiat-500-31c6854b92704dd6933f16295e36188c
        }
    }
}
