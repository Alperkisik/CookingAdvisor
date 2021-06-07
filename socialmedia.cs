using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CookingAdvisor
{
    public partial class socialmedia : Form
    {
        public socialmedia()
        {
            InitializeComponent();
        }
        public user user;
        public Theme theme;

        private void socialmedia_Load(object sender, EventArgs e)
        {
            ArrayList recipes = new ArrayList();
            recipes = user.RecipeLibrary;
            FoodRecipe rcp = new FoodRecipe();
            for (int i = 0; i < recipes.Count; i++)
            {
                rcp = (FoodRecipe)recipes[i];
                comboBox1.Items.Add(rcp.Name);
            }
        }

        private void fill_theme()
        {
            Color bgc = theme.BackgroundColor, pc = theme.PanelColor, bc = theme.BtnColor;

            this.BackColor = bgc;
            panel1.BackColor = pc;
            panel2.BackColor = pc;
            panel3.BackColor = pc;
            panel4.BackColor = pc;

            button1.BackColor = bc;
            button2.BackColor = bc;
            button3.BackColor = bc;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //share
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //main menu
            mainmenu menu = new mainmenu();
            menu.u = user;
            menu.theme = theme;
            this.Close();
            menu.Show();
        }
    }
}
