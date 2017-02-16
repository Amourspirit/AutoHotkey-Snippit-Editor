using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Data
{
    /// <summary>
    /// Interface for tracking error count
    /// </summary>
    public interface IErrorCount
    {
        /// <summary>
        /// Specifies the error count
        /// </summary>
        int ErrorCount { get; set; }
    }
}
