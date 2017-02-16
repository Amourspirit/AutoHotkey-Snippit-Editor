using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Generic
{

    #region KeyItem

    /// <summary>
    /// Creates an Index Property for a Class that has an internal dictionary of String, T
    /// </summary>
    public class KeyItem<T>
    {
        private Dictionary<string, T> m_dic;

        /// <summary>
        /// Creates a new instance of class
        /// </summary>
        /// <param name="dic">The dictionary to Access the value of</param>
        public KeyItem(ref Dictionary<string, T> dic)
        {
            this.m_dic = dic;
        }

        /// <summary>
        /// Gets the instance from the Dictionary
        /// </summary>
        /// <param name="Key">The key of the entry in the dictionary</param>
        /// <returns>
        /// Instance if key is valid; Otherwise null;
        /// </returns>
        public T this[string Key]
        {
            get
            {
                if (this.m_dic.ContainsKey(Key) == true)
                {
                    return this.m_dic[Key];
                }
                return default(T);

            }
        }
    }
    #endregion

    /// <summary>
    /// Generic Class use as a index Property for Classes that have an IList and a dictionary of String, Integer.
    /// The Dictionary Integer value will be the Index of the <see cref="IList{T}"/>
    /// </summary>
    public class KeyIndexItem<T>
    {
        // see Keys.EndCharKeyMap.ItemFromAhkSynbol for an example

        private Dictionary<string, int> m_indexDic;
        private IList<T> m_list;

        /// <summary>
        /// Creates a new instance of class
        /// </summary>
        /// <param name="IndexDic"></param>
        /// <param name="lst"></param>
        public KeyIndexItem(ref Dictionary<string, int> IndexDic, IList<T> lst)
        {
            this.m_indexDic = IndexDic;
            this.m_list = lst;
        }

        /// <summary>
        /// Gets the instance from the Dictionary
        /// </summary>
        /// <param name="Key">The key of the entry in the dictionary</param>
        /// <returns>
        /// Instance if key is valid; Otherwise null;
        /// </returns>
        public T this[string Key]
        {
            get
            {
                if (this.m_indexDic.ContainsKey(Key) == true)
                {
                    int index = this.m_indexDic[Key];
                    return this.m_list[index];
                }
                return default(T);

            }
        }

    }
}
