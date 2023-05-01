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

namespace PapaMarti
{
    //different types of weapons
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
        //type of weapon
        public WeaponType weaponType;

        //a way to calculate the area in which an enemy should be hurt by an attack, if it is null then the weapon is not being used and nothing can take damage
        public Circle areaOfEffect;

        //used to calculate circle (in pixels)
        protected double attackRadius;

        //amount of damage done when something is in the area of effect
        public int damage;

        //the player that is using the weapon
        public Player player;

        //for loading textures
        ContentManager content;

        //used to display weapon on hotbar
        public Texture2D displayTexture;

        public Weapon(ContentManager content, Player player, WeaponType type, int damage, double radius)
        {
            this.content = content;
            this.player = player;
            weaponType = type;
            this.damage = damage;
            this.attackRadius = radius;
            areaOfEffect = null;
        }

        public abstract void draw(SpriteBatch spriteBatch);
        public abstract void update();
        public abstract void reset(); //resets the weapon to its default unused state

        public abstract void enemyHit();
    }
}
