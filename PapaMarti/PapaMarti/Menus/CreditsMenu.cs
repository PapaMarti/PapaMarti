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
    public class CreditsMenu: Submenu
    {
        int timer;
        SpriteFont font;
        int namesTime, martiTime;
        Rectangle martiBackground, creditBackground, background;

        Texture2D papaMarti;
        Rectangle martiRectangle;
        Vector2 martiTextLocation;
        string martiText;

        string[] credits;
        Vector2[] locations;

        public CreditsMenu(ContentManager content) : base(content, "Credits")
        {
            timer = 0;
            font = content.Load<SpriteFont>("SpriteFont1");
            namesTime = 600;
            martiTime = 300;

            papaMarti = content.Load<Texture2D>("realMarti");
            martiRectangle = new Rectangle((Game1.screenRect.Width - papaMarti.Width) / 2, (Game1.screenRect.Height - papaMarti.Height) / 2, papaMarti.Width, papaMarti.Height);
            martiText = "This game is dedicated to the original Papa Marti";
            martiTextLocation = new Vector2((Game1.screenRect.Width - font.MeasureString(martiText).X) / 2, martiRectangle.Y + martiRectangle.Height + 20);

            credits = new string[]{"Scrum Master and Developer: Jacob Bamuel", "Primary Artist: Anna Bessmertnaya", "Developer and Artist: Karan Sharma", "Developer: Ella Whitney", "Developer: Annabel Tu"};
            locations = new Vector2[credits.Length];
            int buffer = 30;
            int height = buffer;
            int maxWidth = 0;
            for(int i = 0; i < credits.Length; i++)
            {
                height += buffer + (int)font.MeasureString(credits[i]).Y;
            }
            int y = (Game1.screenRect.Height - height) / 2;
            int tempY = y;
            for (int i = 0; i < credits.Length; i++)
            {
                int textWidth = (int)font.MeasureString(credits[i]).X;
                locations[i] = new Vector2((Game1.screenRect.Width - textWidth) / 2, tempY);
                tempY += buffer + (int)font.MeasureString(credits[i]).Y;
                if (textWidth > maxWidth)
                    maxWidth = textWidth;
            }

            if (Math.Max(martiRectangle.Width, font.MeasureString(martiText).X) > maxWidth)
            {
                maxWidth = (int)Math.Max(martiRectangle.Width, font.MeasureString(martiText).X);
            }
            maxWidth += buffer * 2;
            creditBackground = new Rectangle((Game1.screenRect.Width - maxWidth) / 2, y - buffer, maxWidth, (int)(locations[locations.Length - 1].Y + font.MeasureString(credits[credits.Length - 1]).Y - y + buffer * 2));
            martiBackground = new Rectangle((Game1.screenRect.Width - maxWidth) / 2, martiRectangle.Y - buffer, maxWidth, (int)(martiTextLocation.Y + font.MeasureString(martiText).Y - martiRectangle.Y + buffer * 2));
            background = martiBackground;
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
            spriteBatch.Draw(whitePixel, background, Color.White);

            if(timer > martiTime)
            {
                for(int i = 0; i < credits.Length; i++)
                {
                    spriteBatch.DrawString(font, credits[i], locations[i], Color.Black);
                }
            }
            else
            {
                spriteBatch.Draw(papaMarti, martiRectangle, Color.White);
                spriteBatch.DrawString(font, martiText, martiTextLocation, Color.Black);
            }
        }

        public override void update()
        {
            base.update();

            if(timer > martiTime)
            {
                background = creditBackground;
            }

            timer++;
        }
    }
}
