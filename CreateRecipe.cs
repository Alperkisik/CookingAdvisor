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
    public partial class CreateRecipe : Form
    {
        public CreateRecipe()
        {
            InitializeComponent();
        }

        public user user;
        public Theme theme;
        FoodRecipe recipe;

        private void CreateRecipe_Load(object sender, EventArgs e)
        {
            lbl_user.Text = "Welcome " + user.username;
            fill_theme();
            recipe = new FoodRecipe();
            recipe.ingredients = new ArrayList();
            recipe.Tags = new ArrayList();
            recipe.categories = new ArrayList();
            recipe.Steps = new ArrayList();
            
            recipe.owner = user.username;
        }

        private void fill_theme()
        {
            Color bgc, pc, bc;
            if (user.username != "admin")
            {
                bgc = theme.BackgroundColor;
                pc = theme.PanelColor;
                bc = theme.BtnColor;
            }
            else
            {
                theme = new Theme();
                string clr1 = "-4599318";
                bgc = Color.FromArgb(Convert.ToInt32(clr1));
                theme.BackgroundColor = bgc;

                string clr2 = "-4599318";
                pc = Color.FromArgb(Convert.ToInt32(clr2));
                theme.PanelColor = pc;

                string clr3 = "-986896";
                bc = Color.FromArgb(Convert.ToInt32(clr3));
                theme.BtnColor = bc;
            }

            this.BackColor = bgc;
            button1.BackColor = bc;
            button2.BackColor = bc;
            button3.BackColor = bc;
            button4.BackColor = bc;
            button5.BackColor = bc;
            button6.BackColor = bc;
            button7.BackColor = bc;

            panel1.BackColor = pc;
            panel2.BackColor = pc;
            panel3.BackColor = pc;
        }

        private void button8_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //adding Ingredient
            string Ingredient = textBox3.Text + " " + textBox2.Text;
            Ingredients ing = new Ingredients();
            ing.name = textBox2.Text;
            ing.amount = textBox3.Text;
            if (checkBox1.Checked)
            {
                //ExtraIngredient
                ing.type = "Extra";
            }
            else 
            {
                //main Ingredient
                ing.type = "Main";
            }

            recipe.ingredients.Add(ing);
            listBox1.Items.Add(ing.type + " ingredients:" + Ingredient);

            textBox2.Text = "";
            textBox3.Text = "";
            checkBox1.Checked = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Deleting Ingredient
            try
            {
                int index = listBox1.SelectedIndex;
                recipe.ingredients.RemoveAt(index);
                listBox1.Items.RemoveAt(index);
            }
            catch
            {
                MessageBox.Show("Pick an ingredients from the list to delete");
            }
        }
        int i = 1;
        private void button2_Click(object sender, EventArgs e)
        {
            //adding step
            if(textBox4.Text != "")
            {
                recipe.Steps.Add(textBox4.Text);
                listBox2.Items.Add(textBox4.Text);
                label8.Text = i + ".step : ";
                i++;
                textBox4.Text = "";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //deleting step
            try
            {
                int index = listBox2.SelectedIndex;
                recipe.Steps.RemoveAt(index);
                listBox2.Items.RemoveAt(index);
            }
            catch
            {
                MessageBox.Show("Pick a step from the list to delete");
            }
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //adding tag
            if(textBox5.Text != "")
            {
                recipe.Tags.Add(textBox5.Text);
                label11.Text = "Tags : ";
                for (int i = 0; i < recipe.Tags.Count; i++)
                {
                    label11.Text += recipe.Tags[i].ToString() + ",";
                }
                textBox5.Text = "";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //adding category
            if(textBox6.Text != "")
            {
                recipe.categories.Add(textBox6.Text);
                label13.Text = "Categories : ";
                for (int i = 0; i < recipe.categories.Count; i++)
                {
                    label13.Text += recipe.categories[i].ToString() + ",";
                }
                textBox6.Text = "";
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //back
            if(user.username != "admin")
            {
                mainmenu mn = new mainmenu();
                mn.u = user;
                mn.theme = theme;
                this.Close();
                mn.Show();
            }
            else if(user.username == "admin")
            {
                adminpanel ap = new adminpanel();
                ap.user = user;
                this.Close();
                ap.Show();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            //save
            recipe.Name = textBox1.Text;
            recipe.Meal = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            recipe.Season = comboBox2.Items[comboBox2.SelectedIndex].ToString();
            recipe.Time = textBox7.Text;
            recipe.owner = user.username;
            recipe.create();
            user.RecipeLibrary.Add(recipe);


            /*string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();*/

            /*SqlCommand cmd = new SqlCommand("INSERT INTO FoodRecipe values (@name,@season,@meal,@time,@tags,@categories,@owner)", con);
            cmd.Parameters.AddWithValue("@name", recipe.Name);
            cmd.Parameters.AddWithValue("@season", recipe.Season);
            cmd.Parameters.AddWithValue("@meal", recipe.Meal);
            cmd.Parameters.AddWithValue("@time", recipe.Time);
            string tag = "";
            for (int z = 0; z < recipe.Tags.Count; z++)
            {
                tag += recipe.Tags[z].ToString() + ",";
            }
            cmd.Parameters.AddWithValue("@tags", tag); tag = "";
            string category = "";
            for (int y = 0; y < recipe.categories.Count; y++)
            {
                category += recipe.categories[y].ToString() + ",";
            }
            cmd.Parameters.AddWithValue("@categories", category); category = "";
            cmd.Parameters.AddWithValue("@owner", user.username);
            cmd.ExecuteNonQuery();
            con.Close(); con.Open();

            string id = "";
            SqlCommand cmd3 = new SqlCommand("SELECT * FROM FoodRecipe Where Owner='" + user.username + "' and Name='" + recipe.Name + "'", con);
            SqlDataReader dr = cmd3.ExecuteReader();
            if (dr.Read())
            {
                id = dr["ID"].ToString();
            }
            dr.Close();
            con.Close(); */
            /*string id = recipe.get_ID();
            for (int i = 0; i < recipe.Steps.Count; i++)
            {
                con.Open();
                SqlCommand cmd2 = new SqlCommand("INSERT INTO Steps values (@id,@rName,@number,@step)", con);
                cmd2.Parameters.AddWithValue("@id", id);
                cmd2.Parameters.AddWithValue("@rName", recipe.Name);
                cmd2.Parameters.AddWithValue("@number", i + 1);
                cmd2.Parameters.AddWithValue("@step", recipe.Steps[i]);
                cmd2.ExecuteNonQuery();
                con.Close();
            }
            
            for (int x = 0; x < recipe.ingredients.Count; x++)
            {
                Ingredients ing = (Ingredients)recipe.ingredients[x];

                ing.Recipe_Add_Ingredients(id);*/
            /*con.Open();
            SqlCommand cmd4 = new SqlCommand("INSERT INTO Recipe_Ingredients values (@rID,@name,@amount,@type)", con);
            cmd4.Parameters.AddWithValue("@rID", id);
            cmd4.Parameters.AddWithValue("@name", ing.name);
            cmd4.Parameters.AddWithValue("@amount", ing.amount);
            cmd4.Parameters.AddWithValue("@type", ing.type);
            cmd4.ExecuteNonQuery();
            con.Close(); */
            //}


            MessageBox.Show("Successfull");

            if(user.username != "admin")
            {
                Library lbr = new Library();
                lbr.user = user;
                lbr.theme = theme;
                this.Close();
                lbr.Show();
            }
            else if (user.username == "admin")
            {
                adminlibrary al = new adminlibrary();
                al.user = user;
                this.Close();
                al.Show();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //logout
            Application.OpenForms["login"].Show();
            //login lgn = new login();
            this.Close();
            //lgn.Show();
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
