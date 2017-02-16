using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Enums;
using System.Collections;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin
{
    public class Plugins: IEnumerable<Plugin>, IEnumerable
    {
        #region Fields
        private profile m_Profile;

        private Dictionary<string, Plugin> m_PluginList;

        #endregion

        #region Properties
        /// <summary>
        /// Gets the <see cref="profile"/> assoicated with current pluigns instance.
        /// </summary>
        public profile Profile
        {
            get
            {
                return this.m_Profile;
            }
        }
        /// <summary>
        /// Gets the Plugin from dictionary of plugins
        /// </summary>
        /// <param name="Key">The Key of the Plugin</param>
        /// <returns>
        /// The <see cref="Plugin"/> for the key or Null if the dictionary does not contain the key
        /// </returns>
        public Plugin this[string Key]
        {
            get
            {
                if (this.m_PluginList.ContainsKey(Key))
                {
                    return this.m_PluginList[Key];
                }
                return default(Plugin);
            }
        }
       
        internal Dictionary<string, Plugin> PluginList
        {
            get { return m_PluginList; }
        }
        /// <summary>
        /// Gets/Sets the the current key representing the current plugin
        /// </summary>
        public string CurrentKey { get; set; }

        /// <summary>
        /// Gets The <see cref="Plugin"/> that represents the current Plugin. Or Null if <see cref="CurrentKey"/> Is Not Valid;
        /// </summary>
        public Plugin CurrentPlugin
        {
            get
            {
                if(this.IsCurrentKeyValid == false)
                {
                    return null;
                }
                return this.m_PluginList[this.CurrentKey];                
            }
        }

        /// <summary>
        /// Gets If <see cref="CurrentKey"/> is a valid key for Plugin
        /// </summary>
        public bool IsCurrentKeyValid
        {
            get
            {
                if (string.IsNullOrEmpty(this.CurrentKey))
                {
                    return false;
                }
                return this.m_PluginList.ContainsKey(this.CurrentKey);
            }

        }
        /// <summary>
        /// Gets if any Plugin contains a Hotkey or include hotkey
        /// </summary>
        public bool HasHotkey
        {
            get
            {
                foreach (var kv in this.m_PluginList)
                {
                    if (kv.Value.HasHotkey == true)
                    {
                        return true;
                    }
                   
                }
                return false;
            }
        }
        /// <summary>
        /// Gets if any Plugin contains a hotstring or include hotstring
        /// </summary>
        public bool HasHotString
        {
            get
            {
                foreach (var kv in this.m_PluginList)
                {
                    if (kv.Value.HasHotString == true)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Gets if any of the Plugins have Changed Data
        /// </summary>
        public bool HasChangedData
        {
            get
            {
                foreach (var kv in this.m_PluginList)
                {
                    if (kv.Value.HasChangedData == true)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Gets the count of the Plugins
        /// </summary>
        public int Count
        {
            get
            {
                return this.m_PluginList.Count;
            }
        }

        

        #endregion

        #region Constructor
        private Plugins()
        {
            this.m_PluginList = new Dictionary<string, Plugin>();
        }
        public Plugins(profile p) : this()
        {

            this.m_Profile = p;
            Load();
        }

        public Plugins(string ProfileFileName) : this()
        {
            try
            {
                this.m_Profile = this.GetProfile(ProfileFileName);
            }
            catch (Exception e)
            {

                new Exception(Properties.Resources.ErrorPluginsConstructorProfileRead, e);
            }
            Load();
        }
        #endregion

        #region Methods

        #region To Display List
        public List<Display.DisplayItem> ToDisplayList()
        {
            List<Display.DisplayItem> lst = new List<Display.DisplayItem>();

            foreach (var kv in this.m_PluginList)
            {
                lst.AddRange(kv.Value.ToDisplayList());
            }

            return lst;
        }

        public List<Display.DisplayItem> ToDisplayList(string Key)
        {
            if (string.IsNullOrEmpty(Key))
            {
                return new List<Display.DisplayItem>();
            }

            if (string.Equals(Key, AppCommon.All, StringComparison.CurrentCultureIgnoreCase) == true)
            {
                return this.ToDisplayList();
            }
            List<Display.DisplayItem> lst;

            if (this.m_PluginList.ContainsKey(Key))
            {
                lst = this.m_PluginList[Key].ToDisplayList();
            } else
            {
                lst = new List<Display.DisplayItem>();
            }
            return lst;
        }

        public List<Display.DisplayItem> ToDisplayList(string Key, string Category)
        {
            if ((string.IsNullOrEmpty(Key)) || (string.IsNullOrEmpty(Category)))
            {
                return new List<Display.DisplayItem>();
            }
            bool AllKeys = false;
            if (string.Equals(Key, AppCommon.All, StringComparison.CurrentCultureIgnoreCase) == true)
            {
                AllKeys = true;
            }

            List<Display.DisplayItem> lst;
            if (AllKeys == true)
            {
                lst = new List<Display.DisplayItem>();
                foreach (var kv in this.m_PluginList)
                {
                    lst.AddRange(kv.Value.ToDisplayList(Category));
                }
            }
            else
            {
                if (this.m_PluginList.ContainsKey(Key))
                {
                    lst = this.m_PluginList[Key].ToDisplayList(Category);
                }
                else
                {
                    lst = new List<Display.DisplayItem>();
                }
            }
           
            return lst;
        }

        public List<Display.DisplayItem> ToDisplayList(string Key, HotTypeEnum HotType)
        {
            if (string.IsNullOrEmpty(Key))
            {
                return new List<Display.DisplayItem>();
            }
            bool AllKeys = false;
            if (string.Equals(Key, AppCommon.All, StringComparison.CurrentCultureIgnoreCase) == true)
            {
                AllKeys = true;
            }

            List<Display.DisplayItem> lst;
            if (AllKeys == true)
            {
                lst = new List<Display.DisplayItem>();
                foreach (var kv in this.m_PluginList)
                {
                    lst.AddRange(kv.Value.ToDisplayList(HotType));
                }
            }
            else
            {
                if (this.m_PluginList.ContainsKey(Key))
                {
                    lst = this.m_PluginList[Key].ToDisplayList(HotType);
                }
                else
                {
                    lst = new List<Display.DisplayItem>();
                }
            }
           
            return lst;
        }

        public List<Display.DisplayItem> ToDisplayList(string Key, HotTypeEnum HotType, string Category)
        {
            if ((string.IsNullOrEmpty(Key)) || (string.IsNullOrEmpty(Category)))
            {
                return new List<Display.DisplayItem>();
            }
            bool AllKeys = false;
            if (string.Equals(Key, AppCommon.All, StringComparison.CurrentCultureIgnoreCase) == true)
            {
                AllKeys = true;
            }
            List<Display.DisplayItem> lst;
            if (AllKeys == true)
            {
                lst = new List<Display.DisplayItem>();
                foreach (var kv in this.m_PluginList)
                {
                    lst.AddRange(kv.Value.ToDisplayList(HotType, Category));
                }
            }
            else
            {
                
                if (this.m_PluginList.ContainsKey(Key))
                {
                    lst = this.m_PluginList[Key].ToDisplayList(HotType, Category);
                }
                else
                {
                    lst = new List<Display.DisplayItem>();
                }
            }
            
            return lst;
        }
        #endregion

        #region TypeList
        /// <summary>
        /// Gets A Dictonary of Unique Categories for the plugin represented by Key. Or all Plugins if <paramref name="Key"/> is set to the value
        /// of <see cref="AppCommon.All"/>
        /// </summary>
        /// <param name="Key">The Key of the Plugin to get the Categoreis from or <see cref="AppCommon.All"/> to get categories from all Plugins</param>
        /// <param name="ToUpper">If true the Values will all be in upper case; Otherwise the value will be as read</param>
        /// <param name="HotType">The Type(s) of Categories to get</param>
        /// <returns>
        /// Dictionary of unique Categories in plugin(s). The key will always be in upper case. The value case depends on the <paramref name="ToUpper"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="HotType"/> supports Flags and can be a combination of <see cref="HotTypeEnum.Hotkey"/> and <see cref="HotTypeEnum.HotString"/>
        /// </remarks>
        public Dictionary<string, string> GetCategories(string Key, bool ToUpper, HotTypeEnum HotType)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(Key))
            {
                return dic;
            }

            bool bAll = AppCommon.IsEqualToAll(Key);
            if ((bAll == false) && (this.m_PluginList.ContainsKey(Key) == false))
            {
                return dic;
            }
            dic.Add("NONE", "NONE"); // Add None just to ensure it can be removed belown to prevent none from showing up in category list
            if (bAll == true)
            {
                foreach (var plug in this.m_PluginList)
                {
                    Dictionary<string, string> pc = plug.Value.GetCategories(ToUpper, HotType);
                    foreach (var item in pc)
                    {
                        if (!dic.ContainsKey(item.Key))
                        {
                            dic.Add(item.Key, item.Value);
                        }
                    }
                }
            }
            else
            {
                Plugin p = this.m_PluginList[Key];
                dic = p.GetCategories(ToUpper, HotType);
               
            }
            if (dic.ContainsKey("NONE"))
            {
                dic.Remove("NONE"); // remove the NONE added above to prevent NONE from showing up in the categories
            }
            return dic;

        }
        #endregion

        #region Profile Methods
        /// <summary>
        /// Gets a Profile from the file Passed In
        /// </summary>
        /// <param name="ProfileFile">The xml Profile to crete the profile for</param>
        /// <returns>Profile representign the <paramref name="ProfileFile"/> if it exist; Otherwise Empty Profile is returned</returns>
        private profile GetProfile(string ProfileFile)
        {
            if (string.IsNullOrEmpty(ProfileFile))
            {
                return null;
            }
            string spath = ProfileFile;
            if (spath.IndexOf(Path.DirectorySeparatorChar) < 0)
            {
                spath = Path.Combine(AppCommon.Instance.PathProfiles, spath);
            }
            if (!File.Exists(spath))
            {
                return null;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(profile));

            StreamReader reader = new StreamReader(spath);
            var p = (profile)serializer.Deserialize(reader);
            reader.Close();
            p.File = spath;
            return p;
        }
        #endregion

        #region Clear Data
        /// <summary>
        /// Clears the record of changed properties and their original values.
        /// </summary>
        /// <remarks>Call this method when the data in the model is saved.</remarks>
        public void ClearChangedData()
        {
            foreach (var kv in this.m_PluginList)
            {
                kv.Value.ClearChangedData();
            }
        }
        #endregion

        #region Load Methods
        private void Load()
        {
            if (this.m_Profile == null)
            {
                return;
            }
            string sPath = this.m_Profile.codeLanguage.paths.mainData;
            if (string.IsNullOrWhiteSpace(sPath))
            {
                return;
            }
            if (!Directory.Exists(sPath))
            {
                sPath = Path.Combine(AppCommon.Instance.PathXml, sPath);
            }
            if (!Directory.Exists(sPath))
            {
                return;
            }

            var files = Directory.GetFiles(sPath, "*.xml");
            foreach (string file in files)
            {
                Plugin p = new Plugin(file);
                this.m_PluginList.Add(file, p);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves any plugins back to their original xml files if there were any changes and resets the changed data to show no changes.
        /// </summary>
        /// <returns>
        /// True if all plugins saved successfully; Otherwise false.
        /// </returns>
        public bool Save()
        {
            return this.Save(true);
        }

        /// <summary>
        /// Saves the data back to the originl Plugin xml files
        /// </summary>
        /// <param name="OnlyIfchanged">If true then will only save if there are changes; Otherwise will Save anyways</param>
        /// <returns>
        /// True if all plugins saved successfully; Otherwise false.
        /// </returns>
        public bool Save(bool OnlyIfchanged)
        {
            bool retval = true;
            if (OnlyIfchanged == true)
            {
                foreach (var kv in this.m_PluginList)
                {
                    if (kv.Value.HasChangedData == true)
                    {
                        retval &= kv.Value.Save();
                    }
                }
            }
            else
            {
                foreach (var kv in this.m_PluginList)
                {
                    retval &= kv.Value.Save();
                }
            }
            if (retval == true)
            {
                this.ClearChangedData();
            }
            return retval;
        }
        #endregion

        #region Internal Dictionary

        #region ContainsKey
        /// <summary>
        /// Gets if <see cref="Plugins"/> instance contains the plugin for the Key
        /// </summary>
        /// <param name="Key">The Key of the Plugin to Check for</param>
        /// <returns>True if Plugin Exist for Key; Othewise false</returns>
        public bool ContainsKey(string Key)
        {
            return this.m_PluginList.ContainsKey(Key);
        }
        #endregion

        #region IEnumerable
        public IEnumerator<Plugin> GetEnumerator()
        {
            return this.m_PluginList.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.m_PluginList.Values.GetEnumerator();
        }
        #endregion

        #endregion

        #endregion
    }
}
