using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomStacksAndQueues
{
    // Name: Noah Kasper
    // Purpose: Represent a custom GameQueue.
    internal class GameQueue<T> : IQueue<T>
    {
        List<T> queue;
        bool isEmpty;
        int count;

        /// <summary>
        /// Gets bool isEmpty
        /// </summary>
        public bool IsEmpty { get { return isEmpty; } }
        /// <summary>
        /// Gets current count
        /// </summary>
        public int Count { get { return count; } }

        /// <summary>
        /// Default constructor for GameQueue
        /// </summary>
        public GameQueue()
        {
            this.queue = new List<T>();
            this.isEmpty = true;
            this.count = queue.Count;
        }

        /// <summary>
        /// Adds a item to the end of the queue.
        /// </summary>
        /// <param name="s">The item to be enqueued</param>
        public void Enqueue(T s)
        {
            queue.Add(s);
            count++;
            isEmpty = false;
        }

        /// <summary>
        /// Removes the item at the front of the queue.
        /// </summary>
        /// <returns>The item dequeued</returns>
        public T Dequeue()
        {
            if (count == 0)
            {
                isEmpty = true;
                throw new Exception("Queue is empty");
            }

            T copy = queue[0];
            queue.RemoveAt(0);
            count--;

            return copy;
        }

        /// <summary>
        /// Peeks at the item at the front of the queue.
        /// </summary>
        /// <returns>The item at the front of the queue</returns>
        /// <exception cref="Exception">Thrown when peeking at an empty queue</exception>
        public T Peek()
        {
            try
            {
                return queue[0];
            }
            catch (Exception e)
            {
                throw new Exception("Queue is empty");
            }
        }
    }
}
