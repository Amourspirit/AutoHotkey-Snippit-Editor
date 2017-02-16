using System;
using System.Collections.Generic;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Maps;
using System.Xml.Serialization;
using System.ComponentModel;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Generic;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Options
{
    /// <summary>
    /// Creates a map that maps from <see cref="Plugin.HotStringOptionsEnum"/> and <see cref="mapItem"/> values.
    /// </summary>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "map", Namespace = "", IsNullable = false)]
    public partial class HotKey2Map : map
    {
        #region Instance

        private static readonly HotKey2Map _instance = HotKey2Map.FromResource();
        /// <summary>
        /// Instance of <see cref="HotstringOptionMap"/>
        /// </summary>
        /// <remarks>
        /// <see cref="HotstringOptionMap"/> uses a singleton pattern
        /// </remarks>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public static HotKey2Map Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        #region Fields
        [Bindable(false), XmlIgnore(), Browsable(false)]
        internal Dictionary<string, int> InternalMap = new Dictionary<string, int>();
        #endregion

        #region Properties
        /// <summary>
        /// Get the AhkKeyMapAhkMapValue represented by the Key
        /// </summary>
        /// <param name="Key">Is a String Key that is a Enum Name value from <see cref="Enums.HotkeysEnum"/></param>
        /// <returns>
        /// Instance of AhkKeyMapAhkMapValue if a Key is matched; Otherwise null.
        /// </returns>
        internal mapItem this[string Key]
        {
            get
            {
                if (this.InternalMap.ContainsKey(Key))
                {
                    int index = this.InternalMap[Key];
                    return this.item[index];
                }
                return default(mapItem);
            }
        }

        /// <summary>
        /// List of Keys that represent the Names of the Enum
        /// </summary>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        protected internal HashSet<string> Keys = new HashSet<string>();
        #endregion

        /// <summary>
        /// Loads an instance of EncCharKeyMap from XML string in the Resources
        /// </summary>
        /// <returns>
        /// Instance of EndCharsKeyMap populated from XML in resource file
        /// </returns>
        internal static HotKey2Map FromResource()
        {
            // Resource is use to allow for other language to create their own version of the resource
            // in a specific language.
            XmlSerializer serializer = new XmlSerializer(typeof(HotKey2Map));

            var reader = new System.IO.StringReader(Properties.Settings.Default.XmlMapKey2);
            var ho = (HotKey2Map)serializer.Deserialize(reader);
            reader.Close();

            if (ho.item != null)
            {

                for (int i = 0; i < ho.item.Length; i++)
                {
                    var m = ho.item[i];
                    if (ho.InternalMap.ContainsKey(m.key) == false)
                    {
                        ho.InternalMap.Add(m.key, i);
                        ho.Keys.Add(m.key);
                    }

                }
            }

            return ho;

        }


        /// <summary>
        /// Gets if the <see cref="HotKey2Map"/> Contains a key. All Keys are names from Enum.
        /// </summary>
        public virtual KeyExist<string> HasKey
        {
            get
            {
                KeyExist<string> ke = new KeyExist<string>(ref this.Keys);
                return ke;
            }
        }
    }
}
