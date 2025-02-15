using LibraryForPaint;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FirstAddon
{
    public class StarDraw : IAddonDraw
    {
        public bool isFill;
        public Color Color { get; set; }
        public int Width { get; set; }
        public int Points { get; set; } = 5; // Количество лучей

        public string Name => "Звезда";
        public string Description => "Отрисовка звезды";
        public Image Icon => null;

        private Point_Int StartPoint;
        private Bitmap PrewiewRect;

        public int Sides { get; set; } = 6;

        private PointF[] GetStarPoints(Point_Int center, float outerRadius, float innerRadius)
        {
            PointF[] points = new PointF[Points * 2];
            double angle = Math.PI / Points;

            for (int i = 0; i < Points * 2; i++)
            {
                float radius = (i % 2 == 0) ? outerRadius : innerRadius;
                points[i] = new PointF(
                    center.x + radius * (float)Math.Cos(angle * i - Math.PI / 2),
                    center.y + radius * (float)Math.Sin(angle * i - Math.PI / 2)
                );
            }
            return points;
        }

        private void DrawStar(Graphics g, Point_Int currentPos)
        {
            float outerRadius = (float)Math.Sqrt(Math.Pow(currentPos.x - StartPoint.x, 2) +
                                               Math.Pow(currentPos.y - StartPoint.y, 2));
            float innerRadius = outerRadius * 0.4f;

            var points = GetStarPoints(StartPoint, outerRadius, innerRadius);
            if (isFill)
                g.FillPolygon(new SolidBrush(Color), points);
            else
                g.DrawPolygon(new Pen(Color, Width), points);
        }

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
            DrawStar(g, currentPos);
        }

        public void EventUpCursor(Bitmap bitmap, Point_Int Position)
        {
            var g = Graphics.FromImage(bitmap);
            g.Clear(Color.Transparent);
            g.DrawImage(PrewiewRect, new Point());
            DrawStar(g, Position);
        }

        private PointF[] GetPolygonPoints(Point_Int center, float radius)
        {
            PointF[] points = new PointF[Sides];
            float angle = (float)(Math.PI * 2 / Sides);

            for (int i = 0; i < Sides; i++)
            {
                points[i] = new PointF(
                    center.x + radius * (float)Math.Cos(angle * i - Math.PI / 2),
                    center.y + radius * (float)Math.Sin(angle * i - Math.PI / 2)
                );
            }
            return points;
        }

        public ToolStripButton PostAddonToToolStip()
        {
            ToolStripButton button = new ToolStripButton();
            button.Text = Name;
            return button;
        }

        public void PostAddonToWindowMDI(Form formMDI)
        {

        }
    }
}
