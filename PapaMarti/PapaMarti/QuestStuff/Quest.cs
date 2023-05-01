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
        private StageManager[] rooms; //All "taskComplete" must be true for status to be Completed
        private int currentTask;

        public Quest(params StageManager[] rooms) {
            this.rooms = rooms;
        }

        public void contentify(ContentManager content, Player p)
        {
            foreach (StageManager s in rooms) s.contentify(content, p);
        }

        public bool isQuestDone() {
            return getCurrentTask() == null;
        }

        public StageManager nextTask() {
            currentTask++;
            if(currentTask < rooms.Length)
                return rooms[currentTask];
            return null;
        }

        public StageManager getCurrentTask() {
            if(currentTask < rooms.Length)
                return rooms[currentTask];
            return null;
        }
                
    }
}
