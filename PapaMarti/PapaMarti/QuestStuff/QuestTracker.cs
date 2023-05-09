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

            //slice 1
            new MapLocation(Math.PI * 2 / 12, 0.75, "p", Color.White, 20f, (float) (Math.PI / 3), @"..\..\..\..\PapaMartiContent\roomTwo.txt", 1),
            new MapLocation(Math.PI * 1 / 12, 0.3, "g5", Color.White, 1f, 0f, @"..\..\..\..\PapaMartiContent\roomOne.txt", 1),
            new MapLocation(Math.PI * 3 / 12, 0.35, "g1", Color.White, 1.2f, -0.2f, @"..\..\..\..\PapaMartiContent\roomOne.txt", 1),

            //slice 2
            new MapLocation(Math.PI * 6 / 12, 0.75, "b1", Color.White, 20f, (float) (Math.PI / 3), @"..\..\..\..\PapaMartiContent\roomOne.txt", 2),
            new MapLocation(Math.PI * 5 / 12, 0.3, "g4", Color.White, 1f, 0f, @"..\..\..\..\PapaMartiContent\roomOne.txt", 2),
            new MapLocation(Math.PI * 7 / 12, 0.35, "g3", Color.White, 1.2f, -0.2f, @"..\..\..\..\PapaMartiContent\roomOne.txt", 2),
        
            //slice 3
            new MapLocation(Math.PI * 10/ 12, 0.75, "b2", Color.White, 20f, (float) (Math.PI / 3), @"..\..\..\..\PapaMartiContent\roomOne.txt", 3),
            new MapLocation(Math.PI * 9 / 12, 0.3, "g2", Color.White, 1f, 0f, @"..\..\..\..\PapaMartiContent\roomOne.txt", 3),
            new MapLocation(Math.PI * 11 / 12, 0.35, "g1", Color.White, 1.2f, -0.2f, @"..\..\..\..\PapaMartiContent\roomOne.txt", 3),
        
            //slice 4
            new MapLocation(Math.PI * 14 / 12, 0.75, "b3", Color.White, 20f, (float) (Math.PI / 3), @"..\..\..\..\PapaMartiContent\roomOne.txt", 4),
            new MapLocation(Math.PI * 13 / 12, 0.3, "g7", Color.White, 1f, 0f, @"..\..\..\..\PapaMartiContent\roomOne.txt", 4),
            new MapLocation(Math.PI * 15 / 12, 0.35, "g6", Color.White, 1.2f, -0.2f, @"..\..\..\..\PapaMartiContent\roomOne.txt", 4),
        
            //slice 5
            new MapLocation(Math.PI * 18 / 12, 0.75, "b4", Color.White, 20f, (float) (Math.PI / 3), @"..\..\..\..\PapaMartiContent\roomOne.txt", 5),
            new MapLocation(Math.PI * 17 / 12, 0.3, "g3", Color.White, 1f, 0f, @"..\..\..\..\PapaMartiContent\roomOne.txt", 5),
            new MapLocation(Math.PI * 19 / 12, 0.35, "g2", Color.White, 1.2f, -0.2f, @"..\..\..\..\PapaMartiContent\roomOne.txt", 5),
        
            //slice 6
            new MapLocation(Math.PI * 22 / 12, 0.75, "b5", Color.White, 20f, (float) (Math.PI / 3), @"..\..\..\..\PapaMartiContent\roomOne.txt", 6),
            new MapLocation(Math.PI * 21 / 12, 0.3, "g4", Color.White, 1f, 0f, @"..\..\..\..\PapaMartiContent\roomOne.txt", 6),
            new MapLocation(Math.PI * 23 / 12, 0.35, "g7", Color.White, 1.2f, -0.2f, @"..\..\..\..\PapaMartiContent\roomOne.txt", 6),};

        public static Queue<Quest> mainlineQuest;
        public static List<Quest> activeSideQuests;
        public static Queue<Quest> inactiveSideQuests;

        public static MapLocation getClosestLocation(double angle, double radius, int unlockedSlices) {
            double r = (1 - radius) * (MapManager.translation - MapManager.innerCircleTranslation) + MapManager.innerCircleTranslation;
            MapLocation closest = mapLocations[0];
            double minDistance = int.MaxValue;
            for(int i = 0; i < mapLocations.Length; i++) {
                double placeRadius = (1 - mapLocations[i].radius) * (MapManager.translation - MapManager.innerCircleTranslation) + MapManager.innerCircleTranslation;
                double distance = Math.Sqrt(Math.Pow(r, 2) + Math.Pow(placeRadius, 2) - 2 * r * placeRadius * Math.Cos(angle - mapLocations[i].angle));
                if (distance < minDistance && mapLocations[i].slice <= unlockedSlices)
                {
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

            mainlineQuest.Enqueue(new Quest(new CookingManager(new Pizza(PizzaShape.Circle, new List<Rectangle>(), new List<Topping>(), 10), mapLocations[0], true, CookingManagerLevel.Tutorial)));
            // here is where quests are queued into the mainquest queue
            // here is where sidequests are queued into the sidequest queue, in the order in which theyre unlocked

            //activeSideQuests.Add(inactiveSideQuests.Dequeue());
        }

        public static void initializeTextures(ContentManager content, Player p) {
            foreach(MapLocation m in mapLocations)
                m.loadTexture(content);

            foreach (Quest q in mainlineQuest) q.contentify(content, p);
            foreach (Quest q in activeSideQuests) q.contentify(content, p);
            foreach (Quest q in inactiveSideQuests) if (!activeSideQuests.Contains(q)) q.contentify(content, p);
        }

        public static StageManager enterRoom(ContentManager content, Player player, MapLocation location) {
            if(mainlineQuest.Count != 0 && mainlineQuest.Peek().getCurrentTask().location == location) {
                if (mainlineQuest.Peek().getCurrentTask().isDone())
                {
                    return mainlineQuest.Peek().nextTask();
                }
                else
                {

                    if (mainlineQuest.Peek().getCurrentTask().getStage() == GameStage.Rooming)
                    {
                        mainlineQuest.Peek().getCurrentTask().contentify(content, player);
                        ((RoomManager)mainlineQuest.Peek().getCurrentTask()).enter();
                    }
                    return mainlineQuest.Peek().getCurrentTask();
                }
            }

            foreach(Quest q in activeSideQuests) {
                if(q.getCurrentTask().location == location)
                    return null;
            }

            RoomManager r = new RoomManager(location.emptyQuest);
            r.contentify(content, player);
            r.enter();
            return r;
        }

        public static void advanceMainquest() {
            mainlineQuest.Peek().nextTask();
            if(mainlineQuest.Peek().isQuestDone())
                mainlineQuest.Dequeue();
        }
    }

    
}
