﻿using Microsoft.Xna.Framework;
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

    class QuestMenu
    {
        //to be done
        public QuestMenu()
        {

        }

        public void draw(SpriteBatch spriteBatch)
        {

        }
        public void update()
        {

        }
        public bool isDone()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Escape);
        }
    }
}
