using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW4_DoublyLinkedList
{   
    /// <summary>
    /// Represents a "node" utilized in a linked list, which holds data, as well as
    /// references to both the previous and next node.
    /// </summary>
    /// <typeparam name="T">Generic type</typeparam>
    internal class CustomLinkedNode <T>
    {
        private T data;
        private CustomLinkedNode<T> nextNode;
        private CustomLinkedNode<T> previousNode;

        /// <summary>
        /// Gets and sets the actual data value of the node.
        /// </summary>
        public T Data { get { return data; } set { data = value; } }
   
        /// <summary>
        /// Gets and sets the next node in the linked list.
        /// </summary>
        public CustomLinkedNode<T> Next { get { return nextNode; } set { nextNode = value; } }


        /// <summary>
        /// Gets and sets the previous node in the linked list.
        /// </summary>
        public CustomLinkedNode<T> Previous { get { return previousNode; } set { previousNode = value; } }


        /// <summary>
        /// Constructor of a data node in the list.
        /// </summary>
        /// <param name="data">The data held in this node</param>
        public CustomLinkedNode(T data)
        {
            this.data = data;
            nextNode = null;
            previousNode = null;
        }
    }
}
