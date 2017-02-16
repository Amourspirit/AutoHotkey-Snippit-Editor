using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Generic
{
    /// <summary>
    /// Creates an Index Property for a Class that has an internal <see cref="HashSet{T}"/>
    /// </summary>
    /// <typeparam name="T">The Type of Key for the HashSet</typeparam>
    public class KeyExist<T>
    {
        private HashSet<T> m_keys;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="keys">HashSet that contains the keys</param>
        public KeyExist(ref HashSet<T> keys)
        {
            this.m_keys = keys;
        }

        /// <summary>
        /// Gets if the Key exist in the internal Keys
        /// </summary>
        /// <param name="Key">The Key to Check</param>
        /// <returns>
        /// True if the Key exist; Otherwise, false.
        /// </returns>
        public bool this[T Key]
        {
            get
            {
                return this.m_keys.Contains(Key);
            }
        }
    }
}
