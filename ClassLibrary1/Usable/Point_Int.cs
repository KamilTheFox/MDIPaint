using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForPaint
{
    public struct Point_Int
    {
        public Point_Int(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public int x;
        public int y;
        public static Point_Int Lerp(Point_Int a, Point_Int b, float t)
        {
            t = t.Clamp(0, 1);
            return LerpUnclamped(a, b, t);
        }
        public static Point_Int LerpUnclamped(Point_Int a, Point_Int b, float t)
        {
            return new Point_Int((int)(a.x + (b.x - a.x) * t), (int)(a.y + (b.y - a.y) * t));
        }
        public static double GetDiagonal(Point_Int point_)
        {
            return Math.Sqrt((double)Math.Pow(point_.x, 2) + (double)Math.Pow(point_.y, 2));
        }

        public static implicit operator Point(Point_Int point_)
        {
            return new Point(point_.x, point_.y);
        }
    }
}
