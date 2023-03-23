using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;


namespace PapaMarti
{
    //Make it inherit CookingStage
    public class OvenScreen : CookingStage
    {
        
        //public Pizza pizza;
        public bool inOven;
        public bool isPizzaDone;
        public bool isBurnt;
        public int timeUntilDone; //tracks seconds, default at 30 seconds
        public int timeUntilBurnt; //tracks seconds, default at 10 seconds

        int cookingTimeSave;
        int burnTimeSave;

        public string splashText;
        string subtext;
        int realTimeTracker; //tracks frames

        //Temporary variables for testing
        public Vector2 startingLocation;
        public Vector2 cookingLocation;
        Rectangle tempPizza;
        Texture2D pizza;
        SpriteFont font;

        KeyboardState oldKB;
        //Press space to put pizza in the oven, press space again to take it out

        Rectangle oven;
        Texture2D ovenText;
        Texture2D textbox;

        bool finished;
        bool instructions;
        int animationTimer;
        public double score;

        int removePizzaTimes = 2;

        string textInstructions;
        int textCounter;

        string warningText;

        Rectangle textBox;

        MouseState oldMouse;

        Rectangle amazingPanel;
        Texture2D amazing;

        public OvenScreen(Pizza pizza, Texture2D texture, Texture2D ovenTexture, Texture2D textbox_, int _timeUntilDone, SpriteFont _font, Texture2D amazing) : base(pizza) //Probably can eliminate some parameters later I didn't know how to access some variables in this class
        {
            tempPizza = new Rectangle((Game1.screenRect.Width - 300) / 2, 750, 300, 300); //change to pizza variable
            startingLocation = new Vector2(tempPizza.X, tempPizza.Y);
            this.pizza = texture;
            inOven = false;
            isPizzaDone = false;
            isBurnt = false;
            timeUntilDone = _timeUntilDone;
            cookingTimeSave = _timeUntilDone;
            timeUntilBurnt = 10;
            burnTimeSave = 10;
            realTimeTracker = 0;

            splashText = "Press SPACE to put the pizza in the oven";
            subtext = "Press ENTER to finish";
            warningText = "You can remove the pizza from the oven " + removePizzaTimes + " more time(s)";

            font = _font;
            oldKB = Keyboard.GetState();
            textbox = textbox_;

            oven = new Rectangle(0, 0, Game1.screenRect.Width, Game1.screenRect.Height);
            cookingLocation = new Vector2((Game1.screenRect.Width - 300) / 2, 300);
            ovenText = ovenTexture;
            instructions = true;
            score = 0;
            animationTimer = 0;
            finished = false;
            textCounter = 2;
            textInstructions = "Drag the pizza into the oven to cook.\nTry to get as close to the optimal cooking time as you can\n to earn maximum points.";

            textBox = new Rectangle((Game1.screenRect.Width - 900) / 2, (Game1.screenRect.Height - 200) / 2, 900, 200);
            oldMouse = Mouse.GetState();

            this.amazing = amazing;
            this.amazingPanel = new Rectangle((Game1.screenRect.Width-400) / 2, (Game1.screenRect.Height - 400) / 2, 400, 400);
        }

        //TEMPORARY, constructor will take in a texture
        
        
        /*Test these cases ->
         Pizza is put into oven and taken out when done
        Pizza is put into oven and taken out when burnt
        Pizza is put into oven and taken out before done, then put back in and taken out when done
        Pizza is put into oven and taken out before done, then put back in and taken out when burnt
        */

