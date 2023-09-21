using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedList
{
    /// <summary>
    /// Represents a node in the linked list, which holds a piece of data
    /// and the next node in the link.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class CustomLinkedNode<T>
    {
        private T data;
        private CustomLinkedNode<T> nextNode;

        /// <summary>
        /// Gets and sets the actual data value of the node
        /// </summary>
        public T Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

        /// <summary>
        /// Gets and sets the next node in the linked list
        /// </summary>
        public CustomLinkedNode<T> Next
        {
            get
            {
                return nextNode;
            }
            set
            {
                nextNode = value;
            }
        }

        /// <summary>
        /// Constructor of a node
        /// </summary>
        /// <param name="data"></param>
        public CustomLinkedNode(T data)
        {
            this.data = data;
            nextNode = null;
        }

           

    }
}
