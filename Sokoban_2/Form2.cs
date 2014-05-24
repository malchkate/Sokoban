using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sokoban_2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            string[] lines = System.IO.File.ReadAllLines("AllLevels.txt");
            int levels = int.Parse(lines[0]);
            for (int i = 0; i < levels; i++)
            {
                levelBox.Items.Add(" Level № " + i);
            }

        }
        public int getName()
        {
            return levelBox.SelectedIndex;
        }

        private void button2_MouseClick(object sender, MouseEventArgs e)//CANCEL
        {
            this.Close();
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)//OK
        {
            this.Close();
        }

    }
}
