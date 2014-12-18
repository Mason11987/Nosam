
namespace XNAPlatformer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Player : Entity
    {
        private float movespeed = 200;
        private float maxV = 198;
        private float maxRotation = 0.5f;

        private bool isOnLeftTurret = false;
        private int fireRate = 100;
        public int bulletType = 0;
        private int ammo = 20;
        private int maxAmmo = 50;
        private int ammoRate = 300;
        private TimeSpan lastAmmo = new TimeSpan(0);
        private TimeSpan lastFire = new TimeSpan(0);
        private bool isRotationLocked = false;
        private float lastRotation = 0.0f;
        public override float Rotation
        {
            get
            {
                if (!isRotationLocked)
                    lastRotation = (Velocity.X / maxV) * maxRotation;
                return lastRotation;
            }
        }


        public Player(NosamGame game, Level level)
            : base(game, level, new Vector2(48, 33), Vector2.Zero, new Vector2(game.screenWidth / 2 - (48) / 2, game.screenHeight - (33) - 30), 0.0f, 100, 50)
        {

        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            HandleInput(gameTime);
            RestrictMotion(gameTime);

            if (ammo < maxAmmo && lastAmmo.Add(new TimeSpan(0, 0, 0, 0, ammoRate)) < gameTime.TotalGameTime)
            {
                ammo++;
                lastAmmo = gameTime.TotalGameTime;
            }
        }

        private void RestrictMotion(GameTime gameTime)
        {
            if (Math.Abs(Velocity.X) > maxV)
                Velocity.X = maxV * Math.Sign(Velocity.X);
        }


        private void HandleInput(GameTime gameTime)
        {

            if (InputHandler.KeyDown(Keys.D))
                Velocity.X += (float)(movespeed * gameTime.ElapsedGameTime.TotalSeconds);
            else if (InputHandler.KeyDown(Keys.A))
                Velocity.X -= (float)(movespeed * gameTime.ElapsedGameTime.TotalSeconds);
            else
                Velocity.X = Velocity.X * 0.999f;
            if (InputHandler.KeyPressed(Keys.L))
                isRotationLocked = !isRotationLocked;
            if (InputHandler.KeyDown(Keys.Enter) && ammo > 0)
            {
                if (lastFire.Add(new TimeSpan(0, 0, 0, 0, fireRate)) < gameTime.TotalGameTime)
                {
                    var bulletVelocity = Vector2.Transform(new Vector2(0, -200), Matrix.CreateRotationZ(Rotation));

                    inLevel.GenerateBullet(bulletType,
                                            bulletVelocity, Rotation, 10, 50,
                                            isOnLeftTurret ? LeftTurretLocation() : RightTurretLocation(),
                                            true);
                    lastFire = gameTime.TotalGameTime;
                    isOnLeftTurret = !isOnLeftTurret;
                    ammo--;
                }
            }

        }

        public override void OnOffScreen()
        {
            
        }
        
        private Vector2 LeftTurretLocation()
        {
            var OffCenter = new Vector2(-11, -13);
            var RotatedOffCenter = Vector2.Transform(OffCenter, Matrix.CreateRotationZ(Rotation));

            return RotatedOffCenter + DrawnRect.Location.ToVector2();
        }

        private Vector2 RightTurretLocation()
        {
            var OffCenter = new Vector2(7, -13);
            var RotatedOffCenter = Vector2.Transform(OffCenter, Matrix.CreateRotationZ(Rotation));

            return RotatedOffCenter + DrawnRect.Location.ToVector2();
        }


        internal override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            
            spriteBatch.Draw(GameRef.playerTexture, DrawnRect, new Rectangle(0, 0, 48, 33), Color.White, 
                Rotation,
                Size / 2, SpriteEffects.None, 0);

            for (int i = 0; i < ammo; i++)
            {
                spriteBatch.Draw(
                    GameRef.bulletTexture,
                    new Rectangle(20 + 10 * i, GameRef.screenHeight - 20, 5, 11),
                    new Rectangle(0, 0, 5, 11),
                    Color.White);
            }

            const float maxHealthWidth = 400f;


            spriteBatch.Draw(GameRef.healthBarTexture, new Rectangle(600, GameRef.screenHeight - 25, (int)((float)Health / MaxHealth * maxHealthWidth), 20), Color.White);
            

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
