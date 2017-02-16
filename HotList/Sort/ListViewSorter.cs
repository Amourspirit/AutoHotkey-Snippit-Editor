
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

using System.Windows.Forms;
namespace BigByteTechnologies.Windows.AHKSnipit.HotList.Sort
{




    /// <summary>
    /// Sort a listview in details mode based on column header click.
    /// </summary>
    /// <remark>
    /// Main feature 1: Handles the column types "Text", "Numeric" and "Date" (with optional autodetection).
    /// Main feature 2: Handles culture settings (default is to use the current Windows settings).
    /// </remark>
    /// <example>
    /// Usage example (for a listview named "lvMain")...:
    /// <code>
    /// ListViewItemSorter oSorter;
    /// 
    /// private void lvMain_ColumnClick(object sender, ColumnClickEventArgs e)
    /// {
    ///     if (lvMain.View != View.Details)
    ///     {
    ///         return;
    ///     }
    ///     if (this.oSorter == null)
    ///     {
    ///         this.oSorter = new Sort.ListViewItemSorter();
    ///     }
    ///     this.oSorter.Column = e.Column;
    ///     this.oSorter.ColumnType = oSorter.DetectColumnType(lvMain, e.Column);
    ///     oSorter.InvertSortOrder();
    ///     
    ///     if (lvMain.ListViewItemSorter == null)
    ///     {
    ///         lvMain.ListViewItemSorter = oSorter;
    ///     }
    ///     else
    ///     {
    ///         lvMain.Sort();
    ///     }
    ///
    /// }
    /// </code>
    /// </example>
    internal class ListViewItemSorter : IComparer, IComparer<ListViewItem>
    {
        // The column to sort on.
        protected int m_nColumn = -1;
        // Text, Numeric or Date.
        protected ListViewColumnType m_nColumnType = ListViewColumnType.Text;
        // Text column CompareMethod can be Binary or Text.
        protected CompareMethod m_nCompareMethod = Sort.CompareMethod.Text;
        protected bool m_bCompareFromLeftToDiff = true;
        // Ascending or Descending sort.
        protected CompareResult m_nSortOrder =  CompareResult.FirstIsBiggerThanSecond;
        // Default to Current Culture Settings.
        protected System.Globalization.CultureInfo m_oCulture = System.Globalization.CultureInfo.CurrentCulture;

        private bool m_IgnoreCase = false;

        internal bool IngnoreCase
        {
            get
            {
                return m_IgnoreCase;
            }
            set
            {
                m_IgnoreCase = value;
            }
        }

        /// <summary>
        /// The column to sort on.
        /// </summary>
        internal int Column
        {
            get { return m_nColumn; }
            set { m_nColumn = value; }
        }



        /// <summary>
        /// Text, Numeric or Date.
        /// </summary>
        internal ListViewColumnType ColumnType
        {
            get { return m_nColumnType; }
            set { m_nColumnType = value; }
        }



        /// <summary>
        /// Text columns can be sorted Binary (case sensitive sort order derived from the internal binary representations of the characters) or Text (case-insensitive text sort order determined by the system's LocaleID value).
        /// </summary>
        internal CompareMethod CompareMethod
        {
            get { return m_nCompareMethod; }
            set { m_nCompareMethod = value; }
        }



        /// <summary>
        /// Gets/Sets the specifies in which way a Text column should be handled.
        /// </summary>
        /// <remark>
        /// True = Compare each char from left until one is bigger or smaller than the other (i.e. "ab2" is smaller than "AB1" when casesensitive, because "a" is smaller than "A"). I.e. behave a lot like StrComp() - but honor the CultureInfo setting!
        /// False = Compare using the dotNET frameworks string.compare() method (which says that "ab" is smaller than "AB" when casesensitive, BUT "ab2" is BIGGER than "AB1" even when casesensitive...).
        /// </remark>
        internal bool CompareFromLeftToDiff
        {
            get { return m_bCompareFromLeftToDiff; }
            set { m_bCompareFromLeftToDiff = value; }
        }



