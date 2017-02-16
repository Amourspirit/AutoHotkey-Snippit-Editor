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
    /// Class that is the base class for implementation of <see cref="SortedMap"/>
    /// </summary>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "SortedMap" ,Namespace = "", IsNullable = false)]
    public class MapGenericSortedBasic : SortedMap
    {

        /// <summary>
        /// Creates a new instance of <see cref="MapGenericSortedBasic"/> from <paramref name="xml"/>
        /// </summary>
        /// <param name="xml">The xml text that represents the profile</param>
        /// <returns><see cref="MapGenericSortedBasic"/> instance</returns>
        public static MapGenericSortedBasic FromXml(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MapGenericSortedBasic));
            var reader = new System.IO.StringReader(xml);
            MapGenericSortedBasic xmlData = (MapGenericSortedBasic)serializer.Deserialize(reader);
            reader.Close();
            if (xmlData.Item != null && xmlData.Item.Length > 0)
            {

                for (int i = 0; i < xmlData.Item.Length; i++)
                {
                    var itm = xmlData.Item[i];
                    if (string.IsNullOrEmpty(itm.Key))
                    {
                        throw new KeyNotFoundException(Properties.Resources.ErrorXmlMapKeyMissing);
                    }
                    if (xmlData.InternalMap.ContainsKey(itm.Key) == false)
                    {
                        xmlData.InternalMap.Add(itm.Key, i);
                    }
                    else
                    {
                        throw new InvalidOperationException(Properties.Resources.ErrorDuplicateKeysNotAllowed);
                    }
                }
            }
            return xmlData;
        }

        /// <summary>
        /// Dictionary of Key Name and index if Item in Array
        /// </summary>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        protected internal Dictionary<string, int> InternalMap = new Dictionary<string, int>();

        /// <summary>
        /// Get the SortedMapItem represented by the Key
        /// </summary>
        /// <param name="Key">Is a String Key that is a Enum Name value</param>
        /// <returns>
        /// Instance of SortedMapItem if a Key is matched; Otherwise null.
        /// </returns>
        public SortedMapItem this[string Key]
        {
            get
            {
                if (this.InternalMap.ContainsKey(Key))
                {
                    int index = this.InternalMap[Key];
                    return this.Item[index];
                }
               
                return default(SortedMapItem);
            }
        }

        /// <summary>
        /// Gets if the Array contains key
        /// </summary>
        /// <param name="Key">The Key to check for</param>
        /// <returns>True if exist; Otherwise, false</returns>
        public bool HasKey(string Key)
        {
            return this.InternalMap.ContainsKey(Key);
        }

    }

    /// <summary>
    /// Class that is the base class for implementation of <see cref="SortedMap"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "SortedMap", Namespace = "", IsNullable = false)]
    public class MapGenericSorted<T> : MapGenericSortedBasic where T : struct
    {

        /// <summary>
        /// Creates a new instance of <see cref="MapGenericSorted{T}"/> from <paramref name="xml"/>
        /// </summary>
        /// <param name="xml">The xml text that represents the profile</param>
        /// <returns><see cref="MapGenericSorted{T}"/> instance</returns>
        public new static MapGenericSorted<T> FromXml(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MapGenericSorted<T>));
            var reader = new System.IO.StringReader(xml);
            MapGenericSorted<T> xmlData = (MapGenericSorted<T>)serializer.Deserialize(reader);
            reader.Close();
            if (xmlData.Item != null && xmlData.Item.Length > 0)
            {
               
                for (int i = 0; i < xmlData.Item.Length; i++)
                {
                    var itm = xmlData.Item[i];
                    if (string.IsNullOrEmpty(itm.Key))
                    {
                        throw new KeyNotFoundException(Properties.Resources.ErrorXmlMapKeyMissing);
                    }
                    if (xmlData.InternalMap.ContainsKey(itm.Key) == false)
                    {
                        xmlData.InternalMap.Add(itm.Key, i);
                    }
                    else
                    {
                        throw new InvalidOperationException(Properties.Resources.ErrorDuplicateKeysNotAllowed);
                    }
                }
            }
            return xmlData;
        }

        /// <summary>
        /// Gets if the Array contains key
        /// </summary>
        /// <param name="Key">The Key to check for</param>
        /// <returns>True if exist; Otherwise, false</returns>
        public bool HasKey(T Key)
        {
            return this.InternalMap.ContainsKey(Key.ToString());
        }

        /// <summary>
        /// Creates a Binding List of <see cref="SortedMapItem"/> objects representing all the Values
        /// in T enum. The List is Sorted Based upon <see cref="SortedMapItem.SortOrder"/>
        /// </summary>
        /// <returns>
        /// Binding list of <see cref="SortedMapItem"/>
        /// </returns>
        /// <remarks>
        /// <see cref="SortedMapItem.Parent"/> is set to the Binding List.
        /// </remarks>
        public virtual BindingList<SortedMapItem> ToBindingList()
        {
            SelectedOptions<T> opt = new SelectedOptions<T>();
            return ToBindingList(opt);
        }


        /// <summary>
        /// Creates a Binding List of <see cref="SortedMapItem"/> objects representing all the Value
        /// in T enum. The List is Sorted Based upon <see cref="SortedMapItem.SortOrder"/>
        /// </summary>
        /// <param name="SelectedValues">
        /// Instance of <see cref="SelectedOptions{T}"/>. Each value in SelectedValues will set the
        /// corresponding <see cref="SortedMapItem.Selected"/> to true;
        /// </param>
        /// <returns>
        /// Binding list of <see cref="SortedMapItem"/>
        /// </returns>
        /// <remarks>
        /// <see cref="SortedMapItem.Parent"/> is set to the Binding List.
        /// </remarks>
        public virtual BindingList<SortedMapItem> ToBindingList(SelectedOptions<T> SelectedValues)
        {
            BindingList<SortedMapItem> bl = new BindingList<SortedMapItem>();
            if (this.Item == null)
            {
                return bl;
            }
            List<SortedMapItem> SortList = new List<SortedMapItem>();
            foreach (var map in this.Item)
            {
                SortedMapItem newMap = new SortedMapItem();
                // newMap.Parent = bl;
                newMap.Key = map.Key;
                newMap.Value = map.Value;
                newMap.SortOrder = map.SortOrder;
                if (SelectedValues.HasKey[map.Key])
                {
                    newMap.Selected = true;
                }
                SortList.Add(newMap);
            }
            // Sort the list based upon sort values for display.
            // This comes into play when the binding list is bound to a list control
            // such as a drop down list.
            SortList.Sort((value1, value2) => value1.SortOrder.CompareTo(value2.SortOrder));

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
        /// Creates a Binding List of <see cref="SortedMapItem"/> objects representing all the Value
        /// in T enum. The List is Sorted Based upon <see cref="SortedMapItem.SortOrder"/>
        /// </summary>
        /// <param name="ExcludeRules">
        /// If any rule matches the exclude then the matched item will not be
        /// outputted in the bind list.
        /// </param>
        /// <returns>
        /// Binding list of <see cref="SortedMapItem"/>
        /// </returns>
        /// <remarks>
        /// <see cref="SortedMapItem.Parent"/> is set to the Binding List.
        /// </remarks>
        public virtual BindingList<SortedMapItem> ToBindingList(EnumRule<T> ExcludeRules)
        {
            BindingList<SortedMapItem> bl = new BindingList<SortedMapItem>();
            if (this.Item == null)
            {
                return bl;
            }
            List<SortedMapItem> SortList = new List<SortedMapItem>();
            foreach (var map in this.Item)
            {

                SortedMapItem newMap = new SortedMapItem();
                if (ExcludeRules[map.Key])
                {
                    // exclude rule continue
                    continue;
                }
                newMap.Key = map.Key;
                newMap.Value = map.Value;
                newMap.SortOrder = map.SortOrder;
                SortList.Add(newMap);
            }
            // Sort the list based upon sort values for display.
            // This comes into play when the binding list is bound to a list control
            // such as a drop down list.
            SortList.Sort((value1, value2) => value1.SortOrder.CompareTo(value2.SortOrder));

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
        /// Creates a Binding List of <see cref="SortedMapItem"/> objects representing all the Value
        /// in T enum. The List is Sorted Based upon <see cref="SortedMapItem.SortOrder"/>
        /// </summary>
        /// <param name="SelectedValues">
        /// Instance of <see cref="SelectedOptions{T}"/>. Each value in SelectedValues will set the
        /// corresponding <see cref="SortedMapItem.Selected"/> to true;
        /// </param>
        /// <param name="ExcludeRules">
        /// If any rule matches the exclude then the matched item will not be
        /// outputted in the bind list.
        /// </param>
        /// <returns>
        /// Binding list of <see cref="SortedMapItem"/>
        /// </returns>
        /// <remarks>
        /// <see cref="SortedMapItem.Parent"/> is set to the Binding List.
        /// </remarks>
        public virtual BindingList<SortedMapItem> ToBindingList(SelectedOptions<T> SelectedValues, EnumRule<T> ExcludeRules)
        {
            BindingList<SortedMapItem> bl = new BindingList<SortedMapItem>();
            if (this.Item == null)
            {
                return bl;
            }
            List<SortedMapItem> SortList = new List<SortedMapItem>();
            foreach (var map in this.Item)
            {

                SortedMapItem newMap = new SortedMapItem();
                if (ExcludeRules[map.Key])
                {
                    // exclude rule continue
                    continue;
                }
                newMap.Key = map.Key;
                newMap.Value = map.Value;
                newMap.SortOrder = map.SortOrder;
                if (SelectedValues.HasKey[map.Key])
                {
                    newMap.Selected = true;
                }
                SortList.Add(newMap);
            }
            // Sort the list based upon sort values for display.
            // This comes into play when the binding list is bound to a list control
            // such as a drop down list.
            SortList.Sort((value1, value2) => value1.SortOrder.CompareTo(value2.SortOrder));

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
        /// Creates a List of <see cref="SortedMapItem"/> objects representing all the Value
        /// in T enum. The List is Sorted Based upon <see cref="SortedMapItem.SortOrder"/>
        /// </summary>
        /// <returns>
        /// List of <see cref="SortedMapItem"/>
        /// </returns>
        /// <remarks>
        /// <see cref="SortedMapItem.Parent"/> is set to the List.
        /// </remarks>
        public virtual List<SortedMapItem> ToList()
        {
            SelectedOptions<T> ec = new SelectedOptions<T>();
            return ToList(ec);
        }

        /// <summary>
        /// Creates a List of <see cref="SortedMapItem"/> objects representing all the Value
        /// in T enum. The List is Sorted Based upon <see cref="SortedMapItem.SortOrder"/>
        /// </summary>
        /// <param name="SelectedValues">
        /// Instance of <see cref="SelectedOptions{T}"/>. Each value in SelectedValues will set the
        /// corresponding <see cref="SortedMapItem.Selected"/> to true;
        /// </param>
        /// <returns>
        /// List of <see cref="SortedMapItem"/>
        /// </returns>
        /// <remarks>
        /// <see cref="SortedMapItem.Parent"/> is set to the List.
        /// </remarks>
        public virtual List<SortedMapItem> ToList(SelectedOptions<T> SelectedValues)
        {
            List<SortedMapItem> SortList = new List<SortedMapItem>();
            if (this.Item == null)
            {
                return SortList;
            }

            foreach (var map in this.Item)
            {
                SortedMapItem newMap = new SortedMapItem();
                newMap.Key = map.Key;
                newMap.Value = map.Value;
                newMap.SortOrder = map.SortOrder;
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
            SortList.Sort((value1, value2) => value1.SortOrder.CompareTo(value2.SortOrder));
            return SortList;

        }

        /// <summary>
        /// Creates a List of <see cref="SortedMapItem"/> objects representing all the Value
        /// in T enum. The List is Sorted Based upon <see cref="SortedMapItem.SortOrder"/>
        /// </summary>
        /// <param name="SelectedValues">
        /// Instance of <see cref="SelectedOptions{T}"/>. Each value in SelectedValues will set the
        /// corresponding <see cref="SortedMapItem.Selected"/> to true;
        /// </param>
        /// <param name="ExcludeRules">
        /// If any rule matches the exclude then the matched item will not be
        /// outputted in the list.
        /// </param>
        /// <returns>
        /// List of <see cref="SortedMapItem"/>
        /// </returns>
        /// <remarks>
        /// <see cref="SortedMapItem.Parent"/> is set to the List.
        /// </remarks>
        public virtual List<SortedMapItem> ToList(SelectedOptions<T> SelectedValues, EnumRule<T> ExcludeRules)
        {
            List<SortedMapItem> SortList = new List<SortedMapItem>();
            if (this.Item == null)
            {
                return SortList;
            }

            foreach (var map in this.Item)
            {
                SortedMapItem newMap = new SortedMapItem();
                if (ExcludeRules[map.Key])
                {
                    // exclude rule continue
                    continue;
                }
                newMap.Key = map.Key;
                newMap.Value = map.Value;
                newMap.SortOrder = map.SortOrder;
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
            SortList.Sort((value1, value2) => value1.SortOrder.CompareTo(value2.SortOrder));
            return SortList;

        }
    }
}
