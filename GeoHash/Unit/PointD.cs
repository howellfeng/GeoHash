using System;
using System.Collections.Generic;
using System.Text;

namespace GeoHash.Unit
{
    public struct PointD
    {
        public double X { get; set; }
        public double Y { get; set; }
        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }
        public override bool Equals(object obj)
        {
            if (obj is PointD)
            {
                PointD pd = (PointD)obj;
                return X == pd.X && Y == pd.Y;

            }
            else
                return false;
        }
        public bool IsEmpty => this.Equals(PointD.Empty);
        //{
        //    get
        //    {
        //        return this.Equals(PointD.Empty);
        //    }
        //}
        public static readonly PointD Empty;
    }
}
