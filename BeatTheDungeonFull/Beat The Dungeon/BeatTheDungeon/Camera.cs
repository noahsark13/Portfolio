using Microsoft.Xna.Framework;

//Group 1
//Camera class that changes the matrix used in drawing the scene

namespace BeatTheDungeon
{
    internal class Camera
    {
        //stores the game's graphics device manager
        private GraphicsDeviceManager _graphics;

        //matrix that will be used to draw according to the player's position
        private Matrix transform;

        /// <summary>
        /// public get and private set for the transformation of the camera matrix
        /// </summary>
        public Matrix Transform { get => transform; private set => transform = value; }

        /// <summary>
        /// constructor that grabs Game1's graphics device manager
        /// </summary>
        /// <param name="_graphics"></param>
        public Camera(GraphicsDeviceManager _graphics)
        {
            this._graphics = _graphics;
        }

        /// <summary>
        /// changes the matrix of the camera to match the position of the player
        /// </summary>
        /// <param name="player"></param>
        public void Follow(Player player)
        {
            //changes the position of the matrix
            Matrix position = Matrix.CreateTranslation((-player.Rectangle.X - player.Width / 2 * 3), (-player.Rectangle.Y - player.Height / 2 * 3), 0);

            //adds an offset based on the graphics device manager
            Matrix offset = Matrix.CreateTranslation(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2, 0);

            //sets the transformation so it can be used in the draw methods
            Transform = position * offset;
        }
    }
}
