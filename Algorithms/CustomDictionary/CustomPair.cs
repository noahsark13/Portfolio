using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomDictionary
{
    // Noah Kasper
    // Purpose: Represents a custompair, which holds both a key and a value.
    // Restrictions: None.
    internal class CustomPair<TKey, TValue>
    {
        private TKey key;
        private TValue value;

        /// <summary>
        /// Gets and sets the value of the custompair key
        /// </summary>
        public TKey Key { get { return key; } set { key = value; } }
        /// <summary>
        /// Gets and sets the value of the custompair value
        /// </summary>
        public TValue Value { get { return value; } set { this.value = value; } }

        /// <summary>
        /// Constructs a custompair object, with its key and value.
        /// </summary>
        /// <param name="key">The key of the custompair</param>
        /// <param name="value">The value of the custompair</param>
        public CustomPair(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }



        
    }
}
