using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BigByteTechnologies.Windows.AHKSnipit.HotList.Sort
{
    internal sealed class ListViewSort
    {
        internal static int CompareItemText(ListViewItem x, ListViewItem y)
        {
            return x.Text.CompareTo(y.Text);
        }
    }
}
