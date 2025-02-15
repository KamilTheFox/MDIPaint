using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using LibraryForPaint;
using System.Windows.Forms;

namespace FirstAddon
{
    [PropertyAddon("1.0", TypeToolStip.Tool)]
    public class TextDraw : IAddonDraw
    {
        public Color Color { get; set; }
        public int Width { get; set; }

        public string TextToWrite { get; set; } = "Текст";

        public Font Font { get; set; }

        public string Name => "Текст";

        public string Description => "Написание текста";

        public Image Icon => null;

        private Point_Int StartPoint;
        private Bitmap PrewiewRect;

        private void DrawText(Graphics g, Point_Int currentPos)
        {
            using (var brush = new SolidBrush(Color))
            {
                using (var font = new Font(Font.FontFamily, Width))
                {
                    g.DrawString(TextToWrite, font, brush, new PointF(currentPos.x, currentPos.y));
                }
            }
        }

        public void EventDownCursor(Bitmap bitmap, Point_Int Position)
        {
            PrewiewRect = (Bitmap)bitmap.Clone();
            StartPoint = Position;

            var g = Graphics.FromImage(bitmap);
            DrawText(g, Position);
        }

        public void EventDragCursor(Bitmap bitmap, Point_Int previousPos, Point_Int currentPos)
        {
        }

        public void EventUpCursor(Bitmap bitmap, Point_Int Position)
        {
        }

        public ToolStripButton PostAddonToToolStip()
        {
            ToolStripButton button = new ToolStripButton();
            button.Text = Name;
            return button;
        }

        public void PostAddonToWindowMDI(Form formMDI)
        {
            TextForm textForm = new TextForm(this);
            textForm.MdiParent = formMDI;
            textForm.Show();
        }
    }
}
