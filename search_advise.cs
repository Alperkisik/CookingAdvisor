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
    public partial class search_advise : Form
    {
        public search_advise()
        {
            InitializeComponent();
        }

        public user user;
        public Theme theme;
        ArrayList recipes;
        public string mode = "online";
        string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";

        private void fill_theme()
        {
            Color bgc = theme.BackgroundColor, pc = theme.PanelColor,bc = theme.BtnColor;

            this.BackColor = bgc;

            panel1.BackColor = pc;

            button1.BackColor = bc;
            button2.BackColor = bc;
            button3.BackColor = bc;
            button4.BackColor = bc;
        }

        private void search_advise_Load(object sender, EventArgs e)
        {
            fill_theme();
            lbl_welcome.Text = "Welcome " + user.username;
            recipes = new ArrayList();

            if(mode == "offline")
            {
                button1.Enabled = false;
            }
            else
            {
                for (int i = 0; i < user.RecipeLibrary.Count; i++)
                {
                    recipes.Add(user.RecipeLibrary[i]);
                }
            }
                

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT* FROM FoodRecipe Where Owner='admin'", con);
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

                recipe.categories = new ArrayList();
                recipe.Tags = new ArrayList();

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
                recipe.owner = admin().username;
                recipes.Add(recipe);
            }
            con.Close();
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
            if (dr4.Read())
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

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            /*ArrayList check = new ArrayList();
            int Mcount = 0;
            FoodRecipe rcp;

            for (int i = 0; i < recipes.Count; i++)
            {
                rcp = (FoodRecipe)recipes[i];
                Mcount = 0;
                for (int t = 0; t < user.Cellar.Count; t++)
                {
                    Ingredients Uing = (Ingredients)user.Cellar[t];
                    for (int x = 0; x < rcp.ingredients.Count; x++)
                    {
                        Ingredients Ring = (Ingredients)rcp.ingredients[x];
                        if (Ring.name.ToLower() == Uing.name.ToLower())
                        {
                            if(Ring.type == "Main")
                            {
                                Mcount++;
                                break;
                            }
                        }
                    }
                }
                if (Mcount > 0)
                    check.Add(rcp.Name + "," + Mcount.ToString());
            }

            ArrayList number = new ArrayList();
            ArrayList name = new ArrayList();
            string na = "", nu = ""; int line = 0;
            for (int i = 0; i < check.Count; i++)
            {
                string hold = check[i].ToString();
                na = "";
                for (int y = 0; y < hold.Length; y++)
                {
                    if (hold[y].ToString() == ",")
                    {
                        line = y+1;
                        break;
                    }
                    else
                        na += hold[y];
                }

                nu = "";
                for (int x = line; x < hold.Length; x++)
                {
                    nu += hold[x];
                }

                name.Add(na);
                number.Add(nu);
            }
            int num;
            int say1=0, say2=0;
            string n;
            for (int y = 0; y < number.Count; y++)
            {
                for (int j = 0; j < number.Count; j++)
                {
                    say1 = Convert.ToInt32(number[y]);
                    say2 = Convert.ToInt32(number[j]);
                    if(say1 > say2)
                    {
                        num = say2;
                        n = name[j].ToString();
                        number[j] = number[y];
                        name[j] = name[y];
                        number[y] = num.ToString();
                        name[y] = n;
                    }
                }
            }

            //ayıklama kodları
            if(checkBox1.Checked)
            {
                string tc = textBox2.Text;
                string season = "", meal = "";
                try
                {
                    season = comboBox1.Items[comboBox1.SelectedIndex].ToString();
                }
                catch
                {
                    season = "";
                }
                try
                {
                    meal = comboBox2.Items[comboBox2.SelectedIndex].ToString();
                }
                catch
                {
                    meal = "";
                }


                for (int i = 0; i < name.Count; i++)
                {
                    FoodRecipe frcp = new FoodRecipe();
                    for (int y = 0; y < recipes.Count; y++)
                    {
                        frcp = (FoodRecipe)recipes[y];
                        if (frcp.Name == name[i])
                            break;
                    }

                    if (tc != "")
                    {
                        bool found1 = false,found2 = false;
                        for (int a = 0; a < frcp.Tags.Count; a++)
                        {
                            string tag = frcp.Tags[a].ToString();
                            if(tc == tag)
                            {
                                found1 = true;
                                break;
                            }
                        }

                        for (int b = 0; b < frcp.categories.Count; b++)
                        {
                            string ctg = frcp.categories[b].ToString();
                            if (tc == ctg)
                            {
                                found2 = true;
                                break;
                            }
                        }

                        if (found1 == false && found2 == false)
                        {
                            name.RemoveAt(i);
                            number.RemoveAt(i);
                        }
                    }
                    
                    if (season != "")
                    {
                        if(frcp.Season != season)
                        {
                            name.RemoveAt(i);
                            number.RemoveAt(i);
                        }
                    }
                    
                    if (meal != "")
                    {
                        if(meal != frcp.Meal)
                        {
                            name.RemoveAt(i);
                            number.RemoveAt(i);
                        }
                    }
                }
                
            }*/
            string tc = textBox2.Text;
            string season = "", meal = "";
            try
            {
                season = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            }
            catch
            {
                season = "";
            }
            try
            {
                meal = comboBox2.Items[comboBox2.SelectedIndex].ToString();
            }
            catch
            {
                meal = "";
            }

            ArrayList result = user.take_advice(checkBox1.Checked,tc,season,meal,recipes);
            if (result.Count == 0)
            {
                MessageBox.Show("There is no search result", "Noification");
            }
            else
            {
                /*for (int i = 0; i < name.Count; i++)
                {
                    listBox1.Items.Add(name[i].ToString() + "-" + number[i].ToString() + " ingredients matched");
                }*/
                for (int i = 0; i < result.Count; i++)
                {
                    listBox1.Items.Add(result[i].ToString());
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string text = listBox1.Items[listBox1.SelectedIndex].ToString();
                string name = "";
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i].ToString() == "-")
                        break;
                    else
                        name += text[i];
                }
                FoodRecipe rcp = new FoodRecipe();
                for (int i = 0; i < recipes.Count; i++)
                {
                    rcp = (FoodRecipe)recipes[i];
                    if(name == rcp.Name)
                    {
                        listBox2.Items.Clear();
                        listBox2.Items.Add("MAIN INGREDIENTS");
                        for (int a = 0; a < rcp.ingredients.Count; a++)
                        {
                            Ingredients ing = (Ingredients)rcp.ingredients[a];
                            if(ing.type == "Main")
                                listBox2.Items.Add(ing.amount + " " + ing.name);
                        }

                        listBox2.Items.Add("EXTRA INGREDIENTS");
                        for (int b = 0; b < rcp.ingredients.Count; b++)
                        {
                            Ingredients ing = (Ingredients)rcp.ingredients[b];
                            if (ing.type == "Extra")
                                listBox2.Items.Add(ing.amount + " " + ing.name);
                        }

                        listBox2.Items.Add("STEPS");
                        for (int y = 0; y < rcp.Steps.Count; y++)
                        {
                            listBox2.Items.Add(y+1 +". Step : " + rcp.Steps[y].ToString());
                        }
                        label2.Text = "Time(min) : " + rcp.Time;

                        break;
                    }
                }
            }
            catch
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //search
            //tüm listeden sile sile gidicez

            listBox1.Items.Clear();
            /*ArrayList searchList = new ArrayList();
            for (int i = 0; i < recipes.Count; i++)
            {
                searchList.Add(recipes[i]);
            }*/

            string name = textBox1.Text;
            string tag = textBox2.Text;
            string season = "", meal = "";
            try
            {
                season = comboBox1.SelectedItem.ToString();
            }
            catch
            {
                season = "";
            }
            try
            {
                meal = comboBox2.SelectedItem.ToString();
            }
            catch
            {
                meal = "";
            }

            /*FoodRecipe rcp = new FoodRecipe();
            for (int i = 0; i < searchList.Count; i++)
            {
                //name 
                rcp = (FoodRecipe)searchList[i];
                if(name != "")
                {
                    if(name.ToLower() != rcp.Name.ToLower())
                    {
                        searchList.RemoveAt(i);
                        i--;
                        continue;
                    }
                }

                if(tag != "")
                {
                    bool var1 = false, var2 = false;
                    for (int x = 0; x < rcp.Tags.Count; x++)
                    {
                        if(rcp.Tags[x].ToString() == tag)
                        {
                            var1 = true;
                            break;
                        }
                    }
                    for (int y = 0; y < rcp.categories.Count; y++)
                    {
                        if (rcp.categories[y].ToString() == tag)
                        {
                            var2 = true;
                            break;
                        }
                    }

                    if(var1 == false && var2 == false)
                    {
                        searchList.RemoveAt(i);
                        i--;
                        continue;
                    }
                }

                if(season != "")
                {
                    if(season != rcp.Season)
                    {
                        searchList.RemoveAt(i);
                        i--;
                        continue;
                    }
                }

                if(meal != "")
                {
                    if (meal != rcp.Meal)
                    {
                        searchList.RemoveAt(i);
                        i--;
                        continue;
                    }
                }
            }*/
            listBox1.Items.Clear();

            ArrayList result = new ArrayList();
            result = user.search(name, tag, season, meal, recipes);

            if (result.Count == 0)
            {
                MessageBox.Show("There is no search result", "Noification");
            }
            else
            {
                for (int a = 0; a < result.Count; a++)
                {
                    FoodRecipe rcp = (FoodRecipe)result[a];
                    listBox1.Items.Add(rcp.Name);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //reset
            listBox1.Items.Clear(); listBox2.Items.Clear();
            textBox1.Text = ""; textBox2.Text = "";
            comboBox1.SelectedItem = null;
            comboBox2.SelectedItem = null;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            mainmenu menu = new mainmenu();
            menu.u = user;
            menu.theme = theme;
            this.Close();
            menu.Show();
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