        /// <summary>
        /// Gets/Sets the Ascending or Descending sort.
        /// </summary>
        internal System.Windows.Forms.SortOrder SortOrder
        {
            get
            {
                // Return last SortOrder.
                switch (m_nSortOrder)
                {
                    case CompareResult.FirstIsSmallerThanSecond:
                        return System.Windows.Forms.SortOrder.Ascending;
                    case CompareResult.FirstIsBiggerThanSecond:
                        return System.Windows.Forms.SortOrder.Descending;
                    default:
                        return System.Windows.Forms.SortOrder.None;
                }
            }
            set
            {
                // Set the SortOrder to be used at next sort.
                switch (value)
                {
                    case System.Windows.Forms.SortOrder.Ascending:
                        m_nSortOrder = CompareResult.FirstIsSmallerThanSecond;
                        break;
                    case System.Windows.Forms.SortOrder.Descending:
                        m_nSortOrder = CompareResult.FirstIsBiggerThanSecond;
                        break;
                    default:
                        // Invalid value (i.e. CompareResult.FirstIsEqualToSecond).
                        // Abort.
                        return;
                }
            }
        }



        /// <summary>
        /// Gets/Sets The Culture Settings to use in variable type conversion and comparisons.
        /// </summary>
        /// The default is the current thread (i.e. system) locale setting. 
        /// Set it to "" to reset it to the default.
        /// Set it to "iv" to use an invariant culture (based on the English culture).
        /// </remark>
        internal string CultureInfoName
        {

            get { return m_oCulture.Name; }
            set
            {
                string sValue = value.ToLower();
                switch (sValue)
                {
                    case "":
                        m_oCulture = System.Globalization.CultureInfo.CurrentCulture;
                        // Use the current thread (i.e. system) locale setting.
                        break;
                    case "iv":
                    case "inv":
                    case "invariant":
                    case "neutral":
                    case "culture-insensitive":
                    case "culture insensitive":
                    case "cultureinsensitive":
                        m_oCulture = System.Globalization.CultureInfo.InvariantCulture;
                        // Use an invariant culture (based on the english culture). Info: Dates convert well if they are in ISO format (i.e. "yyyy-MM-dd") or american format (i.e. "MM/dd/yyyy").
                        break;
                    default:
                        m_oCulture = System.Globalization.CultureInfo.CreateSpecificCulture(value);
                        // Info: Raises an error if Value contains an invalid CultureInfoName.
                        break;
                }
            }
        }



        /// <summary>
        /// An easy way to invert the sortorder of the next sort (ascending turns descending and vice versa).
        /// </summary>
        /// <returns>
        /// The sort order after Inversion
        /// </returns>
        internal SortOrder InvertSortOrder()
        {
            // Return a value from the enum named "SortOrder" in System.Windows.Forms.

            //const string sMemberName = "InvertSortOrder";
            SortOrder retval; ;

            switch (m_nSortOrder)
            {
                case CompareResult.FirstIsEqualToSecond:
                    m_nSortOrder = CompareResult.FirstIsSmallerThanSecond;
                    retval = SortOrder.Ascending;
                    break;
                case CompareResult.FirstIsSmallerThanSecond:
                    m_nSortOrder = CompareResult.FirstIsBiggerThanSecond;
                    retval = SortOrder.Descending;
                    break;
                case CompareResult.FirstIsBiggerThanSecond:
                    m_nSortOrder = CompareResult.FirstIsSmallerThanSecond;
                    retval = SortOrder.Ascending;
                    break;
                default:
                    retval = SortOrder.Ascending;
                    break;
            }
            return retval;
        }



