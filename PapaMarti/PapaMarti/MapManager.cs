using Microsoft.Xna.Framework;
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
        Texture2D road;
        Rectangle roadRect;
        Vector2 roadOrigin;

        public readonly static int translation = 1725; //to calculate where the map needs to be placed on the screen
        public readonly static int innerCircleTranslation = 260; //for inner circle calculations
        double minPosition;
        double maxPosition;

        Texture2D arrow, marker, arrowText;
        Vector2 arrowLocation;
        Vector2 arrowOrigin;
        float arrowAngle;
        float arrowScale;
        float markerTextScale;
        float arrowTextScale;

        Quest primaryQuest;

        Texture2D sliceLock;
        int slicesOpen;

        Car car;
        Texture2D carImage;

        RoomData data;
        public MapLocation closestLocation
        {
            get
            {
                return data.getClosestLocation(angle, position);
            }
        }

        /// <summary>
        /// Making a new map manager to allow the player to explore the island
        /// </summary>
        /// <param name="angle">Angular location on the pizza map, give radians. Typical unit circle stuff, roads are at 0, pi/3, 2pi/3, pi, 4pi/3, 5pi/3</param>
        /// <param name="position">How far up or down they are on a road, 0 for closer to the outside, 1 for the inner ring.</param>
        public MapManager(ContentManager content, double angle, double position, Quest primaryQuest, int slicesOpen, RoomData data) : base(content)
        {
            this.angle = angle % (2 * Math.PI);
            this.position = position;

            map = content.Load<Texture2D>("MapTextures/temporary map"); // REPLACE THIS LATER WITH MAP TEXTURE
            mapSource = new Rectangle(0, 0, map.Width, map.Height);
            mapOrigin = new Vector2(map.Width / 2, map.Height / 2);

            //translation = 1700; //adjust this number to get the scaling right
            //innerCircleTranslation = 250;

            mapPosition = new Vector2(Game1.screenRect.Width / 2, Game1.screenRect.Height / 2 + (int)(translation - position * (translation - innerCircleTranslation)));

            minPosition = 0.005;
            maxPosition = 0.995;
            road = content.Load<Texture2D>("whitePixel"); //REPLACE LATER WITH ROAD TEXTURE
            int roadWidth = 30;
            roadRect = new Rectangle((Game1.screenRect.Width - roadWidth)/2, Game1.screenRect.Height / 2 + (int)(translation - position * (translation - innerCircleTranslation)), roadWidth, translation - innerCircleTranslation);
            roadOrigin = new Vector2((float)road.Width / 2f, road.Height + ((float)road.Height / (translation - innerCircleTranslation) * innerCircleTranslation));

            this.primaryQuest = primaryQuest;

            arrow = content.Load<Texture2D>("Arrow");
            marker = content.Load<Texture2D>("marker");
            arrowText = arrow;
            arrowLocation = new Vector2(0, 0);
            arrowAngle = 0f;
            arrowTextScale = 5f;
            markerTextScale = 5f;
            arrowScale = arrowTextScale;
            updateArrow();
            arrowOrigin = new Vector2(arrow.Width / 2f, arrow.Height / 2f);

            this.slicesOpen = slicesOpen;
            sliceLock = content.Load<Texture2D>("slice lock");

            this.data = data;

            car = new Car();
            carImage = content.Load<Texture2D>("Car");
        }

        private void updateArrow()
        {
            double setAngleDistance = Math.PI / 4.6;
            double setRadiusDistance = 0.5;

            angle = angle % (Math.PI * 2);
            if (angle < 0)
                angle += Math.PI * 2;
            double angleDiff = angle - primaryQuest.angle;
            double secondDiff = -2 * Math.PI + angle - primaryQuest.angle;
            double thirdDiff = 2 * Math.PI - primaryQuest.angle + angle;
            if (Math.Abs(thirdDiff) < Math.Abs(secondDiff))
                secondDiff = thirdDiff;
            if (Math.Abs(secondDiff) < Math.Abs(angleDiff))
                angleDiff = secondDiff;
            double radiusDiff = position - primaryQuest.radius;

            if (radiusDiff < 0)
                setRadiusDistance = 0.3;

            if(Math.Abs(angleDiff) < setAngleDistance && Math.Abs(radiusDiff) < setRadiusDistance)
            {
                arrowText = marker;
                arrowScale = markerTextScale;
                arrowAngle = (float)angle;
                arrowLocation.X = (float)(mapPosition.X + ((1 - primaryQuest.radius) * (translation - innerCircleTranslation) + innerCircleTranslation) * Math.Sin(angleDiff));
                arrowLocation.Y = (float)(mapPosition.Y - ((1 - primaryQuest.radius) * (translation - innerCircleTranslation) + innerCircleTranslation) * Math.Cos(angleDiff));
            }
            else if(Math.Abs(angleDiff) < setAngleDistance)
            {
                arrowScale = arrowTextScale;
                arrowText = arrow;
                arrowLocation.X = Game1.screenRect.Width / 2;
                if(radiusDiff > 0)
                {
                    arrowAngle = 0;
                    arrowLocation.Y = 20 + arrowText.Height * arrowScale / 2;
                }
                else
                {
                    arrowAngle = (float)Math.PI;
                    arrowLocation.Y = Game1.screenRect.Height - 20 - arrowText.Height * arrowScale / 2;
                }
            }
            else
            {
                arrowText = arrow;
                arrowScale = arrowTextScale;
                arrowLocation.Y = Game1.screenRect.Height / 2;
                if(angleDiff < 0)
                {
                    arrowAngle = (float)-Math.PI / 2;
                    arrowLocation.X = arrowText.Width * arrowScale / 2 + 20;
                }
                else
                {
                    arrowAngle = (float)Math.PI / 2;
                    arrowLocation.X = Game1.screenRect.Width - 20 - arrowText.Width * arrowScale / 2;
                }
            }
        }

        //makes sure that the position of the map matches the position of the player
        private void updatePosition()
        {
            mapPosition.Y = Game1.screenRect.Height / 2 + (int)(translation - position * (translation - innerCircleTranslation));
            roadRect.Y = Game1.screenRect.Height / 2 + (int)(translation - position * (translation - innerCircleTranslation));

        }

        /// <summary>
        /// A method to update the quest displayed on the map
        /// </summary>
        /// <param name="newQuest">The quest object of the quest that needs to have an arrow pointing to it</param>
        public void updatePrimaryQuest(Quest newQuest)
        {
            primaryQuest = newQuest;
        }

        public override GameStage getStage()
        {
            return GameStage.Exploring;
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            //map
            spriteBatch.Draw(map, mapPosition, mapSource, Color.White, (float)(angle - Math.PI / 2), mapOrigin, 6f, SpriteEffects.None, 0f);

            //roads
            spriteBatch.Draw(road, roadRect, null, Color.White, (float)(angle), roadOrigin, SpriteEffects.None, 0f);
            spriteBatch.Draw(road, roadRect, null, Color.White, (float)(angle - Math.PI / 3), roadOrigin, SpriteEffects.None, 0f);
            spriteBatch.Draw(road, roadRect, null, Color.White, (float)(angle + Math.PI / 3), roadOrigin, SpriteEffects.None, 0f);
            spriteBatch.Draw(road, roadRect, null, Color.White, (float)(angle + Math.PI), roadOrigin, SpriteEffects.None, 0f);
            spriteBatch.Draw(road, roadRect, null, Color.White, (float)(angle + 2 * Math.PI / 3), roadOrigin, SpriteEffects.None, 0f);
            spriteBatch.Draw(road, roadRect, null, Color.White, (float)(angle + 4 * Math.PI / 3), roadOrigin, SpriteEffects.None, 0f);

            //buildings
            data.drawLocations(spriteBatch, (float)angle, mapPosition);

            //locked slices
            float sliceAngle = (float)Math.PI / 6;
            for(int i = 1; i < 7; i++)
            {
                if(i > slicesOpen)
                {
                    spriteBatch.Draw(sliceLock, mapPosition, mapSource, Color.White, (float)angle - sliceAngle, mapOrigin, 6f, SpriteEffects.None, 0f);
                }
                sliceAngle += (float)Math.PI / 3;
            }

            //car (change later for animation and actual textures)
            car.draw(spriteBatch, carImage, new Rectangle((Game1.screenRect.Width - 225) / 2, (Game1.screenRect.Height - 150) / 2 - 50, 225, 150));

            //arrow
            spriteBatch.Draw(arrowText, arrowLocation, null, Color.White, arrowAngle, arrowOrigin, arrowScale, SpriteEffects.None, 0f);
        }
        public override void update(GameTime time)
        {
            KeyboardState kb = Keyboard.GetState();
            double angleSpeed = 0.005; //in radians per frame
            if ((kb.IsKeyDown(Keys.Left) || kb.IsKeyDown(Keys.A)) && canRotate())
            {
                if (position < minPosition)
                    angle += angleSpeed;
                else
                    angle += angleSpeed * 2;
                car.updateDirection(Direction.left);
            }
            if((kb.IsKeyDown(Keys.Right) || kb.IsKeyDown(Keys.D)) && canRotate())
            {
                if (position < minPosition)
                    angle -= angleSpeed;
                else
                    angle -= angleSpeed * 2;
                car.updateDirection(Direction.right);
            }
            double movementSpeed = 0.005;
            if((kb.IsKeyDown(Keys.Up) || kb.IsKeyDown(Keys.W)) && canMove() && position > 0)
            {
                position -= movementSpeed;
                car.updateDirection(Direction.up);
            }
            if((kb.IsKeyDown(Keys.Down) || kb.IsKeyDown(Keys.S)) && canMove() && position < 1)
            {
                position += movementSpeed;
                car.updateDirection(Direction.down);
            }

            car.updateTime();
            updatePosition();
            updateArrow();
        }
        public override bool isDone()
        {
            KeyboardState kb = Keyboard.GetState();
            return kb.IsKeyDown(Keys.Enter);
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
