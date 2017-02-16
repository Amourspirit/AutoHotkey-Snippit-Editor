using System;
using System.ComponentModel;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Data
{
    /// <summary>
    /// Interface for a data class which implements a smarter dirty flag
    /// </summary>
    public interface INotifyDirtyData
    {
        event PropertyChangedEventHandler DirtyStatusChanged;
        event DataStateEventHandler DataStateChanged;
        Object GetChangedData(string propertyName);
        void ClearChangedData();
        bool HasChangedData { get; }
    }

    /// <summary>
    /// Args to pass wihen calling <see cref="DataStateEventHandler"/>
    /// </summary>
    public class DataStateEventArgs: EventArgs
    {

        public DataStateEventArgs() : base()
        {
            this.HasChangedData = false;
           
        }

        public DataStateEventArgs(bool HasChange): this()
        {
            this.HasChangedData = HasChange;
        }

        public bool HasChangedData { get; set; }
    }

    /// <summary>
    /// Represents the method that will handle the Dirty State. Event is raised when changeing properties for a class that implement this Event.
    /// </summary>
    /// <param name="sender">The source of the event, typically a Class Instance</param>
    /// <param name="e">A <see cref="DataStateEventArgs"/> that contains the event data.</param>
    public delegate void DataStateEventHandler(object sender, DataStateEventArgs e);
}
