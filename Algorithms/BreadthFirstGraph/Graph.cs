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
            //adjMatrix[0, 2] = 1;
            adjMatrix[0, 9] = 1;
            adjMatrix[0, 8] = 1;
            adjMatrix[0, 5] = 1;
            adjMatrix[0, 10] = 1;

            // deck
            rooms["deck"].Add(vertices[0]);
            rooms["deck"].Add(vertices[2]);
            rooms["deck"].Add(vertices[11]);

            adjMatrix[10, 0] = 1;
            adjMatrix[10, 2] = 1;
            adjMatrix[10, 11] = 1;

            // conservatory
            rooms["conservatory"].Add(vertices[10]);
            rooms["conservatory"].Add(vertices[1]);

            adjMatrix[2, 10] = 1;
            adjMatrix[2, 1] = 1;

            // library
            rooms["library"].Add(vertices[2]);
            rooms["library"].Add(vertices[0]);

            adjMatrix[1, 2] = 1;
            adjMatrix[1, 0] = 1;

            // dining room
            rooms["dining room"].Add(vertices[0]);
            rooms["dining room"].Add(vertices[6]);

            adjMatrix[7, 0] = 1;
            adjMatrix[7, 6] = 1;

            // kitchen
            rooms["kitchen"].Add(vertices[7]);

            adjMatrix[6, 7] = 1;

            // gallery
            rooms["gallery"].Add(vertices[0]);
            rooms["gallery"].Add(vertices[8]);
            rooms["gallery"].Add(vertices[3]);

            adjMatrix[9, 0] = 1;
            adjMatrix[9, 8] = 1;
            adjMatrix[9, 3] = 1;

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

            adjMatrix[3, 9] = 1;
            adjMatrix[3, 4] = 1;


            // bathroom
            rooms["bathroom"].Add(vertices[3]);
            rooms["bathroom"].Add(vertices[5]);

            adjMatrix[4, 3] = 1;
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
        /// Resets the data held in the visited array to false.
        /// </summary>
        public void Reset()
        {
            for (int i = 0; i < visited.Length; i++)
            {
                visited[i] = false;
            }
        }

        /// <summary>
        /// Gets a adjacent and unvisited vertex corresponding to the given room.
        /// </summary>
        /// <param name="roomName">The room to check for adjacents</param>
        /// <returns>The first found adjacent and unvisited vertex, otherwise null.</returns>
        public Vertex GetAdjacentUnvisited(string roomName)
        {
            // Checks that room is valid
            if (MapContainsRoom(roomName))
            {
                // gets the index of the given room
                int roomLocation = GetRoomIndex(roomName);

                // loops through the corresponding adjMatrix column and visited array.
               
                for (int i = 0; i < adjMatrix.GetLength(1); i++)
                {
                    // checks if room is adjacent and has not been visited.
                    if (adjMatrix[roomLocation, i] == 1 && visited[i] == false)
                    {
                        return vertices[i];
                    }
                }
            }    

            return null;

        }

        /// <summary>
        /// Searches the graph using the "breadth first" method.
        /// </summary>
        /// <param name="roomName">The room to start the search at.</param>
        public void BreadthFirst(string roomName)
        {
            Vertex current;
            int vertexIndex;
            Queue<Vertex> queue = new Queue<Vertex>();

            // Checks that room is valid
            if (MapContainsRoom(roomName))
            {
                vertexIndex = GetRoomIndex(roomName);
                current = vertices[vertexIndex];

                // Resets current visited vertices.
                Reset();


                Console.WriteLine("- " + current.Name);
                queue.Enqueue(current);
                visited[vertexIndex] = true;

                // Loops while the queue has data
                while (queue.Count > 0)
                {
                    // Checks if the current head of the queue has any adjacent vertices that are unvisited.
                    Vertex adjacent = GetAdjacentUnvisited(queue.Peek().Name);
                    if (adjacent != null)
                    {
                        // Adds adjacent vertex to the queue, and makes it as visited.
                        Console.WriteLine("- " + adjacent.Name);
                        queue.Enqueue(adjacent);
                        visited[GetRoomIndex(adjacent.Name)] = true;
                    }
                    else
                    {
                        // If there is no longer any adjacent vertices to the head vertex, dequeue it.
                        queue.Dequeue();
                    }
                }
            }
            else
            {
                // Room isn't valid, print an error message.
                Console.WriteLine("The room " + roomName + " is not a valid room.");
                return;
            }
        }

        

        /// <summary>
        /// Lists all the vertices in the vertices list.
        /// </summary>
        public void ListAllVertices()
        {
            foreach (Vertex v in vertices)
            {
                Console.WriteLine(v.ToString());
            }
        }

        /// <summary>
        /// Checks that the entire map contains a specific room.
        /// </summary>
        /// <param name="room">The room to check.</param>
        /// <returns>True if the room is in the map, false otherwise.</returns>
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
        /// Checks that two rooms are adjacent.
        /// </summary>
        /// <param name="firstRoom">The first room.</param>
        /// <param name="secondRoom">The second room to check that the first is adjacent to.</param>
        /// <returns>True if they are adjacent, false otherwise.</returns>
        public bool AreAdjacent(string firstRoom, string secondRoom)
        {
            if (MapContainsRoom(firstRoom))
            {
                foreach (Vertex v in rooms[firstRoom])
                {
                    if (v.Name == secondRoom)
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }

        /// <summary>
        /// Gets all the adjacent rooms, of the given room name.
        /// </summary>
        /// <param name="room">The room to check the adjacent of.</param>
        /// <returns>The list of adjacent rooms.</returns>
        public List<Vertex> GetAdjacentList(string room)
        {
            if (MapContainsRoom(room))
            {
                return rooms[room];
            }

            return null;
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
