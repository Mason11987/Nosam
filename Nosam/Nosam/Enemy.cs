using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNAPlatformer
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Enemy : Entity
    {
        private int EnemyType;
        private TimeSpan lastFire = new TimeSpan(0);
        private int fireRate = 3000;

        public Enemy(int enemyType, Vector2 location, NosamGame game, Level level)
            : base(game, level, new Vector2(31, 30), new Vector2(0, 10), location, 0.0f, 100, 10)
        {
            EnemyType = enemyType;
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 oldLocation = Location;

            base.Update(gameTime);

            float a = (GameRef.screenWidth * .45f);
            float b = .04f;
            float c = 0;
            float d = GameRef.screenWidth / 2;
            Location.X = (float)(a * Math.Sin(b * (Location.Y - c)) + d);

            Rotation = (float)(Math.Atan2(Location.Y - oldLocation.Y, Location.X - oldLocation.X)) - MathHelper.PiOver2;

            if (lastFire.Add(new TimeSpan(0, 0, 0, 0, fireRate)) < gameTime.TotalGameTime)
            {
                if (lastFire.TotalMilliseconds == 0)
                {
                    lastFire = gameTime.TotalGameTime.Subtract(new TimeSpan(0,0,0,0,Program.RNG.Next(fireRate)));
                }
                else
                {
                    //Shoot in Rotation Direction 
                    //var bulletVelocity = Vector2.Transform(new Vector2(0, 200), Matrix.CreateRotationZ(Rotation));

                    //inLevel.GenerateBullet(0,
                    //                        bulletVelocity, Rotation + MathHelper.Pi,
                    //                        DrawnRect.Location.ToVector2(),
                    //                        false);


                    inLevel.GenerateBullet(1,
                                            new Vector2(0, 150), MathHelper.Pi, 10, 10,
                                            DrawnRect.Location.ToVector2(),
                                            false);

                    lastFire = gameTime.TotalGameTime;
                }
            }

        }

        public override void OnOffScreen()
        {
            
        }

        internal override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(GameRef.enemyTexture,
                             DrawnRect, 
                             new Rectangle(0, 0, (int)Size.X, (int)Size.Y),
                             Color.White, Rotation, Size / 2,SpriteEffects.None, 0.0f);
        }


        internal override void HitBy(Bullet bullet)
        {
            Health -= bullet.Power;
            if (Health <= 0)
            {
                isAlive = false;
                inLevel.GenerateExplosion(Location, true);
            }
        }
    }
}
