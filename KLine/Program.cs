using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using OwLib;
using System.Runtime.InteropServices;
using System.Text;

namespace KLine
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

            /*int ctpID = CFunctionCTP.Create();
            int ctpRequestID = CFunctionCTP.GenerateReqID(ctpID);
            CFunctionCTP.Start(ctpID, ctpRequestID, "tcp://180.168.146.187:10010", "tcp://180.168.146.187:10000", "9999", "000678", "567890");
            while (CFunctionCTP.IsMdRunning(ctpID) == 0 || CFunctionCTP.IsTdRunning(ctpID) == 0)
            {
                Thread.Sleep(1000);
            }

            ctpRequestID = CFunctionCTP.GenerateReqID(ctpID);
            CFunctionCTP.SubMarketDatas(ctpID, ctpRequestID, "IF1705, rb1712");

            StringBuilder sb = new StringBuilder(1024);
            while (true)
            {
                if (CFunctionCTP.GetDepthMarketData(ctpID, sb) != 0)
                {
                    int i = 0;
                    i--;
                }
            }*/


            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            if (args == null || args.Length == 0)
            {
                MainForm mainForm = new MainForm();
                mainForm.Load("MainFrame");
                Application.Run(mainForm);
            }
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Console.WriteLine("1");
        }
    }
}