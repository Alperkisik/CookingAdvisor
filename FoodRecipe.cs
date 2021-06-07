using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingAdvisor
{
    class FoodRecipe
    {
        public string Name,Season,Meal,Time,id,owner;
        public ArrayList ingredients;
        public ArrayList Tags,categories,Steps;

        public void fill_ingredients()//FoodRecipe r)
        {
            ingredients = new ArrayList();
            string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";
            SqlConnection con2 = new SqlConnection(connectionString);
            con2.Open();
            SqlCommand cmd3 = new SqlCommand("SELECT* FROM Recipe_Ingredients Where RecipeID='" + id + "'", con2);
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
                ingredients.Add(ing);
            }
            dr4.Close();
            con2.Close();
        }

        public void fill_steps()//FoodRecipe r)
        {
            Steps = new ArrayList();
            string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";
            SqlConnection con2 = new SqlConnection(connectionString);
            con2.Open();
            SqlCommand cmd3 = new SqlCommand("SELECT* FROM Steps Where RecipeName='" + Name + "'", con2);
            SqlDataReader dr4 = cmd3.ExecuteReader();
            while (dr4.Read())
            {
                Steps.Add(dr4["Step"].ToString());
            }
            dr4.Close();
            con2.Close();
        }

        private string get_ID()
        {
            string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM FoodRecipe Where Owner='" + owner + "' and Name='" + Name + "'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                id = dr["ID"].ToString();
            }

            dr.Close();
            con.Close();

            return id;
        }

        public void create()
        {
            string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO FoodRecipe values (@name,@season,@meal,@time,@tags,@categories,@owner)", con);
            cmd.Parameters.AddWithValue("@name", Name);
            cmd.Parameters.AddWithValue("@season", Season);
            cmd.Parameters.AddWithValue("@meal", Meal);
            cmd.Parameters.AddWithValue("@time", Time);
            string tag = "";
            for (int z = 0; z < Tags.Count; z++)
            {
                tag += Tags[z].ToString() + ",";
            }
            cmd.Parameters.AddWithValue("@tags", tag); tag = "";
            string category = "";
            for (int y = 0; y < categories.Count; y++)
            {
                category += categories[y].ToString() + ",";
            }
            cmd.Parameters.AddWithValue("@categories", category); category = "";
            cmd.Parameters.AddWithValue("@owner", owner);
            cmd.ExecuteNonQuery();
            con.Close(); con.Open();

            string id = get_ID();
            for (int i = 0; i < Steps.Count; i++)
            {
                con.Open();
                SqlCommand cmd2 = new SqlCommand("INSERT INTO Steps values (@id,@rName,@number,@step)", con);
                cmd2.Parameters.AddWithValue("@id", id);
                cmd2.Parameters.AddWithValue("@rName", Name);
                cmd2.Parameters.AddWithValue("@number", i + 1);
                cmd2.Parameters.AddWithValue("@step", Steps[i]);
                cmd2.ExecuteNonQuery();
                con.Close();
            }

            for (int x = 0; x < ingredients.Count; x++)
            {
                Ingredients ing = (Ingredients)ingredients[x];
                ing.Recipe_Add_Ingredients(id);
            }
        }

        public void delete()
        {
            string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM FoodRecipe WHERE Name='" + Name + "' AND Owner='" + owner + "'", con);
            cmd.ExecuteReader();
            con.Close();
        }
    }
}
