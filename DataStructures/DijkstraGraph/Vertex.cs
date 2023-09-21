using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs
{
    /// <summary>
    /// Represents a individual vertex in a graph.
    /// </summary>
    internal class Vertex
    {
        private string roomName;
        private string roomDescription;

        private int sourceDist;
        private bool permanent;
        private Vertex pathNeighbor;

        /// <summary>
        /// Gets the name of the room.
        /// </summary>
        public string Name { get { return roomName; } }
        /// <summary>
        /// Gets the description of the room.
        /// </summary>
        public string Description { get { return roomDescription; } }
        /// <summary>
        /// Gets and sets the distance of a vertex to it's neighbor.
        /// </summary>
        public int Distance { get { return sourceDist; } set { sourceDist = value; } }
        /// <summary>
        /// Gets and sets if the vertex is permanent.
        /// </summary>
        public bool Permanent { get { return permanent; } set { permanent = value; } }
        /// <summary>
        /// Gets and sets the vertex's closest neighbor.
        /// </summary>
        public Vertex PathNeighbor { get { return pathNeighbor; } set { pathNeighbor = value; } }

        /// <summary>
        /// Constructor for the Vertex room.
        /// </summary>
        /// <param name="roomName">The name of the room.</param>
        /// <param name="roomDescription">The description of the room.</param>
        public Vertex(string roomName, string roomDescription)
        {
            this.roomName = roomName;
            this.roomDescription = roomDescription;

            sourceDist = int.MaxValue;
            permanent = false;
            pathNeighbor = null;
        }

        /// <summary>
        /// ToString override which displays information on the room.
        /// </summary>
        /// <returns>The formatted information.</returns>
        public override string ToString()
        {
            return $"{roomName.ToUpper()}: {roomDescription}";
        }
    }
}
