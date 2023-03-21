using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PapaMarti
{
    abstract class Task
    {
        public bool taskComplete;

        public abstract void check();
    }
    /*Types of tasks include
     * Movement: Player must go to a certain area
     * 
    */
}
