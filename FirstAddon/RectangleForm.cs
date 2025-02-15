using LibraryForPaint;
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
    public partial class RectangleForm : Form
    {
        private RectangleDraw aDraw;
        public RectangleForm(RectangleDraw draw)
        {
            InitializeComponent();
            aDraw = draw;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if(colorDialog.ShowDialog() == DialogResult.OK)
            {
                aDraw.Color = colorDialog.Color;
                pictureBox1.BackColor = colorDialog.Color;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            aDraw.isFill = checkBox1.Checked;
        }
    }
}
