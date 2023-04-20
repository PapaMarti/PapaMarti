using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PapaMarti
{
    abstract class Life
    {
        public Rectangle lifeRemaining;
        public Rectangle lifeMeter;


        public bool isDead;
        public static int maxLife;
        public int currentLife;

        public Life()
        {
            isDead = false;
            
        }

        public int takeDamage(int damage)
        {
            if (currentLife < damage)
            {
                currentLife = 0;
                isDead = true;
            }
            else
            {
                currentLife -= damage;
                
            }
            lifeRemaining.Width = currentLife * 2;
            return currentLife; //returns health after damage has been taken
        }

        public bool status()
        {
            return isDead;
        }

        public int heal(int healed)
        {
            if (currentLife + healed > maxLife)
            {
                currentLife = maxLife;
            }
            else
            {
                currentLife += healed;
            }
            lifeRemaining.Width = currentLife * 2;
            return currentLife;
        }

        public abstract void draw(SpriteBatch spriteBatch);
    }
}
