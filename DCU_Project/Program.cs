using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
namespace DCU_Project
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 login = new Form1();
            Application.Run(login);
            if (login.loged_user!=null & login.loged_user!="")
            {
                Menu menu = new Menu();
                Application.Run(menu);
            }
            Application.Exit();
            
        }

    }
}
