using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PapaMarti
{
    public class DialogueRoom : EmptyRoom
    {
        String[] dialogue;
        List<TextCard> textCards;
        bool loaded;

        public DialogueRoom(string layout, MapLocation location, params String[] text) : base(@"Content\" + layout + ".txt", location)
        {
            dialogue = text;
            textCards = new List<TextCard>();
            loaded = false;
        }

        public override bool isDone()
        {
            return textCards.Count == 0 && loaded && enemies.Count == 0;
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
            if(textCards.Count > 0)
            {
                textCards[0].draw(spriteBatch);
            }
        }

        public override Player update(Player player)
        {
            if(textCards.Count > 0)
            {
                textCards[0].update();
                if (textCards[0].isDone())
                    textCards.RemoveAt(0);
            }
            return base.update(player);
        }

        public void loadTextures(ContentManager content)
        {
            if (loaded)
                return;

            loaded = true;
            char seperator = '|';

            foreach(String str in dialogue)
            {
                int index = str.IndexOf(seperator);

                if(index != -1)
                {
                    textCards.Add( new TextCard(content, str.Substring(0, index + 1), str.Substring(index + 1, str.Length - index - 1)) );
                }
                else
                {
                    textCards.Add(new TextCard(content, str, String.Empty));
                }
            }
        }
    }
}
