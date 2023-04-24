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
        private Room[] rooms; //All "taskComplete" must be true for status to be Completed
        private int currentTask;

        public Quest(params Room[] rooms) {
            this.rooms = rooms;
        }

        public bool isQuestDone() {
            return getCurrentTask() == null;
        }

        public Room nextTask() {
            currentTask++;
            if(currentTask < rooms.Length)
                return rooms[currentTask];
            return null;
        }

        public Room getCurrentTask() {
            if(currentTask < rooms.Length)
                return rooms[currentTask];
            return null;
        }
                
    }
}
