using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedList
{
    /// <summary>
    /// Represents a generic linked list.
    /// </summary>
    /// <typeparam name="T">Generic</typeparam>
    internal class CustomLinkedList<T>
    {
        private CustomLinkedNode<T> headNode;
        private CustomLinkedNode<T> tailNode;
        private int count;

        /// <summary>
        /// Default constructor for the linked list.
        /// </summary>
        public CustomLinkedList()
        {
            headNode = null;
            tailNode = null;
            count = 0;
        }

        /// <summary>
        /// Gets the current count of the list.
        /// </summary>
        public int Count { get { return count; } }

        /// <summary>
        /// Adds the specified data to the linked list
        /// </summary>
        /// <param name="data">The data to be added at the end of the list</param>
        public void Add(T data)
        {
            if (count == 0)
            {
                headNode = new CustomLinkedNode<T>(data);
                tailNode = headNode;
                count++;
            }
            else
            {
                tailNode.Next = new CustomLinkedNode<T>(data);
                tailNode = tailNode.Next;
                count++;
            }
        }

        /// <summary>
        /// Indexer for the linked list, which gets the correct data value at the 
        /// specified index.
        /// </summary>
        /// <param name="index">The index location of the data</param>
        /// <returns>The data value at the index</returns>
        /// <exception cref="IndexOutOfRangeException">Exception when specified index is not in the list</exception>
        public T this[int index]
        {
            get
            {   

                if (index >= count || index < 0)
                {
                    throw new IndexOutOfRangeException("Error: Invalid index specified during removal");
                }

                CustomLinkedNode<T> current = headNode;

                for (int i = 0; i < index; i++)
                {
                    current = current.Next;
                }

                return current.Data;
               
            

                return default;
            }

            
        }

        /// <summary>
        /// Removes a piece of data from the list at a specified index.
        /// </summary>
        /// <param name="index">The index to remove</param>
        /// <returns>The data removed</returns>
        /// <exception cref="IndexOutOfRangeException">Exception when specified index is not in the list</exception>
        public T RemoveAt(int index)
        {
            

            if (index < 0 || index >= count)
            {
                throw new IndexOutOfRangeException("Error: Invalid index specified during removal");
            }
            else if (index == 0) // Start of list
            {
                T returned = headNode.Data;
                headNode = headNode.Next;
                count--;

                return returned;
            }
            else if (index == count - 1) // End of list
            {
                CustomLinkedNode<T> current = headNode;

                for (int i = 0; i < index - 1; i++)
                {
                    current = current.Next;
                }

                T returned = current.Next.Data;

                tailNode = current;
                current.Next = null;
                count--;

                return returned;

            }
            else // Anywhere else
            {
                CustomLinkedNode<T> current = headNode;

                for (int i = 0; i < index - 1; i++)
                {
                    current = current.Next;
                }

                CustomLinkedNode<T> previous = current;
                
                current = current.Next;
                T returned = current.Data;
                previous.Next = current.Next;

                count--;

                return returned;

            }


            return default;
            
        }


    }
}
