using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryForPaint;

namespace MDIPaint
{
    public class EraserBrush : IAddonDraw
    {
        public string Name => "Ластик";

        public string Description => "Рисует альфа-каналом";

        public Image Icon => null;

        public Color Color { get; set; } = Color.FromArgb(0, 0, 0, 0);
        public int Width { get; set; }

        private void SetupGraphics(Graphics g)
        {
            g.CompositingMode = CompositingMode.SourceCopy;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias;
        }

        public void EventDownCursor(Bitmap bitmap, Point_Int Position)
        {
            var g = Graphics.FromImage(bitmap);
            SetupGraphics(g);
            var circleRadius = Width / 2;
            Print_ElipseFix(g, Position, circleRadius);
        }



        public void EventDragCursor(Bitmap bitmap, Point_Int previousPos, Point_Int currentPos)
        {
            var g = Graphics.FromImage(bitmap);
            SetupGraphics(g);
            var circleRadius = Width / 2;

            g.DrawLine(new Pen(new SolidBrush(Color), Width), previousPos, currentPos);

            Print_ElipseFix(g, currentPos, circleRadius);
        }

        private void Print_ElipseFix(Graphics g, Point_Int currentPos, int circleRadius)
        {
            g.FillEllipse(new SolidBrush(Color),
                currentPos.x - circleRadius,
                currentPos.y - circleRadius,
                circleRadius * 2,
                circleRadius * 2);
        }

        public ToolStripButton PostAddonToToolStip()
        {
            //Уже в интерфейсе
            return null;
        }

        public void PostAddonToWindowMDI(Form formMDI)
        {
            //Уже в интерфейсе
        }

        public void EventUpCursor(Bitmap bitmap, Point_Int Position)
        {
        }
    }
}
