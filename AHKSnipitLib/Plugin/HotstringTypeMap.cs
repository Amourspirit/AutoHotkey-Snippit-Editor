using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Maps;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Generic;
using System.Xml.Serialization;
using System.ComponentModel;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin
{
    /// <summary>
    /// Creates a map that maps from <see cref="hotstringType"/> and <see cref="Maps.SortedMapItem"/> values.
    /// </summary>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "SortedMap", Namespace = "", IsNullable = false)]
    public class HotstringTypeMap : MapGenericSorted<hotstringType>
    {
        #region Instance

        /// <summary>
        /// Instance for the singleton of the class
        /// </summary>
        private static readonly HotstringTypeMap _instance = HotstringTypeMap.FromResource();
        /// <summary>
        /// Instance of <see cref="HotstringTypeMap"/>
        /// </summary>
        /// <remarks>
        /// <see cref="HotstringTypeMap"/> uses a singleton pattern
        /// </remarks>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public static HotstringTypeMap Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        #region From XML
        /// <summary>
        /// Loads an instance of <see cref="HotstringTypeMap"/> from XML string in the Resources
        /// </summary>
        /// <returns>
        /// Instance of <see cref="HotstringTypeMap"/> populated from XML in resource file
        /// </returns>
        public static HotstringTypeMap FromResource()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(HotstringTypeMap));
            var reader = new System.IO.StringReader(Properties.Resources.XmlMapHotStringType);
            HotstringTypeMap xmlData = (HotstringTypeMap)serializer.Deserialize(reader);
            reader.Close();
            if (xmlData.Item != null && xmlData.Item.Length > 0)
            {

                for (int i = 0; i < xmlData.Item.Length; i++)
                {
                    var itm = xmlData.Item[i];
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
        #endregion
    }
}
