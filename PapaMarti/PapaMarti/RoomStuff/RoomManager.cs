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

namespace PapaMarti
{
    public class RoomManager: StageManager
    {
        public MapLocation location;
        public Room room;
        public Player player;
        List<Vector2> enemySpots;
        public List<Enemy> enemies;
        Texture2D enemyText;
        Texture2D playerText;
        List<Projectile> projectiles;
        Texture2D projectileText;

        public RoomManager(Room room) : base(room.location)
        {
            this.room = room;
            enemySpots = room.enemySpots;
            enemies = room.enemies;
            projectiles = room.projectiles;
        }

        public override void contentify(ContentManager content, Player p)
        {
            playerText = content.Load<Texture2D>("whitePixel");
            enemyText = content.Load<Texture2D>("whitePixel");
            projectileText = content.Load<Texture2D>("whitePixel");
            player = p;
            if(room is EnemyRoom)
            {
                ((EnemyRoom)room).contentify(content);
            }
        }

        public void enter()
        {

            player = room.enter(player);
        }

        /*public void transition(ContentManager content, MapLocation location, Player player_)
        {
            this.location = location;
            room = location.room;
            //Texture2D playerText = content.Load<Texture2D>("whitePixel");
            //Texture2D lifeBarText = content.Load<Texture2D>("whitePixel");
            Texture2D enemyText = content.Load<Texture2D>("whitePixel");
            //player = new Player(new Rectangle(1800, 500, 60, 60), playerText, 300, lifeBarText);
            player = room.enter(player);
        }*/
        public override GameStage getStage()
        {
            return GameStage.Rooming;
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            room.draw(spriteBatch);
            player.draw(spriteBatch, playerText);
            foreach (Mafia m in enemies)
            {
                m.draw(spriteBatch, enemyText);
            }
            foreach(Projectile p in projectiles)
            {
                p.draw(spriteBatch, projectileText);
            }
        }
        public override void update(GameTime time)
        {
            player = room.update(player);
            enemies = room.updateEnemies(player);
            projectiles = room.updateProjectiles(player);
        }
        public override bool isDone()
        {
            if (room.isDone()) QuestTracker.advanceMainquest();
            return room.isTouchingDoor();
        }
    }
}
