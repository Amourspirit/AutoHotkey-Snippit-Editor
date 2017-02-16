using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Generic;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Maps;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Enums;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Options
{
    /// <summary>
    /// Creates a map that maps from <see cref="HotStringSendEnum"/> and <see cref="AhkKeyMapAhkMapValue"/> values.
    /// </summary>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "AhkKeyMap", Namespace = "", IsNullable = false)]
    public class HotstringSendMethodMap : MapGenericAhk<HotStringSendEnum>
    {

        #region Instance

        /// <summary>
        /// Instance for the singleton of the class
        /// </summary>
        private static readonly HotstringSendMethodMap _instance = HotstringSendMethodMap.FromResource();
        /// <summary>
        /// Instance of <see cref="HotstringSendMethodMap"/>
        /// </summary>
        /// <remarks>
        /// <see cref="HotstringSendMethodMap"/> uses a singleton pattern
        /// </remarks>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public static HotstringSendMethodMap Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        #region From Resource
        /// <summary>
        /// Loads an instance of <see cref="HotstringSendMethodMap"/> from XML string in the Resources
        /// </summary>
        /// <returns>
        /// Instance of <see cref="HotstringSendMethodMap"/> populated from XML in resource file
        /// </returns>
        protected internal static HotstringSendMethodMap FromResource()
        {
            // Resource is use to allow for other language to create their own version of the resource
            // in a specific language.
            XmlSerializer serializer = new XmlSerializer(typeof(HotstringSendMethodMap));

            var reader = new System.IO.StringReader(Properties.Resources.XmlMapSendTypeEnum);
            var mapInst = (HotstringSendMethodMap)serializer.Deserialize(reader);
            reader.Close();
            if (mapInst.AhkMapValue != null)
            {

                for (int i = 0; i < mapInst.AhkMapValue.Length; i++)
                {
                    var m = mapInst.AhkMapValue[i];
                    if (mapInst.InternalMap.ContainsKey(m.Key) == false)
                    {
                        mapInst.InternalMap.Add(m.Key, i);
                    }
                    else
                    {
                        throw new InvalidOperationException(Properties.Resources.ErrorDuplicateKeysNotAllowed);
                    }
                    if (string.IsNullOrEmpty(m.AutoHotKeyValue) == false
                        && mapInst.AhkSymbolMap.ContainsKey(m.AutoHotKeyValue) == false)
                    {
                        mapInst.AhkSymbolMap.Add(m.AutoHotKeyValue, i);
                    }
                   
                }
            }
            return mapInst;

        }
        #endregion

    }
}
