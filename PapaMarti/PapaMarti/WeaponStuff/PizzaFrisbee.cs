using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PapaMarti.WeaponStuff
{
    class PizzaFrisbee: Weapon
    {
        Texture2D texture;
        float textureScale;
        float angle;

        public PizzaFrisbee(ContentManager content, Player player, WeaponType type): base(content, player, type, 50, 50)
        {
            texture = content.Load<Texture2D>("whitePixel");
            angle = 0f;

            //making it less strong if it isnt a frisbee
            if(type == WeaponType.Throw)
            {
                damage = 40;
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {

        }
        public override void update(GameTime time)
        {

        }
    }
}
