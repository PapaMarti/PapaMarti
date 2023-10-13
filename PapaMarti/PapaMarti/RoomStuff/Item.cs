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
    public class Item
    {
        //the image of the item as it appears in a room or in inventory
        Texture2D texture;

        //the dialogue which appears when the item is interacted with
        string dialogue;

        //the x and y position of the object in the room tile grid, becomes inventory slot after added to inventory
        Point position;

        //the player for calculating whether the item has been interacted with
        Player player;

        //the room the item is in
        Room room;

        bool isInInventory;

        //the textcard which shows up when you interact with this object
        TextCard textCard;

        ContentManager content;

        public Item(Texture2D text, string dialog, Player player, Room room, ContentManager content): this(text, dialog, player, room, content, new Point(0, 0)) { }

        public Item(Texture2D text, string dialog, Player player, Room room, ContentManager content, Point pos)
        {
            this.texture = text;
            this.dialogue = dialog;
            this.position = pos;
            this.player = player;
            this.room = room;
            isInInventory = false;
            this.content = content;
            textCard = null;
        }

        public void setPosition(double x, double y)
        {
            position = new Point((int)x, (int)y);
        }

        public void changeImage(Texture2D text)
        {
            this.texture = text;
        }

        public bool update()
        {
            KeyboardState kb = Keyboard.GetState();

            bool isTouching = false;
            if (!isInInventory) //checking for player trying to interact with item in room
            {
                if (kb.IsKeyDown(Keys.Enter) && textCard == null)
                {
                    if (position.X > 0)
                    {
                        if(room.tiles[position.X - 1, position.Y].getRect().Contains(new Point((int)player.center.X, (int)player.center.Y)))
                        {
                            isTouching = true;
                        }
                    }
                    if(position.X < room.tiles.GetLength(0) - 1)
                    {
                        if (room.tiles[position.X + 1, position.Y].getRect().Contains(new Point((int)player.center.X, (int)player.center.Y)))
                        {
                            isTouching = true;
                        }
                    }
                    if(position.Y > 0)
                    {
                        if (room.tiles[position.X, position.Y - 1].getRect().Contains(new Point((int)player.center.X, (int)player.center.Y)))
                        {
                            isTouching = true;
                        }
                    }
                    if(position.Y < room.tiles.GetLength(1) - 1)
                    {
                        if (room.tiles[position.X, position.Y + 1].getRect().Contains(new Point((int)player.center.X, (int)player.center.Y)))
                        {
                            isTouching = true;
                        }
                    }

                    if (isTouching)
                    {
                        textCard = new TextCard(content, dialogue, String.Empty);
                    }
                }
            }
            if (textCard != null)
            {
                textCard.update();
                if (textCard.isDone())
                    textCard = null;
            }
            return isTouching;
        }
        public void draw(SpriteBatch spriteBatch)
        {
            if (!isInInventory)
            {
                spriteBatch.Draw(texture, room.tiles[position.X, position.Y].getRect(), Color.White);
            }
            else
            {

            }

            if(textCard != null)
            {
                textCard.draw(spriteBatch);
            }
        }
    }
}
