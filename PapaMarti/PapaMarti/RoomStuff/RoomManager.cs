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
        Quest quest;

        public RoomManager(ContentManager content, Task task) : base(content)
        {
            this.task = task;
            Texture2D playerText = content.Load<Texture2D>("whitePixel");
            player = new Player(new Rectangle(1800, 500, 60, 60), playerText);
            player = task.room.enter(player);
        }

        public override GameStage getStage()
        {
            return GameStage.Rooming;
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            task.room.draw(spriteBatch);
            player.draw(spriteBatch);
        }
        public override void update(GameTime time)
        {
            player = task.room.update(player);
        }
        public override bool isDone()
        {
            KeyboardState kb = Keyboard.GetState();
            return kb.IsKeyDown(Keys.Tab);
        }
    }
}
