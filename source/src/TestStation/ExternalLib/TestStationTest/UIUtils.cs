using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Testflow.FlowControl;
using Testflow.Usr;

namespace DemoTest
{
    public static class UIUtils
    {
        public static void ShowMessage(string message)
        {
            Application.EnableVisualStyles();
            Application.Run(new MessageForm(message));
        }

        public static void ShowExceptionForm()
        {
            Application.EnableVisualStyles();
            Application.Run(new ExceptionForm());
        }

        public static bool GetInput(out string barCode)
        {
            Application.EnableVisualStyles();
            TextInputForm textInputForm = new TextInputForm();
            Application.Run(textInputForm);
            barCode = textInputForm.BarCode;
            if (!textInputForm.Continue)
            {
                throw new TestflowLoopBreakException(true, null);
            }
            return textInputForm.Continue;
        }
    }
}
