using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Noah Kasper
// Vertex Class

namespace HW6_Dijkstra
{
    /// <summary>
    /// Represents a individual square tile vertex.
    /// </summary>
    internal class Vertex
    {
        private int sourceDist;
        private bool permanent;
        private Vertex pathNeighbor;
        private Color color;

        private Texture2D texture;
        private Rectangle rectangle;

        /// <summary>
        /// Gets the texture of the vertex.
        /// </summary>
        public Texture2D Texture { get { return texture; } }

        /// <summary>
        /// Gets the rectangle of the vertex.
        /// </summary>
        public Rectangle Rectangle { get { return rectangle; } }

        /// <summary>
        /// Gets and sets the current color of the vertex.
        /// </summary>
        public Color Color { get { return color; } set { color = value; } }

        /// <summary>
        /// Gets and sets the permanent status of the vertex.
        /// </summary>
        public bool Permanent { get { return permanent; } set { permanent = value; } }

        /// <summary>
        /// Gets and sets the distance of the vertex.
        /// </summary>
        public int Distance { get { return sourceDist; } set { sourceDist = value; } }

        /// <summary>
        /// Gets and sets the path neighbor of the vertex.
        /// </summary>
        public Vertex PathNeighbor { get { return pathNeighbor; } set { pathNeighbor = value; } }

        /// <summary>
        /// Consstructs the individual vertex tile.
        /// </summary>
        /// <param name="texture">The tile texture.</param>
        /// <param name="rectangle">The rectangle of the vertex.</param>
        public Vertex(Texture2D texture, Rectangle rectangle)
        {
            this.texture = texture;
            this.rectangle = rectangle;

            // Sets default values
            color = Color.White;
            sourceDist = int.MaxValue;
            permanent = false;
            pathNeighbor = null;
        }

        /// <summary>
        /// Draws the individual vertex.
        /// </summary>
        /// <param name="sb">Sprite batch</param>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(
                Texture,
                Rectangle,
                color);
        }

    }
}
