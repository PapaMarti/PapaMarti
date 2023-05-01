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
        Fighting,
        Rooming
    }


    public abstract class StageManager {
        public MapLocation location
        {
            get; private set;
        }

        public StageManager(MapLocation location)
        {
            this.location = location;
        }

        public abstract GameStage getStage();
        public abstract void draw(SpriteBatch spriteBatch);
        public abstract void update(GameTime time);
        public abstract bool isDone();
        public abstract void contentify(ContentManager content, Player p);
    }
}
