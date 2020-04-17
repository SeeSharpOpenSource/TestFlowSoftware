using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TestFlow.DevSoftware.OperationPanel
{
    internal class SizeAdapter
    {
        private Dictionary<string, int> _controlSizeMapping;
        private Dictionary<string, float> _controlFontMapping;
        private Dictionary<string, Control> _controlMapping;
        public SizeAdapter()
        {
            _controlSizeMapping = new Dictionary<string, int>(100);
            _controlFontMapping = new Dictionary<string, float>(100);
            _controlMapping = new Dictionary<string, Control>(100);
        }

        public void RegisterControl(Control control)
        {
            _controlSizeMapping.Add(control.Name, control.Height);
            _controlFontMapping.Add(control.Name, control.Font.Size);
            _controlMapping.Add(control.Name, control);
        }

        public void Resize()
        {
            foreach (string controlName in _controlMapping.Keys)
            {
                float fontSize = _controlFontMapping[controlName];
                int originalSize = _controlSizeMapping[controlName];
                Control control = _controlMapping[controlName];
                int currentSize = control.Height;
                float newFontSize = ((int) (4*fontSize*(currentSize/originalSize)))/4f;
                Font oldFont = control.Font;
                control.Font = new Font(oldFont.FontFamily, newFontSize, oldFont.Style, oldFont.Unit, oldFont.GdiCharSet);
            }
        }
    }
}