using System;
using System.Text;
using System.Collections.Generic;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Maps;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Generic
{
    /// <summary>
    /// Represents a group of Enumeration Values with various options.
    /// </summary>
    public class SelectedOptions<T> where T : struct
    {
        #region Events
        /// <summary>
        /// Option Added Event, Fires when an option has been added
        /// </summary>
        public EventHandler<SelectedOptionEventArgs<T>> OptionAdded;
        /// <summary>
        /// Options Adding Event, Fires when an options is about to be added
        /// </summary>
        /// <remarks>
        /// Event will fire even if the option already exist.
        /// if <see cref="SelectedOptionCancelEventArgs{T}.Cancel"/> is set to true then Option will not be added.
        /// </remarks>
        public EventHandler<SelectedOptionCancelEventArgs<T>> OptionAdding;
        /// <summary>
        /// Option Removed Event, Fires when and option has been removed
        /// </summary>
        public EventHandler<SelectedOptionEventArgs<T>> OptionRemoved;
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Event will fire even if the option already exist
        /// if <see cref="SelectedOptionCancelEventArgs{T}.Cancel"/> is set to true then Option will not be removed.
        /// </remarks>
        public EventHandler<SelectedOptionCancelEventArgs<T>> OptionRemoving;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance
        /// </summary>
        public SelectedOptions()
        {
            if (!typeof(T).IsEnum) throw new NotSupportedException("Only Enum Types can be used");
           
        }
        #endregion

        /// <summary>
        /// Gets if the Enum exist in the List
        /// </summary>
        /// <param name="EnumItem">The T Enum to check for</param>
        /// <returns>
        /// True if exist in the internal list; Otherwise, false.
        /// </returns>
        public bool this[T EnumItem]
        {
            get
            {
                return this.Keys.Contains(EnumItem.ToString());
            }
        }

        #region Properties
        /// <summary>
        /// List of <see cref="EndCharsEnum"/> that represent the AutoHotkey EndChars
        /// </summary>
        protected internal List<T> Options { get; set; } = new List<T>();

        /// <summary>
        /// List of Keys that represent the Names of the Enum
        /// </summary>
        protected internal HashSet<string> Keys = new HashSet<string>();

        /// <summary>
        /// Gets a count of how may End Chars are in the current instance
        /// </summary>
        public virtual int Count
        {
            get
            {
                return this.Options.Count;
            }
        }

        /// <summary>
        /// Gets if the <see cref="SelectedOptions{T}"/> Contains a key. All Keys are names from Enum.
        /// </summary>
        public virtual KeyExist<string> HasKey
        {
            get
            {
                KeyExist<string> ke = new KeyExist<string>(ref this.Keys);
                return ke;
            }
        }
        #endregion

        #region Add / Remove
        /// <summary>
        /// Adds an enum item to the inner list
        /// </summary>
        /// <param name="EnumItem">The enum item to add</param>
        /// <remarks>
        /// If the <paramref name="EnumItem"/> exist it will not be added again.
        /// </remarks>
        public void Add(T EnumItem)
        {
            if(OptionAdding != null)
            {
                SelectedOptionCancelEventArgs<T> cArgs = new SelectedOptionCancelEventArgs<T>(EnumItem);
                OptionAdding(this, cArgs);
                if (cArgs.Cancel == true)
                {
                    return;
                }
            }
            if (this[EnumItem] == false)
            {
                this.Options.Add(EnumItem);
                this.Keys.Add(EnumItem.ToString());
                if (OptionAdded != null)
                    OptionAdded(this, new SelectedOptionEventArgs<T>(EnumItem));
            }

        }

        /// <summary>
        /// Add a range of Enum Items to the internal list
        /// </summary>
        /// <param name="EnumItems">The range of Enum Items to add</param>
        /// <remarks>
        /// If a item exist it will not be added again.
        /// </remarks>
        public void AddRange(T[] EnumItems)
        {
            if (EnumItems == null)
            {
                return;
            }
            foreach (var EnumItem in EnumItems)
            {
                if (OptionAdding != null)
                {
                    SelectedOptionCancelEventArgs<T> cArgs = new SelectedOptionCancelEventArgs<T>(EnumItem);
                    OptionAdding(this, cArgs);
                    if (cArgs.Cancel == true)
                    {
                        return;
                    }
                }
                if (this[EnumItem] == false)
                {
                    this.Options.Add(EnumItem);
                    this.Keys.Add(EnumItem.ToString());
                    if (OptionAdded != null)
                        OptionAdded(this, new SelectedOptionEventArgs<T>(EnumItem));
                }
            }
        }


        /// <summary>
        /// Removes an Enum Item from the inner list
        /// </summary>
        /// <param name="EnumItem">The rule to remove</param>
        public void Remove(T EnumItem)
        {
            if (OptionRemoving != null)
            {
                SelectedOptionCancelEventArgs<T> cArgs = new SelectedOptionCancelEventArgs<T>(EnumItem);
                OptionRemoving(this, cArgs);
                if (cArgs.Cancel == true)
                {
                    return;
                }
            }
            if (this[EnumItem] == true)
            {
                this.Options.Remove(EnumItem);
                this.Keys.Remove(EnumItem.ToString());
                if (OptionRemoved != null)
                    OptionRemoved(this, new SelectedOptionEventArgs<T>(EnumItem));
            }

        }

        /// <summary>
        /// Removes a range of Enum Items
        /// </summary>
        /// <param name="EnumItems">The range to remove</param>
        public void RemoveRange(T[] EnumItems)
        {
            if (EnumItems == null)
            {
                return;
            }
            foreach (var EnumItem in EnumItems)
            {
                if (OptionRemoving != null)
                {
                    SelectedOptionCancelEventArgs<T> cArgs = new SelectedOptionCancelEventArgs<T>(EnumItem);
                    OptionRemoving(this, cArgs);
                    if (cArgs.Cancel == true)
                    {
                        return;
                    }
                }
                if (this[EnumItem] == true)
                {
                    this.Options.Remove(EnumItem);
                    this.Keys.Remove(EnumItem.ToString());
                    if (OptionRemoved != null)
                        OptionRemoved(this, new SelectedOptionEventArgs<T>(EnumItem));
                }
            }
        }
        #endregion


        #region Parse
        /// <summary>
        /// Gets a Instance of <see cref="SelectedOptions{T}"/> From a String value
        /// </summary>
        /// <param name="s">The String to parse for values</param>
        /// <returns>
        /// Instance of <see cref="SelectedOptions{T}"/> if Parse succeeded; Otherwise, null.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static SelectedOptions<T> Parse(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException();
            }
            try
            {
                // all chars except for `n and `t are single chars
                var opt = new SelectedOptions<T>();

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
        /// <param name="ec">The <see cref="SelectedOptions{T}"/> that represents the parsed results</param>
        /// <returns>
        /// Returns True if the parse was successful; Otherwise false
        /// </returns>
        public static bool TryParse(string s, out SelectedOptions<T> ec)
        {
            bool retval = false;
            SelectedOptions<T> outec = null;
            try
            {
                SelectedOptions<T> ecs = SelectedOptions<T>.Parse(s);
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

        #region List
        /// <summary>
        /// Gets a new <see cref="SelectedOptions{T}"/> instance containing the selected values of <paramref name="ListItems"/>
        /// </summary>
        /// <param name="ListItems">The list containing one or more Selected Items to add to the list</param>
        /// <returns>
        /// A new Instance of <see cref="SelectedOptions{T}"/> with the values that were marked as Selected in the
        /// <paramref name="ListItems"/>
        /// </returns>
        public static SelectedOptions<T> FromList(IList<AhkKeyMapAhkMapValue> ListItems)
        {
            var opt = new SelectedOptions<T>();
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
        public virtual T[] ToArray()
        {
            return this.Options.ToArray();
        }

        /// <summary>
        /// Creates a new instance of <see cref="SelectedOptions{T}"/> populated with the 
        /// values in <paramref name="Options"/>
        /// </summary>
        /// <param name="Options">The Array of Enum Values use to populate the instance</param>
        /// <returns>
        /// Instance of <see cref="SelectedOptions{T}"/>. If <paramref name="Options"/> is null
        /// then return instance will have no values.
        /// </returns>
        public static SelectedOptions<T> FromArray(T[] Options)
        {
            SelectedOptions<T> opt = new SelectedOptions<T>();
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

        #region Overrides
        /// <summary>
        /// Gets a comma separated string representing the values of the <see cref="SelectedOptions{T}"/>
        /// </summary>
        /// <returns>Comma delimited string if Count is greater than zero; Otherwise, empty string</returns>
        public override string ToString()
        {
            if (this.Count == 0)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            int iMax = this.Count - 1;
            for (int i = 0; i < this.Count; i++)
            {
                T eValue = this.Options[i];
                sb.Append(eValue.ToString());
                if (i < iMax)
                {
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }
        #endregion
    }
}
