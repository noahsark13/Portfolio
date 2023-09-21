using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomStacksAndQueues
{
    // Name: Noah Kasper
    // Purpose: Represent a custom GameStack.
    internal class GameStack<T> : IStack<T>
    {
        List<T> stack;
        bool isEmpty;
        int count;

        /// <summary>
        /// Gets bool isEmpty
        /// </summary>
        public bool IsEmpty { get { return isEmpty; } }

        /// <summary>
        /// Gets the current count
        /// </summary>
        public int Count { get { return count; } }

        /// <summary>
        /// Default Constructor for a GameStack object.
        /// </summary>
        public GameStack()
        {
            this.stack = new List<T>();
            this.isEmpty = true;
            this.count = stack.Count;
        }

        /// <summary>
        /// "Push method", adds the T item to the end of the stack.
        /// </summary>
        /// <param name="s">The item to be added</param>
        public void Push(T s)
        {
            stack.Add(s);
            isEmpty = false;
            count++;
        }

        /// <summary>
        /// "Pops" the item at the top of the stack
        /// </summary>
        /// <returns>The item to be removed from the top of the stack</returns>
        public T Pop()
        {

            T copy = stack[stack.Count - 1];
            stack.RemoveAt(stack.Count - 1);
            count--;

            if (count == 0)
            {
                isEmpty = true;
            }

            return copy;
        }

        /// <summary>
        /// Peeks at the item at the top of the stack
        /// </summary>
        /// <returns>The item to peek at</returns>
        /// <exception cref="Exception">Thrown when peeking at a empty stack.</exception>
        public T Peek()
        {
            try
            {
                return stack[stack.Count - 1];
            }
            catch (Exception e)
            {
                throw new Exception("Stack is empty");
            }
        }


    }
}
