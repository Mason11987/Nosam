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
using XNAPlatformer.GameState;

namespace XNAPlatformer
{

    public class NosamGame : Game
    {
        GraphicsDeviceManager graphics;
        GraphicsDevice device;
        public SpriteBatch SpriteBatch;
        public Rectangle ScreenRectangle;

        public Texture2D boxTexture;
        public Texture2D playerTexture;
        public Texture2D tilesTexture;
        public Texture2D bulletTexture;
        public Texture2D enemyTexture;
        public Texture2D bigExplosionTexture;
        public Texture2D smallExplosionTexture;
        public Texture2D starTexture;
        public Texture2D cometTexture;
        public Texture2D healthBarTexture;
        public SpriteFont myFont;

        public SoundEffect eShotSound;
        public SoundEffect smallExplosionSound;
        public SoundEffect bigExplosionSound;

        public int screenWidth = 1200;
        public int screenHeight = 800;

        GameStateManager stateManager;

        public StartMenuScreen StartMenuScreen;
        public NewGameScreen NewGameScreen;
        public LevelScreen LevelScreen;
        public ParticleEngine particleEngine;

        #region Frames Per Second Field Region

        private float fps;
        private float updateInterval = 0.10f;
        private float timeSinceLastUpdate = 0.0f;
        private float frameCount = 0;

        #endregion

        public NosamGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.SynchronizeWithVerticalRetrace = true;

            device = graphics.GraphicsDevice;
            
            Mouse.WindowHandle = Window.Handle;
            this.IsMouseVisible = true;

            Content.RootDirectory = "Content";

            ScreenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);

            Components.Add(new InputHandler(this));

            stateManager = new GameStateManager(this);
            Components.Add(stateManager);

            StartMenuScreen = new StartMenuScreen(this, stateManager);
            NewGameScreen = new NewGameScreen(this, stateManager);
            LevelScreen = new LevelScreen(this, stateManager);

            stateManager.ChangeState(StartMenuScreen);

            this.IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            var form = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(this.Window.Handle);
            form.Location = new System.Drawing.Point(System.Windows.Forms.Screen.AllScreens.Count() > 1 ? 1600 : 0, 0);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            //backgroundTexture = Content.Load<Texture2D>("Walls");
            boxTexture = Content.Load<Texture2D>("Box");
            playerTexture = Content.Load<Texture2D>("Player");
            healthBarTexture = Content.Load<Texture2D>("HealthBar");
            bulletTexture = Content.Load<Texture2D>("Bullet");
            enemyTexture = Content.Load<Texture2D>("Enemies");
            smallExplosionTexture = Content.Load<Texture2D>("SmallExplosion");
            bigExplosionTexture = Content.Load<Texture2D>("BigExplosion");


            cometTexture = Content.Load<Texture2D>("Comet");
            myFont = Content.Load<SpriteFont>("myFont");

            this.eShotSound = Content.Load<SoundEffect>("Sounds/Eshot");
            smallExplosionSound = Content.Load<SoundEffect>("Sounds/SmallExplosion");
            bigExplosionSound = Content.Load<SoundEffect>("Sounds/BigExplosion");

            ParticleEngine.AllParticleTextures = new List<Texture2D>();
            ParticleEngine.AllParticleTextures.Add(Content.Load<Texture2D>("circle"));
            //particleEngine = new ParticleEngine(ParticleEngine.AllParticleTextures, new Vector2(400, 240));
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            //particleEngine.EmitterLocation = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            //particleEngine.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //particleEngine.Draw(SpriteBatch);

            base.Draw(gameTime);

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            frameCount++;
            timeSinceLastUpdate += elapsed;

            if (timeSinceLastUpdate > updateInterval)
            {
                fps = frameCount / timeSinceLastUpdate;
#if XBOX360
                System.Diagnostics.Debug.WriteLine("FPS: " + fps.ToString());
#else
                this.Window.Title = "FPS: " + fps.ToString() + " Entities: " + LevelScreen.Level.Entities.Count;
#endif
                frameCount = 0;
                timeSinceLastUpdate -= updateInterval;
            }

            
        }
    }
}
