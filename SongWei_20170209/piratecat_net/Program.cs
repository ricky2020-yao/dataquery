using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace piratecat
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DataCenter.StartService();
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            if (args == null || args.Length == 0)
            {
                MainForm chart = new MainForm();
                Application.Run(chart);
            }
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Console.WriteLine("1");
        }
    }
}