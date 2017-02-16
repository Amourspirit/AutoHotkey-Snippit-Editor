using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Message;
using System.Diagnostics;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.INI;

namespace BigByteTechnologies.Windows.AHKSnipit.ProfileSwap
{
    public partial class Swap : Form
    {
        public Swap()
        {
            InitializeComponent();
            ddlProfiles.DisplayMember = "Key";
            try
            {
                var profiles = this.GetProfiles();
                string current = this.GetCurrentProfile();
                current.Trim();
                current = current.ToLower();
                Byte match = 0;
                if ( ! string.IsNullOrWhiteSpace(current))
                {

                    if (current.LastIndexOf('\\') > -1)
                    {
                        match = 2;
                    } else
                    {
                        match = 1;
                    }

                }

               foreach (var itm in profiles)
               {
                   int index = this.ddlProfiles.Items.Add(itm);
                    if (match > 0)
                    {
                        string strMatch = string.Empty;
                        if (match == 1)
                        {
                            // matching on filename only
                            strMatch = Path.GetFileName(itm.Value.ToLower());
                            
                            if (strMatch == current)
                            {
                                ddlProfiles.SelectedIndex = index;
                                match = 0;
                                this.Text = string.Format("{0} - {1}", this.Text, itm.Key);
                            }
                        } else
                        {
                            // match full path
                            strMatch = itm.Value.ToLower();
                            if (strMatch == current)
                            {
                                ddlProfiles.SelectedIndex = index;
                                match = 0;
                                this.Text = string.Format("{0} - {1}", this.Text, itm.Key);
                            }
                        }
                    }
               }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
           
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private SortedList<string,string> GetProfiles()
        {
            var profiiles = new SortedList<string, string>();
            var files = Directory.GetFiles(AppCommon.Instance.PathProfiles, "*.xml");
            
            foreach (string file in files)
            {
                var p = GetProfile(file);

                profiiles.Add(p.codeLanguage.codeName, file);
            }
            return profiiles;
        }
        private profile GetProfile(string file)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(profile));

            StreamReader reader = new StreamReader(file);
            var p = (profile)serializer.Deserialize(reader);
            reader.Close();

            return p;
        }

        private string GetCurrentProfile()
        {
            string iniFile = AppCommon.Instance.SetingsIniFile;
            string retval = string.Empty;
            if (File.Exists(iniFile))
            {
                var ini = new IniParser.FileIniDataParser();
                IniParser.Model.IniData data = ini.ReadFile(iniFile);
                retval = data["PROFILE"]["current"];
            }
            return retval;
        }

        private void SaveSwap()
        {
            string iniFile = AppCommon.Instance.SetingsIniFile;
            var ini = new IniParser.FileIniDataParser();
            IniParser.Model.IniData data;
            KeyValuePair<string, string> kv;
            if (File.Exists(iniFile))
            {
                data = ini.ReadFile(iniFile);
                kv = (KeyValuePair<string, string>)ddlProfiles.SelectedItem;
                data["PROFILE"]["current"] = Path.GetFileName(kv.Value);

            }
            else
            {
                data = new IniParser.Model.IniData();
                kv = (KeyValuePair<string, string>)ddlProfiles.SelectedItem;
                data["PROFILE"]["current"] = Path.GetFileName(kv.Value);
            }
           ini.WriteFile(iniFile, data);

        }

        /// <summary>
        /// Restarts the main Script if is running or Starts new if not running
        /// </summary>
        /// <remarks>
        /// Reads script location from app.ini
        /// </remarks>
        private void StartScript()
        {
            string scriptPath = IniHelper.GetLauncherScriptPath();

            if (string.IsNullOrEmpty(scriptPath))
            {
                return;
            }
            if (!File.Exists(scriptPath))
            {
                return;
            }
            Process.Start(scriptPath);
        }

       
        private void ReloadAhkScript()
        {
            MessageHelper msg = new MessageHelper();
            int result = 0;
            int hWnd = msg.getWindowId("AutoHotkey", null);
            MessageBox.Show(string.Format("Hwnd:{0}", hWnd.ToString()));
            result = msg.sendWindowsStringMessage(hWnd, 0, "Some_String_Message");
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Save();
            Close();
        }

        private void Save()
        {
            if (ddlProfiles.SelectedIndex >= 0)
            {
                this.SaveSwap();
                this.StartScript();
            }
        }

        private void Swap_Load(object sender, EventArgs e)
        {
            btnOk.Image = Z.IconLibrary.Silk.Icon.Accept.GetImage();
            btnOk.ImageAlign = ContentAlignment.MiddleLeft;
            btnCancel.Image = Z.IconLibrary.Silk.Icon.Cancel.GetImage();
            btnCancel.ImageAlign = ContentAlignment.MiddleLeft;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Save();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
