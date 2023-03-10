﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PapaMarti
{
    class MapManager: StageManager
    {
        double angle;
        double position;
        Texture2D map;
        Vector2 mapPosition;
        Rectangle mapSource;
        Vector2 mapOrigin;

        int translation;
        double minPosition;
        double maxPosition;
        int innerCircleTranslation;
        /// <summary>
        /// Making a new map manager to allow the player to explore the island
        /// </summary>
        /// <param name="angle">Angular location on the pizza map, give radians. Typical unit circle stuff, roads are at 0, pi/3, 2pi/3, pi, 4pi/3, 5pi/3</param>
        /// <param name="position">How far up or down they are on a road, 0 for closer to the outside, 1 for the inner ring.</param>
        public MapManager(ContentManager content, double angle, double position) : base(content)
        {
            this.angle = angle % (2 * Math.PI);
            this.position = position;
            map = content.Load<Texture2D>("MapTextures/temporary map");
            mapSource = new Rectangle(0, 0, map.Width, map.Height);
            mapOrigin = new Vector2(map.Width / 2, map.Height / 2);
            translation = 1700; //adjust this number to get the scaling right
            innerCircleTranslation = 200;
            mapPosition = new Vector2(Game1.screenRect.Width / 2, Game1.screenRect.Height / 2 + (int)(translation - position * (translation - innerCircleTranslation)));
            minPosition = 0.005;
            maxPosition = 0.995;
        }
        private void updatePosition()
        {
            mapPosition.Y = Game1.screenRect.Height / 2 + (int)(translation - position * (translation - innerCircleTranslation));
        }

        public override GameStage getStage()
        {
            return GameStage.Exploring;
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(map, mapPosition, mapSource, Color.White, (float)(angle - Math.PI / 2), mapOrigin, 6f, SpriteEffects.None, 0f);
        }
        public override void update(GameTime time)
        {
            KeyboardState kb = Keyboard.GetState();
            updatePosition();
            double angleSpeed = 0.005; //in radians per frame
            if (kb.IsKeyDown(Keys.Left) && canRotate())
            {
                if (position < minPosition)
                    angle += angleSpeed;
                else
                    angle += angleSpeed * 2;
            }
            if(kb.IsKeyDown(Keys.Right) && canRotate())
            {
                if (position < minPosition)
                    angle -= angleSpeed;
                else
                    angle -= angleSpeed * 2;
            }
            if(kb.IsKeyDown(Keys.Up) && canMove())
            {

            }
            if(kb.IsKeyDown(Keys.Down) && canMove())
            {

            }
        }
        public override bool isDone()
        {
            return false;
        }

        private bool canRotate()
        {
            if (position < minPosition)
                position = 0;
            if (position > maxPosition)
                position = 1;
            return position < minPosition || position > maxPosition;
        }
        private bool canMove()
        {
            angle = angle % (Math.PI * 2);
            bool move = false;
            double min = 0.01;
            if ((angle < (Math.PI + min) && angle > (Math.PI - min)) || (angle < (-Math.PI + min) && angle > (-Math.PI - min)))
            {
                angle = Math.PI;
                move = true;
            }
            else if ((angle < (Math.PI / 3 + min) && angle > (Math.PI / 3 - min)) || (angle < (-5 * Math.PI / 3 + min) && angle > (-5 * Math.PI / 3 - min)))
            {
                angle = Math.PI / 3;
                move = true;
            }
            else if((angle < (2 * Math.PI / 3 + min) && angle > (2 * Math.PI / 3 - min)) || (angle < (-4 * Math.PI / 3 + min) && angle > (-4 * Math.PI / 3 - min)))
            {
                angle =  2 * Math.PI / 3;
                move = true;
            }
            else if((angle < (4 * Math.PI / 3 + min) && angle > (4 * Math.PI / 3 - min)) || (angle < (-2 * Math.PI / 3 + min) && angle > (-2 * Math.PI / 3 - min)))
            {
                angle = 4 * Math.PI / 3;
                move = true;
            }
            else if ((angle < (5 * Math.PI / 3 + min) && angle > (5 * Math.PI / 3 - min)) || (angle < (-Math.PI / 3 + min) && angle > (-Math.PI / 3 - min)))
            {
                angle = 5 * Math.PI / 3;
                move = true;
            }
            else if (angle < min && angle > -min)
            {
                angle = 0;
                move = true;
            }
            return move;
        }
    }
}
