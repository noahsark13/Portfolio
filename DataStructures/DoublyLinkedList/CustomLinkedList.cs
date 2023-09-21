using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW4_DoublyLinkedList
{
    /// <summary>
    /// Represents a recreation of a linked list.
    /// </summary>
    /// <typeparam name="T">Generic type.</typeparam>
    internal class CustomLinkedList <T>
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
        /// Indexer for the linked list, which can get a data value from the 
        /// specified index, as well as set/change the value.
        /// </summary>
        /// <param name="index">The index location of the data</param>
        /// <returns>The data value at the index</returns>
        /// <exception cref="IndexOutOfRangeException">Exception when specified index is not in the list</exception>
        public T this[int index]
        {
            get
            {
                // Checks index is in range.
                if (index >= count || index < 0)
                {
                    throw new IndexOutOfRangeException("Error: Cannot get data from invalid index " + index + ".");
                }

                // Checks for if the data is located in the first half of the list.
                if (index <= (count - 1) / 2)
                {
                    CustomLinkedNode<T> current = headNode;

                    // Loops from the head, forward through the first half.
                    for (int i = 0; i < index; i++)
                    {
                        current = current.Next;
                    }

                    // returns the found data.
                    return current.Data;

                }
                // Checks for if the data is located in the second half of the list.
                else if (index > (count - 1) / 2)
                {
                    CustomLinkedNode<T> current = tailNode;

                    // Loops from the tail, backwards in the second half.
                    for (int i = 0; i < (count - 1) - index; i++)
                    {
                        current = current.Previous;
                    }

                    // returns the found data.
                    return current.Data;
                }
                
                return default;
            }
            set
            {
                if (index >= count || index < 0)
                {
                    throw new IndexOutOfRangeException("Error: Cannot set data at invalid index " + index + ".");
                }

                // Checks for if the data is located in the first half of the list.
                if (index <= (count - 1) / 2)
                {
                    CustomLinkedNode<T> current = headNode;

                    // Loops from the head, forward through the first half.
                    for (int i = 0; i < index; i++)
                    {
                        current = current.Next;
                    }

                    // sets the found index data to the new value
                    current.Data = value;

                }
                // Checks for if the data is located in the second half of the list.
                else if (index > (count - 1) / 2)
                {
                    CustomLinkedNode<T> current = tailNode;

                    // Loops from the tail, backwards in the second half.
                    for (int i = 0; i < (count - 1) - index; i++)
                    {
                        current = current.Previous;
                    }

                    // sets the found index data to the new value
                    current.Data = value;
                }
            }
        }

        /// <summary>
        /// Adds the specified data to the end of the linked list.
        /// </summary>
        /// <param name="data">The data to be added at the end of the list</param>
        public void Add(T data)
        {
            // Checks if list is empty.
            if (count == 0)
            {
                headNode = new CustomLinkedNode<T>(data);
                tailNode = headNode;
                count++;
            }
            else
            {
                // Creates a new node after the current node, properly sets it's previous,
                // then sets it as the new tail node.
                tailNode.Next = new CustomLinkedNode<T>(data);
                tailNode.Next.Previous = tailNode;
                tailNode = tailNode.Next;
                count++;
            }
        }

        /// <summary>
        /// Clears the entire list be cleaning the head and tail nodes, as well as setting
        /// count back to 0.
        /// </summary>
        public void Clear()
        {
            headNode = null;
            tailNode = null;
            count = 0;
        }


        /// <summary>
        /// Removes a piece of data from the list at a specified index.
        /// </summary>
        /// <param name="index">The index to remove data from.</param>
        /// <returns>The data removed</returns>
        /// <exception cref="IndexOutOfRangeException">Exception when specified index is not in the list</exception>
        public T RemoveAt(int index)
        {
            if (index < 0 || index >= count)
            {
                throw new IndexOutOfRangeException("Error: Cannot remove invalid index " + index + ".");
            }

            // If the list only has one piece of data left, the list is cleared.
            if (count == 1)
            {
                T removedData = headNode.Data;
                Clear();
                return removedData;
            }
            // Start of list
            else if (index == 0)
            {
                T removedData = headNode.Data;

                // Sets the head node to the data after the original head node.
                headNode = headNode.Next;
                count--;

                return removedData;
            }
            // End of list
            else if (index == count - 1)
            {
                T removedData = tailNode.Data;

                // Sets the tail node to the old tail node's previous. Then sets the
                // next node to null.
                tailNode = tailNode.Previous;
                tailNode.Next = null;
                count--;

                return removedData;

            }
            // First half
            else if (index <= (count - 1)/2) 
            {
                CustomLinkedNode<T> current = headNode;

                for (int i = 0; i < index; i++)
                {
                    current = current.Next;
                }

                T removedData = current.Data;

                // Sets previous node's next, to the next node after the current node being removed.
                // Then sets the current's next node's previous to the current's previous node.
                // Effectively "removing" the current node.
                current.Previous.Next = current.Next;
                current.Next.Previous = current.Previous;

                count--;

                return removedData;

            }
            // Second half
            else if (index > (count - 1)/2)
            {
                CustomLinkedNode<T> current = tailNode;

                for (int i = 0; i < (count - 1) - index; i++)
                {
                    current = current.Previous;
                }

                T removedData = current.Data;

                // Sets previous node's next, to the next node after the current node being removed.
                // Then sets the current's next node's previous to the current's previous node.
                // Effectively "removing" the current node.
                current.Previous.Next = current.Next;
                current.Next.Previous = current.Previous;

                count--;

                return removedData;
            }

            return default;

        }

        /// <summary>
        /// Inserts a new data node at the specified index of the list.
        /// </summary>
        /// <param name="item">The data to add to the list.</param>
        /// <param name="index">The index to insert the data.</param>
        /// <exception cref="IndexOutOfRangeException">Exception thrown when index is invalid.</exception>
        public void Insert(T item, int index)
        {
            if (index < 0 || index > count)
            {
                throw new IndexOutOfRangeException("Error: Cannot insert into invalid index " + index + ".");
            }

            // Checks for both, the end of the list or also when the list is empty.
            if (index == count) 
            {
                // Since data is just being added for these cases,
                // the add method works here.
                Add(item);

            }
            // Start of list
            else if (index == 0)
            {
                headNode.Previous = new CustomLinkedNode<T>(item);
                headNode.Previous.Next = headNode;
                headNode = headNode.Previous;

                count++;
            }
            // First half
            else if (index <= (count - 1) / 2)
            {
                CustomLinkedNode<T> current = headNode;

                for (int i = 0; i < index; i++)
                {
                    current = current.Next;
                }

                // Set previous node's next to a new node. Then set the new node's previous
                // to the node that is supposed to be behind it. Then set the new current node previous
                // to the new node. Then set the new nodes next to the current node.
                current.Previous.Next = new CustomLinkedNode<T>(item);
                current.Previous.Next.Previous = current.Previous;
                current.Previous = current.Previous.Next;
                current.Previous.Next = current;

                count++;

            }
            // Second half
            else if (index > (count - 1) / 2)
            {
                CustomLinkedNode<T> current = tailNode;

                for (int i = 0; i < (count - 1) - index; i++)
                {
                    current = current.Previous;
                }

                // Sets the previous node's next to a new node. Then sets this new node's previous to the current's previous node.
                // Then the current node's previous is set to the new node. Finally, the new nodes next is set to the current node.
                // "Inserting" this new node at the specified index.
                current.Previous.Next = new CustomLinkedNode<T>(item);
                current.Previous.Next.Previous = current.Previous;
                current.Previous = current.Previous.Next;
                current.Previous.Next = current;

                count++;
            }

        }

        /// <summary>
        /// Prints the current list in the forwards directed order. If the list is empty,
        /// a message is printed.
        /// </summary>
        public void PrintForward()
        {
            CustomLinkedNode<T> current = headNode;

            if (count == 0)
            {
                Console.WriteLine("There are no items in the list.");
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    Console.WriteLine(current.Data);
                    current = current.Next;
                }

            }
          
        }

        /// <summary>
        /// Prints the current list in the backwards directed order. If the list is empty,
        /// a message is printed.
        /// </summary>
        public void PrintBackward()
        {
            CustomLinkedNode<T> current = tailNode;

            if (count == 0)
            {
                Console.WriteLine("There are no items in the list.");
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    Console.WriteLine(current.Data);
                    current = current.Previous;
                }

            }

        }
    }


}
