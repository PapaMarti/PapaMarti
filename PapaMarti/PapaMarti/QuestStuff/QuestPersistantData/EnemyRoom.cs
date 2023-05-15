using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PapaMarti
{
    class EnemyRoom : EmptyRoom
    {

        public EnemyRoom(string layout, MapLocation location) : base(@"Content\" + layout + ".txt", location)
        {

        }

        public override bool isDone()
        {
            return enemies.Count == 0;
        }
    }
}
