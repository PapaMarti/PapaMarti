using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PapaMarti
{
    class Enemy : Life
    {
        public Rectangle rect;
        Texture2D texture;

        public Enemy(Rectangle rect_, Texture2D texture_, int maxLife_) : base()
        {
            rect = rect_;
            texture = texture_;
            maxLife = maxLife_;
            currentLife = maxLife;



        }
        public Rectangle update(int changeX, int changeY)
        {
            return new Rectangle(rect.X + changeX, rect.Y + changeY, rect.Width, rect.Height);
        }
        public void updateX(int changeX)
        {
            this.rect.X += changeX;
        }
        public void updateY(int changeY)
        {
            this.rect.Y += changeY;
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rect, Color.Blue);
        }
    }
}
