using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PapaMarti {
    public enum GameStage {
        Cooking,
        Cutscene,
        Exploring,
        Fighting
    }


    public abstract class StageManager {
        protected readonly ContentManager content;

        public StageManager(ContentManager content) {
            this.content = content;
        }

        public abstract GameStage getStage();
        public abstract void draw(SpriteBatch spriteBatch);
        public abstract void update(GameTime time);
        public abstract bool isDone();
    }
}
