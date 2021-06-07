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
using System.Collections;

namespace CookingAdvisor
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        public Theme theme;
        private void button1_Click(object sender, EventArgs e)
        {
            //login
            user user = new user();
            string name = textBox1.Text, password = textBox2.Text;
            string status = user.login(name, password);
            if(status == "login")
            {
                if (name == "admin" && password == "1234")
                {
                    adminpanel admin = new adminpanel();
                    admin.user = user;
                    //admin.user = send(dr);
                    clean();
                    this.Hide();
                    admin.ShowDialog();
                }
                else
                {
                    mainmenu menu = new mainmenu();
                    menu.mode = "online";
                    menu.u = user;
                    //menu.u = send(dr);
                    menu.theme = theme;
                    clean();
                    this.Hide();
                    menu.ShowDialog();
                }
            }
            else if(status == "username")
            {
                MessageBox.Show("Username is wrong.","Alert");
            }
            else if(status == "password")
            {
                MessageBox.Show("Password is wrong.", "Alert");
            }

            #region bloke kodlar
            /* SqlConnection con = new SqlConnection(connectionString);
             con.Open();

             SqlCommand cmd = new SqlCommand("Select password from users where username='" + name + "'", con);
             SqlDataReader dr = cmd.ExecuteReader();
             if (dr.Read())
             {
                 string p = dr["password"].ToString();
                 int l = p.Length;
                 string new_p = "";
                 for (int i = 0; i < l-2; i++)
                 {
                     if (p[i].ToString() == " ")
                         break;
                     else
                         new_p += p[i];
                 }
                 dr.Close();
                 if (password == new_p)
                 {
                     cmd = new SqlCommand("Select*from users where username='" + name + "' and password='" + password + "'", con);
                     dr = cmd.ExecuteReader();
                     if (dr.Read())
                     {
                         user.get_user_Informations(name);
                         if (name == "admin" && password == "1234")
                         {
                             adminpanel admin = new adminpanel();
                             admin.user = user;
                             //admin.user = send(dr);
                             clean();
                             this.Hide();
                             admin.ShowDialog();
                         }
                         else
                         {
                             mainmenu menu = new mainmenu();
                             menu.mode = "online";
                             menu.u = user;
                             //menu.u = send(dr);
                             menu.theme = theme;
                             con.Close();
                             clean();
                             this.Hide();
                             menu.ShowDialog();
                         }
                     }
                     else
                     {
                         MessageBox.Show("Informations are incorrect.","Alert");
                     }
                     con.Close();
                 }
                 else
                 {
                     MessageBox.Show("Password is wrong.","Alert");
                 }
             }
             else
             {
                 MessageBox.Show("Username is not found.", "Alert");
             }*/
            #endregion
        }

        #region bloke fonksiyonlar
        /*private user send(SqlDataReader dr)
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
            SqlCommand cmd2 = new SqlCommand("SELECT* FROM FoodRecipe Where Owner='"+ u.username+"'", con);
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

            dr3.Close(); con.Close(); con.Open();

            SqlCommand cmd4 = new SqlCommand("SELECT * FROM Theme Where Owner='" + u.username + "'", con);
            SqlDataReader dr5 = cmd4.ExecuteReader();
            if(dr5.Read())
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
                string yedek = "",veri = dr4["Type"].ToString(); 
                for (int i = 0; i < veri.Length; i++)
                {
                    if(veri[i].ToString() == " ")
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
        }*/
        #endregion

        private void login_Load(object sender, EventArgs e)
        {

            theme = new Theme();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            register rgt = new register(); 
            clean();
            this.Hide();
            rgt.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //offline mode
            theme = new Theme();
            string clr1 = "-4599318";
            Color c1 = Color.FromArgb(Convert.ToInt32(clr1));
            theme.BackgroundColor = c1;

            string clr2 = "-4599318";
            Color c2 = Color.FromArgb(Convert.ToInt32(clr2));
            theme.PanelColor = c2;

            string clr3 = "-986896";
            Color c3 = Color.FromArgb(Convert.ToInt32(clr3));
            theme.BtnColor = c3;

            mainmenu menu = new mainmenu();
            menu.mode = "offline";
            menu.theme = theme;
            clean();
            this.Hide();
            menu.ShowDialog();
        }

        public void clean()
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        Color keep1;
        private void button2_MouseEnter(object sender, EventArgs e)
        {
            keep1 = button2.BackColor;
            button2.BackColor = Color.Yellow;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackColor = keep1;
        }

        private void btn_mouseEnter(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            keep1 = btn.BackColor;
            btn.BackColor = Color.Yellow;
        }
        private void btn_mouseLeave(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackColor =  keep1;
        }

        private void login_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Helper hl = new Helper();
            this.Hide();
            hl.ShowDialog();
        }
    }
}