        //Updates cooking timer
        public override void update(GameTime _gameTime)
        {
            KeyboardState kb = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            if (instructions)
            {
                if (mouse.LeftButton == ButtonState.Released && oldMouse.LeftButton == ButtonState.Pressed)
                {
                    textCounter--;
                    if (textCounter == 0)
                    {
                        instructions = false;
                    }
                    else
                    {
                        textInstructions = "You can only take the pizza out twice before it gets ruined from\noverhandling, so use them wisely!";
                    }
                }
            }
            else  if (finished)
            {

            }
            else if (inOven)
            {
                realTimeTracker++;
                
                if (realTimeTracker % 60 == 0)
                {
                    animationTimer++;
                    if (isPizzaDone)
                    {
                        timeUntilBurnt--;
                        if (timeUntilBurnt <= 0)
                        {
                            isBurnt = true;
                            
                            
                            
                        }
                    }
                    else
                    {

                        timeUntilDone--;
                        splashText = "Take the pizza out of the oven after " + cookingTimeSave + " seconds!";
                        if (timeUntilDone <= 0)
                        {
                            isPizzaDone = true;
                            
                        }
                    }
                }
               

                /*if (oldKB.IsKeyDown(Keys.Space) && !kb.IsKeyDown(Keys.Space) && removePizzaTimes > 0) //Press space while in oven to take pizza out
                {
                    
                    
                }*/

                if (mouse.LeftButton == ButtonState.Pressed && isInRectangle(mouse.X, mouse.Y, tempPizza))
                {
                    tempPizza.X += mouse.X - oldMouse.X;
                    tempPizza.Y += mouse.Y - oldMouse.Y;
                }
                if (mouse.LeftButton == ButtonState.Released && oldMouse.LeftButton == ButtonState.Pressed && removePizzaTimes > 0) //STARTHERE
                {
                    double startingDistance = getDistance(mouse.X, mouse.Y, startingLocation);
                    double ovenDistance = getDistance(mouse.X, mouse.Y, cookingLocation);
                    if (startingDistance < ovenDistance && removePizzaTimes > 0)
                    {

                        inOven = false;
                        score = getAccuracy();
                        splashText = "Drag the pizza into the oven to cook";
                        tempPizza.X = (int)startingLocation.X;
                        tempPizza.Y = (int)startingLocation.Y;
                        removePizzaTimes--;
                        if (removePizzaTimes == 0)
                        {
                            warningText = "The pizza will become damaged if you handle it again!";
                        }
                        else
                        {
                            warningText = "You can remove the pizza from the oven " + removePizzaTimes + " more time(s)";
                        }
                    }
                    else
                    {
                        tempPizza.X = (int)cookingLocation.X;
                        tempPizza.Y = (int)cookingLocation.Y;
                    }
                }
            }
            else
            {
                if (isBurnt)
                {
                    splashText = "The pizza is burnt! Press BACKSPACE to restart";
                    if (oldKB.IsKeyDown(Keys.Back) && !kb.IsKeyDown(Keys.Back))
                    {
                        reset();
                    }
                }
                else
                {
                    if (mouse.LeftButton == ButtonState.Pressed && isInRectangle(mouse.X, mouse.Y, tempPizza) && removePizzaTimes > 0)
                    {
                        tempPizza.X += mouse.X - oldMouse.X;
                        tempPizza.Y += mouse.Y - oldMouse.Y;
                    }

                    if (mouse.LeftButton == ButtonState.Released && oldMouse.LeftButton == ButtonState.Pressed && removePizzaTimes > 0)
                    {
                        double startingDistance = getDistance(mouse.X, mouse.Y, startingLocation);
                        double ovenDistance = getDistance(mouse.X, mouse.Y, cookingLocation);
                        if (ovenDistance < startingDistance)
                        {
                            inOven = true;
                            tempPizza.X = (int)cookingLocation.X;
                            tempPizza.Y = (int)cookingLocation.Y;
                        }
                        else
                        {
                            tempPizza.X = (int)startingLocation.X;
                            tempPizza.Y = (int)startingLocation.Y;
                        }
                    }

                    /*if (oldKB.IsKeyDown(Keys.Space) && !kb.IsKeyDown(Keys.Space) && removePizzaTimes > 0) //Press space to put pizza in oven
                    {
                        inOven = true;
                        tempPizza.X = (int)cookingLocation.X;
                        tempPizza.Y = (int)cookingLocation.Y;
                    }*/
                }

                if (oldKB.IsKeyDown(Keys.Enter) && !kb.IsKeyDown(Keys.Enter))
                {
                    finished = true;
                    splashText = "Congrats! You have successfully made a pizza";
                    tempPizza.X = (Game1.screenRect.Width - tempPizza.Width) / 2;
                    tempPizza.Y = (Game1.screenRect.Height - tempPizza.Height) /2;
                    //finish
                }

            }
            oldKB = kb;
            oldMouse = mouse;
        }

