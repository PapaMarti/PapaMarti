using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PapaMarti
{
    public enum QuestStatus {
        Unknown,
        Obtained,
        Completed
    }

    /*Quest types accounted for include:
     *Moving to a certain location
     */
    class Quest //Create a Quest array in a different class to progress the story
    {
        public QuestStatus status;
        public Task[] tasks; //All "taskComplete" must be true for status to be Completed
        public double radius;
        public double angle;

        public Quest(Task[] tasks_, double radius_, double angle_)
        {
            status = QuestStatus.Unknown;
            tasks = tasks_;
            radius = radius_;
            angle = angle_;
        }

        public void obtain()
        {
            status = QuestStatus.Obtained;
        }
        public void update()
        {
            status = QuestStatus.Completed;
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i].check();
                if (!tasks[i].taskComplete)
                {
                    status = QuestStatus.Obtained;
                }
            }
        }
    }
}
