using BigByteTechnologies.Library.Windows.CommandManagement;
using System.Windows.Forms;
using System;
using BigByteTechnologies.Library.Windows.CommandManagement.ApplicationIdleData;

namespace BigByteTechnologies.Windows.AHKSnipit.HotList.Executor
{
    public class ToolStripTextBoxEx : CommandExecutor<ToolStripTextBox>
    {
        protected override void OnInstanceAdded(ToolStripTextBox item, Command cmd)
        {
            item.KeyDown += input_KeyDown;
            item.Leave += TextBox_Event;
            base.OnInstanceAdded(item, cmd);
        }
        protected override void OnInstanceRemoved(ToolStripTextBox item, Command cmd)
        {
            item.TextBox.KeyDown -= input_KeyDown;
            item.Leave -= TextBox_Event;
            base.OnInstanceRemoved(item, cmd);
        }

        public override void Check(ToolStripTextBox item, bool bCheck)
        {
            return;
        }

        public override void Enable(ToolStripTextBox item, bool bEnable)
        {
            if (item.IsDisposed == true)
                return;
            item.Enabled = bEnable;
        }

        public override void Visible(ToolStripTextBox item, bool bVisible)
        {
            if (item.IsDisposed == true)
                return;
            item.Visible = bVisible;
        }

        private void TextBox_Event(object sender, System.EventArgs e)
        {
            Command cmd = GetCommandForInstance((ToolStripTextBox)sender);
            CancelCommandEventArgs args = new CancelCommandEventArgs();
            args.Checked = cmd.Checked;
            args.Enabled = cmd.Enabled;
            args.Visible = cmd.Visible;
            if (this.CommandExecute != null)
                this.CommandExecute(sender, args);
            if (args.Cancel == false)
            {
                cmd.Enabled = args.Enabled;
                cmd.Checked = args.Checked;
                cmd.Visible = args.Visible;
                cmd.Execute();
            }
        }

        private void input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Command cmd = GetCommandForInstance((ToolStripTextBox)sender);
                CancelCommandEventArgs args = new CancelCommandEventArgs();
                args.Checked = cmd.Checked;
                args.Enabled = cmd.Enabled;
                args.Visible = cmd.Visible;
                if (this.CommandExecute != null)
                    this.CommandExecute(sender, args);
                if (args.Cancel == false)
                {
                    cmd.Enabled = args.Enabled;
                    cmd.Checked = args.Checked;
                    cmd.Visible = args.Visible;
                    cmd.Execute();
                }

            }
        }
    }
}
