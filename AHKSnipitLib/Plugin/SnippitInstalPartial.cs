using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Data.Validation;
using System.Xml.Serialization;
using System.ComponentModel;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin
{
    public partial class SnippitInstal
    {
        /// <summary>
        /// Gets/Sets the XML file used to populate the class
        /// </summary>
        [Bindable(false), XmlIgnore(), Browsable(false)]
        public string File { get; set; }

        #region Static Methods
        /// <summary>
        /// Creates a new instance of <see cref="SnippitInstal"/> from <paramref name="FileName"/>
        /// </summary>
        /// <param name="FileName">The FileName to the xml that represents the SnippitInstal</param>
        /// <returns><see cref="SnippitInstal"/> instance</returns>
        /// <exception cref="System.Exception">If there are Validation errors with the xml</exception>
        public static SnippitInstal FromFile(string FileName)
        {
            ValidationResult vResult = ValidateXml.ValidateSnipitInstalFile(FileName);
            if (vResult.HasErrors == true)
            {
                var ex = new System.Exception(vResult.ToString());
                throw ex;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(SnippitInstal));

            System.IO.StreamReader reader = new System.IO.StreamReader(FileName);
            var si = (SnippitInstal)serializer.Deserialize(reader);
            reader.Close();
            si.File = FileName;
            if (si.profile != null && si.plugin != null)
            {
                System.Threading.Tasks.Parallel.ForEach(si.plugin, plg => {
                    lock (si)
                    {
                        plg.ParentProfile = si.profile;
                    }
                });
            }
            return si;
        }

        /// <summary>
        /// Creates a new instance of <see cref="profile"/> from <paramref name="xml"/>
        /// </summary>
        /// <param name="xml">The xml text that represents the SnippitInstal</param>
        /// <returns><see cref="profile"/> instance</returns>
        /// <exception cref="System.Exception">If there are Validation errors with the xml</exception>
        public static SnippitInstal FromXml(string xml)
        {
            ValidationResult vResult = ValidateXml.ValidateSnipitInstalXml(xml);
            if (vResult.HasErrors == true)
            {
                var ex = new System.Exception(vResult.ToString());
                throw ex;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(SnippitInstal));

            var reader = new System.IO.StringReader(xml);
            var si = (SnippitInstal)serializer.Deserialize(reader);
            reader.Close();
            if (si.profile != null && si.plugin != null)
            {
                System.Threading.Tasks.Parallel.ForEach(si.plugin, plg => {
                    lock (si)
                    {
                        plg.ParentProfile = si.profile;
                    }
                });
            }

            return si;
        }
        #endregion
    }
}
