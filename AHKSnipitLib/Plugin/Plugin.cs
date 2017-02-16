using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.IO;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Data;
using System.Xml.Serialization;
using System.Linq;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Enums;
using System.Diagnostics;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Generic;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin
{
    public class Plugin : IComparable, IComparable<Plugin>, INotifyDirtyData, INotifyDataErrorInfo, INotifyPropertyChanged
    {
        #region constructor
        public Plugin(string FileName)
        {

            this.MyType = this.GetType();
            this.m_Hotstrings = new Dictionary<string, HotstringSimple>();
            this.m_Commands = new Dictionary<string, CommandSimple>();
            this.m_IncCommands = new Dictionary<string, includeCommand>();
            this.m_IncHotstrings = new Dictionary<string, includeHotstring>();
            this.m_File = FileName;
            this.m_Name = Path.GetFileNameWithoutExtension(this.m_File);
            try
            {
                this.PopulateFromFile(FileName);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion

        #region Fields
        private Type MyType;
        private Dictionary<string, HotstringSimple> m_Hotstrings;
        private Dictionary<string, CommandSimple> m_Commands;
        private Dictionary<string, includeCommand> m_IncCommands;
        private Dictionary<string, includeHotstring> m_IncHotstrings;
        
        private int m_PreviousChangeCount = 0;
        private int m_HotStringDirtyCount = 0;
        private int m_CommandDirtyCount = 0;
        #endregion

        #region Properties
     
        private string m_Name;
        /// <summary>
        /// Gets/Sets the Name of the plugin
        /// </summary>
        public string Name
        {
            get
            {
                return this.m_Name;
            }
            set
            {
                if (this.m_Name != value)
                {
                    this.SetPropertyValue(value, () => this.m_Name = value);
                }
            }
        }

        private string m_File;
        /// <summary>
        /// Gets/Sets the File of the plugin
        /// </summary>
        public string PluginFile
        {
            get
            {
                return this.m_File;
            }
            set
            {
                if (this.m_File != value)
                {
                    this.SetPropertyValue(value, () => this.m_File = value);
                }
            }
        }
        private string m_version;
        /// <summary>
        /// Gets/Sets the Version of the plugin
        /// </summary>
        public string Version
        {
            get
            {
                return this.m_version;
            }
            set
            {
                if (this.m_version != value)
                {
                    this.SetPropertyValue(value, () => this.m_version = value);
                }
            }
        }

        private string m_MinVersion;
        /// <summary>
        /// Gets/Sets the Minimum version.
        /// </summary>
        public string MinVersion
        {
            get
            {
                return this.m_MinVersion;
            }
            set
            {
                if (this.m_MinVersion != value)
                {
                    this.SetPropertyValue(value, () => this.m_MinVersion = value);
                }

            }
        }

        private bool m_Enabled;
        /// <summary>
        /// Gets/Sets the if the plugine is enabled or disabled
        /// </summary>
        public bool Enabled
        {
            get
            {
                return this.m_Enabled;
            }
            set
            {
                if (this.m_Enabled != value)
                {
                    this.SetPropertyValue(value, () => this.m_Enabled = value);
                }
            }
        }
        private string m_ProfileName;
        /// <summary>
        /// Gets/Sets the Profile Name.
        /// </summary>
        public string ProfileName
        {
            get
            {
                return this.m_ProfileName;
            }
            set
            {
                if (this.m_ProfileName != value)
                {
                    this.SetPropertyValue(value, () => this.m_ProfileName = value);
                }

            }
        }



        /// <summary>
        /// Gets the Count of the Commands
        /// </summary>
        public int CommandCount
        {
            get { return this.m_Commands.Count; }
        }

        /// <summary>
        /// Gets the Count of the hotstrings
        /// </summary>
        public int HotStringCount
        {
            get { return this.m_Hotstrings.Count; }
        }

        /// <summary>
        /// Gets the Count f the Included Commands
        /// </summary>
        public int IncludeCommandCount
        {
            get { return this.m_IncCommands.Count; }
        }

        /// <summary>
        /// Gets the Count fo the Include Hotstrings
        /// </summary>
        public int IncludeHotStringCount
        {
            get { return this.m_IncHotstrings.Count; }
        }
        /// <summary>
        /// Gets if there any hotkeys or include hotkeys for plugin instance.
        /// </summary>
        public bool HasHotkey
        {
            get
            {
                bool retval = false;
                retval |= this.CommandCount > 0;
                retval |= this.IncludeCommandCount > 0;
                return retval;
            }
        }

        /// <summary>
        /// Gets if there are any HotString of include HotString for plugin instance.
        /// </summary>
        public bool HasHotString
        {
            get
            {
                bool retval = false;
                retval |= this.HotStringCount > 0;
                retval |= this.IncludeHotStringCount > 0;
                return retval;
            }
        }
        

        /// <summary>
        /// Gets the <see cref="CommandSimple"/> Instance for the supplied Key
        /// </summary>
        public KeyItem<CommandSimple> ItemHotKeys
        {
            get
            {
                var c = new KeyItem<CommandSimple>(ref this.m_Commands);
                return c;
            }            
        }
        /// <summary>
        /// Gets the <see cref="HotstringSimple"/> Instance for the supplied Key
        /// </summary>
        public KeyItem<HotstringSimple> ItemHotStrings
        {
            get
            {
                var h = new KeyItem<HotstringSimple>(ref this.m_Hotstrings);
                return h;
                
            }
        }

        /// <summary>
        /// Gets the <see cref="includeCommand"/> Instance for the supplied Key
        /// </summary>
        public KeyItem<includeCommand> ItemIncludeHotkeys
        {
            get
            {
                var h = new KeyItem<includeCommand>(ref this.m_IncCommands);
                return h;

            }
        }

        /// <summary>
        /// Gets the <see cref="includeHotstring"/> Instance for the supplied Key
        /// </summary>
        public KeyItem<includeHotstring> ItemIncludeHotStrings
        {
            get
            {
                var h = new KeyItem<includeHotstring>(ref this.m_IncHotstrings);
                return h;

            }
        }


        #endregion

        #region Methods

        #region Populate Class
        /// <summary>
        /// Populates the class from an XML file representing the Plugin
        /// </summary>
        /// <param name="PluginFileName">The XML file to populate the class with</param>
        /// <param name="CurrentPlugFileName">The Name of the current Plugin File</param>
        private void PopulateFromFile(string PluginFileName)
        {

            if (File.Exists(PluginFileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(plugin));

                StreamReader reader = new StreamReader(PluginFileName);
                var rootXml = (plugin)serializer.Deserialize(reader);
                reader.Close();
                rootXml.File = PluginFileName;

                this.PopulateFromRoot(rootXml);
            }
            
        }

        /// <summary>
        /// Populate the Class from an instace of <typeparamref name="root"/>
        /// </summary>
        /// <param name="r">The instance to read value from</param>
        protected void PopulateFromRoot(plugin r)
        {
            this.m_Enabled = r.enabled;
            this.m_version = r.version;
            this.m_ProfileName = r.profileName;
            this.m_MinVersion = r.minVersion;

            if (r.commands != null)
            {
                foreach (var cmd in r.commands)
                {
                    CommandSimple cs = new CommandSimple(cmd);
                    if (string.IsNullOrEmpty(cs.Hotkey))
                    {
                        continue;
                    }
                    if (this.m_Commands.ContainsKey(cs.Hotkey))
                    {
                        continue;
                    }
                    cs.DataStateChanged += this.CommandDataStateEventHandler;
                    this.m_Commands.Add(cs.Hotkey, cs);
                }
            }
            if (r.hotstrings != null)
            {
                foreach (var hs in r.hotstrings)
                {
                    HotstringSimple hss = new HotstringSimple(hs);
                    if (string.IsNullOrEmpty(hs.trigger))
                    {
                        continue;
                    }
                    if (this.m_Hotstrings.ContainsKey(hs.trigger))
                    {
                        continue;
                    }
                    hss.DataStateChanged += this.HotStringDataStateEventHandler;
                    this.m_Hotstrings.Add(hss.Trigger, hss);
                }
            }
            if (r.includes != null)
            {
                foreach (var inc in r.includes)
                {
                    if (inc.commands != null)
                    {
                        foreach (var cmd in inc.commands)
                        {
                            includeCommand hsCmd = new includeCommand(cmd);
                            
                            if (string.IsNullOrEmpty(cmd.hotkey))
                            {
                                continue;
                            }
                            if (this.m_IncCommands.ContainsKey(cmd.hotkey))
                            {
                                continue;
                            }
                            hsCmd.ParentInclude = inc;
                            this.m_IncCommands.Add(hsCmd.hotkey, hsCmd);
                        }
                    }
                    if (inc.hotstrings != null)
                    {
                        foreach (var hs in inc.hotstrings)
                        {
                            includeHotstring h = new includeHotstring(hs, r.File);
                            if (string.IsNullOrEmpty(hs.trigger))
                            {
                                continue;
                            }
                            if (this.m_IncHotstrings.ContainsKey(hs.trigger))
                            {
                                continue;
                            }
                            h.ParentInclude = inc;
                            this.m_IncHotstrings.Add(h.trigger, h);
                        }
                    }
                }
            }

        }
        #endregion

        #region Event Handlers
        private void HotStringDataStateEventHandler(object sender, DataStateEventArgs e)
        {
            if (e.HasChangedData == true)
            {
                this.m_HotStringDirtyCount++;
            } else
            {
                this.m_HotStringDirtyCount--;
            }
            Debug.WriteLine("HotStringDataStateEventHandler: e.HascnagedData:{0}, HotString Dirty count:{1}",e.HasChangedData.ToString(), this.m_HotStringDirtyCount.ToString());
        }

        private void CommandDataStateEventHandler(object sender, DataStateEventArgs e)
        {
            if (e.HasChangedData == true)
            {
                this.m_CommandDirtyCount++;
            }
            else
            {
                this.m_CommandDirtyCount--;
            }
            Debug.WriteLine("CommandDataStateEventHandler: e.HascnagedData:{0}, command Dirty count:{1}", e.HasChangedData.ToString(), this.m_CommandDirtyCount.ToString());
        }
        #endregion

        #region Conatins Key
        /// <summary>
        /// Gets if the <see cref="Plugin"/> contains a <see cref="CommandSimple"/> instance for the <paramref name="Key"/>
        /// </summary>
        /// <param name="Key">The Key to check for</param>
        /// <returns>True if Key is found; Otherwise false</returns>
        public bool ContainsHotkey(string Key)
        {
            if (string.IsNullOrEmpty(Key) == true)
            {
                return false;
            }
            return this.m_Commands.ContainsKey(Key);
        }

        /// <summary>
        /// Gets if the <see cref="Plugin"/> contains a <see cref="HotstringSimple"/> instance for the <paramref name="Key"/>
        /// </summary>
        /// <param name="Key">The Key to check for</param>
        /// <returns>True if Key is found; Otherwise false</returns>
        public bool ContainsHotString(string Key)
        {
            if (string.IsNullOrEmpty(Key) == true)
            {
                return false;
            }
            return this.m_Hotstrings.ContainsKey(Key);
        }

        /// <summary>
        /// Gets if the <see cref="Plugin"/> contains a <see cref="includeHotstring"/> instance for the <paramref name="Key"/>
        /// </summary>
        /// <param name="Key">The Key to check for</param>
        /// <returns>True if Key is found; Otherwise false</returns>
        public bool ContainsIncludeHotString(string Key)
        {
            if (string.IsNullOrEmpty(Key) == true)
            {
                return false;
            }
            return this.m_IncHotstrings.ContainsKey(Key);
        }

        /// <summary>
        /// Gets if the <see cref="Plugin"/> contains a <see cref="includeCommand"/> instance for the <paramref name="Key"/>
        /// </summary>
        /// <param name="Key">The Key to check for</param>
        /// <returns>True if Key is found; Otherwise false</returns>
        public bool ContainsIncludeCommand(string Key)
        {
            if (string.IsNullOrEmpty(Key) == true)
            {
                return false;
            }
            return this.m_IncCommands.ContainsKey(Key);
        }


        #endregion

        #region Get Lits Items

        internal HotstringSimple GetHotString(string Key)
        {
            if (string.IsNullOrEmpty(Key))
            {
                throw new ArgumentException("Key");
            }
            if (this.m_Hotstrings.ContainsKey(Key))
            {
                return this.m_Hotstrings[Key];
            }
            return null;
        }

        internal CommandSimple GetCommand(string Key)
        {
            if (string.IsNullOrEmpty(Key))
            {
                throw new ArgumentException("Key");
            }
            if (this.m_Commands.ContainsKey(Key))
            {
                return this.m_Commands[Key];
            }
            return null;
        }

        internal includeCommand GetIncludeCommand(string Key)
        {
            if (string.IsNullOrEmpty(Key))
            {
                throw new ArgumentException("Key");
            }
            if (this.m_IncCommands.ContainsKey(Key))
            {
                return this.m_IncCommands[Key];
            }
            return null;
        }

        internal includeHotstring GetIncludeHostString(string Key)
        {
            if (string.IsNullOrEmpty(Key))
            {
                throw new ArgumentException("Key");
            }
            if (this.m_IncHotstrings.ContainsKey(Key))
            {
                return this.m_IncHotstrings[Key];
            }
            return null;
        }
        #endregion

        #region Misc Methods
        private int GetChangesCount()
        {
            int i = this._changes.Count;
            i += this.m_CommandDirtyCount;
            i += this.m_HotStringDirtyCount;
            return i;
        }

        /// <summary>
        /// Resets the internal list changes to show no changes
        /// </summary>
        private void ClearListChanges()
        {
            foreach (var kv in this.m_Hotstrings)
            {
                 kv.Value.ClearChangedData();
            }
            foreach (var kv in this.m_Commands)
            {
                kv.Value.ClearChangedData();
            }
            this.m_CommandDirtyCount = 0;
            this.m_HotStringDirtyCount = 0;
        }
        #endregion

        #region To List
        public List<Display.DisplayItem> ToDisplayList()
        {
            List<Display.DisplayItem> lst = new List<Display.DisplayItem>();
            foreach (var kv in this.m_Commands)
            {
                lst.Add(this.CommandToDispalyItme(kv));
            }
            foreach (var kv in this.m_IncCommands)
            {
                lst.Add(this.IncCommandToDispalyItme(kv));
            }

            foreach (var kv in this.m_Hotstrings)
            {
                lst.Add(this.HotstringToDispalyItme(kv));
            }
            foreach (var kv in this.m_IncHotstrings)
            {
                lst.Add(this.IncHotstringToDispalyItme(kv));
            }


            return lst;
        }

        public List<Display.DisplayItem> ToDisplayList(string Category)
        {

            if (string.Equals(Category, AppCommon.All, StringComparison.CurrentCultureIgnoreCase) == true)
            {
                return this.ToDisplayList();
            }

            List<Display.DisplayItem> lst = new List<Display.DisplayItem>();
            if (string.IsNullOrEmpty(Category) == true)
            {
                return lst;
            }
            var lstComands = this.m_Commands.Where(kv => string.Equals(kv.Value.Category, Category, StringComparison.CurrentCultureIgnoreCase));
            foreach (var kv in lstComands)
            {
                lst.Add(this.CommandToDispalyItme(kv));
            }

            var lstHotStrings = this.m_Hotstrings.Where(kv => string.Equals(kv.Value.Category, Category, StringComparison.CurrentCultureIgnoreCase));
            foreach (var kv in lstHotStrings)
            {
                lst.Add(this.HotstringToDispalyItme(kv));
            }
            var lstIncComands = this.m_IncCommands.Where(kv => string.Equals(kv.Value.category, Category, StringComparison.CurrentCultureIgnoreCase));
            foreach (var kv in lstIncComands)
            {
                lst.Add(this.IncCommandToDispalyItme(kv));
            }
            var lstIncHotStrings = this.m_IncHotstrings.Where(kv => string.Equals(kv.Value.category, Category, StringComparison.CurrentCultureIgnoreCase));
            foreach (var kv in lstIncHotStrings)
            {
                lst.Add(this.IncHotstringToDispalyItme(kv));
            }

            return lst;
        }

        public List<Display.DisplayItem> ToDisplayList(HotTypeEnum HotType)
        {
            List<Display.DisplayItem> lst = new List<Display.DisplayItem>();
            if ((HotType & HotTypeEnum.Hotkey) == HotTypeEnum.Hotkey)
            {
                foreach (var kv in this.m_Commands)
                {
                    lst.Add(this.CommandToDispalyItme(kv));
                }
                foreach (var kv in this.m_IncCommands)
                {
                    lst.Add(this.IncCommandToDispalyItme(kv));
                }
            }
            if ((HotType & HotTypeEnum.HotString) == HotTypeEnum.HotString)
            {
                foreach (var kv in this.m_Hotstrings)
                {
                    lst.Add(this.HotstringToDispalyItme(kv));
                }
                foreach (var kv in this.m_IncHotstrings)
                {
                    lst.Add(this.IncHotstringToDispalyItme(kv));
                }
            }

            return lst;
        }

        public List<Display.DisplayItem> ToDisplayList(HotTypeEnum HotType, string Category)
        {
            if (string.IsNullOrEmpty(Category))
            {
                return this.ToDisplayList(HotType);
            }

            List<Display.DisplayItem> lst = new List<Display.DisplayItem>();
            
            var predicateCi = PredicateBuilder.True<KeyValuePair<string,CommandSimple>>();
            var predicateIncCi = PredicateBuilder.True<KeyValuePair<string, includeCommand>>();
            var predicateHs = PredicateBuilder.True<KeyValuePair<string, HotstringSimple>>();
            var predicateIncHs = PredicateBuilder.True<KeyValuePair<string, includeHotstring>>();
            bool hasFilter = false;

            if (string.Equals(Category, AppCommon.All, StringComparison.CurrentCultureIgnoreCase) == false)
            {
                hasFilter = true;
                predicateCi = predicateCi.And(kv => string.Equals(kv.Value.Category, Category, StringComparison.CurrentCultureIgnoreCase));
                predicateIncCi = predicateIncCi.And(kv => string.Equals(kv.Value.category, Category, StringComparison.CurrentCultureIgnoreCase));
                predicateHs = predicateHs.And(kv => string.Equals(kv.Value.Category, Category, StringComparison.CurrentCultureIgnoreCase));
                predicateIncHs = predicateIncHs.And(kv => string.Equals(kv.Value.category, Category, StringComparison.CurrentCultureIgnoreCase));
            }

            if ((HotType & HotTypeEnum.Hotkey) == HotTypeEnum.Hotkey)
            {
                if (hasFilter == true)
                {
                    var lstCmd = this.m_Commands.AsQueryable().Where(predicateCi);
                    foreach (var kv in lstCmd)
                    {
                        lst.Add(this.CommandToDispalyItme(kv));
                    }
                    var lstIncCmd = this.m_IncCommands.AsQueryable().Where(predicateIncCi);
                    foreach (var kv in lstIncCmd)
                    {
                        lst.Add(this.IncCommandToDispalyItme(kv));
                    }
                }
                else
                {
                    foreach (var kv in this.m_Commands)
                    {
                        lst.Add(this.CommandToDispalyItme(kv));
                    }

                    foreach (var kv in this.m_IncCommands)
                    {
                        lst.Add(this.IncCommandToDispalyItme(kv));
                    }
                }
            }

            if ((HotType & HotTypeEnum.HotString) == HotTypeEnum.HotString)
            {
                if (hasFilter == true)
                {
                    var lstHs = this.m_Hotstrings.AsQueryable().Where(predicateHs);
                    foreach (var kv in lstHs)
                    {
                        lst.Add(this.HotstringToDispalyItme(kv));
                    }
                    var lstIncHs = this.m_IncHotstrings.AsQueryable().Where(predicateIncHs);
                    foreach (var kv in lstIncHs)
                    {
                        lst.Add(this.IncHotstringToDispalyItme(kv));
                    }
                }
                else
                {
                    foreach (var kv in this.m_Hotstrings)
                    {
                        lst.Add(this.HotstringToDispalyItme(kv));
                    }

                    foreach (var kv in this.m_IncHotstrings)
                    {
                        lst.Add(this.IncHotstringToDispalyItme(kv));
                    }
                }
            }


                return lst;
        }



        private Display.DisplayItem CommandToDispalyItme(KeyValuePair<string, CommandSimple> kv)
        {
            Display.DisplayItem di = new Display.DisplayItem();
            di.Text = kv.Value.Hotkey;
            di.Name = kv.Value.Name;
            di.Description = kv.Value.Description;
            di.Category = kv.Value.Category;
            di.Enabled = kv.Value.Enabled;
            di.Key = kv.Value.Hotkey;
            di.PluginFile = this.m_File;
            di.PluginType = PluginEnum.HotKey;
            return di;
        }

        private Display.DisplayItem HotstringToDispalyItme(KeyValuePair<string, HotstringSimple> kv)
        {
            Display.DisplayItem di = new Display.DisplayItem();
            di.Text = kv.Value.Trigger;
            di.Name = kv.Value.Name;
            di.Description = kv.Value.Description;
            di.Category = kv.Value.Category;
            di.Enabled = kv.Value.Enabled;
            di.Key = kv.Value.Trigger;
            di.PluginFile = this.m_File;
            di.PluginType = PluginEnum.HotString;
            return di;
        }

        private Display.DisplayItem IncCommandToDispalyItme(KeyValuePair<string, includeCommand> kv)
        {
            Display.DisplayItem di = new Display.DisplayItem();
            di.Text = kv.Value.hotkey;
            di.Name = kv.Value.name;
            di.Description = kv.Value.description;
            di.Category = kv.Value.category;
            di.Key = kv.Value.hotkey;
            di.PluginFile = this.m_File;
            di.PluginType = PluginEnum.IncludeHotKey;
            return di;
        }

        private Display.DisplayItem IncHotstringToDispalyItme(KeyValuePair<string, includeHotstring> kv)
        {
            Display.DisplayItem di = new Display.DisplayItem();
            di.Text = kv.Value.trigger;
            di.Name = kv.Value.name;
            di.Description = kv.Value.description;
            di.Category = kv.Value.category;
            di.Key = kv.Value.trigger;
            di.PluginFile = this.m_File;
            di.PluginType = PluginEnum.IncludeHotString;
            return di;
        }
        #endregion

        #region Categories

        /// <summary>
        /// Gets as dictionary of Unique Categories in the current plugin.
        /// </summary>
        /// <param name="ToUpper">If True the value iwll be stored in as Upper case; Otherwise the value will be stored as read</param>
        /// <param name="HotType">The Type(s) of Categories to get</param>
        /// <returns>
        /// Dictionary of unique Categories in plugin instance. The key will always be in upper case. The value case depends on the <paramref name="ToUpper"/>.
        /// </returns>
        /// <remarks>
        /// <paramref name="HotType"/> supports Flags and can be a combination of <see cref="HotTypeEnum.Hotkey"/> and <see cref="HotTypeEnum.HotString"/>
        /// </remarks>
        public Dictionary<string, string> GetCategories(bool ToUpper, HotTypeEnum HotType)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("NONE", "NONE"); // Add None just to ensure it can be removed belown to prevent none from showing up in category list
            if ((HotType & HotTypeEnum.Hotkey) == HotTypeEnum.Hotkey)
            {
                foreach (var c in this.m_Commands)
                {
                    string CatUp = c.Value.Category.ToUpper();
                    if (!dic.ContainsKey(CatUp))
                    {
                        if (ToUpper == true)
                        {
                            dic.Add(CatUp, CatUp);
                        }
                        else
                        {
                            dic.Add(CatUp, c.Value.Category);
                        }
                    }
                }

                foreach (var ic in this.m_IncCommands)
                {
                    string CatUp = ic.Value.category.ToUpper();
                    if (!dic.ContainsKey(CatUp))
                    {
                        if (ToUpper == true)
                        {
                            dic.Add(CatUp, CatUp);
                        }
                        else
                        {
                            dic.Add(CatUp, ic.Value.category);
                        }
                    }
                }
            }

            if ((HotType & HotTypeEnum.HotString) == HotTypeEnum.HotString)
            {
                foreach (var h in this.m_Hotstrings)
                {
                    string CatUp = h.Value.Category.ToUpper();
                    if (!dic.ContainsKey(CatUp))
                    {
                        if (ToUpper == true)
                        {
                            dic.Add(CatUp, CatUp);
                        }
                        else
                        {
                            dic.Add(CatUp, h.Value.Category);
                        }
                    }
                }

                foreach (var ih in this.m_IncHotstrings)
                {
                    string CatUp = ih.Value.category.ToUpper();
                    if (!dic.ContainsKey(CatUp))
                    {
                        if (ToUpper == true)
                        {
                            dic.Add(CatUp, CatUp);
                        }
                        else
                        {
                            dic.Add(CatUp, ih.Value.category);
                        }
                    }
                }
            }
            
            dic.Remove("NONE"); // remove the NONE added above to prevent NONE from showing up in the categories
            return dic;

        }
        #endregion

        #region Save Methods
        public bool Save()
        {
            plugin r = this.ToRoot();
            if (r == null)
            {
                return false;
            }

            System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(plugin));

            string path = this.m_File;
            System.IO.FileStream file = System.IO.File.Create(path);

            writer.Serialize(file, r);
            file.Close();
            return true;
        }

        private plugin ToRoot()
        {
            plugin r = this.GetRootForCurrent();
            if (r == null)
            {
                return null;
            }
            r.profileName = this.ProfileName;
            r.enabled = this.Enabled;
            r.minVersion = this.MinVersion;
            r.version = this.Version;
            if (r.commands != null)
            {
                foreach (var item in r.commands)
                {
                    if (this.m_Commands.ContainsKey(item.hotkey))
                    {
                        var c = this.m_Commands[item.hotkey];
                        item.category = c.Category;
                        item.description = c.Description;
                        item.enabled = c.Enabled;
                        item.hotkey = c.Hotkey;
                        item.name = c.Name;
                    }
                }
            }
            if (r.hotstrings != null)
            {
                foreach (var item in r.hotstrings)
                {
                    if (this.m_Hotstrings.ContainsKey(item.trigger))
                    {
                        var c = this.m_Hotstrings[item.trigger];
                        item.category = c.Category;
                        item.description = c.Description;
                        item.enabled = c.Enabled;
                        item.trigger = c.Trigger;
                        item.name = c.Name;
                    }
                }
            }
            
            return r;
        }

        private plugin GetRootForCurrent()
        {
            if (File.Exists(this.m_File))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(plugin));

                StreamReader reader = new StreamReader(this.m_File);
                var rootXml = (plugin)serializer.Deserialize(reader);
                reader.Close();
                rootXml.File = this.m_File;

                return rootXml;
            }
            return null;
        }
        #endregion

        #endregion

        #region Compare
        /// <summary>
        /// Compares this instance Name with a specified Object and indicates whether this instance 
        /// precedes, follows, or appears in the same position in the sort order as the specified Object.
        /// </summary>
        /// <param name="obj">The instance to compare ot this instance</param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows, or appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(object obj)
        {
            if (obj is Plugin)
            {
                Plugin other = (Plugin)obj;
                return this.Name.CompareTo(other.Name);
            }
            return 1;
        }

        /// <summary>
        /// Compares this instance Name with a specified PluginFile Name and indicates whether this instance 
        /// precedes, follows, or appears in the same position in the sort order as the specified Object.
        /// </summary>
        /// <param name="other">The instance to compare to this instance</param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows, or appears in the same position in the sort order as the value parameter.
        /// </returns>
        public int CompareTo(Plugin other)
        {
            return this.Name.CompareTo(other.Name);
        }
        #endregion

        #region Dirty Status Management

        // DirtyStatusChanged is the event to notify subscribers that a specific property is now dirty. We're using the
        // PropertyChangedEventHandler class as a convenient way to pass the property name to a subscriber.
        public event PropertyChangedEventHandler DirtyStatusChanged;

        // DataStateEventHandler is the event to notify subscribers that state of the class has been changed to dirty or clean
        public event DataStateEventHandler DataStateChanged;

        // changes is our internal dictionary which holds the changed properties and their original values.
        // static will cause all class instances to share the same dictionary
        //private static ConcurrentDictionary<String, Object> _changes = new ConcurrentDictionary<String, Object>();
        private ConcurrentDictionary<String, Object> _changes = new ConcurrentDictionary<String, Object>();

        /// <summary>
        /// Returns the original value of the property so it can be compared to the current
        /// value or used to restore the original value
        /// </summary>
        /// <param name="propertyName">The name of the class property to fetch the original value for.</param>
        /// <returns>If an original value is present, that value will be returned. If the original value is not present,
        /// the method will return null.</returns>
        public object GetChangedData(string propertyName)
        {
            if (String.IsNullOrEmpty(propertyName) ||
                !_changes.ContainsKey(propertyName)) return null;
            return _changes[propertyName];
        }

        /// <summary>
        /// Clears the record of changed properties and their original values.
        /// </summary>
        /// <remarks>Call this method when the data in the model is saved.</remarks>
        public void ClearChangedData()
        {
            _changes.Clear();
            this.m_PreviousChangeCount = 0;
            this.ClearListChanges();
            // Raise the change events to notify subscribers the dirty status has changed
            RaiseDataChanged("");
            RaiseDataStateChanged();

        }

        /// <summary>
        /// Returns true if one or more monitored properties has changed.
        /// </summary>
        public bool HasChangedData
        {
            get
            {
                int i = GetChangesCount();
                return i > 0;
            }
        }

        // CheckDataChange should be called in property setters BEFORE the property value is set. It will
        // check to see if it already has a memory of the properties original value. If not, it will inspect
        // the property to get the original value and then save that back raising the DirtyStatusChanged event
        // in the process. If the new value is the same as the original value, the property will be removed from
        // the list of dirty properties.
        private void CheckDataChange(string propertyName, Object newPropertyValue)
        {
            // If we were passed an empty property name, eject.
            if (string.IsNullOrWhiteSpace(propertyName))
                return;

            // Check to see if the property already exists in the dictionary...
            if (_changes.ContainsKey(propertyName))
            {
                // Already exists in the change collection
                if (_changes[propertyName].Equals(newPropertyValue))
                {
                    // The old value and the new value match
                    object oldValueObject = null;
                    _changes.TryRemove(propertyName, out oldValueObject);
                    RaiseDataChanged(propertyName);
                    RaiseDataStateChanged();
                }
                else
                {
                    // New value is different than the original value...
                    // Don't do anything because we already know this value changed.
                }
            }
            else
            {
                // Key is not in the dictionary. Get the original value and save it back
                if (!_changes.TryAdd(propertyName, TestAndCastClassProperty(propertyName)))
                {
                    throw new ArgumentException("Unable to add specified property to the changed data dictionary.");
                }
                else
                {
                    RaiseDataChanged(propertyName);
                    RaiseDataStateChanged();
                }
            }
        }

        // Raises the events to notify interested parties that one or more monitored properties are now dirty
        private void RaiseDataChanged(string propertyName)
        {
            // Raise the DirtyStatusChanged event passing the name of the changed property
            if (DirtyStatusChanged != null)
                DirtyStatusChanged(this, new PropertyChangedEventArgs(propertyName));

            // Raise property changed on HasChangedData in case something is bound to that property
            RaisePropertyChanged("HasChangedData");
        }

        // Raises the evento to notify the instrested parties that the Modifiled state has changed to dirty or clean
        private void RaiseDataStateChanged()
        {
            int i = this.GetChangesCount();
            if (this.m_PreviousChangeCount != i)
            {
                this.m_PreviousChangeCount = i;
                if (DataStateChanged != null)
                    DataStateChanged(this, new DataStateEventArgs(this.HasChangedData));

            }
        }

        // Internal method which will get the value of the specified property
        private object TestAndCastClassProperty(string Property)
        {
            if (string.IsNullOrWhiteSpace(Property))
                return null;
            // _myType is the type info for this class and is fetched during construction.
            PropertyInfo propInfo = MyType.GetProperty(Property);
            if (propInfo == null) { return null; }
            return propInfo.GetValue(this, null);
        }

        #endregion Dirty Status Management

        #region INotifyPropertyChanged Implements

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Property Notification
        protected void SetPropertyValue(object newValue, Action setValue, [CallerMemberName] string propertyName = null)
        {
            // This is a general way of checking and setting properties which can be called via a lambda.
            CheckDataChange(propertyName, newValue);
            setValue();
            RaisePropertyChanged(propertyName);
        }

        // Standard property change notification
        // NOTICE: The CallerMemberName attribute is not available in Portable Class Libraries unless you add it yourself!
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region INotifyDataErrorInfo boilerplate

        private Dictionary<String, List<String>> errors = new Dictionary<string, List<string>>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;


        // Adds the specified error to the errors collection if it is not
        // already present, inserting it in the first position if isWarning is
        // false. Raises the ErrorsChanged event if the collection changes.
        private void AddError(string propertyName, string error, bool isWarning)
        {
            if (!errors.ContainsKey(propertyName))
                errors[propertyName] = new List<string>();

            if (!errors[propertyName].Contains(error))
            {
                if (isWarning) errors[propertyName].Add(error);
                else errors[propertyName].Insert(0, error);
                RaiseErrorsChanged(propertyName);
            }
        }

        // Removes the specified error from the errors collection if it is
        // present. Raises the ErrorsChanged event if the collection changes.
        private void RemoveError(string propertyName, string error)
        {
            if (errors.ContainsKey(propertyName) &&
                errors[propertyName].Contains(error))
            {
                errors[propertyName].Remove(error);
                if (errors[propertyName].Count == 0) errors.Remove(propertyName);
                RaiseErrorsChanged(propertyName);
            }
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (String.IsNullOrEmpty(propertyName) ||
                !errors.ContainsKey(propertyName)) return null;
            return errors[propertyName];
        }

        public bool HasErrors
        {
            get
            {
                return errors.Count > 0;
            }
        }

        #endregion INotifyDataErrorInfo boilerplate

       
    }
}
