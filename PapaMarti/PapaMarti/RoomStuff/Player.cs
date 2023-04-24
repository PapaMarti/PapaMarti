using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PapaMarti
{
    class Player : Life
    {
        public Rectangle rect;
        Texture2D texture;

        Texture2D lifeTexture;

        public Vector2 center;

        public Player(Rectangle rect_, Texture2D texture_, int maxLife_, Texture2D lifeTexture_) : base()
        {
            rect = rect_;
            texture = texture_;
            maxLife = maxLife_;
            currentLife = maxLife;
            lifeTexture = lifeTexture_;
            center = new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);

            lifeMeter = new Rectangle(20, 20, maxLife * 2, 50);
            lifeRemaining = new Rectangle(20, 20, currentLife * 2, 50);
        }
        public void updateCenter()
        {
            center = new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            
        }
        public Rectangle update(int changeX, int changeY)
        {
            
            return new Rectangle(rect.X + changeX, rect.Y + changeY, rect.Width, rect.Height);

        }
        public void updateX(int changeX)
        {
            this.rect.X += changeX;
            updateCenter();
        }
        public void updateY(int changeY)
        {
            this.rect.Y += changeY;
            updateCenter();
        }
        
        public override void draw(SpriteBatch spriteBatch, Texture2D texture_)
        {
            spriteBatch.Draw(texture, rect, Color.Red);

            spriteBatch.Draw(lifeTexture, lifeMeter, Color.Red);
            spriteBatch.Draw(lifeTexture, lifeRemaining, Color.Green);
        }
    }
}
