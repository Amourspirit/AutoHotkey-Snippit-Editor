using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Enums;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib
{
    public partial class PluginTagInfo
    {
        public PluginTagInfo()
        {
            this.Enabled = true;
        }
        /// <summary>
        /// Gets/Sets the Plugin Item Type
        /// </summary>
        public PluginEnum PluginType { get; set; }
        /// <summary>
        /// Gets/Sets if the Tag Plugin Item is Enabled or not
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Gets/Sets the key to the plugin. This will most likely be the xml file path the item belongs to
        /// </summary>
        public string PluginKey { get; set; }

        /// <summary>
        /// This is the Key for the plugin item. This will most likely be the plugin item hotkey or hotstring value
        /// </summary>
        public string ItemKey { get; set; }


    }
}
