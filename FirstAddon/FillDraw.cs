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
    public class FillDraw : IAddonDraw
    {
        public Color Color { get; set; }
        public int Width { get; set; }
        public string Name => "Заливка";
        public string Description => "Заливка области";
        public Image Icon => null;

        private void FloodFill(Bitmap bitmap, Point_Int pt, Color targetColor, Color replacementColor)
        {
            if (targetColor.ToArgb() == replacementColor.ToArgb()) return;

            Stack<Point> pixels = new Stack<Point>();
            pixels.Push(new Point(pt.x, pt.y));

            while (pixels.Count > 0)
            {
                Point a = pixels.Pop();
                if (a.X < bitmap.Width && a.X > 0 &&
                    a.Y < bitmap.Height && a.Y > 0)
                {
                    if (bitmap.GetPixel(a.X, a.Y).ToArgb() == targetColor.ToArgb())
                    {
                        bitmap.SetPixel(a.X, a.Y, replacementColor);
                        pixels.Push(new Point(a.X - 1, a.Y));
                        pixels.Push(new Point(a.X + 1, a.Y));
                        pixels.Push(new Point(a.X, a.Y - 1));
                        pixels.Push(new Point(a.X, a.Y + 1));
                    }
                }
            }
        }

        public void EventDownCursor(Bitmap bitmap, Point_Int Position)
        {
            Color targetColor = bitmap.GetPixel(Position.x, Position.y);
            FloodFill(bitmap, Position, targetColor, Color);
        }


        public ToolStripButton PostAddonToToolStip()
        {
            ToolStripButton button = new ToolStripButton();
            button.Text = Name;
            return button;
        }

        public void PostAddonToWindowMDI(Form formMDI)
        {
            ColorDialog dialog = new ColorDialog();
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                Color = dialog.Color;
            }
        }

        public void EventDragCursor(Bitmap bitmap, Point_Int previousPos, Point_Int currentPos)
        {
        }

        public void EventUpCursor(Bitmap bitmap, Point_Int Position)
        {

        }
    }
}
