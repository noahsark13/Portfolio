using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Noah Kasper
// Graph Class

namespace HW6_Dijkstra
{
    /// <summary>
    /// Represents a square graph of small vertex tiles.
    /// </summary>
    internal class Graph
    {
        private List<Vertex> vertices;
        private int[,] adjMatrix;
        private int size;

        private int startIndex;
        private int endIndex;

        /// <summary>
        /// Constructs the "graph" of square vertices.
        /// </summary>
        /// <param name="size">The width/height size of the square graph.</param>
        /// <param name="screenWidth">The width of the screen.</param>
        /// <param name="screenHeight">The height of the screen.</param>
        /// <param name="texture">Texture of the square.</param>
        public Graph(int size, int screenWidth, int screenHeight, Texture2D texture)
        {
            this.size = size;
            vertices = new List<Vertex>();
            
            // creates the adj matrix of all the square vertices.
            adjMatrix = new int[size * size, size * size];
            
            // centers the graph
            int startingPositionX = (screenWidth / 2) - (size / 2) * 41;
            int startingPositionY = (screenHeight / 2) - (size / 2) * 41;
         
            // create squares
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Vertex v = new Vertex(texture, new Rectangle(startingPositionX + (j * 41), startingPositionY + (i * 41), 40, 40));
 
                    vertices.Add(v);
                }
                
            }

            // Hard coded walls
            vertices[12].Color = Color.LightGray;
            vertices[13].Color = Color.LightGray;
            vertices[23].Color = Color.LightGray;
            vertices[33].Color = Color.LightGray;
            vertices[43].Color = Color.LightGray;
            vertices[44].Color = Color.LightGray;

            vertices[66].Color = Color.LightGray;
            vertices[67].Color = Color.LightGray;
            vertices[77].Color = Color.LightGray;
            vertices[87].Color = Color.LightGray;
            vertices[88].Color = Color.LightGray;
            vertices[89].Color = Color.LightGray;


            // Create the start and end vertices.

            Random rng = new Random();
            bool startFound = false;
            bool endFound = false;
            
            // Loops until a start and end have been created
            while ((startFound == false || endFound == false) == true)
            {
                int randIndex = rng.Next(0, size * size);
                Vertex randomVert = vertices[randIndex];

                // Creates start position
                if (randomVert.Color != Color.LightGray && startFound == false)
                {
                    startFound = true;
                    randomVert.Color = Color.Blue;
                    startIndex = randIndex;
                }
                // Creates end position
                else if (randomVert.Color != Color.LightGray && randomVert.Color != Color.Blue)
                {
                    endFound = true;
                    randomVert.Color = Color.Red;
                    endIndex = randIndex;
                }
            }

            // Populate the adj matrix.

