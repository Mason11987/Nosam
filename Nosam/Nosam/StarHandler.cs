using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNAPlatformer
{
    using System.Diagnostics;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    internal class StarParticle
    {
        public Vector2 Location;
        public Vector2 Velocity;
        public int Size;
        public const int maxDepth = 3;
        public static List<Color> Colors;

        public StarParticle(Vector2 location, int depth)
        {
            this.Location = location;

            Velocity.Y = (float)(depth * 10f + (Program.RNG.NextDouble() * 4 - 2));

            Size = depth;
            //Size = size;
        }
    }

    class StarHandler : DrawableGameComponent
    {
        private NosamGame GameRef;
        private Level inLevel;
        private List<StarParticle> Stars;
 
        public StarHandler(NosamGame game, Level level) : base(game)
        {
            GameRef = game;
            inLevel = level;
            Stars = new List<StarParticle>();
            for (int i = 0; i < 1000; i++)
            {
                Stars.Add(
                    new StarParticle(
                        new Vector2(Program.RNG.Next(0, GameRef.screenWidth), Program.RNG.Next(0, GameRef.screenHeight)),
                        Program.RNG.Next(1, StarParticle.maxDepth)));
            }
            GameRef.starTexture = new Texture2D(GameRef.GraphicsDevice, 1, 1);
            GameRef.starTexture.SetData(new Color[] { Color.White });

            StarParticle.Colors = new List<Color>();
            int factor = 0;
            for (int i = 0; i < StarParticle.maxDepth; i++)
            {
                factor = 120 + (int)(125f / StarParticle.maxDepth * i);
                StarParticle.Colors.Add(Color.FromNonPremultiplied(factor, factor, factor, 255));
            }
        }

        public override void Update(GameTime gameTime)
        {

            foreach (var star in Stars)
            {
                star.Location += star.Velocity * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
                if (star.Location.Y > GameRef.screenHeight + 20)
                    star.Location = new Vector2(Program.RNG.Next(0, GameRef.screenWidth), -20);
            }
            base.Update(gameTime);
        }

        internal void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var star in Stars)
            {
                Rectangle sourceRectangle = new Rectangle(0, 0, GameRef.starTexture.Width, GameRef.starTexture.Height);
                //Vector2 origin = new Vector2(GameRef.starTexture.Width / 2, GameRef.starTexture.Height / 2);

                spriteBatch.Draw(GameRef.starTexture, star.Location, sourceRectangle, StarParticle.Colors[star.Size],0f,Vector2.Zero,star.Size,SpriteEffects.None, 0f);
            }
            base.Update(gameTime);
        }
    }
}