        /// <summary>
        /// Returns column type based on column contents.
        /// </summary>
        internal ListViewColumnType DetectColumnType(ListView theListView, int columnIndex)
        {
            int nNumericCount = 0;
            int nDateCount = 0;
            //ListViewItem oRowItem = default(ListViewItem);
            string sTxt = null;
            double nValue = 0;
            DateTime dDate = default(System.DateTime);

            if (theListView == null)
            {
                return ListViewColumnType.Text;
                // Invalid. Abort!
            }

            if (columnIndex < 0)
            {
                return ListViewColumnType.Text;
                // Invalid. Abort!
            }

            try
            {
                theListView.Cursor = Cursors.WaitCursor;
                for (int i = 0; i < theListView.Items.Count; i++)
                {
                    ListViewItem oRowItem = theListView.Items[i];
                    if (columnIndex < oRowItem.SubItems.Count)
                    {
                        // The column exists in listview rowitem.
                        sTxt = oRowItem.SubItems[columnIndex].Text;
                        if (!string.IsNullOrEmpty(sTxt))
                        {
                            // Not empty. Detect type...:

                            if (nNumericCount > 0)
                            {
                                try
                                {
                                    nValue = double.Parse(sTxt, m_oCulture.NumberFormat);
                                    // Convert to double.
                                }
                                catch
                                {
                                    return ListViewColumnType.Text;
                                    // If one row in the column is text (i.e. not ANY other type) then the entire column is classed as columntype text.
                                }


                            }
                            else if (nDateCount > 0)
                            {
                                try
                                {
                                    dDate = System.DateTime.Parse(sTxt, m_oCulture.DateTimeFormat);
                                    // Convert to date.
                                }
                                catch
                                {
                                    return ListViewColumnType.Text;
                                    // If one row in the column is text (i.e. not ANY other type) then the entire column is classed as columntype text.
                                }


                            }
                            else {
                                try
                                {
                                    nValue = double.Parse(sTxt, m_oCulture.NumberFormat);
                                    // Convert to double.
                                    nNumericCount += 1;

                                }
                                catch
                                {
                                    // Not a numeric.
                                    try
                                    {
                                        dDate = System.DateTime.Parse(sTxt, m_oCulture.DateTimeFormat);
                                        // Convert to date.
                                        nDateCount += 1;

                                    }
                                    catch
                                    {
                                        // Not a numeric and not a date = a text.
                                        return ListViewColumnType.Text;
                                        // If one row in the column is text (i.e. not ANY other type) then the entire column is classed as columntype text.
                                    }

                                }

                            }

                        }
                        else {
                            // Ignore empty strings.
                        }

                    }
                    else {
                        // Ignore rows with missing subitems.
                    }
                }

                // All rows are checked. Return the result of the autodetection.
                if (nDateCount > 0)
                {
                    return ListViewColumnType.Date;
                }

                if (nNumericCount > 0)
                {
                    return ListViewColumnType.Numeric;
                }

                return ListViewColumnType.Text;

            }
            finally
            {
                theListView.Cursor = Cursors.Default;
            }

        }

