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


        public SaveManager(Game1 game)
        {
            string[] saveFiles = new string[NUM_SAVES];
            try
            {
                using(StreamReader reader = new StreamReader(@"Content/Saves/GameDataSave.txt"))
                {

                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
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
    }
}
