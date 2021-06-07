using System;
using System.Collections;
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
    public partial class adminusers : Form
    {
        public adminusers()
        {
            InitializeComponent();
        }

        string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";
        //string connectionString = "Data Source=192.168.1.80; Network Library=DBMSSOCN;Initial Catalog=CookingAdvisor;User ID=Alper; Password=capofrohan2009;";
        public user user;
        private void adminusers_Load(object sender, EventArgs e)
        {
            fill_lb();
        }

        private void fill_lb()
        {
            listBox1.Items.Clear();
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand cmd = new SqlCommand("Select username from users", con);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                string user = dr["username"].ToString();
                if (user != "admin")
                    listBox1.Items.Add(user);
            }

            dr.Close();
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            adminpanel ap = new adminpanel();
            ap.user = user;
            this.Close();
            ap.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //delete
            try
            {
                string name = listBox1.SelectedItem.ToString();

                /*SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("Select ID from users where username='" + name + "'", con);
                SqlDataReader dr = cmd.ExecuteReader();

                string userid = "";
                if (dr.Read())
                    userid = dr["ID"].ToString();

                dr.Close();

                cmd = new SqlCommand("Select ID from FoodRecipe where Owner='" + name + "'", con);
                SqlDataReader dr2 = cmd.ExecuteReader();
                ArrayList recipeIDs = new ArrayList();

                while (dr2.Read())
                {
                    recipeIDs.Add(dr2["ID"].ToString());
                }
                dr2.Close();

                SqlCommand cmd7 = new SqlCommand("DELETE FROM users WHERE username='" + name + "'", con);
                //cmd.Parameters.AddWithValue("@id", name);
                cmd7.ExecuteReader();
                con.Close(); con.Open();
                SqlCommand cmd2 = new SqlCommand("DELETE FROM FoodRecipe WHERE Owner=@id", con);
                cmd2.Parameters.AddWithValue("@id", name);
                cmd2.ExecuteReader();
                con.Close(); con.Open();
                SqlCommand cmd3 = new SqlCommand("DELETE FROM User_Ingredients WHERE UserID=@id", con);
                cmd3.Parameters.AddWithValue("@id", userid);
                cmd3.ExecuteReader();
                con.Close(); 

                for (int i = 0; i < recipeIDs.Count; i++)
                {
                    con.Open();
                    SqlCommand cmd4 = new SqlCommand("DELETE FROM Recipe_Ingredients WHERE RecipeID=@id", con);
                    cmd4.Parameters.AddWithValue("@id", recipeIDs[i].ToString());
                    cmd4.ExecuteReader();
                    con.Close();
                }

                for (int x = 0; x < recipeIDs.Count; x++)
                {
                    con.Open();
                    SqlCommand cmd5 = new SqlCommand("DELETE FROM Steps WHERE RecipeID=@id", con);
                    cmd5.Parameters.AddWithValue("@id", recipeIDs[x].ToString());
                    cmd5.ExecuteReader();
                    con.Close();
                }

                con.Open();
                SqlCommand cmd6 = new SqlCommand("DELETE FROM Mail WHERE Receiver=@id", con);
                cmd6.Parameters.AddWithValue("@id", name);
                cmd6.ExecuteReader();
                con.Close();
                //userla ilgili diğer şeyleri de sil*/

                user.delete(name);
                MessageBox.Show("successfull");
                
                fill_lb();
            }
            catch
            {
                MessageBox.Show("Warning", "Pick an user to delete.");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            string name = listBox1.Items[listBox1.SelectedIndex].ToString();
            SqlCommand cmd = new SqlCommand("Select*from users where username='" + name + "'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                label3.Text = "Username : " + dr["username"].ToString();
                label4.Text = "Password : " + dr["password"].ToString();
                label5.Text = "Email : " + dr["mail"].ToString();
                label6.Text = "Phone : " + dr["phonenumber"].ToString();
                label7.Text = "Country : " + dr["country"].ToString();
                label8.Text = "Realname : " + dr["realname"].ToString();

                textBox1.Text = name;
            }

            dr.Close();
            con.Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //send
            if (textBox1.Text != "" && textBox2.Text != "" && richTextBox1.Text != "")
            {
                send(textBox1.Text, textBox2.Text, richTextBox1.Text);

                textBox1.Text = ""; textBox2.Text = ""; richTextBox1.Text = "";
                MessageBox.Show("Successfull");
            }
            else
            {
                MessageBox.Show("Alert", "There is a missing information");
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //send everyone
            if (textBox2.Text != "" && richTextBox1.Text != "")
            {
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    string r = listBox1.Items[i].ToString();
                    send(r, textBox2.Text, richTextBox1.Text);
                }

                textBox1.Text = ""; textBox2.Text = ""; richTextBox1.Text = "";
                MessageBox.Show("Successfull");
            }
            else
            {
                MessageBox.Show("Alert", "There is a missing information");
            }
        }

        private void send(string receiver, string header, string message)
        {
            mail m = new mail();
            m.set(message, header,"admin", receiver);
            m.sent();
            /*m.receiver = receiver;
            m.sender = "admin";
            m.header = header;
            m.message = message;

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO Mail values (@sender,@receiver,@header,@message)", con);
            cmd.Parameters.AddWithValue("@sender", m.sender);
            cmd.Parameters.AddWithValue("@receiver", m.receiver);
            cmd.Parameters.AddWithValue("@header", m.header);
            cmd.Parameters.AddWithValue("@message", m.message);
            cmd.ExecuteNonQuery();

            con.Close();*/
        }
    }
}
