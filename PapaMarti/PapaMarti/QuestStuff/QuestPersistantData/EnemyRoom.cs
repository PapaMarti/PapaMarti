using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PapaMarti
{
    class EnemyRoom : EmptyRoom
    {

        public EnemyRoom(string layout, MapLocation location) : base(layout, location)
        {

        }

        public override bool isDone()
        {
            return enemies.Count == 0;
        }
    }
}
