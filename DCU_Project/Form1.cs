using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
namespace DCU_Project
{
    public partial class Form1 : Form
    {

        public object Me { get; private set; }
        public string loged_user="";

        //sqlite_conn = CreateConnection();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimized_Click(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Login_Click(object sender, EventArgs e)
        {
            if (textBox1.Text!="Admin")
            {
                
                MessageBox.Show("Error, Invalid UserName "+UserName.Text, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (textBox2.Text!="1234")
            {
                MessageBox.Show("Error, Invalid Password.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                loged_user = textBox1.Text;
                this.Close();
            }
        }

        int m, mx, my;
        private void titleBar_MouseDown(object sender, MouseEventArgs e)
        {
            m = 1;
            mx = e.X;
            my = e.Y;
        }

        private void titleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (m == 1)
            {
                this.SetDesktopLocation(MousePosition.X - mx, MousePosition.Y - my);
            }
        }

        private void titleBar_MouseUp(object sender, MouseEventArgs e)
        {
            m = 0;
        }
    }

}
