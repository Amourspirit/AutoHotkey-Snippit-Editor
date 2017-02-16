using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Data.Validation
{
    /// <summary>
    /// Contains error list of failed validations
    /// </summary>
    public partial class ValidationResult
    {
        /// <summary>
        /// Construncts a new instace of <see cref="ValidationResult"/>
        /// </summary>
        public ValidationResult()
        {
            this.m_Errors = new List<string>();
        }

        /// <summary>
        /// Gets if there are any errors
        /// </summary>
        public bool HasErrors
        {
            get { return this.m_Errors.Count > 0; }
        }

        private List<string> m_Errors;
        /// <summary>
        /// Gets List of Error Strings
        /// </summary>
        public List<string> Errors
        {
            get { return m_Errors; }
        }

        /// <summary>
        /// Gets the String representation of the current errors.
        /// </summary>
        /// <returns>
        /// String with error on a new line if there are errors; Otherwise Empty String.
        /// </returns>
        public override string ToString()
        {
            if (this.HasErrors == false)
            {
                return string.Empty;
            }
            var sb = new StringBuilder();
            foreach (var e in this.m_Errors)
            {
                sb.AppendLine(e);
            }
            return sb.ToString();
        }
    }
}
