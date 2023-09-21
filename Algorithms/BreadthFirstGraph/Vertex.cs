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

        /// <summary>
        /// Gets the name of the room.
        /// </summary>
        public string Name { get { return roomName; } }
        /// <summary>
        /// Gets the description of the room.
        /// </summary>
        public string Description { get { return roomDescription; } }

        /// <summary>
        /// Constructor for the Vertex room.
        /// </summary>
        /// <param name="roomName">The name of the room.</param>
        /// <param name="roomDescription">The description of the room.</param>
        public Vertex(string roomName, string roomDescription)
        {
            this.roomName = roomName;
            this.roomDescription = roomDescription;
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