        public double getDistance(int x, int y, Vector2 point)
        {
            return Math.Sqrt(Math.Pow(point.X - x, 2) + Math.Pow(point.Y - y, 2));
        }
        public bool isInRectangle(int x, int y, Rectangle rect)
        {
            if (x >= rect.X && x <= rect.X + rect.Width && y >= rect.Y && y <= rect.Y + rect.Height)
            {
                return true;

            }
            return false;
        }
        public override double getAccuracy() //Percent difference between optimal cooking time and current cooking time. If pizza is burnt, 0 points are earned
        {
            double difference = 0;
            if (isBurnt)
            {
                return 0;
            }
            else if (isPizzaDone)
            {
                difference = Math.Abs((timeUntilBurnt - burnTimeSave * 1.0) / burnTimeSave) * 100;
                difference = 1000 - difference * 10;
            }
            else
            {
                difference = Math.Abs((timeUntilDone - cookingTimeSave * 1.0) / cookingTimeSave) * 100;
                difference = difference * 10;
            }
            
            return Math.Round(difference) ;

        }
        private void reset()
        {
            tempPizza = new Rectangle((int)startingLocation.X, (int)startingLocation.Y, 200, 200);
            inOven = false;
            isPizzaDone = false;
            isBurnt = false;
            timeUntilDone = cookingTimeSave;
            timeUntilBurnt = 10;
            realTimeTracker = 0;
            score = 0;

            splashText = "Drag the pizza into the oven to cook";
        }

        public override void draw(SpriteBatch _spriteBatch)
        {
            
            if (instructions)
            {
                _spriteBatch.Draw(textbox, textBox, Color.White);

                _spriteBatch.DrawString(font, textInstructions, new Vector2((Game1.screenRect.Width - 900) / 2 + 20, (Game1.screenRect.Height - 200) / 2 + 20), Color.Black);
            }
            else if (finished)
            {
                _spriteBatch.DrawString(font, "Final Score: " + score + "pts", new Vector2((Game1.screenRect.Width - font.MeasureString("Final Score: " + score + "pts").X) / 2, 1000), Color.Gold);

                _spriteBatch.Draw(amazing, amazingPanel, Color.White);
            }
            else
            {
                if (inOven)
                {
                    if (animationTimer % 2 == 0)
                    {
                        _spriteBatch.Draw(ovenText, oven, new Rectangle(240, 0, 240, 135), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(ovenText, oven, new Rectangle(480, 0, 240, 135), Color.White);
                    }
                    _spriteBatch.DrawString(font, "Score: ???", new Vector2(Game1.screenRect.Width - 230, Game1.screenRect.Height - 70), Color.Gold);
                }
                else
                {
                    _spriteBatch.Draw(ovenText, oven, new Rectangle(0, 0, 240, 135), Color.White);
                    _spriteBatch.DrawString(font, "Score: " + score, new Vector2(Game1.screenRect.Width - 230, Game1.screenRect.Height - 70), Color.Gold);
                    _spriteBatch.DrawString(font, subtext, new Vector2((Game1.screenRect.Width - font.MeasureString(subtext).X) / 2, Game1.screenRect.Height - 110), Color.Black);
                    
                }
                _spriteBatch.DrawString(font, warningText, new Vector2((Game1.screenRect.Width - font.MeasureString(warningText).X) / 2, Game1.screenRect.Height - 70), Color.Red);
            }
            if (!instructions)
            {
                if (isBurnt)
                {
                    _spriteBatch.Draw(pizza, tempPizza, Color.Gray);
                }
                else if (isPizzaDone)
                {
                    _spriteBatch.Draw(pizza, tempPizza, Color.Gold);
                }
                else
                {
                    _spriteBatch.Draw(pizza, tempPizza, Color.White);
                }
                if (finished || removePizzaTimes > 0)
                {
                    _spriteBatch.DrawString(font, splashText, new Vector2((Game1.screenRect.Width - font.MeasureString(splashText).X) / 2, 100), Color.Blue);
                }
                
                if (inOven && !isPizzaDone && !isBurnt) //to be removed
                {
                    _spriteBatch.DrawString(font, timeUntilDone + "", new Vector2(100, Game1.screenRect.Height - 180), Color.Red);
                }
            }



            



            //_spriteBatch.DrawString(font, "In Oven: " + inOven, new Vector2(0, 200), Color.Blue);
            //_spriteBatch.DrawString(font, "Is Done: " + isDone, new Vector2(0, 230), Color.Blue);
            //_spriteBatch.DrawString(font, "Is Bunrt: " + isBurnt, new Vector2(0, 260), Color.Blue);

            

            
            
            
        }

        public override CookStage getStage() {
            return CookStage.Cooking;
        }

        public override bool isDone() {
            return finished;
        }
    }
}
