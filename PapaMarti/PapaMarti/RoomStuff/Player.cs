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
    public class Player
    {
        public Rectangle rect;
        Texture2D texture;
        Weapon weapon;
        List<Weapon> weapons;
        int selectedWeapon;

        public Vector2 directionFacing;

        public Player(ContentManager content, Rectangle rect_, Texture2D texture_)
        {
            rect = rect_;
            texture = texture_;
            weapon = new PizzaFrisbee(content, this, WeaponType.Throw);
            weapons = new List<Weapon>();
            weapons.Add(weapon);
            selectedWeapon = 0;

            directionFacing = new Vector2(-Room.MOVEMENTSPEED, 0);
        }

        public void addWeapon(Weapon weapon)
        {
            weapons.Add(weapon);
        }

        public void update(int changeX, int changeY)
        {
            KeyboardState kb = Keyboard.GetState();

            if(changeX != 0 || changeY != 0)
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
        }
        public void updateX(int changeX)
        {
            this.rect.X += changeX;
        }
        public void updateY(int changeY)
        {
            this.rect.Y += changeY;
        }
        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rect, Color.Red);
            weapon.draw(spriteBatch);
        }

        private void resetWeapons()
        {
            foreach(Weapon w in weapons)
            {
                w.reset();
            }
        }
    }
}
