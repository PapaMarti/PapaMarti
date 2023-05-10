using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PapaMarti
{
     public abstract class Enemy : Life
    {
        public Rectangle rect;
        public int maxXVel;
        public int maxYVel;
        public int xVel;
        public int yVel;
        public int fireSpeed;
        public int frequency;
        public Vector2 center;
        public int damageFrames;
        public Color defaultColor;
        public Color currentColor;
        

        public Enemy(Rectangle rect_, int maxLife_, int maxXVel_, int maxYVel_, int fireSpeed_, int frequency_) : base()
        {
            rect = rect_;
            maxLife = maxLife_;
            currentLife = maxLife;
            maxXVel = maxXVel_;
            maxYVel = maxYVel_;
            xVel = 0;
            yVel = 0;
            fireSpeed = fireSpeed_;
            frequency = frequency_;
            center = new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            defaultColor = Color.Blue;
            currentColor = defaultColor;
            damageFrames = 0;
        }
        
        public Rectangle update() //for tests
        {
            return new Rectangle(rect.X + xVel, rect.Y + yVel, rect.Width, rect.Height);
        }
        public abstract Vector2 trajectory(Player p);
        public void updateX(int changeX)
        {
            this.rect.X += changeX;
            center = new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
        }
        public void updateY(int changeY)
        {
            this.rect.Y += changeY;
            center = new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
        }
        public override void draw(SpriteBatch spriteBatch, Texture2D enemyText)
        {
            spriteBatch.Draw(enemyText, rect, currentColor);
        }
        public abstract void bounceOffX();
        public abstract void bounceOffY();
    }
}
