using System;
using System.Collections.Generic;
using System.Text;

namespace GeoHash.Unit
{
    public struct RectangleD
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public double Right { get; set; }
        public double Bottom { get; set; }
        public RectangleD(double left, double top, double right, double bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
        public double Width => Right - Left;      
        public double Height => Bottom - Top;
        public bool IsEmpty => Left == 0 && Top == 0 && Right == 0 && Bottom == 0;
        public PointD Center => new PointD((Left + Right) / 2, (Top + Bottom) / 2);
        public static RectangleD FromLTRB(double left, double top, double right, double bottom) => new RectangleD(left, top, right, bottom);
        public static readonly RectangleD Empty;

        public static RectangleD Intersect(RectangleD rect1, RectangleD rect2)
        {
            if (rect1.Left > rect2.Right || rect1.Right < rect2.Left || rect1.Top > rect2.Bottom || rect1.Bottom < rect2.Top)
                return RectangleD.Empty;
            return RectangleD.FromLTRB(Math.Max(rect1.Left, rect2.Left), Math.Max(rect1.Top, rect2.Top), Math.Min(rect1.Right, rect2.Right), Math.Min(rect1.Bottom, rect2.Bottom));
        }
        public bool IsIntersect(RectangleD rect)
        {
            RectangleD intersect = RectangleD.Intersect(this, rect);
            return !intersect.IsEmpty;
        }
        public bool Contains(double longitude, double latitude) => longitude >= Left && longitude <= Right && latitude >= Top && latitude <= Bottom;
        public bool Contains(RectangleD rect) => Contains(rect.Left, rect.Top) && Contains(rect.Right, rect.Bottom);
        public void Union(RectangleD rect)
        {
            if (Left > rect.Left)
                Left = rect.Left;
            if (Top > rect.Top)
                Top = rect.Top;
            if (Right < rect.Right)
                Right = rect.Right;
            if (Bottom < rect.Bottom)
                Bottom = rect.Bottom;
        }
    }
}
