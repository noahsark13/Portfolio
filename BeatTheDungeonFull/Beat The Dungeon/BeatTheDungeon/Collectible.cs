using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace BeatTheDungeon
{
    /// <summary>
    /// Represents the visual collectible within the dungeon.
    /// </summary>
    internal class Collectible : GameObject
    {
        private Item item;
        private Rectangle detectionRectangle;

        /// <summary>
        /// Gets and sets the item held by the collectible.
        /// </summary>
        public Item Item { get { return item; } set { item = value; } }

        /// <summary>
        /// Collectible constructor.
        /// </summary>
        /// <param name="item">The item held by the constructor</param>
        /// <param name="texture">The texture of the visual collectible</param>
        /// <param name="rectangle">The rectangle of the visual collectible</param>
        public Collectible(Item item, Rectangle rectangle) : base(null, rectangle)
        {
            this.item = item;

            if (item != null)
            {
                this.Texture = item.DisplayIcon;
            }



            detectionRectangle = new Rectangle(rectangle.X - 30 + Width / 2, rectangle.Y - 30 + Height / 2, 60, 60);
        }

        /// <summary>
        /// Checks if a object is within the detection range to interact with the collectible.
        /// </summary>
        /// <param name="other">The object to detect.</param>
        /// <returns>True or false depending on if a object is within the arae.</returns>
        public bool inArea(GameObject other)
        {
            if (detectionRectangle.Intersects(other.Rectangle))
            {
                return true;
            }
            return false;
        }


    }

}