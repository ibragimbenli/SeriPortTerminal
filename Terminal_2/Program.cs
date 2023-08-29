using System;
using System.Windows.Forms;
using Terminal_1;

namespace Terminal_2
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Terminal_1.Terminal_2());
        }
    }
}
