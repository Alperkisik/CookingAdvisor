using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingAdvisor
{
    public class Theme
    {
        public Color BackgroundColor,BtnColor,PanelColor;

        public void Set_Colors(Color Background_Color,Color Panel_Color,Color Button_Color)
        {
            BackgroundColor = Background_Color;
            PanelColor = Panel_Color;
            BtnColor = Button_Color;
        }

        public void Set_theme(string username)
        {
            string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            string clr1, clr2, clr3;
            SqlCommand cmd = new SqlCommand("Select * from Theme where Owner='" + username + "'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                dr.Close(); con.Close(); con.Open();
                cmd = new SqlCommand("UPDATE Theme SET BackgroundColor=@bgc,PanelColor=@pc,ButtonColor=@bc WHERE Owner=@name", con);
                cmd.Parameters.AddWithValue("@name", username);
                clr1 = BackgroundColor.ToArgb().ToString();
                cmd.Parameters.AddWithValue("@bgc", clr1);
                clr2 = PanelColor.ToArgb().ToString();
                cmd.Parameters.AddWithValue("@pc", clr2);
                clr3 = BtnColor.ToArgb().ToString();
                cmd.Parameters.AddWithValue("@bc", clr3);

                cmd.ExecuteNonQuery();
            }
            else
            {
                dr.Close(); con.Close(); con.Open();
                cmd = new SqlCommand("INSERT INTO Theme values (@owner,@bgc,@pc,@bc)", con);
                cmd.Parameters.AddWithValue("@owner", username);
                clr1 = BackgroundColor.ToArgb().ToString();
                cmd.Parameters.AddWithValue("@bgc", clr1);
                clr2 = PanelColor.ToArgb().ToString();
                cmd.Parameters.AddWithValue("@pc", clr2);
                clr3 = BtnColor.ToArgb().ToString();
                cmd.Parameters.AddWithValue("@bc", clr3);

                cmd.ExecuteNonQuery();
            }
            con.Close();
        }
    }
}
