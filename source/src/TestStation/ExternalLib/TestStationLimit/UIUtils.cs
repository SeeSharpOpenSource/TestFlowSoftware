using System;
using System.Windows.Forms;
using DemoTest;
using Testflow.FlowControl;

namespace TestStationLimit
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
            return textInputForm.Continue;
        }

        public static string GetRandomSerialNumber()
        {
            Random random = new Random();
            int value = random.Next(0, 100000);
            return value.ToString();
        }
    }
}
