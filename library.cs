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
    public partial class Library : Form
    {
        public Library()
        {
            InitializeComponent();
        }

        public user user;
        public Theme theme;
        ArrayList recipes, defaultRecipes;
        ArrayList allrecipes;
        string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";

        private void fill_theme()
        {
            Color bgc = theme.BackgroundColor, pc = theme.PanelColor, bc = theme.BtnColor;

            this.BackColor = bgc;
            button1.BackColor = bc;
            button2.BackColor = bc;
            button3.BackColor = bc;
        }

        private void library_Load(object sender, EventArgs e)
        {
            fill_theme();
            lbl_welcome.Text = "Welcome " + user.username;

            recipes = new ArrayList();
            defaultRecipes = new ArrayList();
            allrecipes = new ArrayList();

            for (int i = 0; i < user.RecipeLibrary.Count; i++)
            {
                recipes.Add(user.RecipeLibrary[i]);
                allrecipes.Add(user.RecipeLibrary[i]);
            }

            user admin = new user();
            admin.get_user_Informations("admin");

            for (int i = 0; i < admin.RecipeLibrary.Count; i++)
            {
                defaultRecipes.Add(admin.RecipeLibrary[i]);
                allrecipes.Add(admin.RecipeLibrary[i]);
            }

            comboBox1.SelectedIndex = 0;

            /*SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand cmd = new SqlCommand("Select*from users where username='admin'", con);
            SqlDataReader dr = cmd.ExecuteReader();*/

            /*SqlCommand cmd = new SqlCommand("SELECT* FROM FoodRecipe Where Owner='admin'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                FoodRecipe recipe = new FoodRecipe();
                recipe.id = dr["ID"].ToString();
                recipe.Name = dr["Name"].ToString();
                recipe.Season = dr["Season"].ToString();
                recipe.Meal = dr["Meal"].ToString();
                recipe.Time = dr["Time"].ToString();

                fill_steps(recipe);
                fill_ingredients(recipe);

                recipe.Tags = new ArrayList();
                recipe.categories = new ArrayList();

                string tag = dr["Tags"].ToString();
                string line = "";
                for (int i = 0; i < tag.Length; i++)
                {
                    if (tag[i].ToString() == ",")
                    {
                        recipe.Tags.Add(line);
                        line = "";
                    }
                    else
                    {
                        line += tag[i];
                    }
                }
                string categories = dr["Categories"].ToString();
                string line2 = "";
                for (int y = 0; y < categories.Length; y++)
                {
                    if (categories[y].ToString() == ",")
                    {
                        recipe.Tags.Add(line2);
                        line2 = "";
                    }
                    else
                    {
                        line2 += categories[y];
                    }
                }

                recipe.owner = admin();
                //recipe.owner = null;
                defaultRecipes.Add(recipe);
            }
            con.Close();*/

            //fill_lb1();
        }

        private void fill_steps(FoodRecipe r)
        {
            r.Steps = new ArrayList();
            SqlConnection con2 = new SqlConnection(connectionString);
            con2.Open();
            SqlCommand cmd3 = new SqlCommand("SELECT* FROM Steps Where RecipeName='" + r.Name + "'", con2);
            SqlDataReader dr4 = cmd3.ExecuteReader();
            while (dr4.Read())
            {
                r.Steps.Add(dr4["Step"].ToString());
            }
            dr4.Close();
            con2.Close();
        }

        private void fill_ingredients(FoodRecipe r)
        {
            r.ingredients = new ArrayList();
            SqlConnection con2 = new SqlConnection(connectionString);
            con2.Open();
            SqlCommand cmd3 = new SqlCommand("SELECT* FROM Recipe_Ingredients Where RecipeID='" + r.id + "'", con2);
            SqlDataReader dr4 = cmd3.ExecuteReader();
            while (dr4.Read())
            {
                Ingredients ing = new Ingredients();
                ing.name = dr4["Name"].ToString();
                ing.amount = dr4["Amount"].ToString();
                string yedek = "", veri = dr4["Type"].ToString();
                for (int i = 0; i < veri.Length; i++)
                {
                    if (veri[i].ToString() == " ")
                    {
                        break;
                    }
                    else
                        yedek += veri[i];
                }
                ing.type = yedek;
                r.ingredients.Add(ing);
            }
            dr4.Close();
            con2.Close();
        }

        private user admin()
        {
            user u = new user();
            SqlConnection con2 = new SqlConnection(connectionString);
            con2.Open();
            SqlCommand cmd3 = new SqlCommand("SELECT*FROM users Where username='admin'", con2);
            SqlDataReader dr4 = cmd3.ExecuteReader();
            if(dr4.Read())
            {
                u = send(dr4);
            }
            dr4.Close();
            con2.Close();

            return u;
        }

        private user send(SqlDataReader dr)
        {
            user u = new user();
            u.id = dr["ID"].ToString();
            u.username = dr["username"].ToString();
            u.password = dr["password"].ToString();
            u.country = dr["country"].ToString();
            u.phone = dr["phonenumber"].ToString();
            u.realname = dr["realname"].ToString();
            u.email = dr["mail"].ToString();
            u.Cellar = new ArrayList();
            u.RecipeLibrary = new ArrayList();
            u.mails = new ArrayList();

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("Select*from User_Ingredients where UserID='" + u.id + "'", con);
            SqlDataReader dr2 = cmd.ExecuteReader();
            while (dr2.Read())
            {
                Ingredients ing = new Ingredients();
                ing.name = dr2["Name"].ToString();
                ing.amount = dr2["Amount"].ToString();
                ing.type = dr2["Type"].ToString();
                u.Cellar.Add(ing);
            }
            dr2.Close(); con.Close();

            con.Open();
            SqlCommand cmd2 = new SqlCommand("SELECT* FROM FoodRecipe Where Owner='" + u.username + "'", con);
            SqlDataReader dr3 = cmd2.ExecuteReader();
            while (dr3.Read())
            {
                FoodRecipe recipe = new FoodRecipe();
                recipe.id = dr3["ID"].ToString();
                recipe.Name = dr3["Name"].ToString();
                recipe.Season = dr3["Season"].ToString();
                recipe.Meal = dr3["Meal"].ToString();
                recipe.Time = dr3["Time"].ToString();

                fill_steps(recipe);
                fill_ingredients(recipe);

                recipe.categories = new ArrayList();
                recipe.Tags = new ArrayList();
                string tag = dr3["Tags"].ToString();
                string line = "";
                for (int i = 0; i < tag.Length; i++)
                {
                    if (tag[i].ToString() == ",")
                    {
                        recipe.Tags.Add(line);
                        line = "";
                    }
                    else
                    {
                        line += tag[i];
                    }
                }
                string categories = dr3["Categories"].ToString();
                string line2 = "";
                for (int y = 0; y < categories.Length; y++)
                {
                    if (categories[y].ToString() == ",")
                    {
                        recipe.Tags.Add(line2);
                        line2 = "";
                    }
                    else
                    {
                        line2 += categories[y];
                    }
                }
                recipe.owner = u.username;
                u.RecipeLibrary.Add(recipe);
            }

            dr3.Close();

            SqlCommand cmd4 = new SqlCommand("SELECT * FROM Theme Where Owner='" + u.username + "'", con);
            SqlDataReader dr5 = cmd4.ExecuteReader();
            if (dr5.Read())
            {
                string clr1 = dr5["BackgroundColor"].ToString();
                Color c1 = Color.FromArgb(Convert.ToInt32(clr1));
                theme.BackgroundColor = c1;

                string clr2 = dr5["PanelColor"].ToString();
                Color c2 = Color.FromArgb(Convert.ToInt32(clr2));
                theme.PanelColor = c2;

                string clr3 = dr5["ButtonColor"].ToString();
                Color c3 = Color.FromArgb(Convert.ToInt32(clr3));
                theme.BtnColor = c3;
            }
            else
            {
                /*SqlCommand cmd4 = new SqlCommand("Select * from Theme where Owner='default'", con);
                SqlDataReader dr5 = cmd.ExecuteReader();

                string clr1 = dr["BackgroundColor"].ToString();
                Color c1 = Color.FromArgb(Convert.ToInt32(clr1));
                theme.BackgroundColor = c1;

                string clr2 = dr["PanelColor"].ToString();
                Color c2 = Color.FromArgb(Convert.ToInt32(clr2));
                theme.PanelColor = c2;

                string clr3 = dr["ButtonColor"].ToString();
                Color c3 = Color.FromArgb(Convert.ToInt32(clr3));
                theme.BtnColor = c3;

                dr5.Close();*/

                string clr1 = "-4599318";
                Color c1 = Color.FromArgb(Convert.ToInt32(clr1));
                theme.BackgroundColor = c1;

                string clr2 = "-4599318";
                Color c2 = Color.FromArgb(Convert.ToInt32(clr2));
                theme.PanelColor = c2;

                string clr3 = "-986896";
                Color c3 = Color.FromArgb(Convert.ToInt32(clr3));
                theme.BtnColor = c3;
            }

            dr5.Close();
            con.Close();
            return u;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CreateRecipe cr = new CreateRecipe();
            cr.user = this.user;
            cr.theme = theme;
            this.Close();
            cr.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //delete
            string name = listBox1.Items[listBox1.SelectedIndex].ToString();
            for (int i = 0; i < recipes.Count; i++)
            {
                FoodRecipe rcp = (FoodRecipe)recipes[i];
                if(name == rcp.Name)
                {
                    rcp.delete();
                    recipes.RemoveAt(i);
                    user.RecipeLibrary.RemoveAt(i);

                    /*SqlConnection con = new SqlConnection(connectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM FoodRecipe WHERE Name='" + rcp.Name + "' AND Owner='" + user.username + "'", con);
                    cmd.ExecuteReader();
                    con.Close();*/

                    MessageBox.Show("Successfully deleted");

                    break;
                }
            }
            fill_lb1();
        }

        private void fill_lb1()
        {
            listBox1.Items.Clear();
            for (int y = 0; y < recipes.Count; y++)
            {
                FoodRecipe rcp = (FoodRecipe)recipes[y];
                listBox1.Items.Add(rcp.Name);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string name = listBox1.Items[listBox1.SelectedIndex].ToString();
                FoodRecipe rcp;
                for (int i = 0; i < allrecipes.Count; i++)
                {
                    rcp = (FoodRecipe)allrecipes[i];
                    if (name == rcp.Name)
                    {
                        listBox3.Items.Clear();
                        Ingredients ing;
                        listBox3.Items.Add("MAIN INGREDIENTS");
                        for (int a = 0; a < rcp.ingredients.Count; a++)
                        {
                            ing = (Ingredients)rcp.ingredients[a];
                            if(ing.type == "Main")
                                listBox3.Items.Add(ing.amount + " " + ing.name);
                        }

                        listBox3.Items.Add("EXTRA INGREDIENTS");
                        for (int b = 0; b < rcp.ingredients.Count; b++)
                        {
                            ing = (Ingredients)rcp.ingredients[b];
                            if (ing.type == "Extra")
                                listBox3.Items.Add(ing.amount + " " + ing.name);
                        }

                        listBox3.Items.Add("STEPS");
                        for (int y = 0; y < rcp.Steps.Count; y++)
                        {
                            listBox3.Items.Add(rcp.Steps[y].ToString());
                        }
                        label5.Text = "Time(min) : " + rcp.Time;
                        label6.Text = "Name : " + rcp.Name;

                        break;
                    }
                }
            }
            catch
            {

            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //logout
            //log-off
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pick = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            if(pick == "All")
            {
                fill_lb1();
                fill_default();
            }
            else if(pick=="Your")
            {
                fill_lb1();
                button1.Enabled = true;
            }
            else if(pick == "Default")
            {
                listBox1.Items.Clear();
                fill_default();
            }
        }

        private void fill_default()
        {
            button1.Enabled = false;
            for (int i = 0; i < defaultRecipes.Count; i++)
            {
                FoodRecipe rcp = (FoodRecipe)defaultRecipes[i];
                listBox1.Items.Add(rcp.Name);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            mainmenu menu = new mainmenu();
            menu.u = user;
            menu.theme = theme;
            this.Close();
            menu.Show();
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
