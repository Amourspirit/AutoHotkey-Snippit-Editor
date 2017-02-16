using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Concurrent;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Data;


namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin
{
    public partial class CommandSimple: IComparable, IComparable<CommandSimple>, INotifyDirtyData, INotifyDataErrorInfo, INotifyPropertyChanged
    {

        #region Constructor
        public CommandSimple()
        {
            this.m_myType = this.GetType();
            this.m_Enabled = true;
        }
        public CommandSimple(command cmd): this()
        {
            this.m_Hotkey = cmd.hotkey;
            this.m_Name = cmd.name;
            this.m_Description = cmd.description;
            this.m_Category = cmd.category;
            this.m_Enabled = cmd.enabled;
            
        }
        public CommandSimple(command cmd, string FileName) : this(cmd)
        {
            this.m_File = FileName;
            
        }

        #endregion

        #region Fields
        private Type m_myType;
        private int m_PreviousChangeCount = 0;
        #endregion

        #region Properties

        private string m_Category;
        /// <summary>
        /// Gets/Sets the Category for the Command
        /// </summary>
        public string Category
        {
            get
            {
                return this.m_Category;
            }
            set
            {
                if (this.m_Category != value)
                {
                    this.SetPropertyValue(value, () => this.m_Category = value);
                }

            }
        }
        private string m_Hotkey;
        /// <summary>
        /// Gets/Sets the Hotkey for the Command
        /// </summary>
        public string Hotkey
        {
            get
            {
                return this.m_Hotkey;
            }
            set
            {
                if (this.m_Hotkey != value)
                {
                    this.SetPropertyValue(value, () => this.m_Hotkey = value);
                }

            }
        }
        private string m_Name;
        /// <summary>
        /// Gets/Sets the Name of the HotKey command
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
        private string m_Description;
        /// <summary>
        /// Gets/Sets the Description of the Hotkey command
        /// </summary>
        public string Description
        {
            get
            {
                return this.m_Description;
            }
            set
            {
                if (this.m_Description != value)
                {
                    this.SetPropertyValue(value, () => this.m_Description = value);
                }

            }
        }
        private string m_File;
        /// <summary>
        /// Gets/Sets the File that the current Hotkey command exist in
        /// </summary>
        public string File
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
        private bool m_Enabled;
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

        #endregion

        #region Compare
        public int CompareTo(object obj)
        {
            if(obj is CommandSimple)
            {
                CommandSimple oth = (CommandSimple)obj;
                return this.Name.CompareTo(oth.Name);
            }
            return 1;
        }

        public int CompareTo(CommandSimple other)
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
            this._changes.Clear();
            this.m_PreviousChangeCount = 0;
            // Raise the change events to notify subscribers the dirty status has changed
            this.RaiseDataChanged("");
            this.RaiseDataStateChanged();

        }

        /// <summary>
        /// Returns true if one or more monitored properties has changed.
        /// </summary>
        public bool HasChangedData
        {
            get
            {
                return _changes.Count > 0;
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
            if (this.m_PreviousChangeCount != this._changes.Count)
            {
                this.m_PreviousChangeCount = this._changes.Count;
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
            PropertyInfo propInfo = this.m_myType.GetProperty(Property);
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
