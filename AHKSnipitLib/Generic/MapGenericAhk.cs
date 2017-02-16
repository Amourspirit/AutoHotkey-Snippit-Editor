using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Generic
{
    /// <summary>
    /// Abstract Class that is the base class for implementation of <see cref="AhkKeyMap"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MapGenericAhk<T> : AhkKeyMap where T : struct
    {

        #region Properties

        /// <summary>
        /// Gets instance of <see cref="AhkKeyMapAhkMapValue"/> that represents the AutoHotKey Key Symbol passed in.
        /// If Symbol is not found then Null is returned.
        /// </summary>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public KeyIndexItem<AhkKeyMapAhkMapValue> ItemFromAhkSynbol
        {
            get
            {
                KeyIndexItem<AhkKeyMapAhkMapValue> itm = new KeyIndexItem<AhkKeyMapAhkMapValue>(ref this.AhkSymbolMap, this.AhkMapValue);
                return itm;
            }
        }
        #endregion

        /// <summary>
        /// Dictionary of Key Name and index if Item in Array
        /// </summary>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        protected internal Dictionary<string, int> InternalMap = new Dictionary<string, int>();

        /// <summary>
        /// Dictionary of Key and index of item in array
        /// </summary>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        protected internal Dictionary<string, int> AhkSymbolMap = new Dictionary<string, int>();



        /// <summary>
        /// Get the AhkKeyMapAhkMapValue represented by the Key
        /// </summary>
        /// <param name="Key">Is a String Key that is a Enum Name value</param>
        /// <returns>
        /// Instance of AhkKeyMapAhkMapValue if a Key is matched; Otherwise null.
        /// </returns>
        protected internal AhkKeyMapAhkMapValue this[string Key]
        {
            get
            {
                if (this.InternalMap.ContainsKey(Key))
                {
                    int index = this.InternalMap[Key];
                    return this.AhkMapValue[index];
                }

                return default(AhkKeyMapAhkMapValue);
            }
        }


        

        /// <summary>
        /// Creates a Binding List of <see cref="AhkKeyMapAhkMapValue"/> objects representing all the Values
        /// in T enum. The List is Sorted Based upon <see cref="AhkKeyMapAhkMapValue.Sort"/>
        /// </summary>
        /// <returns>
        /// Binding list of <see cref="AhkKeyMapAhkMapValue"/>
        /// </returns>
        /// <remarks>
        /// <see cref="AhkKeyMapAhkMapValue.Parent"/> is set to the Binding List.
        /// </remarks>
        public virtual BindingList<AhkKeyMapAhkMapValue> ToBindingList()
        {
            SelectedOptions<T> opt = new SelectedOptions<T>();
            return ToBindingList(opt);
        }


        /// <summary>
        /// Creates a Binding List of <see cref="AhkKeyMapAhkMapValue"/> objects representing all the Value
        /// in T enum. The List is Sorted Based upon <see cref="AhkKeyMapAhkMapValue.Sort"/>
        /// </summary>
        /// <param name="SelectedValues">
        /// Instance of <see cref="SelectedOptions{T}"/>. Each value in SelectedValues will set the
        /// corresponding <see cref="AhkKeyMapAhkMapValue.Selected"/> to true;
        /// </param>
        /// <returns>
        /// Binding list of <see cref="AhkKeyMapAhkMapValue"/>
        /// </returns>
        /// <remarks>
        /// <see cref="AhkKeyMapAhkMapValue.Parent"/> is set to the Binding List.
        /// </remarks>
        public virtual BindingList<AhkKeyMapAhkMapValue> ToBindingList(SelectedOptions<T> SelectedValues)
        {
            BindingList<AhkKeyMapAhkMapValue> bl = new BindingList<AhkKeyMapAhkMapValue>();
            if (this.AhkMapValue == null)
            {
                return bl;
            }
            List<AhkKeyMapAhkMapValue> SortList = new List<AhkKeyMapAhkMapValue>();
            foreach (var map in this.AhkMapValue)
            {
                AhkKeyMapAhkMapValue newMap = new AhkKeyMapAhkMapValue();
                // newMap.Parent = bl;
                newMap.AutoHotKeyValue = map.AutoHotKeyValue;
                newMap.DisplayValue = map.DisplayValue;
                newMap.Key = map.Key;
                newMap.Sort = map.Sort;
                if (SelectedValues.HasKey[map.Key])
                {
                    newMap.Selected = true;
                }
                SortList.Add(newMap);
            }
            // Sort the list based upon sort values for display.
            // This comes into play when the binding list is bound to a list control
            // such as a drop down list.
            SortList.Sort((value1, value2) => value1.Sort.CompareTo(value2.Sort));

            // Now that the list is sorted add the values to the binding list.
            foreach (var map in SortList)
            {
                map.Parent = bl;
                bl.Add(map);
            }
            SortList.Clear();
            return bl;
        }

        /// <summary>
        /// Creates a Binding List of <see cref="AhkKeyMapAhkMapValue"/> objects representing all the Value
        /// in T enum. The List is Sorted Based upon <see cref="AhkKeyMapAhkMapValue.Sort"/>
        /// </summary>
        /// <param name="SelectedValues">
        /// Instance of <see cref="SelectedOptions{T}"/>. Each value in SelectedValues will set the
        /// corresponding <see cref="AhkKeyMapAhkMapValue.Selected"/> to true;
        /// </param>
        /// <param name="ExcludeRules">
        /// If any rule matches the exclude then the matched item will not be
        /// outputted in the bind list.
        /// </param>
        /// <returns>
        /// Binding list of <see cref="AhkKeyMapAhkMapValue"/>
        /// </returns>
        /// <remarks>
        /// <see cref="AhkKeyMapAhkMapValue.Parent"/> is set to the Binding List.
        /// </remarks>
        public virtual BindingList<AhkKeyMapAhkMapValue> ToBindingList(SelectedOptions<T> SelectedValues, EnumRule<T> ExcludeRules)
        {
            BindingList<AhkKeyMapAhkMapValue> bl = new BindingList<AhkKeyMapAhkMapValue>();
            if (this.AhkMapValue == null)
            {
                return bl;
            }
            List<AhkKeyMapAhkMapValue> SortList = new List<AhkKeyMapAhkMapValue>();
            foreach (var map in this.AhkMapValue)
            {
               
                AhkKeyMapAhkMapValue newMap = new AhkKeyMapAhkMapValue();
                if (ExcludeRules[map.Key])
                {
                    // exclude rule continue
                    continue;
                }
                // newMap.Parent = bl;
                newMap.AutoHotKeyValue = map.AutoHotKeyValue;
                newMap.DisplayValue = map.DisplayValue;
                newMap.Key = map.Key;
                newMap.Sort = map.Sort;
                if (SelectedValues.HasKey[map.Key])
                {
                    newMap.Selected = true;
                }
                SortList.Add(newMap);
            }
            // Sort the list based upon sort values for display.
            // This comes into play when the binding list is bound to a list control
            // such as a drop down list.
            SortList.Sort((value1, value2) => value1.Sort.CompareTo(value2.Sort));

            // Now that the list is sorted add the values to the binding list.
            foreach (var map in SortList)
            {
                map.Parent = bl;
                bl.Add(map);
            }
            SortList.Clear();
            return bl;
        }

        /// <summary>
        /// Creates a List of <see cref="AhkKeyMapAhkMapValue"/> objects representing all the Value
        /// in T enum. The List is Sorted Based upon <see cref="AhkKeyMapAhkMapValue.Sort"/>
        /// </summary>
        /// <returns>
        /// List of <see cref="AhkKeyMapAhkMapValue"/>
        /// </returns>
        /// <remarks>
        /// <see cref="AhkKeyMapAhkMapValue.Parent"/> is set to the List.
        /// </remarks>
        public virtual List<AhkKeyMapAhkMapValue> ToList()
        {
            SelectedOptions<T> ec = new SelectedOptions<T>();
            return ToList(ec);
        }

        /// <summary>
        /// Creates a List of <see cref="AhkKeyMapAhkMapValue"/> objects representing all the Value
        /// in T enum. The List is Sorted Based upon <see cref="AhkKeyMapAhkMapValue.Sort"/>
        /// </summary>
        /// <param name="SelectedValues">
        /// Instance of <see cref="SelectedOptions{T}"/>. Each value in SelectedValues will set the
        /// corresponding <see cref="AhkKeyMapAhkMapValue.Selected"/> to true;
        /// </param>
        /// <returns>
        /// List of <see cref="AhkKeyMapAhkMapValue"/>
        /// </returns>
        /// <remarks>
        /// <see cref="AhkKeyMapAhkMapValue.Parent"/> is set to the List.
        /// </remarks>
        public virtual List<AhkKeyMapAhkMapValue> ToList(SelectedOptions<T> SelectedValues)
        {
            List<AhkKeyMapAhkMapValue> SortList = new List<AhkKeyMapAhkMapValue>();
            if (this.AhkMapValue == null)
            {
                return SortList;
            }

            foreach (var map in this.AhkMapValue)
            {
                AhkKeyMapAhkMapValue newMap = new AhkKeyMapAhkMapValue();
                // newMap.Parent = bl;
                newMap.AutoHotKeyValue = map.AutoHotKeyValue;
                newMap.DisplayValue = map.DisplayValue;
                newMap.Key = map.Key;
                newMap.Sort = map.Sort;
                if (SelectedValues.HasKey[map.Key])
                {
                    newMap.Selected = true;
                }
                newMap.Parent = SortList;
                SortList.Add(newMap);
            }
            // Sort the list based upon sort values for display.
            // This comes into play when the binding list is bound to a list control
            // such as a drop down list.
            SortList.Sort((value1, value2) => value1.Sort.CompareTo(value2.Sort));
            return SortList;

        }

        /// <summary>
        /// Creates a List of <see cref="AhkKeyMapAhkMapValue"/> objects representing all the Value
        /// in T enum. The List is Sorted Based upon <see cref="AhkKeyMapAhkMapValue.Sort"/>
        /// </summary>
        /// <param name="SelectedValues">
        /// Instance of <see cref="SelectedOptions{T}"/>. Each value in SelectedValues will set the
        /// corresponding <see cref="AhkKeyMapAhkMapValue.Selected"/> to true;
        /// </param>
        /// <param name="ExcludeRules">
        /// If any rule matches the exclude then the matched item will not be
        /// outputted in the list.
        /// </param>
        /// <returns>
        /// List of <see cref="AhkKeyMapAhkMapValue"/>
        /// </returns>
        /// <remarks>
        /// <see cref="AhkKeyMapAhkMapValue.Parent"/> is set to the List.
        /// </remarks>
        public virtual List<AhkKeyMapAhkMapValue> ToList(SelectedOptions<T> SelectedValues, EnumRule<T> ExcludeRules)
        {
            List<AhkKeyMapAhkMapValue> SortList = new List<AhkKeyMapAhkMapValue>();
            if (this.AhkMapValue == null)
            {
                return SortList;
            }

            foreach (var map in this.AhkMapValue)
            {
                AhkKeyMapAhkMapValue newMap = new AhkKeyMapAhkMapValue();
                if (ExcludeRules[map.Key])
                {
                    // exclude rule continue
                    continue;
                }
                // newMap.Parent = bl;
                newMap.AutoHotKeyValue = map.AutoHotKeyValue;
                newMap.DisplayValue = map.DisplayValue;
                newMap.Key = map.Key;
                newMap.Sort = map.Sort;
                if (SelectedValues.HasKey[map.Key])
                {
                    newMap.Selected = true;
                }
                newMap.Parent = SortList;
                SortList.Add(newMap);
            }
            // Sort the list based upon sort values for display.
            // This comes into play when the binding list is bound to a list control
            // such as a drop down list.
            SortList.Sort((value1, value2) => value1.Sort.CompareTo(value2.Sort));
            return SortList;

        }

    }
}
