﻿using System;
using System.Windows.Forms;

namespace UkTote.UI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var dlg = new AgreementForm())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new MainForm());
                }
            };
            
        }
    }
}
