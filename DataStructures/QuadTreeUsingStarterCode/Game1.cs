using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Noah Kasper
// QuadTree
// 4/9/2023

namespace QuadTree_STARTER
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Mouse related
        private Rectangle mouseRect;
        private QuadTreeNode mouseQuad;

        // Basic textures
        private Texture2D whitePixel;

        // The quad tree
        private QuadTreeNode quadTree;

        // A list of game objects
        private List<GameObject> gameObjects;

        // Random number generator
        private Random random;

        // A color used to flash game objects
        private Color flash;

        // Constants
        private const int NumGameObjects = 50;
        private const int MinGameObjectSize = 5;
        private const int MaxGameObjectSize = 15;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            // Set up the game object list
            gameObjects = new List<GameObject>();

            // Make the random object
            random = new Random();

            // Set up the mouse rectangle
            mouseRect = new Rectangle(0, 0, 25, 25);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Create a basic 1x1 white pixel texture
            whitePixel = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            whitePixel.SetData<Color>(new Color[1] { Color.White });

            // Create the quad tree
            quadTree = new QuadTreeNode(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            // Create a bunch of randomly placed game objects
            for (int i = 0; i < NumGameObjects; i++)
            {
                // Choose a random size
                int size = random.Next(MinGameObjectSize, MaxGameObjectSize + 1);

                // Choose x and y values (with a buffer around the border of the window)
                int x = random.Next(size, GraphicsDevice.Viewport.Width - size);
                int y = random.Next(size, GraphicsDevice.Viewport.Height - size);
                Color color = new Color(
                    (float)Math.Max(random.NextDouble(), 0.25f),
                    (float)Math.Max(random.NextDouble(), 0.25f),
                    (float)Math.Max(random.NextDouble(), 0.25f),
                    1.0f);

                // Make the game object
                GameObject gameObj = new GameObject(new Rectangle(x, y, size, size), whitePixel, color);

                // Add it to the list and the quad tree
                gameObjects.Add(gameObj);
                quadTree.AddObject(gameObj);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Change the flash color
            flash = new Color(0.0f, 0.0f, 0.0f, (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 10) * 0.5f + 0.5f);

            // Update the mouse rectangle
            MouseState mState = Mouse.GetState();
            mouseRect.X = mState.X;
            mouseRect.Y = mState.Y;

            // Get the quad that the mouse rectangle is in
            mouseQuad = quadTree.GetContainingQuad(mouseRect);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Clear the screen
            GraphicsDevice.Clear(Color.Black);

            // Start the sprite batch
            _spriteBatch.Begin();

            // Is the mouse inside of a quad?
            if (mouseQuad != null)
            {
                DrawBox(mouseQuad.RectangleArea, Color.CornflowerBlue);
            }

            // Get all the quad tree rectangles and draw them
            List<Rectangle> rects = quadTree.GetAllRectangles();
            foreach (Rectangle rect in rects)
                DrawRectangleOutline(rect, Color.White);

            // Draw all of the objects
            foreach (GameObject gameObj in gameObjects)
                _spriteBatch.Draw(gameObj.Texture, gameObj.Rectangle, gameObj.Color);

            // Flash any objects in the current quad
            if (mouseQuad != null)
            {
                // Flash the quad's objects
                foreach (GameObject gameObj in mouseQuad.GameObjects)
                {
                    _spriteBatch.Draw(gameObj.Texture, gameObj.Rectangle, flash);
                    DrawRectangleOutline(gameObj.Rectangle, Color.White);
                }
            }

            // Draw the mouse rectangle
            _spriteBatch.Draw(whitePixel, mouseRect, Color.White);

            // End the sprite batch
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
		/// Draws the outline of the specified rectangle
		/// </summary>
		/// <param name="rect">The rectangle to draw</param>
		/// <param name="color">The color to use when drawing</param>
		private void DrawRectangleOutline(Rectangle rect, Color color)
        {
            // Draw the 4 lines as 4 thin boxes:
            // Top, right, bottom, left
            DrawBox(rect.X, rect.Y, rect.Width, 1, color);
            DrawBox(rect.X + rect.Width, rect.Y, 1, rect.Height, color);
            DrawBox(rect.X, rect.Y + rect.Height, rect.Width, 1, color);
            DrawBox(rect.X, rect.Y, 1, rect.Height, color);
        }

        /// <summary>
        /// Draws a box
        /// </summary>
        /// <param name="x">The x position of the box</param>
        /// <param name="y">The y position of the box</param>
        /// <param name="width">The width of the box</param>
        /// <param name="height">The height of the box</param>
        /// <param name="color">The color to use when drawing</param>
        private void DrawBox(int x, int y, int width, int height, Color color)
        {
            // Draw the box
            _spriteBatch.Draw(
                whitePixel,
                new Rectangle(x, y, width, height),
                color);
        }

        /// <summary>
        /// Draws a box using a rectangle
        /// </summary>
        /// <param name="rect">The rectangle that defines the box</param>
        /// <param name="color">The color of the rectangle</param>
        private void DrawBox(Rectangle rect, Color color)
        {
            // Draw the box
            _spriteBatch.Draw(
                whitePixel,
                rect,
                color);
        }
    }
}