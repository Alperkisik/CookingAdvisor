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
    public partial class mainmenu : Form
    {
        public mainmenu()
        {
            InitializeComponent();
        }

        public user u;
        public Theme theme;
        public string mode = "online";

        private void button1_Click(object sender, EventArgs e)
        {
            //log-off
            Application.OpenForms["login"].Show();
            //login lgn = new login();
            this.Close();
            //lgn.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //food cellar
            foodcellar clr = new foodcellar();
            clr.theme = theme;
            clr.mode = mode;
            clr.user = u;
            this.Close();
            clr.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //search-advise
            search_advise sa = new search_advise();
            sa.theme = theme;
            sa.mode = mode;
            sa.user = u;
            this.Close();
            sa.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //recipe library
            Library lbr = new Library();
            lbr.user = u;
            lbr.theme = theme;
            this.Close();
            lbr.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //social media
            socialmedia media = new socialmedia();
            media.user = u;
            media.theme = theme;
            this.Close();
            media.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //profile
            profile p = new profile();
            p.theme = theme;
            p.u = u;
            this.Close();
            p.Show();
        }

        private void mainmenu_Load(object sender, EventArgs e)
        {
            theme = u.theme;
            this.BackColor = theme.BackgroundColor;
            //button1.BackColor = theme.BtnColor;
            //button2.BackColor = theme.BtnColor;
            button3.BackColor = theme.BtnColor;
            button4.BackColor = theme.BtnColor;
            button5.BackColor = theme.BtnColor;
            button6.BackColor = theme.BtnColor;

            if (mode == "online")
            {
                label1.Text = "Welcome " + u.username;
                //button2.Enabled = true;
                button3.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = false;
            }
            else
            {
                label1.Text = "Offline Mode ";
                //button2.Enabled = false;
                button3.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
            }   
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //profile
            profile p = new profile();
            p.theme = theme;
            p.u = u;
            this.Close();
            p.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //log-off
            Application.OpenForms["login"].Show();
            //login lgn = new login();
            this.Close();
            //lgn.Show();
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            keep1 = pb.BackColor;
            pictureBox2.BackColor = Color.Yellow;
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            pictureBox2.BackColor = keep1;
        }

        private void pB_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            keep1 = pb.BackColor;
            pb.BackColor = Color.Yellow;
        }

        private void pB_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            pb.BackColor = keep1;
        }
    }
}
