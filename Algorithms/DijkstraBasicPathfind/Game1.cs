using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Noah Kasper
// Dijkstra's Algorithm
// 4/27/2023

namespace HW6_Dijkstra
{
    /// <summary>
    /// The core Game1 class which runs the functionality of the graph.
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Graph graph;
        private Texture2D block;

        /// <summary>
        /// Game1 constructor.
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Initializes data used in Game1.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// Loads texture content and creates the graph object. After creating the graph object, runs
        /// Dijkstra's Algorithm, and updates the tiles to show the calculated shortest path.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            block = this.Content.Load<Texture2D>("block");
            graph = new Graph(10, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, block);

            graph.RunAlgorithm();
            graph.UpdateTiles();

        }

        /// <summary>
        /// Code which is run when the game "updates" every frame.
        /// </summary>
        /// <param name="gameTime">The time of the game</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the visual graph to the monogame window.
        /// </summary>
        /// <param name="gameTime">The time of the game</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            graph.Draw(_spriteBatch);
        
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}