        /// <summary>
        /// Compare two strings
        /// </summary>
        /// <param name="string1">The first String to compare</param>
        /// <param name="string2">The second Strign to compare</param>
        /// <returns>
        /// Less than zero if the first substring precedes the second substring in the sort order.
        /// Zero if the substrings occur in the same position in the sort order, or length is zero
        /// Greater than zero if the first substring follows the second substring in the sort order.
        /// </returns>
        protected int StringCompare(string string1, string string2)
        {
            return this.StringCompare(string1, string2, System.Globalization.CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Compare two strings using the specified CultureInfo.
        /// </summary>
        /// <param name="string1">The first String to compare</param>
        /// <param name="string2">The second Strign to compare</param>
        /// <param name="culture">The Culture used to comapre the string with</param>
        /// <returns>
        /// Less than zero if the first substring precedes the second substring in the sort order.
        /// Zero if the substrings occur in the same position in the sort order, or length is zero
        /// Greater than zero if the first substring follows the second substring in the sort order.
        /// </returns>
        protected int StringCompare(string string1, string string2, System.Globalization.CultureInfo culture)
        {
            return StringCompare(string1, string2, culture, this.IngnoreCase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="string1">The first String to compare</param>
        /// <param name="string2">The second Strign to compare</param>
        /// <param name="culture">The Culture used to comapre the string with</param>
        /// <param name="Compare"></param>
        /// <returns>
        /// Less than zero if the first substring precedes the second substring in the sort order.
        /// Zero if the substrings occur in the same position in the sort order, or length is zero
        /// Greater than zero if the first substring follows the second substring in the sort order.
        /// </returns>
        protected int StringCompare(string string1, string string2, System.Globalization.CultureInfo culture, bool IgnoreCase)
        {
            int result;

            int cResult = string.Compare(string1, string2, IgnoreCase, culture);
            if (cResult > 0)
            {
                result = 1;
            } else if(cResult < 0)
            {
                result = -1;
            } else
            {
                result = 1;
            }
            return result;
        }



        /// <summary>
        /// Compares two specified <see cref="ListViewItem"/> objects and returns an integer that indicates their relative position in the sort order.
        /// </summary>
        /// <param name="object1">The first Item to compare</param>
        /// <param name="object2">The second item to compare</param>
        /// <returns>
        /// Less than zero if the object1 precedes the second substring in the sort order.
        /// Zero if the object1 equals object2 occur in the same position in the sort order, or length is zero.
        /// Greater than zero if object1 follows the object2 in the sort order.
        /// </returns>
        /// <remarks>
        /// Sort is determined by the current value of <see cref="Column"/>
        /// </remarks>
        public int Compare(object object1, object object2)
        {
            ListViewItem x = default(ListViewItem);
            ListViewItem y = default(ListViewItem);
          
            try
            {
                // Convert from Object type to ListViewItem type...:
                x = (ListViewItem)object1;
                y = (ListViewItem)object2;
            }
            catch
            {
                return 0;
                // No sorting.
            }

            return this.Compare(x, y);
        }

        /// <summary>
        /// Compares two specified <see cref="ListViewItem"/> objects and returns an integer that indicates their relative position in the sort order.
        /// </summary>
        /// <param name="x">The first Item to compare</param>
        /// <param name="y">The second item to compare</param>
        /// <returns>
        /// Less than zero if <paramref name="x"/> precedes <paramref name="y"/> in the sort order.
        /// Zero if the <paramref name="x"/> and <paramref name="x"/> occur in the same position in the sort order, or null.
        /// Greater than zero if <paramref name="x"/> follows the <paramref name="y"/> in the sort order.
        /// </returns>
        /// <remarks>
        /// Sort is determined by the current value of <see cref="Column"/>
        /// </remarks>
        public int Compare(ListViewItem x, ListViewItem y)
        {
            
            string sTxt1 = null;
            string sTxt2 = null;
            

            if (m_nColumn < 0)
            {
                // Invalid column.
                return 0;
                // No sorting.
            }

            // Get the text in the column in the rowitem...:
            if (m_nColumn < x.SubItems.Count)
            {
                // The subitem exists in listview rowitem 1.
                sTxt1 = x.SubItems[m_nColumn].Text;
            }
            if (m_nColumn < y.SubItems.Count)
            {
                // The subitem exists in listview rowitem 2.
                sTxt2 = y.SubItems[m_nColumn].Text;
            }


            // Handle empty values...:
            if (string.IsNullOrEmpty(sTxt1))
            {
                if (string.IsNullOrEmpty(sTxt2))
                {
                    return (int)m_nSortOrder;
                    // Both are zero length = No sorting.
                }
                return -(int)m_nSortOrder;
                // The first is empty, but not the second = first is smaller than the second.

            }
            else if (string.IsNullOrEmpty(sTxt2))
            {
                return 1;
                // The second is empty, but not the first = first is bigger than the second.

            }


            // Compare the two values...:
            switch (m_nColumnType)
            {
                case ListViewColumnType.Text:
                    if (m_bCompareFromLeftToDiff == true)
                    {
                        return -(int)m_nSortOrder * StringCompare(sTxt1, sTxt2, m_oCulture, true);
                    }
                    else {
                        return -(int)m_nSortOrder * string.Compare(sTxt1, sTxt2, (m_nCompareMethod == CompareMethod.Text), m_oCulture);
                    }

                case ListViewColumnType.Numeric:
                    double nValue1 = 0;
                    double nValue2 = 0;

                    try
                    {
                        nValue1 = Convert.ToDouble(sTxt1);
                        nValue1 = double.Parse(sTxt1, m_oCulture.NumberFormat);
                        // Convert to double.
                    }
                    catch
                    {
                        return 0;
                        // No sorting.
                    }
                    try
                    {
                        nValue2 = double.Parse(sTxt2, m_oCulture.NumberFormat);
                        // Convert to double.
                    }
                    catch
                    {
                        return 0;
                        // No sorting.
                    }

                    if (nValue1 < nValue2)
                    {
                        return (int)m_nSortOrder;
                    }

                    if (nValue1 > nValue2)
                    {
                        return -(int)m_nSortOrder;
                    }

                    // No sorting.
                    return 0;

                case ListViewColumnType.Date:
                    System.DateTime dDate1 = default(System.DateTime);
                    System.DateTime dDate2 = default(System.DateTime);
                    try
                    {
                        dDate1 = System.DateTime.Parse(sTxt1, m_oCulture.DateTimeFormat);
                        // Convert to date.
                    }
                    catch
                    {
                        return 0;
                        // No sorting.
                    }
                    try
                    {
                        dDate2 = System.DateTime.Parse(sTxt2, m_oCulture.DateTimeFormat);
                        // Convert to date.
                    }
                    catch
                    {
                        return 0;
                        // No sorting.
                    }

                    return -(int)m_nSortOrder * System.DateTime.Compare(dDate1, dDate2);
                default:
                    // No sorting.
                    return 0;

            }
        }
    }


    internal enum ListViewColumnType
    {
        Text = 1,
        Numeric,
        Date,
        AutoDetect
    }
    internal enum CompareResult
    {
        FirstIsSmallerThanSecond = -1,
        FirstIsEqualToSecond = 0,
        FirstIsBiggerThanSecond = 1
    }

    internal enum CompareMethod
    {
        Binary,
        Text
    }
}

