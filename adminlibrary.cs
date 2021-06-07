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
    public partial class adminlibrary : Form
    {
        public adminlibrary()
        {
            InitializeComponent();
        }

        public user user;
        ArrayList defaultRecipes;
        string connectionString = "Data source=Alper\\sqlexpress; Initial Catalog=CookingAdvisor; integrated security=True";

        private void button2_Click(object sender, EventArgs e)
        {
            CreateRecipe cr = new CreateRecipe();
            cr.user = user;
            this.Close();
            cr.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //delete
            string name = listBox1.Items[listBox1.SelectedIndex].ToString();
            for (int i = 0; i < defaultRecipes.Count; i++)
            {
                FoodRecipe rcp = (FoodRecipe)defaultRecipes[i];
                if (name == rcp.Name)
                {
                    rcp.delete();
                    defaultRecipes.RemoveAt(i);
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string name = listBox1.Items[listBox1.SelectedIndex].ToString();
                FoodRecipe rcp = new FoodRecipe();
                for (int i = 0; i < defaultRecipes.Count; i++)
                {
                    rcp = (FoodRecipe)defaultRecipes[i];
                    if (name == rcp.Name)
                    {
                        listBox2.Items.Clear();
                        listBox2.Items.Add("MAIN INGREDIENTS");
                        Ingredients ing = new Ingredients();
                        for (int a = 0; a < rcp.ingredients.Count; a++)
                        {
                            ing = (Ingredients)rcp.ingredients[a];
                            if(ing.type == "Main")
                                listBox2.Items.Add(ing.amount + " " + ing.name);
                        }

                        listBox2.Items.Add("EXTRA INGREDIENTS");
                        for (int b = 0; b < rcp.ingredients.Count; b++)
                        {
                            ing = (Ingredients)rcp.ingredients[b];
                            if (ing.type == "Extra")
                                listBox2.Items.Add(ing.amount + " " + ing.name);
                        }

                        listBox2.Items.Add("STEPS");
                        for (int y = 0; y < rcp.Steps.Count; y++)
                        {
                            listBox2.Items.Add(y+1 + ". Step : " + rcp.Steps[y].ToString());
                        }
                        label3.Text = "Time(min) : " + rcp.Time;
                        label4.Text = "Name : " + rcp.Name;

                        break;
                    }
                }
            }
            catch
            {

            }
        }

        private void adminlibrary_Load(object sender, EventArgs e)
        {
            defaultRecipes = new ArrayList();
            for (int i = 0; i < user.RecipeLibrary.Count; i++)
            {
                defaultRecipes.Add(user.RecipeLibrary[i]);
            }

            fill_lb1();
        }

        private void fill_lb1()
        {
            listBox1.Items.Clear();
            for (int y = 0; y < defaultRecipes.Count; y++)
            {
                FoodRecipe rcp = (FoodRecipe)defaultRecipes[y];
                listBox1.Items.Add(rcp.Name);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            adminpanel ap = new adminpanel();
            ap.user = user;
            this.Close();
            ap.Show();
        }
    }
}
