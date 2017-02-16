using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Generic;
using BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Plugin;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Generic
{
    /// <summary>
    /// Class for Creating Rules Options for Enums and various options.
    /// </summary>
    public class EnumRule<T> : RuleBase<string> where T : struct
    {
        /// <summary>
        /// Adds a new rule to the rules
        /// </summary>
        /// <param name="rule">The rule to add</param>
        /// <remarks>
        /// If a rule exist it will not be added again.
        /// </remarks>
        public void Add(T rule)
        {
            if (this.HasRule(rule.ToString()) == false)
            {
                base.AddRule(rule.ToString());
            }
            
        }

        /// <summary>
        /// Add a range of rules to the internal rules
        /// </summary>
        /// <param name="rules">The range of rules to add</param>
        /// <remarks>
        /// If a rule exist it will not be added again.
        /// </remarks>
        public void AddRange(T[] rules)
        {
            if (rules == null)
            {
                return;
            }
            foreach (var rule in rules)
            {
                if (this.HasRule(rule.ToString()) == false)
                {
                    base.AddRule(rule.ToString());
                }
            }
        }


        /// <summary>
        /// Removes a rule from the rules
        /// </summary>
        /// <param name="rule">The rule to remove</param>
        public void Remove(T rule)
        {
            if (this.HasRule(rule.ToString()) == true)
            {
                base.RemoveRule(rule.ToString());
            }
            
        }

        /// <summary>
        /// Removes a range of rules
        /// </summary>
        /// <param name="rules">The range to remove</param>
        public void RemoveRange(T[] rules)
        {
            if (rules == null)
            {
                return;
            }
            foreach (var rule in rules)
            {
                if (this.HasRule(rule.ToString()) == true)
                {
                    base.RemoveRule(rule.ToString());
                }
            }
        }

        /// <summary>
        /// Gets if the rule exist in the rules
        /// </summary>
        /// <param name="rule">The rule to Check</param>
        /// <returns>
        /// True if the rule exist; Otherwise, false.
        /// </returns>
        public bool this[T rule]
        {
            get
            {
                return base[rule.ToString()];
            }
        }
    }
}
