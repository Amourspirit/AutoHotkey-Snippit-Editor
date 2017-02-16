using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.ListHelper;

namespace BigByteTechnologies.Windows.AHKSnipit.HotList.UserForms.Editor.Dialog
{
    public partial class frmProflieSelection : Form
    {
        public frmProflieSelection()
        {
            InitializeComponent();
            btnOk.Image = Z.IconLibrary.Silk.Icon.Accept.GetImage();
            btnOk.ImageAlign = ContentAlignment.MiddleLeft;
            btnCancel.Image = Z.IconLibrary.Silk.Icon.Cancel.GetImage();
            btnCancel.ImageAlign = ContentAlignment.MiddleLeft;
        }
        #region Properties
        /// <summary>
        /// The Profile to exclude from the list of profiles. If null then all profile are shown
        /// </summary>
        public string CurrentProfleFile { get; set; }
        /// <summary>
        /// The Initial Selected Profile file selected when the form is displayed
        /// </summary>
        public string SelectedProfileFile { get; set; }
        /// <summary>
        /// The result that was selected by the user input.
        /// </summary>
        public string ProfileResult { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Gets a list of Profiles from the App Profiles folder Key is Name and Value is file
        /// </summary>
        /// <returns>List of CodeLanguage and Xml File name</returns>
        private List<KeyValuePair<string, string>> GetProfiles()
        {
            var profiiles = new List<KeyValuePair<string, string>>();
            var files = Directory.GetFiles(AppCommon.Instance.PathProfiles, "*.xml");
            bool DoCurrentCheck = true;
            DoCurrentCheck &= string.IsNullOrEmpty(this.CurrentProfleFile) == false;
            if (DoCurrentCheck == true)
            {
                DoCurrentCheck &= File.Exists(this.CurrentProfleFile);
            }
            Parallel.ForEach(files, file => {
                lock (profiiles)
                {
                    if (DoCurrentCheck == true)
                    {
                        if (string.Equals(this.CurrentProfleFile, file,StringComparison.CurrentCultureIgnoreCase) == true)
                        {
                            // this profile matches current profile so will skip
                            return;
                        }
                    }
                    var p = profile.FromFile(file);
                    profiiles.Add(new KeyValuePair<string, string>(p.codeLanguage.codeName, file));
                }
            });
           
            return profiiles;
        }

        private void PopulateProfiles()
        {
            ddlProfiles.Items.Clear();
            var Profiles = this.GetProfiles();
            Profiles.Sort(Compare.KeyValuePairCompareStringKey);
            ddlProfiles.DataSource = Profiles;
            int SetIndex = -1;
            if (string.IsNullOrEmpty(this.SelectedProfileFile) == false)
            {
                for (int i = 0; i < ddlProfiles.Items.Count; i++)
                {
                    KeyValuePair<string, string> c = (KeyValuePair < string, string>)ddlProfiles.Items[i];
                    if (string.Equals(c.Value, this.SelectedProfileFile, StringComparison.CurrentCultureIgnoreCase))
                    {
                        SetIndex = i;
                        break;
                    }
                }
            }
            if (SetIndex > -1)
            {
                ddlProfiles.SelectedIndex = SetIndex;
            }

        }
        #endregion

        #region Events
        private void frmProflieSelection_Load(object sender, EventArgs e)
        {
            PopulateProfiles();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ddlProfiles.SelectedIndex >= 0)
            {
                KeyValuePair<string, string> sel = (KeyValuePair<string, string>)ddlProfiles.SelectedItem;

                this.ProfileResult = sel.Value;
            }
            else
            {
                this.ProfileResult = string.Empty;
            }

        }
        #endregion

    }
}
