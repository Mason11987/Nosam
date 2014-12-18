using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace XNAPlatformer
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Level : DrawableGameComponent
    {
        NosamGame GameRef;

        public List<Entity> Entities = new List<Entity>();
        List<Entity> AddEntities = new List<Entity>();
        List<Entity> RemoveEntities = new List<Entity>();
        List<Explosion> Explosions = new List<Explosion>();
        Player Player;

        private StarHandler starHandler;

        public Level(NosamGame game)
            : base(game)
        {
            GameRef = game;

            Player = new Player(game, this);
            Entities.Add(Player);

            for (int i = 0; i < 50; i++)
            {
                Entities.Add(new Enemy(0, new Vector2(0, -1 * (i * 10)), game, this));
            }

        }


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
            
        }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);

            HandleInput();

            Entities.AddRange(AddEntities);
            AddEntities.Clear();

            foreach (Entity entity in Entities)
                entity.Update(gameTime);
            foreach (Explosion explosion in Explosions)
                explosion.Update(gameTime);

            HandleEntityCollisions();

            Entities.RemoveAll(x => !x.isAlive);
            Explosions.RemoveAll(x => !x.isAlive);

            starHandler.Update(gameTime);
            InputHandler.Flush();
        }

        private void HandleEntityCollisions()
        {
            var Enemies = Entities.Where(x => x is Enemy).Cast<Enemy>();
            var Bullets = Entities.Where(x => x is Bullet).Cast<Bullet>();

            foreach (var bullet in Bullets)
            {
                if (bullet.isFriendly)
                {
                    foreach (var enemy in Enemies.Where(x => x.isAlive))
                    {
                        if (bullet.Intersecting(enemy))
                        {
                            bullet.Hit(enemy);
                            break;
                        }
                    }
                }
                else
                {
                    if (Player.isAlive && bullet.Intersecting(Player))
                    {
                        bullet.Hit(Player);
                        break;
                    }
                }
            }
        }


        private void HandleInput()
        {

            if (InputHandler.LeftMouseButtonDown())
            {

            }

            if (InputHandler.RightMouseButtonDown())
            {

            }
        }

        internal void StartGame()
        {
            starHandler = new StarHandler(GameRef, this);
        }

        internal void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            starHandler.Draw(spriteBatch, gameTime);
            foreach (var explosion in Explosions)
                explosion.Draw(spriteBatch, gameTime);
            foreach (Entity ent in Entities)
                ent.Draw(spriteBatch, gameTime);
            Player.Draw(spriteBatch, gameTime);
        }

        internal void GenerateBullet(int bulletType, Vector2 velocity, float rotation, int health, int power, Vector2 location, bool isFriendly)
        {

            AddEntities.Add(new Bullet(bulletType, velocity, location, rotation, health, power, isFriendly, GameRef, this));

        }

        internal void GenerateExplosion(Vector2 location, bool isBig)
        {
            Explosions.Add(new Explosion(location, isBig, GameRef, this));
        }
    }
}
