using GeoHash.Unit;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoHash
{
    public static class GeoHashSizeTable
    {
        public static readonly double[] LonBitRange = new double[GeoHash.MaxBitLength];
        public static readonly double[] LatBitRange = new double[GeoHash.MaxBitLength];
        public static readonly double[] LonCharacterRange = new double[GeoHash.MaxCharacterLength + 1];
        public static readonly double[] LatCharacterRange = new double[GeoHash.MaxCharacterLength + 1];
        static GeoHashSizeTable()
        {
            //_lonRangle和_latRange的index=Bit的长度，StringLength=8,BitLength=8*5=40，_lonRangle[index]和_latRangle[index]表示在BitLength=index时，每个网格的经度和纬度范围
            for (int i = 0; i < GeoHash.MaxBitLength; i++)
            {
                LonBitRange[i] = getLonRange(i);
                LatBitRange[i] = getLatRange(i);
            }
            for (int i = 0; i <= GeoHash.MaxCharacterLength; i++)
            {
                int bits = i * GeoHash.Base32Bits;
                LonCharacterRange[i] = getLonRange(bits);
                LatCharacterRange[i] = getLatRange(bits);
            }
        }
        private static double getLonRange(int bits) => 360d / Math.Pow(2, (bits + 1) / 2);
        private static double getLatRange(int bits) => 180d / Math.Pow(2, bits / 2);
        /// <summary>
        /// 获取完全包含矩形的网格Bit长度
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static int NumberOfBits(RectangleD rect)
        {
            int bits = GeoHash.MaxBitLength - 1;
            double width = rect.Width, height = rect.Height;
            //找到网格完全包含该矩形的级别，网格的width和height大于等于当前矩形width和height
            while ((LonBitRange[bits] < width || LatBitRange[bits] < height) && bits > 0)
                bits--;
            return bits;
        }
        /// <summary>
        /// 获取完全包含矩形的网格字符串长度
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static int NumberOfCharacter(RectangleD rect)
        {
            int length = GeoHash.MaxCharacterLength;
            double width = rect.Width, height = rect.Height;
            while ((LonCharacterRange[length] < width || LatCharacterRange[length] < height) && length > 0)
                length--;
            return length;
        }
    }
}
