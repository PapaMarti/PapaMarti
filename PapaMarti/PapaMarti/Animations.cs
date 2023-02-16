using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PapaMarti
{
    class Animations
    {
        Texture2D spriteSheet;
        Rectangle[] sourcePanels;
        int currentIndex;
        int framesPerUpdate;
        int timer;
        
        public Animations(Texture2D spriteSheet, Rectangle[] sourcePanels, int framesPerUpdate)
        {
            this.spriteSheet = spriteSheet;
            this.sourcePanels = sourcePanels;
            this.framesPerUpdate = framesPerUpdate;
            currentIndex = 0;
            timer = 0;
        }

        public void update()
        {
            timer++;
            if (timer > framesPerUpdate)
            {
                timer = 0;
                currentIndex++;
                if (currentIndex >= sourcePanels.Length)
                {
                    currentIndex = 0;
                }
            }
        }

        public void draw(SpriteBatch spriteBatch, Rectangle panel)
        {
            spriteBatch.Draw(spriteSheet, panel, sourcePanels[currentIndex], Color.White);
        }
        public void draw(SpriteBatch spriteBatch, Rectangle panel, float rotation, Vector2 origin, SpriteEffects effects)
        {
            spriteBatch.Draw(spriteSheet, panel, sourcePanels[currentIndex], Color.White, rotation, origin, effects, 0);
        }
    }
}
