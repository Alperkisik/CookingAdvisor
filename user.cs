using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace CookingAdvisor
{
    public class user
    {
        public string id, username, password, email, phone, country, realname;
        public ArrayList RecipeLibrary, Cellar, mails;
        public Theme theme;

        public string login(string uUserName, string pPassword)
        {
            //string connectionString = "Data Source=192.168.1.80; Network Library=DBMSSOCN;Initial Catalog=CookingAdvisor;User ID=Alper; Password=capofrohan2009;";
            string status = "nothing";
            string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand cmd = new SqlCommand("Select password from users where username='" + uUserName + "'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                string p = dr["password"].ToString();
                int l = p.Length;
                string new_p = "";
                for (int i = 0; i < l - 2; i++)
                {
                    if (p[i].ToString() == " ")
                        break;
                    else
                        new_p += p[i];
                }

                dr.Close(); con.Close();
                get_user_Informations(uUserName);
                if (pPassword == new_p)
                {
                    status = "login";
                    /*con.Open();
                    SqlCommand cmd2 = new SqlCommand("Select*from users where username='" + username + "'", con);
                    SqlDataReader dr2 = cmd2.ExecuteReader();
                    if (dr2.Read())
                    {
                        get_user_Informations(username);
                        status = "yes";
                    }
                    else
                    {
                        status = "no";
                    }
                    con.Close();*/
                }
                else
                {
                    status = "password";
                }
            }
            else
            {
                status = "username";
            }
            return status;
        }

        public void get_user_Informations(string uUserName)
        {
            string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand cmd = new SqlCommand("Select*from users where username='" + uUserName + "'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                id = dr["ID"].ToString();
                username = dr["username"].ToString();
                password = dr["password"].ToString();
                country = dr["country"].ToString();
                phone = dr["phonenumber"].ToString();
                realname = dr["realname"].ToString();
                email = dr["mail"].ToString();
                Cellar = new ArrayList();
                RecipeLibrary = new ArrayList();
                mails = new ArrayList();

                Cellar = get_user_Ingredients(id);
                RecipeLibrary = get_user_recipes(username);
                theme = get_user_theme(username);
            }
            //user u = new user();

            dr.Close();
            con.Close();
            //return u;
        }

        private ArrayList get_user_Ingredients(string userid)
        {
            ArrayList ingredients = new ArrayList();
            string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("Select*from User_Ingredients where UserID='" + userid + "'", con);
            SqlDataReader dr2 = cmd.ExecuteReader();
            while (dr2.Read())
            {
                Ingredients ing = new Ingredients();
                ing.name = dr2["Name"].ToString();
                ing.amount = dr2["Amount"].ToString();
                ing.type = dr2["Type"].ToString();
                ingredients.Add(ing);
            }
            dr2.Close();
            con.Close();

            return ingredients;
        }

        private Theme get_user_theme(string username)
        {
            Theme theme = new Theme();
            string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand cmd4 = new SqlCommand("SELECT * FROM Theme Where Owner='" + username + "'", con);
            SqlDataReader dr5 = cmd4.ExecuteReader();

            if (dr5.Read())
            {
                string clr1 = dr5["BackgroundColor"].ToString();
                Color c1 = Color.FromArgb(Convert.ToInt32(clr1));
                //theme.BackgroundColor = c1;

                string clr2 = dr5["PanelColor"].ToString();
                Color c2 = Color.FromArgb(Convert.ToInt32(clr2));
                //theme.PanelColor = c2;

                string clr3 = dr5["ButtonColor"].ToString();
                Color c3 = Color.FromArgb(Convert.ToInt32(clr3));

                theme.Set_Colors(c1, c2, c3);
                //theme.BtnColor = c3;
            }
            else
            {
                string clr1 = "-4599318";
                Color c1 = Color.FromArgb(Convert.ToInt32(clr1));
                //theme.BackgroundColor = c1;

                string clr2 = "-4599318";
                Color c2 = Color.FromArgb(Convert.ToInt32(clr2));
                //theme.PanelColor = c2;

                string clr3 = "-986896";
                Color c3 = Color.FromArgb(Convert.ToInt32(clr3));
                //theme.BtnColor = c3;
                theme.Set_Colors(c1, c2, c3);
            }

            dr5.Close();
            con.Close();

            return theme;
        }

        private ArrayList get_user_recipes(string username)
        {
            ArrayList recipes = new ArrayList();
            string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd2 = new SqlCommand("SELECT* FROM FoodRecipe Where Owner='" + username + "'", con);
            SqlDataReader dr3 = cmd2.ExecuteReader();
            while (dr3.Read())
            {
                FoodRecipe recipe = new FoodRecipe();
                recipe.id = dr3["ID"].ToString();
                recipe.Name = dr3["Name"].ToString();
                recipe.Season = dr3["Season"].ToString();
                recipe.Meal = dr3["Meal"].ToString();
                recipe.Time = dr3["Time"].ToString();

                recipe.fill_steps();//recipe);
                recipe.fill_ingredients();//recipe);

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
                recipe.owner = username;
                recipes.Add(recipe);
            }

            dr3.Close(); con.Close();
            return recipes;
        }

        public void register(string Uname, string password, string mail, string country, string phone, string Rname)
        {
            string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";
            SqlConnection con = new SqlConnection(connectionString);
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

                MessageBox.Show("Register successfull");
            }

            dr.Close();
            con.Close();
        }

        public void update_user(string Uname, string password, string mail, string country, string phone, string Rname)
        {
            string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand cmd = new SqlCommand("UPDATE users SET password=@p,mail=@mail,country=@c,phonenumber=@phone,realname=@Rn WHERE username=@Un", con);
            cmd.Parameters.AddWithValue("@Un", Uname);
            cmd.Parameters.AddWithValue("@p", password);
            cmd.Parameters.AddWithValue("@mail", mail);
            cmd.Parameters.AddWithValue("@c", country);
            cmd.Parameters.AddWithValue("@phone", phone);
            cmd.Parameters.AddWithValue("@Rn", Rname);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void change_theme(Color c1, Color c2, Color c3)
        {
            theme.Set_Colors(c1, c2, c3);
            theme.Set_theme(username);

            /*string clr1, clr2, clr3;
            SqlCommand cmd = new SqlCommand("Select * from Theme where Owner='" + username + "'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                dr.Close(); con.Close(); con.Open();
                cmd = new SqlCommand("UPDATE Theme SET BackgroundColor=@bgc,PanelColor=@pc,ButtonColor=@bc WHERE Owner=@name", con);
                cmd.Parameters.AddWithValue("@name", username);
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
                cmd.Parameters.AddWithValue("@owner", username);
                clr1 = theme.BackgroundColor.ToArgb().ToString();
                cmd.Parameters.AddWithValue("@bgc", clr1);
                clr2 = theme.PanelColor.ToArgb().ToString();
                cmd.Parameters.AddWithValue("@pc", clr2);
                clr3 = theme.BtnColor.ToArgb().ToString();
                cmd.Parameters.AddWithValue("@bc", clr3);

                cmd.ExecuteNonQuery();
            }

            con.Close();*/
        }

        public void update(string name, string pass, string m, string ca, string p, string Rname)
        {
            string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand cmd = new SqlCommand("UPDATE users SET password=@p,mail=@mail,country=@c,phonenumber=@phone,realname=@Rn WHERE username=@Un", con);
            cmd.Parameters.AddWithValue("@Un", name);
            cmd.Parameters.AddWithValue("@p", pass);
            cmd.Parameters.AddWithValue("@mail", m);
            cmd.Parameters.AddWithValue("@c", ca);
            cmd.Parameters.AddWithValue("@phone", p);
            cmd.Parameters.AddWithValue("@Rn", Rname);
            cmd.ExecuteNonQuery();
            con.Close();

            password = pass;
            email = m;
            country = ca;
            phone = p;
            realname = Rname;

            MessageBox.Show("Update Successfull", "Notification");
        }

        public ArrayList search(string name, string tag, string season, string meal, ArrayList recipes)
        {
            ArrayList SearchResult = new ArrayList();

            for (int i = 0; i < recipes.Count; i++)
            {
                SearchResult.Add(recipes[i]);
            }

            FoodRecipe rcp = new FoodRecipe();
            for (int i = 0; i < SearchResult.Count; i++)
            {
                //name 
                rcp = (FoodRecipe)SearchResult[i];
                if (name != "")
                {
                    if (name.ToLower() != rcp.Name.ToLower())
                    {
                        SearchResult.RemoveAt(i);
                        i--;
                        continue;
                    }
                }

                if (tag != "")
                {
                    bool var1 = false, var2 = false;
                    for (int x = 0; x < rcp.Tags.Count; x++)
                    {
                        if (rcp.Tags[x].ToString() == tag)
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

                    if (var1 == false && var2 == false)
                    {
                        SearchResult.RemoveAt(i);
                        i--;
                        continue;
                    }
                }

                if (season != "")
                {
                    if (season != rcp.Season)
                    {
                        SearchResult.RemoveAt(i);
                        i--;
                        continue;
                    }
                }

                if (meal != "")
                {
                    if (meal != rcp.Meal)
                    {
                        SearchResult.RemoveAt(i);
                        i--;
                        continue;
                    }
                }
            }

            return SearchResult;
        }

        public ArrayList take_advice(bool checkbox, string tc, string season, string meal, ArrayList recipes)
        {
            ArrayList adviceResult = new ArrayList();

            ArrayList check = new ArrayList();
            int Mcount = 0;
            FoodRecipe rcp;

            for (int i = 0; i < recipes.Count; i++)
            {
                rcp = (FoodRecipe)recipes[i];
                Mcount = 0;
                for (int t = 0; t < Cellar.Count; t++)
                {
                    Ingredients Uing = (Ingredients)Cellar[t];
                    for (int x = 0; x < rcp.ingredients.Count; x++)
                    {
                        Ingredients Ring = (Ingredients)rcp.ingredients[x];
                        if (Ring.name.ToLower() == Uing.name.ToLower())
                        {
                            if (Ring.type == "Main")
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
                        line = y + 1;
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
            int say1 = 0, say2 = 0;
            string n;
            for (int y = 0; y < number.Count; y++)
            {
                for (int j = 0; j < number.Count; j++)
                {
                    say1 = Convert.ToInt32(number[y]);
                    say2 = Convert.ToInt32(number[j]);
                    if (say1 > say2)
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
            if (checkbox == true)
            {
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
                        bool found1 = false, found2 = false;
                        for (int a = 0; a < frcp.Tags.Count; a++)
                        {
                            string tag = frcp.Tags[a].ToString();
                            if (tc == tag)
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
                        if (frcp.Season != season)
                        {
                            name.RemoveAt(i);
                            number.RemoveAt(i);
                        }
                    }

                    if (meal != "")
                    {
                        if (meal != frcp.Meal)
                        {
                            name.RemoveAt(i);
                            number.RemoveAt(i);
                        }
                    }
                }
            }

            for (int i = 0; i < name.Count; i++)
            {
                adviceResult.Add(name[i].ToString() + "-" + number[i].ToString() + " ingredients matched");
            }

            return adviceResult;
        }

        public void delete(string Uname)
        {
            string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand cmd = new SqlCommand("Select ID from users where username='" + Uname + "'", con);
            SqlDataReader dr = cmd.ExecuteReader();

            string userid = "";
            if (dr.Read())
                userid = dr["ID"].ToString();

            dr.Close();

            cmd = new SqlCommand("Select ID from FoodRecipe where Owner='" + Uname + "'", con);
            SqlDataReader dr2 = cmd.ExecuteReader();
            ArrayList recipeIDs = new ArrayList();

            while (dr2.Read())
            {
                recipeIDs.Add(dr2["ID"].ToString());
            }
            dr2.Close();

            SqlCommand cmd7 = new SqlCommand("DELETE FROM users WHERE username='" + Uname + "'", con);
            //cmd.Parameters.AddWithValue("@id", name);
            cmd7.ExecuteReader();
            con.Close(); con.Open();
            SqlCommand cmd2 = new SqlCommand("DELETE FROM FoodRecipe WHERE Owner=@id", con);
            cmd2.Parameters.AddWithValue("@id", Uname);
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
            cmd6.Parameters.AddWithValue("@id", Uname);
            cmd6.ExecuteReader();
            con.Close();
        }

        public void Create_Food_recipe(string rName,string rMeal,string rSeason,string rTime,ArrayList rIng,ArrayList rSteps)
        {
            FoodRecipe recipe = new FoodRecipe();
            recipe.Name = rName;
            recipe.Meal = rMeal;
            recipe.Season = rSeason;
            recipe.Time = rTime;
            recipe.owner = username;
            recipe.ingredients = rIng;
            recipe.Steps = rSteps;
            recipe.create();
            RecipeLibrary.Add(recipe);
        }
    }
}
