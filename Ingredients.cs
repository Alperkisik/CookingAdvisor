using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingAdvisor
{
    public class Ingredients
    {
        public string name, amount,type;

        public void User_Ingredients_Update(string Userid)
        {
            string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            
            SqlCommand cmd = new SqlCommand("UPDATE User_Ingredients SET Name=@name,Amount=@a,Type=@t WHERE UserID=@id", con);
            cmd.Parameters.AddWithValue("@id", Userid);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@a", amount);
            cmd.Parameters.AddWithValue("@t", type);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void User_Add_Ingredients(string userid)
        {
            string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO User_Ingredients values (@Uid,@name,@amount,@t)", con);
            cmd.Parameters.AddWithValue("@Uid", userid);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.Parameters.AddWithValue("@t", type);

            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void User_Delete_Ingredients(string userid)
        {
            string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM User_Ingredients WHERE UserID='" + userid + "' AND Name='" + name + "'", con);
            cmd.ExecuteReader();
            con.Close();
        }

        public void Recipe_Add_Ingredients(string recipeID)
        {
            string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand cmd4 = new SqlCommand("INSERT INTO Recipe_Ingredients values (@rID,@name,@amount,@type)", con);
            cmd4.Parameters.AddWithValue("@rID", recipeID);
            cmd4.Parameters.AddWithValue("@name", name);
            cmd4.Parameters.AddWithValue("@amount", amount);
            cmd4.Parameters.AddWithValue("@type", type);

            cmd4.ExecuteNonQuery();
            con.Close();
        }
    }
}
