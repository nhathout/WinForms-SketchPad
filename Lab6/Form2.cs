using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab6
{
    public partial class settingsDialog : Form
    {
        public settingsDialog()
        {
            InitializeComponent();

            listBox3.DataSource = penWidths;

            listBox1.SelectedIndex = 0;
            listBox2.SelectedIndex = 0;
            listBox3.SelectedIndex = 0;
        }

        public static int[] penWidths = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        private int fillColorIndex;

        private int outlineColorIndex;

        private int penWidthIndex;

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }

        private void Cancel_button_Click(object sender, EventArgs e)
        {
            this.listBox1.SelectedIndex = outlineColorIndex;
            this.listBox2.SelectedIndex = fillColorIndex;
            this.listBox3.SelectedIndex = penWidthIndex;

            this.Close();
        }

        private void OK_button_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        protected override void OnShown(EventArgs e)
        {
            outlineColorIndex = this.listBox1.SelectedIndex;
            fillColorIndex = this.listBox2.SelectedIndex;
            penWidthIndex = this.listBox3.SelectedIndex;
        }
    }
}
