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
    public partial class foodcellar : Form
    {
        public foodcellar()
        {
            InitializeComponent();
        }

        public user user;
        public Theme theme;
        string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";
        public string mode = "online";

        private void button3_Click(object sender, EventArgs e)
        {
            //add ingredients
            //string ing = textBox1.Text;
            //string amount = textBox2.Text;
            if(textBox1.Text != "" && textBox2.Text != "")
            {
                Ingredients ing = new Ingredients();
                ing.name = textBox1.Text;
                ing.amount = textBox2.Text;
                ing.type = "Cellar";

                ing.User_Add_Ingredients(user.id);

                /*SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO User_Ingredients values (@Uid,@name,@amount,@t)", con);
                cmd.Parameters.AddWithValue("@Uid", user.id);
                cmd.Parameters.AddWithValue("@name", ing.name);
                cmd.Parameters.AddWithValue("@amount", ing.amount);
                cmd.Parameters.AddWithValue("@t", ing.type);

                cmd.ExecuteNonQuery();
                con.Close();*/

                user.Cellar.Add(ing);
                listBox1.Items.Add(ing.amount + " " + ing.name);

                MessageBox.Show("Successfull");
                textBox1.Text = "";
                textBox2.Text = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //update
            if(listBox1.SelectedItem != null)
            {
                Ingredients ing = new Ingredients();
                ing.name = textBox1.Text;
                ing.amount = textBox2.Text;
                ing.type = "Cellar";

                listBox1.Items[listBox1.SelectedIndex] = textBox1.Text + " " + textBox2.Text;
                user.Cellar[listBox1.SelectedIndex] = ing;

                ing.User_Ingredients_Update(user.id);

                /*
                //database update
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("UPDATE User_Ingredients SET Name=@name,Amount=@a,Type=@t WHERE UserID=@id", con);
                cmd.Parameters.AddWithValue("@id", user.id);
                cmd.Parameters.AddWithValue("@name", ing.name);
                cmd.Parameters.AddWithValue("@a", ing.amount);
                cmd.Parameters.AddWithValue("@t", ing.type);
                cmd.ExecuteNonQuery();
                con.Close();*/

                textBox1.Text = "";
                textBox2.Text = "";
            }
            else
            {
                MessageBox.Show("Select a ingredients to update from the list");
            }
        }

        private void foodcellar_Load(object sender, EventArgs e)
        {
            lbl_welcome.Text = "Welcome " + user.username;
            fill_theme();
            listBox1.Items.Clear();
            for (int i = 0; i < user.Cellar.Count; i++)
            {
                Ingredients ing = (Ingredients)user.Cellar[i];
                listBox1.Items.Add(ing.amount + " " + ing.name);
            }
        }

        private void fill_theme()
        {
            Color bgc = theme.BackgroundColor,pc = theme.PanelColor,bc = theme.BtnColor;

            this.BackColor = bgc;
            panel1.BackColor = pc;

            button1.BackColor = bc;
            button2.BackColor = bc;
            button3.BackColor = bc;
            button4.BackColor = bc;
            button5.BackColor = bc;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //delete
            int index = listBox1.SelectedIndex;
            Ingredients ing = (Ingredients)user.Cellar[index];
            ing.User_Delete_Ingredients(user.id);

            //database remove
            /*SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM User_Ingredients WHERE UserID='" + user.id + "' AND Name='" + ing.name + "'", con);
            cmd.ExecuteReader();
            con.Close();*/

            user.Cellar.RemoveAt(index);
            listBox1.Items.RemoveAt(index);

            MessageBox.Show("Successfully deleted");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            mainmenu menu = new mainmenu();
            menu.u = user;
            menu.theme = theme;
            this.Close();
            menu.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Library lbr = new Library();
            lbr.user = user;
            lbr.theme = theme;
            this.Close();
            lbr.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //logout
            Application.OpenForms["login"].Show();
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //profile
            profile p = new profile();
            p.theme = theme;
            p.u = user;
            this.Close();
            p.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            //search
            string text = txt_search.Text;
            bool var = false;
            string result="";
            for (int i = 0; i < user.Cellar.Count; i++)
            {
                Ingredients ing = (Ingredients)user.Cellar[i];
                if (text.ToLower() == ing.name.ToLower())
                {
                    var = true;
                    result = ing.amount + " " + ing.name;
                    break;
                }
                else
                    var = false;
            }

            if(var)
            {
                MessageBox.Show(result,"Found");
            }
            else
            {
                MessageBox.Show("No search result.","Alert");
            }
        }
        Color keep1;
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
