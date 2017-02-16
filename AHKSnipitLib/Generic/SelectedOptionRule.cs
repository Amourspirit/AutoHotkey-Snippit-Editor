using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Maps;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Generic
{
    /// <summary>
    /// Represents a group of Enumeration Values with various options and Exclude list for filtering output.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SelectedOptionRule<T> : SelectedOptions<T> where T : struct
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public SelectedOptionRule()
            : base()
        {

        }

        /// <summary>
        /// Creates a new instance specifying the exclude rules
        /// </summary>
        /// <param name="Exclude"></param>
        public SelectedOptionRule(EnumRule<T> Exclude)
            : this()
        {
            this.m_ExcludeRules = Exclude;
        }
        private EnumRule<T> m_ExcludeRules;
        /// <summary>
        /// Specifies the Rules to Exclude from output methods
        /// </summary>
        public EnumRule<T> ExcludeRules
        {
            get
            {
                if (this.m_ExcludeRules == null)
                {
                    this.m_ExcludeRules = new EnumRule<T>();
                }
                return m_ExcludeRules;
            }
            set
            {
                m_ExcludeRules = value;
            }
        }

        #region Parse
        /// <summary>
        /// Gets a Instance of <see cref="SelectedOptionRule{T}"/> From a String value
        /// </summary>
        /// <param name="s">The String to parse for values</param>
        /// <returns>
        /// Instance of <see cref="SelectedOptionRule{T}"/> if Parse succeeded; Otherwise, null.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static SelectedOptionRule<T> Parse(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException();
            }
            try
            {
                // all chars except for `n and `t are single chars
                var opt = new SelectedOptionRule<T>();

                var es = s.Split(',');

                foreach (string strEs in es)
                {
                    if (string.IsNullOrWhiteSpace(strEs))
                    {
                        continue;
                    }
                    T eValue = (T)Enum.Parse(typeof(T), strEs, false);
                    string sKey = eValue.ToString();
                    if (opt.HasKey[sKey] == false)
                    {
                        opt.Options.Add(eValue);
                        opt.Keys.Add(sKey);
                    }
                }

                return opt;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// Attempts to Parse a string representing one or more Options.
        /// </summary>
        /// <param name="s">The Options to Parse</param>
        /// <param name="ec">The <see cref="SelectedOptionRule{T}"/> that represents the parsed results</param>
        /// <returns>
        /// Returns True if the parse was successful; Otherwise false
        /// </returns>
        public static bool TryParse(string s, out SelectedOptionRule<T> ec)
        {
            bool retval = false;
            SelectedOptionRule<T> outec = null;
            try
            {
                SelectedOptionRule<T> ecs = SelectedOptionRule<T>.Parse(s);
                outec = ecs;
                retval = true;
            }
            catch (Exception)
            {
            }
            ec = outec;
            return retval;
        }
        #endregion Parse

        #region Overrides
        /// <summary>
        /// Gets a comma separated string representing the values of the <see cref="SelectedOptionRule{T}"/>
        /// </summary>
        /// <returns>Comma delimited string if Count is greater than zero; Otherwise, empty string</returns>
        /// <remarks>
        /// Excluded vales in <see cref="ExcludeRules"/> are committed from the output.
        /// </remarks>
        public override string ToString()
        {
            if (this.ExcludeRules.Count == 0)
            {
                return base.ToString();
            }
            if (this.Count == 0)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            int iMax = this.Count;
            int iCount = 0;
            for (int i = 0; i < this.Count; i++)
            {

                T eValue = this.Options[i];
                if (this.ExcludeRules[eValue.ToString()])
                {
                    iMax--;
                    continue;
                }
                iCount++;
                sb.Append(eValue.ToString());
                if (iCount < iMax)
                {
                    sb.Append(",");
                }
               
            }
            return sb.ToString();

        }
        #endregion

        #region List
        /// <summary>
        /// Gets a new <see cref="SelectedOptionRule{T}"/> instance containing the selected values of <paramref name="ListItems"/>
        /// </summary>
        /// <param name="ListItems">The list containing one or more Selected Items to add to the list</param>
        /// <returns>
        /// A new Instance of <see cref="SelectedOptionRule{T}"/> with the values that were marked as Selected in the
        /// <paramref name="ListItems"/>
        /// </returns>
        public new static SelectedOptionRule<T> FromList(IList<AhkKeyMapAhkMapValue> ListItems)
        {
            var opt = new SelectedOptionRule<T>();
            if (ListItems.Count == 0)
            {
                return opt;
            }
            foreach (var item in ListItems)
            {
                if (item.Selected == true)
                {
                    if (opt.HasKey[item.Key] == false)
                    {
                        if (Enum.IsDefined(typeof(T), item.Key))
                        {
                            T en;
                            if (Enum.TryParse(item.Key, out en) == true)
                            {
                                opt.Options.Add(en);
                                opt.Keys.Add(item.Key);
                            }
                        }
                    }
                }
            }
            return opt;
        }

        /// <summary>
        /// Gets An array of all the End Chars in current instance
        /// </summary>
        /// <returns>
        /// Array of options or if no values are present then Empty Array
        /// </returns>
        public override T[] ToArray()
        {
            return base.ToArray();
        }

        /// <summary>
        /// Creates a new instance of <see cref="SelectedOptionRule{T}"/> populated with the 
        /// values in <paramref name="Options"/>
        /// </summary>
        /// <param name="Options">The Array of Enum Values use to populate the instance</param>
        /// <returns>
        /// Instance of <see cref="SelectedOptionRule{T}"/>. If <paramref name="Options"/> is null
        /// then return instance will have no values.
        /// </returns>
        public new static SelectedOptionRule<T> FromArray(T[] Options)
        {
            SelectedOptionRule<T> opt = new SelectedOptionRule<T>();
            if (Options == null)
            {
                return opt;
            }
            foreach (var e in Options)
            {
                string sKey = e.ToString();
                if (opt.HasKey[sKey] == false)
                {
                    opt.Options.Add(e);
                    opt.Keys.Add(sKey);
                }
            }
            return opt;
        }
        #endregion
    }
}
