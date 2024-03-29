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
    public class Player : Life
    {
        public Rectangle rect;
        Texture2D texture;
        public Weapon weapon;
        public List<Weapon> weapons;
        public List<Rectangle> weaponHotbar;
        int selectedWeapon;
        public Color defaultColor;
        public Color currentColor;
        public int iFrames;
        static Color shade = new Color(0.2f, 0.2f, 0.2f, 0.5f);
        Texture2D whiteSquare;

        public Vector2 directionFacing;

        static int hotbarHeight = 80;
        static int buffer = 10;

        Texture2D lifeTexture;

        public Vector2 center;

        public int timer;
        public int anim;

        public Player(ContentManager content, Rectangle rect_, int maxLife_) : base()
        {
            rect = rect_;
            maxLife = maxLife_;
            currentLife = maxLife;
            lifeTexture = content.Load<Texture2D>("whitePixel");
            center = new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);

            lifeMeter = new Rectangle(20, 20, maxLife * 2, 50);
            lifeRemaining = new Rectangle(20, 20, currentLife * 2, 50);
            texture = content.Load<Texture2D>("PapaMarti");

            weapon = new PizzaFrisbee(content, this, WeaponType.Throw);
            weapons = new List<Weapon>();

            weaponHotbar = new List<Rectangle>();

            weapons.Add(weapon);
            weaponHotbar.Add(new Rectangle(buffer, Game1.screenRect.Height - buffer - hotbarHeight, hotbarHeight, hotbarHeight));

            selectedWeapon = 0;

            directionFacing = new Vector2(-Room.MOVEMENTSPEED, 0);

            whiteSquare = content.Load<Texture2D>("whitePixel");
            defaultColor = Color.White;
            currentColor = defaultColor;
            iFrames = 0;

            timer = 0;
            anim = 0;
        }
        public void colorSwitch()
        {
            if (currentColor != Color.Transparent)
                currentColor = Color.Transparent;
            else if (currentColor != defaultColor)
                currentColor = defaultColor;
        }
        public void addWeapon(Weapon weapon)
        {
            weaponHotbar.Add(new Rectangle(weaponHotbar[weaponHotbar.Count - 1].X + hotbarHeight + buffer, Game1.screenRect.Height - buffer - hotbarHeight, hotbarHeight, hotbarHeight));
            weapons.Add(weapon);
        }

        public void update(int changeX, int changeY)
        {
            KeyboardState kb = Keyboard.GetState();

            if (changeX != 0 || changeY != 0)
            {
                directionFacing = new Vector2(changeX, changeY);
            }

            //changing selected weapon when keyboard is pressed
            if (kb.IsKeyDown(Keys.D1))
            {
                selectedWeapon = 0;
            }
            else if (kb.IsKeyDown(Keys.D2) && weapons.Count >= 2)
            {
                selectedWeapon = 1;
            }
            else if (kb.IsKeyDown(Keys.D3) && weapons.Count >= 3)
            {
                selectedWeapon = 2;
            }
            else if (kb.IsKeyDown(Keys.D4) && weapons.Count >= 4)
            {
                selectedWeapon = 3;
            }
            else if (kb.IsKeyDown(Keys.D5) && weapons.Count >= 5)
            {
                selectedWeapon = 4;
            }
            else if (kb.IsKeyDown(Keys.D6) && weapons.Count >= 6)
            {
                selectedWeapon = 5;
            }
            else if (kb.IsKeyDown(Keys.D7) && weapons.Count >= 7)
            {
                selectedWeapon = 6;
            }
            if (weapons[selectedWeapon].weaponType != weapon.weaponType)
            {
                resetWeapons();
                weapon = weapons[selectedWeapon];
            }

            weapon.update();

            timer++;
            if (timer > 15)
            {
                timer = 0;
                if (anim > 2)
                    anim = 0;
                else
                    anim++;
            }
        }
        public void updateCenter()
        {
            center = new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            
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
            int y = 0;
            if (directionFacing.X > 0)
                y = 48;
            if (directionFacing.X < 0)
                y = 16;
            if (directionFacing.Y > 0)
                y = 0;
            if (directionFacing.Y < 0)
                y = 32;



            Rectangle source = new Rectangle(anim * 12, y, 12, 16);

            spriteBatch.Draw(texture, rect, source, currentColor);
            weapon.draw(spriteBatch);
            drawHotbar(spriteBatch);

            spriteBatch.Draw(lifeTexture, lifeMeter, Color.Red);
            spriteBatch.Draw(lifeTexture, lifeRemaining, Color.Green);
        }
        private void drawHotbar(SpriteBatch spriteBatch)
        {
            int pad = 10;
            for(int i = 0; i < weapons.Count; i++)
            {
                spriteBatch.Draw(whiteSquare, weaponHotbar[i], Color.White);
                spriteBatch.Draw(weapons[i].displayTexture, new Rectangle(weaponHotbar[i].X + pad, weaponHotbar[i].Y + pad, weaponHotbar[i].Width - 2 * pad, weaponHotbar[i].Height - 2 * pad), Color.White);
                if(i != selectedWeapon)
                {
                    //draw shaded square
                    spriteBatch.Draw(whiteSquare, weaponHotbar[i], shade);
                }
            }
        }

        private void resetWeapons()
        {
            foreach(Weapon w in weapons)
            {
                w.reset();
            }

        }

        public int enemyDamage(Vector2 enemyCenter)
        {
            if(weapon.areaOfEffect != null)
            {
                if (weapon.areaOfEffect.isInCircle(enemyCenter))
                {
                    return weapon.damage;
                }
            }
            return 0;
        }
    }
}
