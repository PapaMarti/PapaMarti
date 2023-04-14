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
    public enum WeaponType
    {
        Throw,
        Frisbee,
        Bomb,
        Sword,
        SauceSprayer,
        Cheeser,
        Mace
    }
    public abstract class Weapon
    {
        public WeaponType weaponType;

        //a way to calculate the area in which an enemy should be hurt by an attack
        public Circle areaOfEffect;

        public Weapon(WeaponType type)
        {
            weaponType = type;
            areaOfEffect = null;
        }

        public abstract void draw(SpriteBatch spriteBatch);
        public abstract void update(GameTime time);
    }
}
