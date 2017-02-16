using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Enums;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin.Display
{
    public partial class DisplayItem : IComparable, IComparable<DisplayItem>
    {
        #region Constructor
        public DisplayItem()
        {
            this.Enabled = true;
        }
        #endregion

        #region Properties
        public string Text { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Key { get; set; }
        public string PluginFile { get; set; }
        public bool Enabled { get; set; }

        public PluginEnum PluginType { get; set; }
        #endregion

        #region Compare
        public int CompareTo(object obj)
        {
            if (obj is DisplayItem)
            {
                DisplayItem other = (DisplayItem)obj;
                return this.Text.CompareTo(other.Text);
            }
            return 1;
        }

        public int CompareTo(DisplayItem other)
        {
            return this.Text.CompareTo(other.Text);
        }
        #endregion
    }
}
