using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Generic;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Maps;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Keys
{
    /// <summary>
    /// Contains a map from <see cref="EndCharsEnum"/> Names to Extended Information
    /// </summary>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "AhkKeyMap" , Namespace = "", IsNullable = false)]
    public sealed class EndCharKeyMap : MapGenericAhk<EndCharsEnum>
    {
        #region Instance

        private static readonly EndCharKeyMap _instance = EndCharKeyMap.FromResource();
        /// <summary>
        /// Instance of <see cref="EndCharKeyMap"/>
        /// </summary>
        /// <remarks>
        /// <see cref="EndCharKeyMap"/> uses a singleton pattern
        /// </remarks>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public static EndCharKeyMap Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// Loads an instance of EncCharKeyMap from XML string in the Resources
        /// </summary>
        /// <returns>
        /// Instance of EndCharsKeyMap populated from XML in resource file
        /// </returns>
        internal static EndCharKeyMap FromResource()
        {
            // Resource is use to allow for other language to create their own version of the resource
            // in a specific language.
            XmlSerializer serializer = new XmlSerializer(typeof(EndCharKeyMap));

            var reader = new System.IO.StringReader(Properties.Resources.XmlMapEndCharKeys);
            var ec = (EndCharKeyMap)serializer.Deserialize(reader);
            reader.Close();
            if (ec.AhkMapValue != null)
            {
                
                for (int i = 0; i < ec.AhkMapValue.Length; i++)
                {
                    var m = ec.AhkMapValue[i];
                    if (ec.InternalMap.ContainsKey(m.Key) == false)
                    {
                        ec.InternalMap.Add(m.Key, i);
                    }
                    if (string.IsNullOrEmpty(m.AutoHotKeyValue) == false
                        && ec.AhkSymbolMap.ContainsKey(m.AutoHotKeyValue) == false)
                    {
                        ec.AhkSymbolMap.Add(m.AutoHotKeyValue, i);
                    }
                }
            }
            
            return ec;
            
        }

        /// <summary>
        /// Creates a Binding List of <see cref="AhkKeyMapAhkMapValue"/> objects representing all the Value
        /// in <see cref="EndCharsEnum"/>. The List is Sorted Based upon <see cref="AhkKeyMapAhkMapValue.Sort"/>
        /// </summary>
        /// <returns>
        /// Binding list of <see cref="AhkKeyMapAhkMapValue"/>
        /// </returns>
        /// <remarks>
        /// <see cref="AhkKeyMapAhkMapValue.Parent"/> is set to the Binding List.
        /// </remarks>
        public override BindingList<AhkKeyMapAhkMapValue> ToBindingList()
        {
            return base.ToBindingList();
        }


        /// <summary>
        /// Creates a Binding List of <see cref="AhkKeyMapAhkMapValue"/> objects representing all the Value
        /// in <see cref="EndCharsEnum"/>. The List is Sorted Based upon <see cref="AhkKeyMapAhkMapValue.Sort"/>
        /// </summary>
        /// <param name="SelectedValues">
        /// Instance of <see cref="EndCharsSelectedOptions"/>. Each value in SelectedValues will set the
        /// corresponding <see cref="AhkKeyMapAhkMapValue.Selected"/> to true;
        /// </param>
        /// <returns>
        /// Binding list of <see cref="AhkKeyMapAhkMapValue"/>
        /// </returns>
        /// <remarks>
        /// <see cref="AhkKeyMapAhkMapValue.Parent"/> is set to the Binding List.
        /// </remarks>
        public override BindingList<AhkKeyMapAhkMapValue> ToBindingList(SelectedOptions<EndCharsEnum> SelectedValues)
        {
            return base.ToBindingList(SelectedValues);
        }

        /// <summary>
        /// Creates a List of <see cref="AhkKeyMapAhkMapValue"/> objects representing all the Value
        /// in <see cref="EndCharsEnum"/>. The List is Sorted Based upon <see cref="AhkKeyMapAhkMapValue.Sort"/>
        /// </summary>
        /// <returns>
        /// List of <see cref="AhkKeyMapAhkMapValue"/>
        /// </returns>
        /// <remarks>
        /// <see cref="AhkKeyMapAhkMapValue.Parent"/> is set to the List.
        /// </remarks>
        public override List<AhkKeyMapAhkMapValue> ToList()
        {
            return base.ToList();
        }

        /// <summary>
        /// Creates a List of <see cref="AhkKeyMapAhkMapValue"/> objects representing all the Value
        /// in <see cref="EndCharsEnum"/>. The List is Sorted Based upon <see cref="AhkKeyMapAhkMapValue.Sort"/>
        /// </summary>
        /// <param name="SelectedValues">
        /// Instance of <see cref="EndCharsSelectedOptions"/>. Each value in SelectedValues will set the
        /// corresponding <see cref="AhkKeyMapAhkMapValue.Selected"/> to true;
        /// </param>
        /// <returns>
        /// List of <see cref="AhkKeyMapAhkMapValue"/>
        /// </returns>
        /// <remarks>
        /// <see cref="AhkKeyMapAhkMapValue.Parent"/> is set to the List.
        /// </remarks>
        public override List<AhkKeyMapAhkMapValue> ToList(SelectedOptions<EndCharsEnum> SelectedValues)
        {
            return base.ToList(SelectedValues);
           
        }
    }
}
