using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PapaMarti {
    public class QuestTracker {
        public static Dictionary<MapLocation, QuestPair> maplocs = new Dictionary<MapLocation, QuestPair>();

        static QuestTracker() {
            foreach(MapLocationQuestData d in AllQuests.locationsAndData) {
                maplocs.Add(d.getLocation(), new QuestPair(d.getInactiveQuest(), d.getActiveQuest()));
            }
        }

        public static void drawLocations(SpriteBatch spriteBatch, float angle, Vector2 mapLocation) {
            foreach(MapLocation m in maplocs.Keys)
                m.draw(spriteBatch, angle, mapLocation);
        }

        public static RoomManager enter(MapLocation location, ContentManager content) {
            return new RoomManager(content, location);
        }

    }

    public class QuestPair {
        public Quest defaultQuest {
            get; private set;
        }

        public Quest activeQuest {
            get; private set;
        }

        public bool active {
            get; set;
        }

        public QuestPair(Quest defaultQuest, Quest activeQuest) {
            this.defaultQuest = defaultQuest;
            this.activeQuest = activeQuest;
            active = false;
        }

    }
}
