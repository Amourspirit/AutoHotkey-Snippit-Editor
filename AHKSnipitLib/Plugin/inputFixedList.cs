using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin.Dialog;
using System.Xml.Serialization;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin
{
    public partial class inputFixedList : IReplacement
    {
        [XmlIgnore()]
        private static string m_ReplacementName = Properties.Resources.ReplacementNameDialogList;

        [XmlIgnore()]
        public string ReplacementName
        {
            get
            {
                return m_ReplacementName;
            }
        }

        [XmlIgnore()]
        public string ReplacementTitle
        {
            get
            {
                return this.dialogtitleField;
            }
        }

        [XmlIgnore()]
        public ReplacementEnum ReplacementType
        {
            get
            {
                return ReplacementEnum.InputFixedList;
            }
        }
    }
}
