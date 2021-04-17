using PCSC;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// 
/// </summary>
namespace Nmind.pcsc.reader {

    /// <summary>
    /// 
    /// </summary>
    static class Program {

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FrmMain form = new FrmMain();
            Application.Run(form);
        }
    }
}
