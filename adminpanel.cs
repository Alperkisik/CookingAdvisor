using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CookingAdvisor
{
    public partial class adminpanel : Form
    {
        public adminpanel()
        {
            InitializeComponent();
        }

        public user user;
        private void button3_Click(object sender, EventArgs e)
        {
            Application.OpenForms["login"].Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            adminlibrary al = new adminlibrary();
            al.user = user;
            this.Close();
            al.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            adminusers au = new adminusers();
            au.user = user;
            this.Close();
            au.Show();
        }

        private void adminpanel_Load(object sender, EventArgs e)
        {
            
        }

        Color keep1;

        private void btn_mouseEnter(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            keep1 = btn.BackColor;
            btn.BackColor = Color.Yellow;
        }
        private void btn_mouseLeave(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackColor = keep1;
        }
    }
}
