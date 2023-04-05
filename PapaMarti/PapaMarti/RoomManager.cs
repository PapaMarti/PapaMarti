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
    class RoomManager: StageManager
    {
        MapLocation location;
        Player player;
        Room room;
        public RoomManager(ContentManager content, MapLocation location) : base(content)
        {
            this.location = location;
            room = location.room;

        }

        public override GameStage getStage()
        {
            return GameStage.Rooming;
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            room.draw(spriteBatch);
            player.draw(spriteBatch);
        }
        public override void update(GameTime time)
        {
            player = room.update(player);
        }
        public override bool isDone()
        {
            KeyboardState kb = Keyboard.GetState();
            return kb.IsKeyDown(Keys.Tab);
        }
    }
}
