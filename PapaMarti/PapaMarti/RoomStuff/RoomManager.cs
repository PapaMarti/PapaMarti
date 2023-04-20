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
        public MapLocation location;
        public Room room;
        public Player player;
        

        public RoomManager(ContentManager content, MapLocation location, Player player_) : base(content)
        {
            this.location = location;
            room = location.room;
            Texture2D playerText = content.Load<Texture2D>("whitePixel");
            Texture2D lifeBarText = content.Load<Texture2D>("whitePixel");
            player = new Player(new Rectangle(1800, 500, 60, 60), playerText, 300, lifeBarText);
            //player = new Player(new Rectangle(1800, 500, 60, 60), playerText, 300, playerText);
            //player = room.enter(player);
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
