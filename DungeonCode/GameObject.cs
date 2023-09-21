using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

//Group 1
//Class that handles all of the drawn/collidable objects of the game

namespace BeatTheDungeon
{
    internal class GameObject
    {
        //Fields
        private Texture2D texture;
        private Rectangle rectangle;

        //Properties
        //texture of the gameobject
        public Texture2D Texture { get => texture; set => texture = value; }
        //x coordinate of the rectangle
        public int X { get => rectangle.X; set => rectangle.X = value; }
        //y coordinate of the rectangle
        public int Y { get => rectangle.Y; set => rectangle.Y = value; }
        //width of the rectangle
        public int Width { get => rectangle.Width; }
        //height of the rectangle
        public int Height { get => rectangle.Height; }

        public virtual Rectangle Rectangle { get => rectangle; }

        /// <summary>
        /// basic constructor that creates the basic gameobject texture/rectangle
        /// </summary>
        /// <param name="texture">texture of the gameobject</param>
        /// <param name="rectangle">rectangle of the gameobject</param>
        public GameObject(Texture2D texture, Rectangle rectangle)
        {
            this.texture = texture;
            this.rectangle = rectangle;
        }

        /// <summary>
        /// overridable update method for game updates
        /// </summary>
        /// <param name="gameTime">current time in game</param>
        public virtual void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// overloaded (player) overridable update method for game updates
        /// </summary>
        /// <param name="gameTime">current time in game</param>
        public virtual void Update(GameTime gameTime, Player player)
        {

        }

        /// <summary>sss
        /// overridable draw method that draws the object to the screen
        /// </summary>
        /// <param name="sb">sprite batch</param>
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, rectangle, Color.White);
        }

        /// <summary>sa
        /// overloaded (texture) overridable draw method that draws the object to the screen
        /// </summary>
        /// <param name="sb">sprite batch</param>
        public virtual void Draw(SpriteBatch sb, Texture2D texture)
        {
            sb.Draw(texture, rectangle, Color.White);
        }

        /// <summary>
        /// checks the collision of two gameobjects
        /// </summary>
        /// <param name="other">gameobject being checked for collision</param>
        /// <returns>true if there is a collision, false if not</returns>
        public virtual bool Collision(GameObject other)
        {
            if (Rectangle.Intersects(other.Rectangle))
            {
                return true;
            }
            return false;
        }
    }
}
