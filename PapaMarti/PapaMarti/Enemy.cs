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
        public int directionTimer;
        public int textNum;
        public bool otherside;

        public Enemy(Rectangle rect_, int maxLife_, int maxXVel_, int maxYVel_, int fireSpeed_, int frequency_, int textNum) : base()
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
            defaultColor = Color.White;
            currentColor = defaultColor;
            damageFrames = 0;
            directionTimer = 180;
            this.textNum = textNum;
            otherside = false;
        }
        
        public void resetTimer()
        {
            directionTimer = 180;
        }
        public Rectangle update() //for tests
        {
            return new Rectangle(rect.X + xVel, rect.Y + yVel, rect.Width, rect.Height);
        }
        public abstract Vector2 trajectory(Player p);
        public void updateX(int changeX)
        {
            this.rect.X += changeX;
            if (changeX > 0 && !otherside)
            {
                otherside = true;
            }
            else if (changeX < 0 && otherside)
            {
                otherside = false;
            }
            center = new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
        }
        public void updateY(int changeY)
        {
            this.rect.Y += changeY;
            center = new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
        }
        public override void draw(SpriteBatch spriteBatch, Texture2D enemyText)
        {
            SpriteEffects effect;
            if (otherside)
                effect = SpriteEffects.FlipHorizontally;
            else
                effect = SpriteEffects.None;


            spriteBatch.Draw(enemyText, rect, new Rectangle(textNum * 12, 0, 12, 16), currentColor, 0, new Vector2(0,0), effect, 0);
        }
        public abstract void hitVertical();
        public abstract void hitHorizontal();
        public abstract void changeDirection();
    }
}
