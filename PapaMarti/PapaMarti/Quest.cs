using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PapaMarti
{
    public enum Status{
        Unknown,
        Obtained,
        Completed
    }

    /*Quest types accounted for include:
     *Moving to a certain location
     */
    class Quest //Create a Quest array in a different class to progress the story
    {
        public bool questComplete;
        public Task[] tasks; //"completed" must be true for isComplete to be true

        public Quest(Task[] tasks_)
        {
            questComplete = false;
            tasks = tasks_;
        }

        public void update()
        {
            questComplete = true;
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i].check();
                if (!tasks[i].taskComplete)
                {
                    questComplete = false;
                }
            }
        }
    }
}
