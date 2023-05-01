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
    public class Bomb : Weapon
    {
        Texture2D bombText;
        bool isInFlight;
        static double maxDistance = 400;
        static double speed = 7;
        static int explosionTime = 60;

        static double rad = 100;
        static int dam = 200;

        static int bombRad = 23;

        double currentDistance;
        double direction;
        int time;
        Rectangle bombRect;
        Vector2 location;

        public Bomb(ContentManager content, Player player, WeaponType type) : base(content, player, type, dam, rad)
        {
            bombText = content.Load<Texture2D>("whitePixel");
            displayTexture = bombText;

            isInFlight = false;

            currentDistance = 0;
            direction = 0;
            time = 0;
            bombRect = new Rectangle(0, 0, bombRad * 2, bombRad * 2);
            location = new Vector2(0, 0);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            if(isInFlight)
            {
                spriteBatch.Draw(bombText, bombRect, Color.White);
            }
            else if(time > 0)
            {
                spriteBatch.Draw(bombText, new Rectangle((int)(areaOfEffect.location.X - attackRadius), (int)(areaOfEffect.location.Y - attackRadius), (int)(attackRadius * 2), (int)(attackRadius * 2)), Color.White);
            }
        }
        public override void update()
        {
            if (!isInFlight && time <= 0) //no bomb active
            {
                KeyboardState kb = Keyboard.GetState();
                if (kb.IsKeyDown(Keys.Space)) //if the player wants to start an attack
                {
                    isInFlight = true;


                    currentDistance = speed;

                    //calculating direction of flight
                    direction = Math.Atan(player.directionFacing.Y / player.directionFacing.X);
                    if (player.directionFacing.X < 0)
                        direction += Math.PI;
                    location.X = player.rect.X + player.rect.Width / 2.0f + getX();
                    location.Y = player.rect.Y + player.rect.Height / 2.0f + getY();
                    bombRect.X = (int)(location.X - bombRect.Width / 2.0);
                    bombRect.Y = (int)(location.Y - bombRect.Height / 2.0);

                    areaOfEffect = new Circle(bombRad, location);
                }
            }
            else if (!isInFlight) //bomb is exploding
            {
                time--;
                if(time <= 0)
                {
                    reset();
                }
            }
            else if (currentDistance >= maxDistance) //if it is finished with its flight 
            {
                explode();
            }
            else
            {
                currentDistance += speed;
                location.X += getX();
                location.Y += getY();
                bombRect.X = (int)(location.X - bombRect.Width / 2.0);
                bombRect.Y = (int)(location.Y - bombRect.Height / 2.0);
            }
        }
        public override void reset()
        {
            areaOfEffect = null;
            isInFlight = false;
            location = new Vector2(0, 0);
            direction = 0;
            currentDistance = 0;
            time = 0;
            bombRect.X = 0;
            bombRect.Y = 0;
            location = new Vector2(0, 0);
        }

        private void explode()
        {
            if (!isInFlight)
                return;

            //circle used to calculate damage
            areaOfEffect = new Circle(attackRadius, new Vector2(location.X, location.Y));

            currentDistance = 0;
            direction = 0;
            isInFlight = false;
            location = new Vector2(0, 0);
            bombRect.X = 0;
            bombRect.Y = 0;

            time = explosionTime;
        }

        private float getX()
        {
            return (float)(Math.Cos(direction) * speed);
        }
        private float getY()
        {
            return (float)(Math.Sin(direction) * speed);
        }


        public override void enemyHit()
        {
            explode();
        }
    }
}
