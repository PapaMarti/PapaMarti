using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PapaMarti {
    public class EmptyRoom : Room {

        public EmptyRoom(string layout, MapLocation location) : base(parseRoomFile(layout), location) {

        }

        public override bool isDone() {
            return false;
        }
    }
}
