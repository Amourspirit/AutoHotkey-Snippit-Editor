using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;

namespace BigByteTechnologies.Windows.AHKSnipit.HotList.Extensions
{
    /// <summary>
    /// Extends Textbox to set Text without firing TextChanged event
    /// </summary>
    public static class TextboxExt
    {
        private static readonly FieldInfo _field;
        private static readonly PropertyInfo _prop;

        static TextboxExt()
        {
            Type type = typeof(Control);
            _field = type.GetField("text", BindingFlags.Instance | BindingFlags.NonPublic);
            _prop = type.GetProperty("WindowText", BindingFlags.Instance | BindingFlags.NonPublic);
        }

       
        /// <summary>
        /// Sets the text value without firing TextChanged event
        /// </summary>
        /// <param name="box">Textbox to set</param>
        /// <param name="text">The text value to set</param>
        public static void SetText(this TextBox box, string text)
        {
            _field.SetValue(box, text);
            _prop.SetValue(box, text, null);
        }
    }


    /// <summary>
    /// Extends ToolStripTextBox to set Text without firing TextChanged event
    /// </summary>
    public static class ToolStripTextboxExt
    {

     
        /// <summary>
        /// Set the text value without firing TextChanged event
        /// </summary>
        /// <param name="box">ToolstripTextBox to set</param>
        /// <param name="text">The text value to set</param>
        public static void SetText(this ToolStripTextBox box, string text)
        {
            Type t = typeof(ToolStripTextBox);
            var txtBox = (TextBox)t.GetProperty("TextBox").GetValue(box);
            txtBox.SetText(text);

        }

       
    }
}
