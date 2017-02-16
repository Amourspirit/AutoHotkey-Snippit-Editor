using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Data
{
    /// <summary>
    /// Tracks error count
    /// </summary>
    public class ErrorCounter : IErrorCount
    {
        private int m_errorCount = 0;
        /// <summary>
        /// Specifies the number of errors
        /// </summary>
        public int ErrorCount
        {
            get
            {
                return m_errorCount;
            }

            set
            {
                m_errorCount = value;
            }
        }
    }
}
