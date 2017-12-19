using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Tetris
{
    /// <summary>
    /// Main menu
    /// </summary>
    public partial class Menu : Form
    { 
         /// <summary>
        /// Create XML for storing scores and name
        /// </summary>
        
        public Menu()
        {
            InitializeComponent();

        }
       

        /// <summary>
        /// check if user has entered his/her name, if not display message to ask for it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (string.IsNullOrWhiteSpace(textBox1.Text))
                    {
                        MessageBox.Show("Enter your name");
                    }
                    else
                    {
                        var game = new Form1(textBox1.Text);
                        game.Show();
                        this.Hide();
                    }
                break;

            }
        }




    }
}
