using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomStacksAndQueues
{
    internal interface IQueue<T>
    {
        /// <summary>
        /// Interface property for isEmpty
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Interface property for count
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Interface method for Enqueue
        /// </summary>
        /// <param name="s">Item to enqueue into the queue</param>
        void Enqueue(T s);

        /// <summary>
        /// Interface method for dequeue
        /// </summary>
        /// <returns>The item removed from the front of the queue</returns>
        T Dequeue();

        /// <summary>
        /// Interface method for peek
        /// </summary>
        /// <returns>The item at the front of the queue</returns>
        T Peek();
    }
}
