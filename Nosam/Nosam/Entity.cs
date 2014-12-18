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
    public class Entity : DrawableGameComponent
    {

        public ParticleEngine particleEngine;
        public Vector2 Location;

        public Vector2 TopLeft
        {
            get { return Location; }
        }
        public Vector2 TopRight
        {
            get { return new Vector2(Location.X + Size.X, Location.Y); }
        }
        public Vector2 BottomLeft
        {
            get { return new Vector2(Location.X, Location.Y + Size.Y); }
        }
        public Vector2 BottomRight
        {
            get { return new Vector2(Location.X + Size.X, Location.Y + Size.Y); }
        }

        public Rectangle Bounds
        {
            get { return new Rectangle((int)Location.X, (int)Location.Y, (int)Size.X, (int)Size.Y); }
        }

        public Rectangle DrawnRect
        {
            get { return Bounds.Shift(-Size / 2); }
        }

        public virtual Vector2 ParticleEmitterLocation
        {
            get
            {
                return Location;
            }

        }

        public Vector2 Velocity;
        public virtual float  Rotation { get; set; }
        public Vector2 Size;
        public Level inLevel;
        public int Power { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public bool isAlive = true;
        public NosamGame GameRef;

        public Entity(NosamGame game, Level level)
            : base(game)
        {
            GameRef = game;
            inLevel = level;
        }

        public Entity(NosamGame game, Level level, Vector2 size, Vector2 velocity, Vector2 location, float rotation, int health, int power)
            : this(game, level)
        {
            Size = size;
            Location = location;
            Velocity = velocity;
            Rotation = rotation;
            Health = health;
            MaxHealth = health;
            Power = power;
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
            
            Location += Velocity * (float)(gameTime.ElapsedGameTime.TotalSeconds);

            if (particleEngine != null)
            {
                particleEngine.EmitterLocation = ParticleEmitterLocation;
                particleEngine.Update(gameTime);
            }

            if (isOffScreen())
            {
                OnOffScreen();
            }
            base.Update(gameTime);
        }


        public virtual void OnOffScreen()
        {
            isAlive = false;
        }

        private bool isOffScreen()
        {
            return BottomLeft.Y < 0 || BottomRight.X < 0 || // Off top or left sides
                   TopLeft.Y > GameRef.screenHeight || TopLeft.X > GameRef.screenWidth; //Off bottom or top sides
        }

        internal virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(GameRef.boxTexture, new Rectangle((int)Location.X , (int)Location.Y, (int)Size.X, (int)Size.Y), Color.White);
        }


        internal void PlaceAt(Vector2 Location)
        {
            PlaceAtX(Location.X);
            PlaceAtY(Location.Y);
        }

        internal void PlaceAtX(float XLoc)
        {
            Location.X = XLoc;
            Velocity.X = 0;
        }

        internal void PlaceAtY(float YLoc)
        {
            Location.Y = YLoc;
            Velocity.Y = 0;
        }

        internal bool Intersecting(Entity entity)
        {
            return Bounds.Intersects(entity.Bounds);
        }

        virtual internal void HitBy(Bullet bullet)
        {

        }

    }
}
