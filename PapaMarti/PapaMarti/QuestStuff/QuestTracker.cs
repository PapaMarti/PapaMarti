using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PapaMarti {
    public class QuestTracker {
        public static MapLocation[] mapLocations = new MapLocation[] {
            new MapLocation(Math.PI / 6, 0.8, "wood", Color.White, 1.1f, (float) (Math.PI / 3)),
            new MapLocation(0.22, 0.52, "wood", Color.Red, 1f, 0f),
            new MapLocation(0.2, 0.16, "wood", Color.Yellow, 1.2f, -0.2f),
            new MapLocation(Math.PI / 6, 0.15, "wood", Color.Green, 0.9f, (float)(-Math.PI / 6)),
            new MapLocation(0.85, 0.2, "wood", Color.Purple, 1.3f, (float)(-Math.PI / 3)),
            new MapLocation(0.8, 0.55, "wood", Color.Orange, 1f, (float)(-Math.PI / 3))};

        public static Queue<Quest> mainlineQuest;
        public static List<Quest> activeSideQuests;
        public static Queue<Quest> inactiveSideQuests;

        static QuestTracker() {
            mainlineQuest.Enqueue(new Quest(RoomData.generateRoom("roomOne.txt")));
            // here is where quests are queued into the mainquest queue
            // here is where sidequests are queued into the sidequest queue, in the order in which theyre unlocked

            activeSideQuests.Add(inactiveSideQuests.Dequeue());
        }

        public static void initializeTextures(ContentManager content) {
            foreach(MapLocation m in mapLocations)
                m.loadTexture(content);
        }

        public static RoomManager enterRoom(ContentManager content, MapLocation location) {
            if(mainlineQuest.Peek().getCurrentTask().location == location) {
                if(mainlineQuest.Peek().getCurrentTask().isDone())
                    return new RoomManager(content, mainlineQuest.Peek().nextTask());
                else
                    return new RoomManager(content, mainlineQuest.Peek().getCurrentTask());
            }

            foreach(Quest q in activeSideQuests) {
                if(q.getCurrentTask().location == location)
                    return null;
            }

            return null;
        }

        public static void advanceMainquest() {
            mainlineQuest.Peek().nextTask();
            if(mainlineQuest.Peek().isQuestDone())
                mainlineQuest.Dequeue();
        }
    }

    
}
