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

    public class TitleScreen
    {
        Button newGame;
        Button oldGame;
        Button settings;
        Button credits;
        Button quit;
        Game1 game;
        SaveManager saveManager;
        ContentManager content;
        Texture2D whitePixel;
        Submenu submenu;
        bool done;

        public TitleScreen(Game1 game, SaveManager saveManager)
        {
            this.game = game;
            this.saveManager = saveManager;
            content = game.Content;
            whitePixel = content.Load<Texture2D>("whitePixel");
            submenu = null;
            done = false;

            bool hasSave = false;
            for(int i = 0; i < SaveManager.NUM_SAVES; i++)
            {
                if (saveManager.saveExists[i])
                    hasSave = true;
            }

            int buttonWidth = Game1.screenRect.Width / 2;
            int buttonHeight = 75;
            int buffer = 20;
            SpriteFont buttonFont = content.Load<SpriteFont>("ButtonTitle");
            quit = new Button(whitePixel, new Rectangle(Game1.screenRect.Width / 2 - buttonWidth / 2, Game1.screenRect.Height - buffer - buttonHeight, buttonWidth, buttonHeight), "Quit", buttonFont);
            credits = new Button(whitePixel, new Rectangle(quit.rectangle.X, quit.rectangle.Y - buffer - buttonHeight, buttonWidth, buttonHeight), "Credits", buttonFont);
            settings = new Button(whitePixel, new Rectangle(credits.rectangle.X, credits.rectangle.Y - buffer - buttonHeight, buttonWidth, buttonHeight), "Settings", buttonFont);

            if (hasSave) //making both buttons for loading and new game
            {
                oldGame = new Button(whitePixel, new Rectangle(settings.rectangle.X, settings.rectangle.Y - buffer - buttonHeight, buttonWidth, buttonHeight), "Load Game", buttonFont);
                newGame = new Button(whitePixel, new Rectangle(oldGame.rectangle.X, oldGame.rectangle.Y - buffer - buttonHeight, buttonWidth, buttonHeight), "New Game", buttonFont);
            }
            else //making just new game button
            {
                oldGame = null;
                newGame = new Button(whitePixel, new Rectangle(settings.rectangle.X, settings.rectangle.Y - buffer - buttonHeight, buttonWidth, buttonHeight), "New Game", buttonFont);
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            newGame.draw(spriteBatch);
            if (oldGame != null)
                oldGame.draw(spriteBatch);
            settings.draw(spriteBatch);
            credits.draw(spriteBatch);
            quit.draw(spriteBatch);

            if (submenu != null)
            {
                submenu.draw(spriteBatch);
            }
        }
        public void update()
        {
            if(submenu != null)
            {
                submenu.update();
                if (submenu.GetType() == typeof(SaveMenu))
                {
                    if (((SaveMenu)submenu).startedSave())
                    {
                        done = true;
                    }
                }
                if (submenu.isDone())
                {
                    submenu = null;
                }
            }
            else
            {
                newGame.update();
                if (oldGame != null)
                    oldGame.update();
                settings.update();
                credits.update();
                quit.update();

                if (newGame.wasPressed())
                {
                    submenu = new SaveMenu(content, "New Game", saveManager, SaveMenuPurpose.NewGame);
                }
                else if (settings.wasPressed())
                {
                    submenu = new SettingsMenu(content, saveManager);
                }
                else if (credits.wasPressed())
                {
                    submenu = new CreditsMenu(content);
                }
                else if (quit.wasPressed())
                {
                    game.Exit();
                }
                else if (oldGame != null)
                {
                    if (oldGame.wasPressed())
                    {
                        submenu = new SaveMenu(content, "Load Game", saveManager, SaveMenuPurpose.LoadGame);
                    }
                }
            }
        }
        public bool isDone()
        {
            return done;
        }
    }
}
