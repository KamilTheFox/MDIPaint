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
    public partial class TextForm : Form
    {
        TextDraw draw1;
        public TextForm(TextDraw draw)
        {
            draw1 = draw;
            InitializeComponent();
            richTextBox1.Text = draw1.TextToWrite;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                draw1.Color = colorDialog.Color;
                pictureBox1.BackColor = colorDialog.Color;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            if(fontDialog.ShowDialog() == DialogResult.OK)
            {
                draw1.Font = fontDialog.Font;
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            draw1.TextToWrite = richTextBox1.Text;
        }
    }
}
