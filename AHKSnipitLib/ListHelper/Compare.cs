using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BigByteTechnologies.Windows.Library.AHKSnipit.AHKSnipitLib.ListHelper
{
    public sealed class Compare
    {
        /// <summary>
        /// Sort Method for KeyValuePair with a key of string and a value of integer. Sort is performed on Key
        /// </summary>
        /// <param name="a">The first KeyValuePair to compare</param>
        /// <param name="b">The second KeyValuePair to compare to the first KeyValuePair</param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows, or appears in the same position in the sort order as the value parameter.
        /// Less than zero <paramref name="a"/> precedes <paramref name="b"/>, Zero <paramref name="a"/> and <paramref name="b"/> are equal, Greater than zero
        /// <paramref name="a"/> follows <paramref name="b"/> or <paramref name="b"/> is null
        /// </returns>
        public static int KeyValuePairCompareStringInt(KeyValuePair<string, int> a, KeyValuePair<string, int> b)
        {
            return a.Key.CompareTo(b.Key);
        }

        /// <summary>
        /// Sort Method for KeyValuePair with a key of string and a value of string. Sort is performed on Key
        /// </summary>
        /// <param name="a">The first KeyValuePair to compare</param>
        /// <param name="b">The second KeyValuePair to compare to the first KeyValuePair</param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows, or appears in the same position in the sort order as the value parameter.
        /// Less than zero <paramref name="a"/> precedes <paramref name="b"/>, Zero <paramref name="a"/> and <paramref name="b"/> are equal, Greater than zero
        /// <paramref name="a"/> follows <paramref name="b"/> or <paramref name="b"/> is null
        /// </returns>
        public static int KeyValuePairCompareStringKey(KeyValuePair<string, string> a, KeyValuePair<string, string> b)
        {
            return a.Key.CompareTo(b.Key);
        }

        /// <summary>
        /// Sort Method for KeyValuePair with a key of string and a value of string. Sort is performed on value
        /// </summary>
        /// <param name="a">The first KeyValuePair to compare</param>
        /// <param name="b">The second KeyValuePair to compare to the first KeyValuePair</param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows, or appears in the same position in the sort order as the value parameter.
        /// Less than zero <paramref name="a"/> precedes <paramref name="b"/>, Zero <paramref name="a"/> and <paramref name="b"/> are equal, Greater than zero
        /// <paramref name="a"/> follows <paramref name="b"/> or <paramref name="b"/> is null
        /// </returns>
        public static int KeyValuePairCompareStringValue(KeyValuePair<string, string> a, KeyValuePair<string, string> b)
        {
            return a.Value.CompareTo(b.Value);
        }

        /// <summary>
        /// Generic Sort Method for KeyValuePair with a key of string and a value of T. Sort is performed on Key
        /// </summary>
        /// <param name="a">The first KeyValuePair to compare</param>
        /// <param name="b">The second KeyValuePair to compare to the first KeyValuePair</param>
        /// <returns>
        /// A 32-bit signed integer that indicates whether this instance precedes, follows, or appears in the same position in the sort order as the value parameter.
        /// Less than zero <paramref name="a"/> precedes <paramref name="b"/>, Zero <paramref name="a"/> and <paramref name="b"/> are equal, Greater than zero
        /// <paramref name="a"/> follows <paramref name="b"/> or <paramref name="b"/> is null
        /// </returns>
        public static int KeyValuePairCompareStringAny<T>(KeyValuePair<string, T> a, KeyValuePair<string, T> b)
        {
            return a.Key.CompareTo(b.Key);
        }

    }
}
