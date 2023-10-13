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
    class Menu
    {
        bool done;

        Texture2D whitePixel;
        SpriteFont titleFont;

        static int buttonWidth = (int)(Game1.screenRect.Width * 0.5);
        static int buttonHeight = 100;
        static int buffer = 20;

        List<Button> buttons;

        List<TextCard> cards;

        Game1 game;

        Submenu submenu;

        //in the future the menu constructor will need a quest data thing to use for list of quests
        public Menu(Game1 game, ContentManager content, bool tutorial)
        {
            this.game = game;

            done = false;

            whitePixel = content.Load<Texture2D>("whitePixel");
            titleFont = content.Load<SpriteFont>("ButtonTitle");

            submenu = null;

            Rectangle placeholder = new Rectangle(0, 0, buttonWidth, buttonHeight);

            buttons = new List<Button>();

            buttons.Add(new Button(whitePixel, placeholder, "Resume", titleFont));
            buttons.Add(new Button(whitePixel, placeholder, "Settings", titleFont));
            buttons.Add(new Button(whitePixel, placeholder, "Quit", titleFont));
            rearrange();

            cards = new List<TextCard>();
            if(tutorial)
                cards.Add(new TextCard(content, "Welcome to the menu! You can click the \"Settings\" button to see a list of adjustable settings. You can also quit the game using the \"Quit\" button, resume the game using the \"Resume\" button, and save at any point with the \"save\" button.", String.Empty));
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(whitePixel, Game1.screenRect, Game1.shaded);

            if (submenu == null)
            {
                foreach (Button button in buttons)
                {
                    button.draw(spriteBatch);
                }

                if (cards.Count > 0)
                    cards[0].draw(spriteBatch);
            }
            else
            {
                submenu.draw(spriteBatch);
            }
        }

        public void update()
        {
            if(cards.Count > 0 && submenu == null)
            {
                cards[0].update();
                if (cards[0].isDone())
                    cards.RemoveAt(0);
            }
            else if(submenu == null)
            {
                foreach (Button button in buttons)
                {
                    button.update();
                    if (button.wasPressed())
                    {
                        if (button.title.Equals("Resume"))
                        {
                            done = true;
                        }
                        else if (button.title.Equals("Quit"))
                        {
                            game.Exit();
                        }
                        else if (button.title.Equals("Settings"))
                        {
                            submenu = new SettingsMenu(game.Content);
                        }
                    }
                }
            }
            else
            {
                submenu.update();
                if (submenu.isDone())
                {
                    submenu = null;
                }
            }
        }

        public bool isDone()
        {
            return done;
        }

        private void rearrange()
        {
            int height = buttonHeight * buttons.Count + (buttons.Count - 1) * buffer;
            int currentY = (Game1.screenRect.Height - height) / 2;
            int currentX = (Game1.screenRect.Width - buttonWidth) / 2;

            foreach(Button button in buttons)
            {
                button.rectangle = new Rectangle(currentX, currentY, buttonWidth, buttonHeight);
                currentY += buttonHeight + buffer;
            }
        }

        public void reset()
        {
            submenu = null;
            done = false;
        }
    }
}
