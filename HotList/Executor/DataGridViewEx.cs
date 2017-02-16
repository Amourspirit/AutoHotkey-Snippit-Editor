using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigByteTechnologies.Library.Windows.CommandManagement;
using System.Windows.Forms;
namespace BigByteTechnologies.Windows.AHKSnipit.HotList.Executor
{
    /// <summary>
    /// Command Executor to handle DataGridView
    /// </summary>
    public class DataGridViewEx : CommandExecutor<DataGridView>
    {
        
        /// <summary>
        /// Action Taken when Command Check State has changed
        /// </summary>
        /// <param name="item">The DataGritView to take the action on</param>
        /// <param name="bCheck">The current Check State of the Command</param>
        /// <remarks>The Method takes no action on the DataGridView</remarks>
        public override void Check(DataGridView item, bool bCheck)
        {
            return;
        }

        /// <summary>
        /// Action Taken when Command Enabled State has changed
        /// </summary>
        /// <param name="item">The DataGritView to take the action on</param>
        /// <param name="bEnable">The current Enabled State of the Command</param>
        public override void Enable(DataGridView item, bool bEnable)
        {
            if (item.IsDisposed == true)
                return;
            item.Enabled = bEnable;
        }

        /// <summary>
        /// Action Taken when Command Visible State has changed
        /// </summary>
        /// <param name="item">The DataGritView to take the action on</param>
        /// <param name="bVisible">The current Visible State of the Command</param>
        public override void Visible(DataGridView item, bool bVisible)
        {
            if (item.IsDisposed == true)
                return;
            item.Visible = bVisible;
        }

       
    }
}
