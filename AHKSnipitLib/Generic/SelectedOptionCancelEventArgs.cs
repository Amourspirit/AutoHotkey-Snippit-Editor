using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Generic
{
    /// <summary>
    /// <see cref="SelectedOptions{T}"/> cancel Event Args
    /// </summary>
    /// <typeparam name="T">The Enum Type</typeparam>
    public class SelectedOptionCancelEventArgs<T> : SelectedOptionEventArgs<T> where T : struct
    {
        /// <summary>
        /// The Cancel Option
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="eValue">The Enum Value of the event</param>
        public SelectedOptionCancelEventArgs(T eValue) : base(eValue)
        {
            Cancel = false;
        }
    }
}