            for (int i = 0; i < adjMatrix.GetLength(0); i++)
            {
                // Checks that this square is not the left edge of the graph.
                if (i % size != 0 && vertices[i - 1].Color != Color.LightGray)
                {
                    // Sets the vertex neighboring left of the current box in the adjMatrix.
                    adjMatrix[i, i - 1] = 1;
                }

                // Checks that this square is not the right edge of the graph.
                if (i % size != (size - 1) && vertices[i + 1].Color != Color.LightGray)
                {
                    // Sets the vertex neighboring right of the current box in the adjMatrix.
                    adjMatrix[i, i + 1] = 1;
                }

                // Checks that this square is not the top edge of the graph
                if (i > size - 1 && vertices[i - size].Color != Color.LightGray)
                {
                    // Sets the vertex neighboring above of the current box in the adjMatrix.
                    adjMatrix[i, i - size] = 1;
                }

                // Checks that this square is not the bottom edge of the graph
                if (i < adjMatrix.GetLength(0) - size && vertices[i + size].Color != Color.LightGray)
                {
                    // Sets the vertex neighboring below of the current box in the adjMatrix.
                    adjMatrix[i, i + size] = 1;
                }
                

            }

        }

        /// <summary>
        /// Runs Dijkstra's Algorithm, finding the shortest path to the end target from the start.
        /// </summary>
        public void RunAlgorithm()
        {
            Reset();

            // sets the vertex at the startIndex as the starting square.
            int currentIndex = startIndex;
            vertices[currentIndex].Distance = 0;
            vertices[currentIndex].Permanent = true;

            // Loops while there are still unvisited vertices.
            while (NonPermanentsExist())
            {
                for (int i = 0; i < vertices.Count; i++)
                {
                    // gets adjacent
                    int adjWeight = adjMatrix[currentIndex, i];

                    // gets the calculated "cost" from the start vertex
                    int calculatedCost = vertices[currentIndex].Distance + 1;

                    // checks that the vert is adjacent and permanent
                    if (adjWeight != 0 && vertices[i].Permanent == false)
                    {
                        // if the calculated cost is less than the adj verts current distance, update the distance and the corresponding neighbor.
                        if (calculatedCost < vertices[i].Distance)
                        {
                            vertices[i].Distance = calculatedCost;
                            vertices[i].PathNeighbor = vertices[currentIndex];
                        }    
                    }
                }

                // sets the fully traversed vertex to permanent.
                vertices[currentIndex].Permanent = true;
                // gets the new index, which correspond to the smallest vertex.
                currentIndex = FindSmallestDistance();

                // checks if the end vertex has been found
                if(vertices[currentIndex].Color == Color.Red)
                {
                    // ends early
                    break;
                }


            }

       
        }

        /// <summary>
        /// Resets all the vertices in the graph.
        /// </summary>
        public void Reset()
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                // default values
                vertices[i].Distance = int.MaxValue;
                vertices[i].Permanent = false;
                vertices[i].PathNeighbor = null;
            }
        }

        /// <summary>
        /// Updates the tile colors to show all permanent visited vertices, and the shortest path to the target.
        /// </summary>
        public void UpdateTiles()
        {
            int destinationIndex = endIndex;
            Vertex current = vertices[destinationIndex];
 
            
            // Gets shortest path, by traversing path neighbors, coloring it green.
            while (current.Distance != 0)
            {
                current = current.PathNeighbor;

                if (current.Color != Color.Blue)
                {
                    current.Color = Color.Green;
                }

            }

            // Changes all permanent vertices to orange.
            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i].Permanent && vertices[i].Color != Color.Red && vertices[i].Color != Color.Blue && vertices[i].Color != Color.LightGray && vertices[i].Color != Color.Green)
                {
                    vertices[i].Color = Color.Orange;
                }
            }
        }

        /// <summary>
        /// Checks if non permanents still exist in the graph.
        /// </summary>
        /// <returns>True if non permanents exist, false otherwise</returns>
        public bool NonPermanentsExist()
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                // Ignores wall vertices.
                if (vertices[i].Permanent == false && vertices[i].Color != Color.LightGray)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the index value of the vertex with the smallest distance.
        /// </summary>
        /// <returns>The index of the vertex with the smallest distance.</returns>
        public int FindSmallestDistance()
        {
            int distance = int.MaxValue;
            int smallest = int.MaxValue;

            for (int i = 0; i < vertices.Count; i++)
            {
                // Checks if the vertex is not permanent and if it's distance is less than the current smallest distance.
                if (vertices[i].Permanent == false && vertices[i].Distance < distance)
                {
                    distance = vertices[i].Distance;
                    smallest = i;
                }
            }

            return smallest;

        }

        /// <summary>
        /// Prints out the adj matrix. (for bug testing)
        /// </summary>
        public void PrintAdj()
        {
            for (int i = 0; i < adjMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < adjMatrix.GetLength(1); j++)
                {
                    Debug.Write(adjMatrix[i, j]);
                }

                Debug.WriteLine("");
            }
        }

        /// <summary>
        /// Draws the mraph.
        /// </summary>
        /// <param name="sb">Sprite batch</param>
        public void Draw(SpriteBatch sb)
        {
            foreach (Vertex v in vertices)
            {
                v.Draw(sb);
            }
        }

    }

  
}
