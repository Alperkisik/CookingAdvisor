using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CookingAdvisor
{
    public partial class register : Form
    {
        public register()
        {
            InitializeComponent();
        }

        //string connectionString = "Data Source=127.0.0.1; Network Library=DBMSSOCN;Initial Catalog=CookingAdvisor;User ID=Alper; Password=capofrohan2009;";
        string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";
        private void button1_Click(object sender, EventArgs e)
        {
            back();
        }

        public void back()
        {
            Application.OpenForms["login"].Show();
            this.Close();
        }

        public void clean()
        {
            txt_country.Text = "";
            txt_mail.Text = "";
            txt_password.Text = "";
            txt_phone.Text = "";
            txt_Rname.Text = "";
            txt_Uname.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string Uname = txt_Uname.Text,Rname = txt_Rname.Text;
            string password = txt_password.Text, mail = txt_mail.Text;
            string country = txt_country.Text, phone = txt_phone.Text;

            bool condition1 = true,condition2 = true;

            if (Uname == "" || Uname == "admin")
                condition1 = false;
            else
                condition1 = true;

            if (password == "" || Rname == "" || mail == "")
                condition2 = false;
            else
                condition2 = true;

            if(condition1 && condition2)
            {
                user user = new user();
                user.register(Uname, password, mail, country, phone, Rname);
                /*SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("Select*from users where username='" + Uname + "' and realname='" + Rname + "' and mail='" + mail + "'", con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    MessageBox.Show("Warning", "This user does exist.Check your informations.");
                }
                else
                {
                    dr.Close();
                    cmd = new SqlCommand("INSERT INTO users values (@Un,@p,@mail,@c,@phone,@Rn)", con);
                    cmd.Parameters.AddWithValue("@Un", Uname);
                    cmd.Parameters.AddWithValue("@p", password);
                    cmd.Parameters.AddWithValue("@mail", mail);
                    cmd.Parameters.AddWithValue("@c", country);
                    cmd.Parameters.AddWithValue("@phone", phone);
                    cmd.Parameters.AddWithValue("@Rn", Rname);

                    cmd.ExecuteNonQuery();
                    con.Close();

                    clean();
                    MessageBox.Show("Register successfull");
                    back();
                }

                dr.Close();
                con.Close();*/
            }
            else
            {
                if(Uname == "admin")
                    MessageBox.Show("you cant choose admin as a username");
                else
                    MessageBox.Show("There is a missing information.");
            }
            
        }

        private void register_Load(object sender, EventArgs e)
        {

        }
    }
}
