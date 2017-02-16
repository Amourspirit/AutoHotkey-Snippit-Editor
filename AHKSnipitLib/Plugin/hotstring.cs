using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml.Serialization;
using System.ComponentModel;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Security;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Options;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Generic;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin
{
    /// <summary>
    /// Represents a hot string to be use in AutoHotKey
    /// </summary>
    public partial class hotstring
    {
        private void Options_AddRemove(object sender, SelectedOptionEventArgs<HotStringOptionsEnum> e)
        {
            this.hotStringOptions = this.Options.ToArray();
        }

        #region Properties
        HotstringSelectedOptions m_optoins;

        /// <summary>
        /// Gets the <see cref="HotstringSelectedOptions"/> for the current instance of <see cref="hotstring"/>
        /// </summary>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public HotstringSelectedOptions Options
        {
            get
            {
                if (m_optoins == null)
                {
                    if (this.hotStringOptions == null || this.hotStringOptions.Length == 0)
                    {
                        m_optoins = new HotstringSelectedOptions();
                        m_optoins.OptionAdded += Options_AddRemove;
                        m_optoins.OptionRemoved += Options_AddRemove;
                    }
                    else
                    {
                        m_optoins = HotstringSelectedOptions.FromArray(this.hotStringOptions);
                        m_optoins.OptionAdded += Options_AddRemove;
                        m_optoins.OptionRemoved += Options_AddRemove;
                    }
                }
                return m_optoins;
            }
        }

        /// <summary>
        /// Parent List
        /// </summary>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public IList<hotstring> Parent { get; set; }

        private string m_hash;

        /// <summary>
        /// Gets a Unique Name for the Hot string in MDF formate.
        /// </summary>
        /// <remarks>
        /// UniqueName is valid for file names.
        /// </remarks>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public string UniqueName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_hash))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(this.name);
                    sb.Append(this.category);
                    sb.Append(this.enabled.ToString());
                    sb.Append(this.tabify.ToString());
                    sb.Append(this.tabifySpecified.ToString());
                    sb.Append(this.trigger);
                    sb.Append(this.type.ToString());
                    // Add the code to unique name. This will force a different name if only
                    // the code has changed. Later this will be used to help decide if the contents
                    // of a plugin are changed for writing to disk.
                    sb.Append(this.code);
                    this.m_hash = Util.GetHashString(sb.ToString());
                }
                return this.m_hash;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Populates instance class with another instance of class, Arrays are Ignored
        /// </summary>
        /// <param name="oth">The other instance to populate from</param>
        public void Populate(hotstring oth)
        {
            Populate(oth, true);

        }

        /// <summary>
        /// Populates instance class with another instance of class
        /// </summary>
        /// <param name="oth">The other instance to populate from</param>
        /// <param name="IgnoreArray">If True then Arrays are Ignored; Otherwise arrays are included</param>
        public void Populate(hotstring oth, bool IgnoreArray)
        {
            foreach (PropertyInfo sourcePropertyInfo in oth.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (sourcePropertyInfo.Name == @"Parent")
                {
                    continue;
                }
                if ((IgnoreArray == true) && (sourcePropertyInfo.PropertyType.IsArray == true))
                {
                    continue;
                }
                PropertyInfo destPropertyInfo = this.GetType().GetProperty(sourcePropertyInfo.Name);

                if (destPropertyInfo.CanWrite == true)
                {
                    destPropertyInfo.SetValue(
                       this,
                       sourcePropertyInfo.GetValue(oth, null),
                       null);
                }
            }

        }
        #endregion
    }
}
