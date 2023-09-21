using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomDictionary
{
    // Noah Kasper
    // Purpose: A custom dictionary, that holds an array of lists of custom pairs.
    // Restrictions: None.

    internal class CustomDictionary<TKey, TValue>
    {
        private List<CustomPair<TKey, TValue>>[] data;
        private int count;

        // ========== Properties & Indexer ===========

        /// <summary>
        /// Gets the count of the dictionary
        /// </summary>
        public int Count { get { return count; } }

        /// <summary>
        /// Gets the load factor by returning the count divided by the length of the data array.
        /// </summary>
        public double LoadFactor
        {
            get
            {
                return (double)count / data.Length;
            }
        }

        /// <summary>
        /// Indexer for the custom dictionary
        /// </summary>
        /// <param name="index">The index or in this case the key of a custompair in the dictionary,
        /// to get or set their value</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">Exception thrown when there is no key to get.</exception>
        public TValue this[TKey index]
        {
            get
            {
                int hash = HashFunction(index);
                if (data[hash] != null)
                {

                    for (int i = 0; i < data[hash].Count; i++)
                    {
                        if (data[hash][i].Key.Equals(index))
                        {
                            return data[hash][i].Value;
                        }
                    }

                    throw new KeyNotFoundException("That key does not exist.");
                }
                else
                {
                    throw new KeyNotFoundException("That key does not exist.");
                }

            }
            set
            {
                int hash = HashFunction(index);
                bool keyFound = false;

                // creates a new list if one didn't exist
                if (data[hash] == null)
                {

                    data[hash] = new List<CustomPair<TKey, TValue>>();

                }

                // checks the list for the designated key
                for (int i = 0; i < data[hash].Count; i++)
                {
                    // if they key is found, the custompair's value is changed to the new value
                    if (data[hash][i].Key.Equals(index))
                    {
                        data[hash][i].Value = value;
                        keyFound = true; // bool to keep track if the correct key was found
      
                    }
                }
                
                // if the key wasnt found, a new custom pair is created at the designated index, with the value.
                if (keyFound == false)
                {
                    data[hash].Add(new CustomPair<TKey, TValue>(index, value));
                    count++;
                }


            }


        }

        // ========== Constructors ===========

        /// <summary>
        /// Default constructor, the array is initialized to 100 indices.
        /// </summary>
        public CustomDictionary()
        {
            data = new List<CustomPair<TKey, TValue>>[100];
            count = 0;
        }

        /// <summary>
        /// Parameterized constructor, that takes in the specified size of the array
        /// </summary>
        /// <param name="size">The size of the array</param>
        public CustomDictionary(int size)
        {
            data = new List<CustomPair<TKey, TValue>>[size];
            count = 0;
        }

        // ========== Methods ===========

        /// <summary>
        /// Checks if the Dictionary contains the specified key
        /// </summary>
        /// <param name="key">The key to find in the dictionary</param>
        /// <returns>True or False depending on if the key is found in the dictionary</returns>
        public bool ContainsKey(TKey key)
        {
            int hash = HashFunction(key);
            
            if (data[hash] != null)
            {
                for (int i = 0; i < data[hash].Count(); i++)
                {
                    if (data[hash][i].Key.Equals(key))
                    {
                        return true;
                    }
                }
            }

            return false;

   
        }

        /// <summary>
        /// Adds a new CustomPair to the dictionary. If a key is already in the dictionary, a exception will be thrown
        /// otherwise, if there is no list at the hash index, a new list is created, then a new CustomPair is added to the list
        /// with the specified Key, Value pair.
        /// </summary>
        /// <param name="key">The key of the new CustomPair</param>
        /// <param name="value">The value of the new CustomPair</param>
        /// <exception cref="Exception">Thrown when the key to be added already exists.</exception>
        public void Add(TKey key, TValue value)
        {

            if (ContainsKey(key))
            {
                throw new Exception("Error! That key already exists.");
            }
            else
            {
                int hash = HashFunction(key);

                if (data[hash] == null)
                {
                    data[hash] = new List<CustomPair<TKey, TValue>>();
                }

                CustomPair<TKey, TValue> cp = new CustomPair<TKey, TValue>(key, value);
                data[hash].Add(cp);
                count++;
            }
        }

        /// <summary>
        /// Removes the specified CustomPair with the key from the dictionary.
        /// </summary>
        /// <param name="key">The key of the custompair to be removed</param>
        /// <returns>True or False depending on if the CustomPair was removed or not.</returns>
        public bool Remove(TKey key)
        {

            if (ContainsKey(key))
            {
                int hash = HashFunction(key);

                for (int i = 0; i < data[hash].Count(); i++)
                {
                    if (data[hash][i].Key.Equals(key))
                    {
                        data[hash].RemoveAt(i);
                        count--;
                        return true;
                    }
                }
            }


            return false;
        }

        /// <summary>
        /// Clears the entire dictionary, setting every array index back to null, and
        /// resetting the count.
        /// </summary>
        public void Clear()
        {
            for(int i = 0; i < data.Length; i++)
            {
                data[i] = null;

            }

            count = 0;
        }

        /// <summary>
        /// The hash function used to compute the hash index of a custompair
        /// </summary>
        /// <param name="key">The key to be converted into a hash index</param>
        /// <returns>The computed hash code</returns>
        public int HashFunction(TKey key)
        {

            return Math.Abs(key.GetHashCode() % data.Length);
        }


    }

   
}
