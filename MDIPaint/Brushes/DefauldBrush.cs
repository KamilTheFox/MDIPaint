using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryForPaint;

namespace MDIPaint
{
    public class DefauldBrush : IAddonDraw
    {
        public string Name => "Базовая кисть";

        public string Description => "Самая обычная кисточка";

        public Image Icon => null;

        public Color Color { get; set; } = Color.White;
        public int Width { get; set; } = 3;


        public void EventDownCursor(Bitmap bitmap, Point_Int Position)
        {
        }

        public void EventDragCursor(Bitmap bitmap, Point_Int previousPos, Point_Int currentPos)
        {
            var circleRadius = MainForm.PaintWidth / 2;

            Brush brush = new SolidBrush(Color);

            var g = Graphics.FromImage(bitmap);

            g.DrawLine(new Pen(brush, Width), previousPos, currentPos);

            g.FillEllipse(brush,
                currentPos.x - circleRadius,
                currentPos.y - circleRadius,
                circleRadius * 2,
                circleRadius * 2);
        }

        public void EventUpCursor(Bitmap bitmap, Point_Int Position)
        {
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
    }
}
