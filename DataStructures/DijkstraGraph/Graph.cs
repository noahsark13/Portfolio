using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs
{
    /// <summary>
    /// Represents a graph map made up of vertices.
    /// </summary>
    internal class Graph
    {
        private List<Vertex> vertices;
        private Dictionary<string, List<Vertex>> rooms;
        private int[,] adjMatrix;
        private bool[] visited;

        /// <summary>
        /// Constructor for the graph: creates the list of vertices and the dictionary of their adjacent verts.
        /// </summary>
        public Graph()
        {
            vertices = new List<Vertex>();
            rooms = new Dictionary<string, List<Vertex>>();
            

            AddData("main hall", "The main hall is central to the house."); //0
            AddData("library", "This library is packed with floor-to-ceiling bookshelves."); //1
            AddData("conservatory", "The glass wall allows sunlight to reach the plants here."); //2
            AddData("billiards room", "We got a pool table!"); //3
            AddData("bathroom", "Adorned with the finest tilework."); //4
            AddData("study", "Two large chairs, a fireplace, and a bearskin rug."); //5
            AddData("kitchen", "Large enough to prepare a feast."); //6
            AddData("dining room", "A huge table for sixteen has gold place settings."); //7
            AddData("ballroom", "A room full of balls."); //8
            AddData("gallery", "Exquisite artwork decorates the walls."); //9
            AddData("deck", "This covered deck looks over the landscaped grounds."); //10
            AddData("exit", "Cobblestone pathway leads you to the gardens."); //11

            adjMatrix = new int[vertices.Count, vertices.Count];
            
            visited = new bool[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                visited[i] = false;
            }


            // main hall
            rooms["main hall"].Add(vertices[1]);
            rooms["main hall"].Add(vertices[7]);
            rooms["main hall"].Add(vertices[9]);
            rooms["main hall"].Add(vertices[8]);
            rooms["main hall"].Add(vertices[5]);
            rooms["main hall"].Add(vertices[10]);

            // [row, col]
            adjMatrix[0, 1] = 1;
            adjMatrix[0, 7] = 1;
            adjMatrix[0, 2] = 7;
            adjMatrix[0, 9] = 4;
            adjMatrix[0, 8] = 1;
            adjMatrix[0, 5] = 1;
            adjMatrix[0, 10] = 8;

            // deck
            rooms["deck"].Add(vertices[0]);
            rooms["deck"].Add(vertices[2]);
            rooms["deck"].Add(vertices[11]);

            adjMatrix[10, 0] = 8;
            adjMatrix[10, 2] = 1;
            adjMatrix[10, 11] = 1;

            // conservatory
            rooms["conservatory"].Add(vertices[10]);
            rooms["conservatory"].Add(vertices[1]);

            adjMatrix[2, 10] = 1;
            adjMatrix[2, 1] = 5;
            adjMatrix[2, 0] = 7;

            // library
            rooms["library"].Add(vertices[2]);
            rooms["library"].Add(vertices[0]);

            adjMatrix[1, 2] = 5;
            adjMatrix[1, 0] = 1;

            // dining room
            rooms["dining room"].Add(vertices[0]);
            rooms["dining room"].Add(vertices[6]);

            adjMatrix[7, 0] = 1;
            adjMatrix[7, 6] = 4;

            // kitchen
            rooms["kitchen"].Add(vertices[7]);

            adjMatrix[6, 7] = 4;

            // gallery
            rooms["gallery"].Add(vertices[0]);
            rooms["gallery"].Add(vertices[8]);
            rooms["gallery"].Add(vertices[3]);

            adjMatrix[9, 0] = 4;
            adjMatrix[9, 8] = 1;
            adjMatrix[9, 3] = 4;

            // ballroom
            rooms["ballroom"].Add(vertices[0]);
            rooms["ballroom"].Add(vertices[9]);
            rooms["ballroom"].Add(vertices[5]);

            adjMatrix[8, 0] = 1;
            adjMatrix[8, 9] = 1;
            adjMatrix[8, 5] = 1;

            // billiards room
            rooms["billiards room"].Add(vertices[9]);
            rooms["billiards room"].Add(vertices[4]);

            adjMatrix[3, 9] = 4;
            adjMatrix[3, 4] = 2;


            // bathroom
            rooms["bathroom"].Add(vertices[3]);
            rooms["bathroom"].Add(vertices[5]);

            adjMatrix[4, 3] = 2;
            adjMatrix[4, 5] = 1;


            // study
            rooms["study"].Add(vertices[4]);
            rooms["study"].Add(vertices[8]);
            rooms["study"].Add(vertices[0]);

            adjMatrix[5, 4] = 1;
            adjMatrix[5, 8] = 1;
            adjMatrix[5, 0] = 1;


            // exit
            rooms["exit"].Add(vertices[10]);

            adjMatrix[11, 10] = 1;


        }

        /// <summary>
        /// Determines the shortest path for each vertex room using Dijkstra's algorithm.
        /// </summary>
        /// <param name="roomName">The room to start at.</param>
        public void ShortestPath(string roomName)
        {
            Vertex current;
            Reset();

            // Sets starting vertex.
            int sourceIndex = GetRoomIndex(roomName);
            vertices[sourceIndex].Permanent = true;
            vertices[sourceIndex].Distance = 0;
            current = vertices[sourceIndex];

            // Loops while there are still non permanent, comparing adjacent and nonpermanent vertices
            // to determine their smallest distance.
            while (NonPermanentsExist())
            {
                // Loops through all vertices
                for (int i = 0; i < vertices.Count; i++)
                {
                    int adjWeight = adjMatrix[GetRoomIndex(current.Name), i];
                    int calculated = current.Distance + adjWeight;

                    // Checks if a vertex is adjacent and is not permanent.
                    if (adjWeight != 0 && vertices[i].Permanent == false)
                    {
                        // If the weight to travel to the adjacent matrix is less than it's current 
                        // distance, change the vertex's label.
                        if (calculated < vertices[i].Distance)
                        {
                            vertices[i].Distance = adjWeight;
                            vertices[i].PathNeighbor = current;

                        }
                    }
                }

                // After checking all adj vertices of the current vertex, set the
                // current to the vertex with the smallest distance.
                current = FindSmallestDistance();
                current.Permanent = true;
            }

      
        }

        /// <summary>
        /// Prints the shortest path to get to a given vertex.
        /// </summary>
        /// <param name="roomName">The vertex to locate.</param>
        public void PrintPath(string roomName)
        {
            
            int destinationIndex = GetRoomIndex(roomName);
            Vertex current = vertices[destinationIndex];
            int totalCost = current.Distance;

            //// Get total distance
            //while (current.Distance != 0)
            //{
            //    current = current.PathNeighbor;
            //    totalCost += current.Distance;
            //}

            Console.WriteLine("The shortest path (cost of " + (current.Distance + 1) + ") is: ");

            // Print path
            current = vertices[destinationIndex];
            Console.WriteLine(" " + current.Name);

            while (current.Distance != 0)
            {
                current = current.PathNeighbor;
                Console.WriteLine(" " + current.Name);
                
            }

        }

        /// <summary>
        /// Finds the vertex with the smallest distance.
        /// </summary>
        /// <returns>The vertex with the smallest distance.</returns>
        public Vertex FindSmallestDistance()
        {
            int distance = int.MaxValue;
            Vertex smallest = null;

            for (int i = 0; i < vertices.Count; i++)
            {
                // Checks if the vertex is not permanent and if it's distance is less than the current smallest distance.
                if (vertices[i].Permanent == false && vertices[i].Distance < distance)
                {
                    distance = vertices[i].Distance;
                    smallest = vertices[i];
                }
            }

            return smallest;


        }

        /// <summary>
        /// Helper method that checks that there are still nonpermanents in the graph.
        /// </summary>
        /// <returns>True if there are still nonpermanents. False otherwise.</returns>
        public bool NonPermanentsExist()
        {

            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i].Permanent == false)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Resets the labels of the vertices.
        /// </summary>
        public void Reset()
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i].Distance = int.MaxValue;
                vertices[i].Permanent = false;
                vertices[i].PathNeighbor = null;
            }
        }

       

        ///// <summary>
        ///// Checks that the entire map contains a specific room.
        ///// </summary>
        ///// <param name="room">The room to check.</param>
        ///// <returns>True if the room is in the map, false otherwise.</returns>
        public bool MapContainsRoom(string room)
        {
            // Uses dictionary method for simplicity.
            if (rooms.ContainsKey(room))
            {
                return true;
            }

            return false;
        }

        

        /// <summary>
        /// Helper method to add the data to both the list and dictonary.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public void AddData(string name, string description)
        {
            Vertex v = new Vertex(name, description);
            vertices.Add(v);
            rooms.Add(name, new List<Vertex>());
        }

        /// <summary>
        /// Helper method to get the index of the specified room.
        /// </summary>
        /// <param name="name">The name of the room</param>
        /// <returns>The index of the room in the verteces list.</returns>
        public int GetRoomIndex(string name)
        {
 
            for (int i = 0; i <= vertices.Count; i++)
            {
                if (vertices[i].Name == name)
                {
                    return i;
                }

            }

            return -1;
        }
    }
}
