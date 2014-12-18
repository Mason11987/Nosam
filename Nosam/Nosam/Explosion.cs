using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNAPlatformer
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    class Explosion : Entity
    {
        private bool isBig = false;
        private const int maxStepBig = 19;
        private const int maxStepSmall = 8;

        private int curStep = 0;

        private TimeSpan lastStep = TimeSpan.MinValue;

        public Explosion(Vector2 location, bool isBig, NosamGame game, Level level) : base(game, level)
        {
            Location = location;
            this.isBig = isBig;
            if (isBig)
            {
                Size = new Vector2(32, 32);
                GameRef.bigExplosionSound.Play();
            }
            else
            {
                Size = new Vector2(15, 15);
                GameRef.smallExplosionSound.Play();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (lastStep == TimeSpan.MinValue)
            {
                lastStep = gameTime.TotalGameTime;
                return;
            }
            if (gameTime.TotalGameTime.TotalMilliseconds - lastStep.TotalMilliseconds > 50)
            {
                curStep++;
                lastStep = gameTime.TotalGameTime;
            }
            if (curStep > (isBig ? maxStepBig : maxStepSmall)) 
                isAlive = false;

            base.Update(gameTime);
        }


        internal override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            spriteBatch.Draw(isBig ? GameRef.bigExplosionTexture : GameRef.smallExplosionTexture,
                DrawnRect, new Rectangle((int)(Size.X * curStep), 0, (int)Size.X, (int)Size.Y), Color.White);
        }

    }
}
