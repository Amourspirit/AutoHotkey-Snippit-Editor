using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.Generic
{
    /// <summary>
    /// Base Class for Creating Rules
    /// </summary>
    public abstract class RuleBase<T>
    {
        private HashSet<T> m_rules = new HashSet<T>();

        /// <summary>
        /// Add a new Rule t the list
        /// </summary>
        /// <param name="Rule">The Rule to Add to the list</param>
        protected void AddRule(T Rule)
        {
            if (m_rules.Contains(Rule) == false)
            {
                m_rules.Add(Rule);
            }
        }
        /// <summary>
        /// Removes a rule from the rules
        /// </summary>
        /// <param name="Rule"></param>
        protected void RemoveRule(T Rule)
        {
            if (m_rules.Contains(Rule) == false)
            {
                m_rules.Remove(Rule);
            }
        }

        /// <summary>
        /// Gets if the rule exist in the internal rules
        /// </summary>
        /// <param name="rule">The rule to Check</param>
        /// <returns>
        /// True if the rule exist; Otherwise, false.
        /// </returns>
        /// <remarks>
        /// Shortcut for <see cref="HasRule(T)"/>
        /// </remarks>
        public bool this[T rule]
        {
            get
            {
                return this.m_rules.Contains(rule);
            }
        }

        /// <summary>
        /// Gets the Count of the rules
        /// </summary>
        public int Count
        {
            get
            {
                return this.m_rules.Count;
            }
        }
        /// <summary>
        /// Gets if the rule exist in the internal rules
        /// </summary>
        /// <param name="rule">The rule to Check</param>
        /// <returns>
        /// True if the rule exist; Otherwise, false.
        /// </returns>
        public bool HasRule(T rule)
        {
            return this.m_rules.Contains(rule);
        }
    }
}
