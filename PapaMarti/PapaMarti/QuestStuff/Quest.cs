using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PapaMarti
{

    /*Quest types accounted for include:
     *Moving to a certain location
     */
    public class Quest {
        private Task[] tasks; //All "taskComplete" must be true for status to be Completed
        private int currentTask;

        public Quest(params Task[] tasks) {
            this.tasks = tasks;
        }

        public bool isQuestDone() {
            return getCurrentTask() == null;
        }

        public Task nextTask() {
            currentTask++;
            if(currentTask < tasks.Length)
                return tasks[currentTask];
            return null;
        }

        public Task getCurrentTask() {
            if(currentTask < tasks.Length)
                return tasks[currentTask];
            return null;
        }

        public void update() {
            if(getCurrentTask() == null)
                return;
            getCurrentTask().update();
            if(getCurrentTask().isDone())
                nextTask();
        }

        public void draw(SpriteBatch spriteBatch) {
            if(getCurrentTask() == null)
                return;
            getCurrentTask().draw(spriteBatch);
        }
        
    }
}
