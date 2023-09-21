using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomStacksAndQueues
{
    internal interface IStack<T>
    {
        /// <summary>
        /// Interface property for IsEmpty
        /// </summary>
        bool IsEmpty { get; }
        
        /// <summary>
        /// Interface property for count
        /// </summary>
        int Count { get; }
        
        /// <summary>
        /// Interface method for push
        /// </summary>
        /// <param name="s">The T item to add to the stack</param>
        void Push(T s);
        
        /// <summary>
        /// Interface method for Pop
        /// </summary>
        /// <returns>The popped item</returns>
        T Pop();

        /// <summary>
        /// Interface method for peek
        /// </summary>
        /// <returns>The item at the top of the stack</returns>
        T Peek();
    }
}
