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
            new MapLocation(Math.PI / 6, 0.8, "p", Color.White, 20f, (float) (Math.PI / 3), @"..\..\..\..\PapaMartiContent\roomOne.txt"),
            new MapLocation(Math.PI / 12, 0.4, "g5", Color.Red, 1f, 0f, @"..\..\..\..\PapaMartiContent\roomOne.txt"),
            new MapLocation(Math.PI * 3 / 12, 0.45, "g1", Color.Yellow, 1.2f, -0.2f, @"..\..\..\..\PapaMartiContent\roomOne.txt"),
            //new MapLocation(Math.PI / 6, 0.15, "wood", Color.Green, 0.9f, (float)(-Math.PI / 6), @"..\..\..\..\PapaMartiContent\roomOne.txt"),
            //new MapLocation(0.85, 0.2, "wood", Color.Purple, 1.3f, (float)(-Math.PI / 3), @"..\..\..\..\PapaMartiContent\roomOne.txt"),
            /*new MapLocation(0.8, 0.55, "wood", Color.Orange, 1f, (float)(-Math.PI / 3), @"..\..\..\..\PapaMartiContent\roomOne.txt")*/};

        public static Queue<Quest> mainlineQuest;
        public static List<Quest> activeSideQuests;
        public static Queue<Quest> inactiveSideQuests;

        public static MapLocation getClosestLocation(double angle, double radius) {
            double r = (1 - radius) * (MapManager.translation - MapManager.innerCircleTranslation) + MapManager.innerCircleTranslation;
            MapLocation closest = mapLocations[0];
            double minDistance = int.MaxValue;
            for(int i = 0; i < mapLocations.Length; i++) {
                double placeRadius = (1 - mapLocations[i].radius) * (MapManager.translation - MapManager.innerCircleTranslation) + MapManager.innerCircleTranslation;
                double distance = Math.Sqrt(Math.Pow(r, 2) + Math.Pow(placeRadius, 2) - 2 * r * placeRadius * Math.Cos(angle - mapLocations[i].angle));
                if(distance < minDistance) {
                    closest = mapLocations[i];
                    minDistance = distance;
                }
            }
            return closest;
        }

        static QuestTracker() {
            mainlineQuest = new Queue<Quest>();
            activeSideQuests = new List<Quest>();
            inactiveSideQuests = new Queue<Quest>();

            mainlineQuest.Enqueue(new Quest(new EmptyRoom(@"..\..\..\..\PapaMartiContent\roomTest.txt", mapLocations[0])));
            // here is where quests are queued into the mainquest queue
            // here is where sidequests are queued into the sidequest queue, in the order in which theyre unlocked

            //activeSideQuests.Add(inactiveSideQuests.Dequeue());
        }

        public static void initializeTextures(ContentManager content) {
            foreach(MapLocation m in mapLocations)
                m.loadTexture(content);
        }

        public static RoomManager enterRoom(ContentManager content, Player player, MapLocation location) {
            if(mainlineQuest.Peek().getCurrentTask().location == location) {
                if(mainlineQuest.Peek().getCurrentTask().isDone())
                    return new RoomManager(content, mainlineQuest.Peek().nextTask(), player);
                else
                    return new RoomManager(content, mainlineQuest.Peek().getCurrentTask(), player);
            }

            foreach(Quest q in activeSideQuests) {
                if(q.getCurrentTask().location == location)
                    return null;
            }

            return new RoomManager(content, location.emptyQuest, player);
        }

        public static void advanceMainquest() {
            mainlineQuest.Peek().nextTask();
            if(mainlineQuest.Peek().isQuestDone())
                mainlineQuest.Dequeue();
        }
    }

    
}
