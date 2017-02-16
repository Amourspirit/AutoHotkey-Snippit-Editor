using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Maps;
using System.Xml.Serialization;
using System.ComponentModel;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Options
{
    /// <summary>
    /// Creates a map that maps from <see cref="Plugin.HotStringOptionsEnum"/> and <see cref="mapItem"/> values.
    /// </summary>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(ElementName ="map", Namespace = "", IsNullable = false)]
    public partial class HotstringOptionMap : map
    {
        #region Instance

        private static readonly HotstringOptionMap _instance = HotstringOptionMap.FromResource();
        /// <summary>
        /// Instance of <see cref="HotstringOptionMap"/>
        /// </summary>
        /// <remarks>
        /// <see cref="HotstringOptionMap"/> uses a singleton pattern
        /// </remarks>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public static HotstringOptionMap Instance
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
        /// <param name="Key">Is a String Key that is a Enum Name value from <see cref="Plugin.HotStringOptionsEnum"/></param>
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
        #endregion

        /// <summary>
        /// Loads an instance of EncCharKeyMap from XML string in the Resources
        /// </summary>
        /// <returns>
        /// Instance of EndCharsKeyMap populated from XML in resource file
        /// </returns>
        internal static HotstringOptionMap FromResource()
        {
            // Resource is use to allow for other language to create their own version of the resource
            // in a specific language.
            XmlSerializer serializer = new XmlSerializer(typeof(HotstringOptionMap));

            var reader = new System.IO.StringReader(Properties.Settings.Default.XmlMapHotstringOpt);
            var ho = (HotstringOptionMap)serializer.Deserialize(reader);
            reader.Close();

            if (ho.item != null)
            {

                for (int i = 0; i < ho.item.Length; i++)
                {
                    var m = ho.item[i];
                    if (ho.InternalMap.ContainsKey(m.key) == false)
                    {
                        ho.InternalMap.Add(m.key, i);
                    }
                  
                }
            }

            return ho;

        }
    }
}
