using System;
using Microsoft.Xna.Framework;

//TODO: Display Health
//TODO: Display Score
//TODO: Enemies
//TODO: Power ups
//TODO: Screens
//TODO: Particle System

namespace XNAPlatformer
{
#if WINDOWS || XBOX
    static class Program
    {
        public static Random RNG;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Program.RNG = new Random();
            using (NosamGame game = new NosamGame())
            {
                game.Run();
            }
        }

        public static Vector2 ToVector2(this Point pt)
        {
            return new Vector2(pt.X, pt.Y);
        }

        public static Rectangle Shift(this Rectangle rect, Vector2 offset)
        {
            return new Rectangle((int)(rect.Left - offset.X), (int)(rect.Top - offset.Y), rect.Width, rect.Height);
        }
    }
#endif
}

