using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FirstAddon
{
    public partial class CircleForm : Form
    {
        CircleDraw circle1;
        public CircleForm(CircleDraw circle)
        {
            circle1 = circle;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                circle1.Color = colorDialog.Color;
                pictureBox1.BackColor = colorDialog.Color;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            circle1.isFill = checkBox1.Checked;
        }
    }
}
