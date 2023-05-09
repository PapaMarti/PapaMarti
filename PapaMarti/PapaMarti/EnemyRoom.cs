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

    class EnemyRoom : Room
    {
        public List<TextCard> before;
        public List<TextCard> after;
        public List<String[]> beforeStrings;
        public List<String[]> afterStrings;
        public EnemyRoom(string layout, MapLocation location, List<String[]> before, List<String[]> after) : base(parseRoomFile(layout), location)
        {
            this.beforeStrings = before;
            this.afterStrings = after;
            this.before = new List<TextCard>();
            this.after = new List<TextCard>();
        }

        public void contentify(ContentManager content)
        {
            try
            {
                foreach (String[] str in beforeStrings)
                {
                    before.Add(new TextCard(content, str[0], str[1]));
                }
                foreach (String[] str in afterStrings)
                {
                    after.Add(new TextCard(content, str[0], str[1]));
                }

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public override bool isDone()
        {
            return enemies.Count <= 0 && after.Count <= 0;
        }

        public override Player update(Player player)
        {
            if(before.Count > 0)
            {
                before[0].update();
                if (before[0].isDone())
                    before.RemoveAt(0);
            }
            else if(enemies.Count > 0)
            {
                return base.update(player);
            }
            else if(after.Count > 0)
            {
                after[0].update();
                if (after[0].isDone())
                    after.RemoveAt(0);
            }
            return player;
        }

        public override void draw(SpriteBatch spriteBatch){
            base.draw(spriteBatch);
            if(before.Count > 0)
            {
                before[0].draw(spriteBatch);
            }
            else if(after.Count > 0)
            {
                after[0].draw(spriteBatch);
            }
        }
    }
}
