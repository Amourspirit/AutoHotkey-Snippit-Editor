using BigByteTechnologies.Library.Windows.CommandManagement;
using System.Windows.Forms;
using System;

namespace BigByteTechnologies.Windows.AHKSnipit.HotList.Executor
{
    public class ToolStripEx : CommandExecutor<ToolStrip>
    {
       
        public override void Check(ToolStrip item, bool bCheck)
        {
            return;
        }

        public override void Enable(ToolStrip item, bool bEnable)
        {
            if (item.IsDisposed == true)
                return;

            item.Enabled = bEnable;
        }

        public override void Visible(ToolStrip item, bool bVisible)
        {
            if (item.IsDisposed == true)
                return;
            item.Visible = bVisible;
        }

    }
}
