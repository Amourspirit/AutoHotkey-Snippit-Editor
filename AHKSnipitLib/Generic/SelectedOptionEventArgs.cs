using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Generic
{
    /// <summary>
    /// Event Arguments for <see cref="SelectedOptions{T}"/>
    /// </summary>
    /// <typeparam name="T">The Enum Type</typeparam>
    public class SelectedOptionEventArgs<T> : EventArgs where T: struct
    {
        /// <summary>
        /// The Enum Value of the event
        /// </summary>
        public T EnumValue { get; set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="eValue">The Enum Value of the event</param>
        public SelectedOptionEventArgs(T eValue) : base()
        {
            if (!typeof(T).IsEnum) throw new NotSupportedException("Only Enum Types can be used");
            this.EnumValue = eValue;
        }

    }
}
