using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CookingAdvisor
{
    public partial class profile : Form
    {
        public profile()
        {
            InitializeComponent();
        }

        public user u;
        public Theme theme;
        string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";

        private void profile_Load(object sender, EventArgs e)
        {
            this.BackColor = theme.BackgroundColor;
            button2.BackColor = theme.BtnColor;
            button5.BackColor = theme.BtnColor;
            button1.BackColor = theme.BtnColor;
            button4.BackColor = theme.BtnColor;

            panel1.BackColor = theme.PanelColor;
            panel2.BackColor = theme.PanelColor;

            button6.BackColor = theme.BackgroundColor;
            button7.BackColor = theme.PanelColor;
            button8.BackColor = theme.BtnColor;

            fill_box();
            fill();
        }

        private void fill_box()
        {
            txt_Uname.Text = u.username;
            txt_country.Text = u.country;
            txt_mail.Text = u.email;
            txt_password.Text = u.password;
            txt_phone.Text = u.phone;
            txt_Rname.Text = u.realname;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //update
            string name = txt_Uname.Text, pass = txt_password.Text, email = txt_mail.Text,country = txt_country.Text,phone = txt_phone.Text,realname = txt_Rname.Text;
            u.update(name,pass,email,country,phone,realname);
            /*SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand cmd = new SqlCommand("UPDATE users SET password=@p,mail=@mail,country=@c,phonenumber=@phone,realname=@Rn WHERE username=@Un", con);
            cmd.Parameters.AddWithValue("@Un", txt_Uname.Text);
            cmd.Parameters.AddWithValue("@p", txt_password.Text);
            cmd.Parameters.AddWithValue("@mail", txt_mail.Text);
            cmd.Parameters.AddWithValue("@c", txt_country.Text);
            cmd.Parameters.AddWithValue("@phone", txt_phone.Text);
            cmd.Parameters.AddWithValue("@Rn", txt_Rname.Text);
            cmd.ExecuteNonQuery();
            con.Close();

            u.password = txt_password.Text;
            u.email = txt_mail.Text;
            u.country = txt_country.Text;
            u.phone = txt_phone.Text;
            u.realname = txt_Rname.Text;*/
            fill_box();

            //MessageBox.Show("Update Successfull","Notification");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lbl_header.Text = "";
                rtb_message.Text = "";
                lbl_who.Text = "";
                string text = listBox1.Items[listBox1.SelectedIndex].ToString();
                string head = "";
                string sen = "";
                bool check = false;

                for (int i = 0; i < text.Length; i++)
                {
                    
                    if(text[i].ToString() == "-")
                    {
                        check = true;
                        continue;
                    }

                    if (check == false)
                        sen += text[i];
                    else
                        head += text[i];
                }

                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("Select * from Mail where Receiver='" + u.username + "' AND Header='" + head+"' AND Sender='"+sen+"'", con);
                SqlDataReader dr = cmd.ExecuteReader();
                if(dr.Read())
                {
                    lbl_header.Text ="Header : " + dr["Header"].ToString();
                    rtb_message.Text = dr["Message"].ToString();
                    lbl_who.Text = "From " + dr["Sender"].ToString();
                }

                dr.Close();
                con.Close();
            }
            catch
            {

            }
        }

        private void fill()
        {
            listBox1.Items.Clear();
            SqlConnection con = new SqlConnection(connectionString); con.Open();
            SqlCommand cmd = new SqlCommand("Select Header,Sender from Mail where Receiver='" + u.username + "'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                listBox1.Items.Add(dr["Sender"].ToString() + "-" + dr["Header"].ToString());
            }
            con.Close();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            //delete
            try
            {
                string header = listBox1.Items[listBox1.SelectedIndex].ToString();
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("DELETE FROM Mail WHERE Receiver='" + u.username+ "' AND Header='" + header + "'", con);
                cmd.Parameters.AddWithValue("@id", u.username);
                cmd.ExecuteReader();

                MessageBox.Show("Succesfull");
                fill();
            }
            catch
            {
                MessageBox.Show("Alert","Choose a mail from the box");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //send
            if(txt_who.Text != "" && txt_header.Text != "" && richTextBox1.Text != "")
            {
                mail m = new mail();
                m.set(richTextBox1.Text, txt_header.Text, u.username, txt_who.Text);
                m.sent();

                /*m.sender = u.username;
                m.receiver = txt_who.Text;
                m.header = txt_header.Text;
                m.message = richTextBox1.Text;*/

                /*SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO Mail values (@sender,@receiver,@header,@message)", con);
                cmd.Parameters.AddWithValue("@sender", m.sender);
                cmd.Parameters.AddWithValue("@receiver", m.receiver);
                cmd.Parameters.AddWithValue("@header", m.header);
                cmd.Parameters.AddWithValue("@message", m.message);
                cmd.ExecuteNonQuery();

                con.Close();*/

                MessageBox.Show("Sended", "Notification");
                txt_who.Text = ""; txt_header.Text = ""; richTextBox1.Text = "";
            }
            else
            {
                MessageBox.Show("You cant send mail.There is a missing information.", "Alert");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mainmenu menu = new mainmenu();
            menu.u = u;
            menu.theme = theme;
            this.Close();
            menu.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //background color
            ColorDialog colorBox = new ColorDialog();
            colorBox.ShowDialog();
            button6.BackColor = colorBox.Color;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //panelcolor
            ColorDialog colorBox = new ColorDialog();
            colorBox.ShowDialog();
            button7.BackColor = colorBox.Color;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //btn color
            ColorDialog colorBox = new ColorDialog();
            colorBox.ShowDialog();
            button8.BackColor = colorBox.Color;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //change

            reset_theme(button6.BackColor, button7.BackColor, button8.BackColor);

            /*SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string clr1, clr2, clr3;
            SqlCommand cmd = new SqlCommand("Select * from Theme where Owner='"+ u.username+"'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if(dr.Read())
            {
                dr.Close(); con.Close(); con.Open();
                cmd = new SqlCommand("UPDATE Theme SET BackgroundColor=@bgc,PanelColor=@pc,ButtonColor=@bc WHERE Owner=@name", con);
                cmd.Parameters.AddWithValue("@name", u.username);
                clr1 = theme.BackgroundColor.ToArgb().ToString();
                cmd.Parameters.AddWithValue("@bgc", clr1);
                clr2 = theme.PanelColor.ToArgb().ToString();
                cmd.Parameters.AddWithValue("@pc", clr2);
                clr3 = theme.BtnColor.ToArgb().ToString();
                cmd.Parameters.AddWithValue("@bc", clr3);

                cmd.ExecuteNonQuery();
            }
            else
            {
                dr.Close(); con.Close(); con.Open();
                cmd = new SqlCommand("INSERT INTO Theme values (@owner,@bgc,@pc,@bc)", con);
                cmd.Parameters.AddWithValue("@owner", u.username);
                clr1 = theme.BackgroundColor.ToArgb().ToString();
                cmd.Parameters.AddWithValue("@bgc", clr1);
                clr2 = theme.PanelColor.ToArgb().ToString();
                cmd.Parameters.AddWithValue("@pc", clr2);
                clr3 = theme.BtnColor.ToArgb().ToString();
                cmd.Parameters.AddWithValue("@bc", clr3);

                cmd.ExecuteNonQuery();
            }
            
            con.Close();


            MessageBox.Show("Successfull");*/
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //reset
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand cmd = new SqlCommand("Select * from Theme where Owner='default'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if(dr.Read())
            {
                string clr1 = dr["BackgroundColor"].ToString();
                Color c1 = Color.FromArgb(Convert.ToInt32(clr1));

                string clr2 = dr["PanelColor"].ToString();
                Color c2 = Color.FromArgb(Convert.ToInt32(clr2));

                string clr3 = dr["ButtonColor"].ToString();
                Color c3 = Color.FromArgb(Convert.ToInt32(clr3));

                reset_theme(c1, c2, c3);
            }

            dr.Close(); con.Close(); con.Open(); 
            cmd = new SqlCommand("DELETE FROM Theme where Owner='" + u.username + "'", con);
            cmd.ExecuteReader();

            con.Close();

            MessageBox.Show("Successfull");

        }

        private void reset_theme(Color bgc,Color pc,Color bc)
        {
            this.BackColor = bgc;

            button2.BackColor = bc;
            button5.BackColor = bc;
            button1.BackColor = bc;
            button4.BackColor = bc;

            panel1.BackColor = pc;
            panel2.BackColor = pc;

            theme.Set_Colors(bgc, pc, bc);

            button6.BackColor = theme.BackgroundColor;
            button7.BackColor = theme.PanelColor;
            button8.BackColor = theme.BtnColor;

            u.change_theme(bgc, pc, bc);
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
