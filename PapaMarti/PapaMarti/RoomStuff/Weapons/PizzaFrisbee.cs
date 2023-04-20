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
    class PizzaFrisbee: Weapon
    {
        Texture2D texture;
        float angle;
        bool isInFlight;
        double maxDistance;
        double direction;
        double currentDistance;
        double speed;

        //these are used in initialization depending on whether it is a frisbee or a regular pizza
        static float angleSpeed = 0.05f;

        static int frisbeeDamage = 50;
        static int throwDamage = 40;

        static double frisbeeRadius = 23;
        static double throwRadius = 18;

        static double frisbeeDistance = 600;
        static double throwDistance = 500;

        static double frisbeeSpeed = 11;
        static double throwSpeed = 9;

        public PizzaFrisbee(ContentManager content, Player player, WeaponType type): base(content, player, type, frisbeeDamage, frisbeeRadius)
        {
            texture = content.Load<Texture2D>("whitePixel");
            displayTexture = texture;
            angle = 0f;

            isInFlight = false;

            maxDistance = frisbeeDistance;
            speed = frisbeeSpeed;

            direction = 0;

            //making it less strong if it isnt a frisbee
            if(type == WeaponType.Throw)
            {
                speed = throwSpeed;
                damage = throwDamage;
                maxDistance = throwDistance;
                attackRadius = throwRadius;
            }
        }

        //upgrades a regular pizza to a frisbee
        public void upgrade()
        {
            reset();
            weaponType = WeaponType.Frisbee;
            speed = frisbeeSpeed;
            damage = frisbeeDamage;
            maxDistance = frisbeeDistance;
            attackRadius = frisbeeRadius;
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            if (areaOfEffect != null)
                spriteBatch.Draw(texture, new Rectangle((int)(areaOfEffect.location.X), (int)(areaOfEffect.location.Y), (int)(attackRadius * 2), (int)(attackRadius * 2)), new Rectangle(0, 0, texture.Width, texture.Height), Color.White, angle, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), SpriteEffects.None, 0f);
        }
        public override void update()
        {
            if (!isInFlight)
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

                    //circle used to calculate damage
                    areaOfEffect = new Circle( attackRadius, new Vector2( (int)(player.rect.X + player.rect.Width / 2.0 + getX()), (int)(player.rect.Y + player.rect.Height / 2.0 + getY()) ) );
                }
            }
            else if(currentDistance >= maxDistance) //if it is finished with its flight 
            {
                reset();
            }
            else //if it is in flight
            {
                currentDistance += speed;
                areaOfEffect.location.X += getX();
                areaOfEffect.location.Y += getY();
                angle += angleSpeed;
            }
        }

        //resets weapon to not be firing
        public override void reset()
        {
            areaOfEffect = null;
            angle = 0f;
            isInFlight = false;
            direction = 0;
            currentDistance = 0;
        }

        //gets the x and y velocities based on flight direction
        private float getX()
        {
            return (float)(Math.Cos(direction) * speed);
        }
        private float getY()
        {
            return (float)(Math.Sin(direction) * speed);
        }
    }
}
