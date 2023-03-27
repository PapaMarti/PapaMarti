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
    class RoomManager: StageManager
    {
        MapLocation location;
        public RoomManager(ContentManager content, MapLocation location) : base(content)
        {
            this.location = location;
        }

        public override GameStage getStage()
        {
            return GameStage.Rooming;
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            Console.WriteLine(location.angle + ", " + location.radius);
            spriteBatch.Draw(location.texture, new Rectangle(0, 0, location.texture.Width, location.texture.Height), location.color);
        }
        public override void update(GameTime time)
        {

        }
        public override bool isDone()
        {
            return false;
        }
    }
}
