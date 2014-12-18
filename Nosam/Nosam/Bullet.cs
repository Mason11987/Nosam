using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNAPlatformer
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Bullet : Entity
    {
        private int BulletType;

        public static List<Rectangle> BulletTextureBounds =
            new List<Rectangle> { new Rectangle(0, 0, 5, 11), new Rectangle(0, 11, 5, 5), new Rectangle(5, 0, 3, 16), new Rectangle(8, 0, 3, 5) };

        public override Vector2 ParticleEmitterLocation
        {
            get
            {
                return new Vector2(BottomLeft.X , BottomLeft.Y - 5);
            }
            
        }
        public bool isFriendly;

        public Bullet(int bulletType, Vector2 velocity, Vector2 location, float rotation, int health, int power, bool isFriendly, NosamGame game, Level level)
            : base(game, level, new Vector2(BulletTextureBounds[bulletType].Width, BulletTextureBounds[bulletType].Height), velocity, location, rotation, health, power)
        {
            BulletType = bulletType;
            this.isFriendly = isFriendly;
            GameRef.eShotSound.Play();
            //particleEngine = new ParticleEngine(ParticleEngine.AllParticleTextures,
            //                                    ParticleEmitterLocation,
            //                                    100,
            //                                    3,
            //                                    MathHelper.PiOver2,
            //                                    MathHelper.PiOver4,
            //                                    .1f,
            //                                    .2f);
        }

        internal override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //particleEngine.Draw(spriteBatch);
            Vector2 origin = new Vector2(BulletTextureBounds[BulletType].Width / 2, BulletTextureBounds[BulletType].Height / 2);

            spriteBatch.Draw(
                GameRef.bulletTexture,
                DrawnRect,
                BulletTextureBounds[BulletType],
                Color.White, Rotation, origin, SpriteEffects.None, 0f);
        }

        internal void Hit(Entity entity)
        {
            isAlive = false;
            inLevel.GenerateExplosion(Location, false);

            entity.HitBy(this);
        }


    }
}
