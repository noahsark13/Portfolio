using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuadTree_STARTER
{
	class GameObject
	{
		/// <summary>
		/// This game object's rectangle
		/// </summary>
		public Rectangle Rectangle { get; private set; }

		/// <summary>
		/// This game object's texture
		/// </summary>
		public Texture2D Texture { get; private set; }

		/// <summary>
		/// This object's color
		/// </summary>
		public Color Color { get; private set; }


		/// <summary>
		/// Creates a new game object
		/// </summary>
		/// <param name="rect">The rectangle for this game object</param>
		/// <param name="texture">The texture for this object</param>
		/// <param name="color">The color of this object</param>
		public GameObject(Rectangle rect, Texture2D texture, Color color)
		{
			Rectangle = rect;
			Texture = texture;
			Color = color;
		}
	}
}
