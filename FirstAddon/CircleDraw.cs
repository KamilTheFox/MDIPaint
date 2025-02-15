using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryForPaint;
using System.Windows.Forms;

namespace FirstAddon
{
    [PropertyAddon("1.0", TypeToolStip.Tool)]
    public class CircleDraw : IAddonDraw
    {
        public bool isFill;
        public Color Color { get; set; }
        public int Width { get; set; }

        public string Name => "Элипс";

        public string Description => "Отрисовка округлостей";

        public Image Icon => null;

        private Point_Int StartPoint;

        private Bitmap PrewiewRect;

        public void EventDownCursor(Bitmap bitmap, Point_Int Position)
        {
            PrewiewRect = (Bitmap)bitmap.Clone();
            StartPoint = Position;
        }

        public void EventDragCursor(Bitmap bitmap, Point_Int previousPos, Point_Int currentPos)
        {
            var g = Graphics.FromImage(bitmap);
            g.Clear(Color.Transparent);
            g.DrawImage(PrewiewRect, new Point());
            var brush = new SolidBrush(Color);
            int left = Math.Min(StartPoint.x, currentPos.x);
            int top = Math.Min(StartPoint.y, currentPos.y);
            int width = Math.Abs(currentPos.x - StartPoint.x);
            int height = Math.Abs(currentPos.y - StartPoint.y);
            if (isFill)
            {
                g.FillEllipse(new Pen(brush, Width).Brush, left, top,
                width, height);
            }
            else
                g.DrawEllipse(new Pen(brush, Width), left, top,
                width, height);
        }

        public void EventUpCursor(Bitmap bitmap, Point_Int Position)
        {
            var g = Graphics.FromImage(bitmap);
            g.Clear(Color.Transparent);
            g.DrawImage(PrewiewRect, new Point());
            int left = Math.Min(StartPoint.x, Position.x);
            int top = Math.Min(StartPoint.y, Position.y);
            int width = Math.Abs(Position.x - StartPoint.x);
            int height = Math.Abs(Position.y - StartPoint.y);
            if (isFill)
            {
                g.FillEllipse(new Pen(Color, Width).Brush, left, top,
                width, height);
            }
            else
                g.DrawEllipse(new Pen(Color, Width), left, top,
                width, height);
        }

        public ToolStripButton PostAddonToToolStip()
        {
            ToolStripButton button = new ToolStripButton();
            button.Text = Name;
            return button;
        }

        public void PostAddonToWindowMDI(Form formMDI)
        {
            CircleForm rectangle = new CircleForm(this);
            rectangle.MdiParent = formMDI;
            rectangle.Show();
        }
    }
}
