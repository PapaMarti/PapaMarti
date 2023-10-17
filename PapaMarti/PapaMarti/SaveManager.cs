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
    //currently there are 3 save slots and one file for saving game data such as number of saves and settings

    public class SaveManager
    {
        Game1 game;
        public static int NUM_SAVES = 3;

        //the game data in order as it is in the file
        public bool[] saveExists; //whether each save file has data

        //game volume
        public int volume;

        //whether the game should use the pizza the player made or the default sprite for weapons
        public bool usePlayerPizza;

        string[] saveDescription;

        public SaveManager(Game1 game)
        {
            saveDescription = new string[NUM_SAVES];
            string[] saveFiles = new string[NUM_SAVES];
            try
            {
                using (StreamReader reader = new StreamReader(@"Content/Saves/GameDataSave.txt"))
                {
                    //checking whether each save file has data
                    saveExists = new bool[NUM_SAVES];
                    for (int i = 0; i < NUM_SAVES; i++)
                    {
                        saveExists[i] = Convert.ToBoolean(reader.ReadLine().Split(' ')[1]);

                    }

                    volume = Convert.ToInt32(reader.ReadLine().Split(' ')[1]);

                    usePlayerPizza = Convert.ToBoolean(reader.ReadLine().Split(' ')[1]);
                }
                for(int i = 0; i < NUM_SAVES; i++)
                {
                    if (saveExists[i])
                    {
                        using (StreamReader reader = new StreamReader(@"Content/Saves/Save" + i + "/Save" + i + ".txt"))
                        {
                            saveDescription[i] = reader.ReadLine().Split(' ')[1];
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public bool newSave(int saveSlot)
        {
            return false;
        }

        public bool loadSave(int saveSlot)
        {
            return false;
        }

        public bool save(int saveSlot)
        {
            try
            {

            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }

        public bool saveSettings(int volume, bool playerPizza)
        {
            return saveGameData(saveExists, volume, playerPizza);
        }

        public bool saveGameData(bool[] saveExists, int volume, bool playerPizza)
        {
            this.saveExists = saveExists;
            this.volume = volume;
            usePlayerPizza = playerPizza;

            try
            {
                using (StreamWriter writer = new StreamWriter(@"Content/Saves/GameDataSave.txt", false))
                {
                    for(int i = 0; i < NUM_SAVES; i++)
                    {
                        writer.WriteLine("Save" + i + " " + saveExists);
                    }

                    writer.WriteLine("Volume " + volume);

                    writer.WriteLine("PlayerPizza " + playerPizza);
                }
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public string getDescription(int saveSlot)
        {
            if (saveExists[saveSlot])
            {
                return saveDescription[saveSlot];
            }
            return "No save data";
        }
    }
}
