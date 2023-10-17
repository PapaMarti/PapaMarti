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
    public enum SaveMenuPurpose
    {
        Save,
        NewGame,
        LoadGame
    }

    public class SaveMenu: Submenu
    {
        SpriteFont font;
        SaveMenuPurpose purpose; //the goal of opening the menu
        Button[] saveSlots;
        SaveManager saveManager;
        bool done;
        Texture2D whitePixel;

        public SaveMenu(ContentManager content, string name, SaveManager saveManager, SaveMenuPurpose purpose): base(content, name)
        {
            this.saveManager = saveManager;
            font = content.Load<SpriteFont>("ButtonTitle");
            this.purpose = purpose;
            done = false;

            whitePixel = content.Load<Texture2D>("whitePixel");
            int buffer = 20;
            int height = (Game1.screenRect.Height - buffer * (SaveManager.NUM_SAVES + 1)) / SaveManager.NUM_SAVES;
            int y = buffer;
            int width = Game1.screenRect.Width / 2;
            int x = (Game1.screenRect.Width - width) / 2;
            saveSlots = new Button[SaveManager.NUM_SAVES];
            for(int i = 0; i < SaveManager.NUM_SAVES; i++, y += buffer + height)
            {
                saveSlots[i] = new Button(whitePixel, new Rectangle(x, y, width, height), "Save " + (i + 1) + " - " + saveManager.getDescription(i), font);
            }
        }

        public override void update()
        {
            base.update();
            for(int i = 0; i < SaveManager.NUM_SAVES; i++)
            {
                saveSlots[i].update();
                if (saveSlots[i].wasPressed())
                {
                    if(purpose == SaveMenuPurpose.Save)
                    {
                        saveManager.save(i);
                        done = true;
                    }
                    else if(purpose == SaveMenuPurpose.NewGame)
                    {
                        saveManager.newSave(i);
                        done = true;
                    }
                    else if(purpose == SaveMenuPurpose.LoadGame)
                    {
                        saveManager.loadSave(i);
                        done = true;
                    }
                }
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
            for (int i = 0; i < SaveManager.NUM_SAVES; i++)
            {
                saveSlots[i].draw(spriteBatch);
            }
        }

        public bool startedSave()
        {
            return done;
        }
    }
}
