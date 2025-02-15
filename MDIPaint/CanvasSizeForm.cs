using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDIPaint
{
    public partial class CanvasSizeForm : Form, IRequesterSize
    {
        private const string stringHeadWidth = "Wight";
        private const string stringHeadHeight = "Height";

        private FixedTextToValue<int> width;
        private FixedTextToValue<int> height;

        public int Width => width.Value;
        public int Height => height.Value;
        public CanvasSizeForm()
        {
            InitializeComponent();
            width = new FixedTextToValue<int>(stringHeadWidth, write_Wight, 100);
            height = new FixedTextToValue<int>(stringHeadHeight, write_Height, 100);
        }
        public void SetDefaultValue(int width, int height)
        {
            this.width.Value = width;
            this.height.Value = height;
        }
    }
}
