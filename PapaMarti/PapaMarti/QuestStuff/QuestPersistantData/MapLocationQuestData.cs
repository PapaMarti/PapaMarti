using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PapaMarti {
    public abstract class MapLocationQuestData {
        public abstract MapLocation getLocation();
        public abstract Quest getInactiveQuest();
        public abstract Quest getActiveQuest();
    }
}
