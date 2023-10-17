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
        SaveManager saveManager;
        ContentManager content;
        Texture2D whitePixel;
        Submenu submenu;

        public TitleScreen(ContentManager content, SaveManager saveManager)
        {
            this.saveManager = saveManager;
            this.content = content;
            whitePixel = content.Load<Texture2D>("whitePixel");
            submenu = null;

            bool hasSave = false;
            for(int i = 0; i < SaveManager.NUM_SAVES; i++)
            {
                if (saveManager.saveExists[i])
                    hasSave = true;
            }

            int buttonWidth = 300;
            int buttonHeight = 100;
            int buffer = 20;
            SpriteFont buttonFont = content.Load<SpriteFont>("ButtonTitle");
            settings = new Button(whitePixel, new Rectangle(Game1.screenRect.Width / 2 - buttonWidth / 2, Game1.screenRect.Height - buffer - buttonHeight, buttonWidth, buttonHeight), "Settings", buttonFont);

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
            if (submenu != null)
            {
                submenu.draw(spriteBatch);
            }
            else
            {
                newGame.draw(spriteBatch);
                if (oldGame != null)
                    oldGame.draw(spriteBatch);
                settings.draw(spriteBatch);
            }
        }
        public void update()
        {
            if(submenu != null)
            {
                if (submenu.isDone())
                {
                    if(submenu.GetType() == typeof(SaveMenu))
                    {
                        if (!((SaveMenu)submenu).startedSave())
                        {
                            submenu = null;
                        }
                    }
                    else if(submenu.GetType() == typeof(SettingsMenu))
                    {
                        submenu = null;
                    }
                }
            }
            else
            {
                newGame.update();
                if (oldGame != null)
                    oldGame.update();
                settings.update();
                if (newGame.wasPressed())
                {
                    submenu = new SaveMenu(content, "New Game", saveManager, SaveMenuPurpose.NewGame);
                }
                else if (settings.wasPressed())
                {
                    submenu = new SettingsMenu(content, saveManager);
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
            if(submenu != null)
            {
                if(submenu.GetType() == typeof(SaveMenu)){
                    if (submenu.isDone())
                    {
                        if (((SaveMenu)submenu).startedSave())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
