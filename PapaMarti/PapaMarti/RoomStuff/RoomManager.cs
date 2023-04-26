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
    public class RoomManager: StageManager
    {
        Player player;
        Room room;

        public RoomManager(ContentManager content, Room room) : base(content)
        {
            this.room = room;
            int playerHeight = 85;
            int playerWidth = 65;
            Texture2D playerText = content.Load<Texture2D>("whitePixel");
            player = new Player(new Rectangle(1800, 500, playerWidth, playerHeight), playerText);
            player = room.enter(player);
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
            if(room.isDone())
                QuestTracker.advanceMainquest();
            KeyboardState kb = Keyboard.GetState();
            return kb.IsKeyDown(Keys.Tab);
        }
    }
}
