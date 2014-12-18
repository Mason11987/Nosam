using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using XNAPlatformer.Controls;

namespace XNAPlatformer.GameState
{
    public class LevelScreen : BaseGameState
    {
        #region Field region

        public Level Level;

        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public LevelScreen(NosamGame game, GameStateManager manager)
            : base(game, manager)
        {
            Level = new Level(game);
            Components.Add(Level);

        }

        #endregion

        #region XNA Method Region

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

        }

        public override void Update(GameTime gameTime)
        {

            Level.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.SpriteBatch.Begin();

            base.Draw(gameTime);

            Level.Draw(GameRef.SpriteBatch, gameTime);

            GameRef.SpriteBatch.End();
        }

        #endregion

        #region Game State Method Region
        public void EnterState()
        {
            Level.StartGame();
        }
        #endregion

    }
}
