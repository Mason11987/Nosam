// -----------------------------------------------------------------------
// <copyright file="ParticleEngine.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace XNAPlatformer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ParticleEngine
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private List<Particle> particles;
        private List<Texture2D> textures;

        private int maxParticles;
        private int maxSize;
        private float defaultAngle;
        private float maxAngleSpread;
        private float minVelocity;
        private float maxVelocity;

        public ParticleEngine(List<Texture2D> textures, Vector2 location, int maxParticles, int maxSize, float defaultAngle, float maxAngleSpread, float minVelocity, float maxVelocity)
        {
            EmitterLocation = location;
            this.textures = textures;
            this.particles = new List<Particle>();
            random = new Random();
            this.maxParticles = maxParticles;
            this.maxSize = maxSize;
            this.defaultAngle = defaultAngle;
            this.maxAngleSpread = maxAngleSpread;
            this.minVelocity = minVelocity;
            this.maxVelocity = maxVelocity;
        }

        public void Update(GameTime gameTime)
        {
            int total = (maxParticles - particles.Count) / 10;

            for (int i = 0; i < total; i++)
            {
                particles.Add(GenerateNewParticle(gameTime));
            }

            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update(gameTime);
                if (!particles[particle].Alive)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        private Particle GenerateNewParticle(GameTime gameTime)
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = EmitterLocation;
            //Vector2 velocity = new Vector2(
            //                        1f * (float)(random.NextDouble() * 2 - 1),
            //                        1f * (float)(random.NextDouble() * 2 - 1));

            Vector2 velocityMagnitude = new Vector2(minVelocity + (float)(random.NextDouble() * (maxVelocity - minVelocity)),0);

            Vector2 velocity = Vector2.Transform(
                velocityMagnitude,
                Matrix.CreateRotationZ(
                    (float)(defaultAngle  + ((random.NextDouble() * maxAngleSpread) - (maxAngleSpread / 2)))));
            
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);

            int brightness = random.Next(0, 50);
            Color color = new Color(brightness * random.Next(0, 255), brightness, brightness,50);
            
            float size = (float)random.NextDouble() * (maxSize / 16f);
            int ttl = 40 + random.Next(40);

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, gameTime.TotalGameTime, ttl);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
            //spriteBatch.End();
        }

        public static List<Texture2D> AllParticleTextures { get; set; }
    }
}
