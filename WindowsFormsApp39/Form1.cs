using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp39
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var NewOneonOne = new One_on_One();
            NewOneonOne.Show();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            var NewAgainstComputer = new Aginst_Computer ();
            NewAgainstComputer.Show();
        }       
    }
}
