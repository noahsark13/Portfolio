using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace QuadTree_STARTER
{
	class QuadTreeNode
	{
		// The maximum number of objects in a quad
		// before a subdivision occurs
		private const int MaxObjectsBeforeSubdivide = 3;

		// The game objects held at this level of the tree
		private List<GameObject> objects;

		// This quad's rectangle area
		private Rectangle rectArea;

		// This quad's divisions
		private QuadTreeNode[] divisions;


		/// <summary>
		/// The divisions of this quad
		/// </summary>
		public QuadTreeNode[] Divisions { get { return divisions; } }

		/// <summary>
		/// This quad's rectangle area
		/// </summary>
		public Rectangle RectangleArea { get { return rectArea; } }

		/// <summary>
		/// The game objects inside this quad
		/// </summary>
		public List<GameObject> GameObjects { get { return objects; } }


		/// <summary>
		/// Creates a new Quad Tree
		/// </summary>
		/// <param name="x">This quad's x position</param>
		/// <param name="y">This quad's y position</param>
		/// <param name="width">This quad's width</param>
		/// <param name="height">This quad's height</param>
		public QuadTreeNode(int x, int y, int width, int height)
		{
			// Save the rectangle
			rectArea = new Rectangle(x, y, width, height);

			// Create the object list
			objects = new List<GameObject>();

			// No divisions yet
			divisions = null;
		}


		/// <summary>
		/// Adds a game object to the quad.  If the quad has too many
		/// objects in it, and hasn't been divided already, it should
		/// be divided
		/// </summary>
		/// <param name="gameObj">The object to add</param>
		public void AddObject(GameObject gameObj)
		{
			// bool value to track if the gameObj was found to fit in any of the divisions
			bool foundRectangle = false;

			// ACTIVITY: Complete this method

			// Checks if the gameObj is in the current rectangle area, otherwise nothing will happen.
			if (rectArea.Contains(gameObj.Rectangle))
			{
				// If no divisions, the object is simply added.
				if (divisions == null)
				{
					objects.Add(gameObj);
				}
				// Has divided already
				else if (divisions != null)
				{
					// Checks if divisions contain the gameObj
					foreach (QuadTreeNode node in divisions)
					{
						if (node.RectangleArea.Contains(gameObj.Rectangle))
						{
                            node.AddObject(gameObj);
							foundRectangle = true;
                        }
					}

					// if the gameObj is not contained in any of the divisions, it is added
					// to the parent objects.
					if (foundRectangle == false)
					{
						objects.Add(gameObj);
					}

                }

				// Checks if the objects list has gone past the maxObjects, and that there still has yet to be any divisions.
				// This allows for future objects to be added above the max, that do not fit into the divisions.
				if (objects.Count > MaxObjectsBeforeSubdivide && divisions == null)
				{
					Divide();
				}

			}
		}

		/// <summary>
		/// Divides this quad into 4 smaller quads.  Moves any game objects
		/// that are completely contained within the new smaller quads into
		/// those quads and removes them from this one.
		/// </summary>
		public void Divide()
		{
			// ACTIVITY: Complete this method

			// Creates divisions
			QuadTreeNode a = new QuadTreeNode(RectangleArea.X, RectangleArea.Y, RectangleArea.Width / 2, RectangleArea.Height / 2);
            QuadTreeNode b = new QuadTreeNode(RectangleArea.X + (RectangleArea.Width / 2), RectangleArea.Y, RectangleArea.Width / 2, RectangleArea.Height / 2);
            QuadTreeNode c = new QuadTreeNode(RectangleArea.X, RectangleArea.Y + (RectangleArea.Height / 2), RectangleArea.Width / 2, RectangleArea.Height / 2);
            QuadTreeNode d = new QuadTreeNode(RectangleArea.X + (RectangleArea.Width /2), RectangleArea.Y + (RectangleArea.Height / 2), RectangleArea.Width / 2, RectangleArea.Height / 2);

			QuadTreeNode[] newDivisions = { a, b, c, d };

			divisions = newDivisions;

			// Loops through every division and distributes the current gameObjects.
			foreach (QuadTreeNode node in divisions)
			{
				// Loops through all the current gameObjects in the objects list.
				for (int i = 0; i < GameObjects.Count; i++)
				{
					// if the node contains a game object it is added to the node's objects list and removed from the parents.
					if (node.RectangleArea.Contains(GameObjects[i].Rectangle))
					{
						// Add object is called here to check that the node's object list doesn't go past the max, or another division will be called.
						node.AddObject(GameObjects[i]);
						// The added object is removed from the parents list.
						GameObjects.Remove(GameObjects[i]);
						i--;
					}
				}
			}
		}



		/// <summary>
		/// Recursively populates a list with all of the rectangles in this
		/// quad and any subdivision quads.  Use the "AddRange" method of
		/// the list class to add the elements from one list to another.
		/// </summary>
		/// <returns>A list of rectangles</returns>
		public List<Rectangle> GetAllRectangles()
		{
			List<Rectangle> rects = new List<Rectangle>();

			// ACTIVITY: Complete this method

			// Adds the current rectangle to the list.
			rects.Add(RectangleArea);

			// If there are divisions...
			if (divisions != null)
			{
				// goes through all the divisions and runs the recursive GetAllRectangles method. Returning a longer
				// and longer list of all the rectangles.
				foreach (QuadTreeNode node in divisions)
				{
					// Final list of rectangles added to the current list.
					rects.AddRange(node.GetAllRectangles());
				}
			}

			// Returns current list.
			return rects;
		}

		/// <summary>
		/// A possibly recursive method that returns the
		/// smallest quad that contains the specified rectangle
		/// </summary>
		/// <param name="rect">The rectangle to check</param>
		/// <returns>The smallest quad that contains the rectangle</returns>
		public QuadTreeNode GetContainingQuad(Rectangle rect)
		{
			// ACTIVITY: Complete this method

			// Checks that the area contains the rectangle
			if (RectangleArea.Contains(rect))
			{
				// if there are divisions, loop through each division node.
                if (divisions != null)
                {
                    foreach (QuadTreeNode node in divisions)
                    {
						// checks if the divided node will contain the rectangle
						if (node.GetContainingQuad(rect) != null)
						{
							// if it does contain the rectangle, return the result of running
							// the recursive method of GetContainingQuad. The recursion will occur until there are no longer
							// divisions that contain the rectangle.
							return node.GetContainingQuad(rect);
						}

                    }
                }

				// if there are no more divisions that contain the rectangle, it is 
				// in the current quad and is thus returned.
				return this;
            }


            // Return null if this quad doesn't completely contain
            // the rectangle that was passed in
            return null;
		}
	}
}
