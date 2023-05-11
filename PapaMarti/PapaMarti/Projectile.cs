using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PapaMarti
{
    public class Projectile //hostile only
    {
        public int xVel;
        public int yVel;
        public Rectangle rect;
        public int strength;
        public bool friendlyFire;
        public int ricochet;
        public Projectile(Rectangle rect_, int xVel_, int yVel_, int strength_)
        {
            rect = rect_;
            xVel = xVel_;
            yVel = yVel_;
            strength = strength_;
            friendlyFire = false;
            ricochet = 2;
        }
        public void updateX(int x)
        {
            rect.X += x;
        }
        public void updateY(int y)
        {
            rect.Y += y;
        }
        public void update()
        {
            rect.X += xVel;
            rect.Y += yVel;
        }
        public void draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, this.rect, Color.Yellow);
        }
    }
}
