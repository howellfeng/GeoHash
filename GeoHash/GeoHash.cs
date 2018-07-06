using GeoHash.Unit;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoHash
{
    public class GeoHash
    {
        /*GeoHash存在边界问题：两个位置距离得越近是否意味着Geohash前面相同的越多呢？答案是否定的，两个很近的地点[116.3967,44.9999]和[116.3967,45.0009]的Geohash分别是wxfzbxvr和y84b08j2，这就是Geohash存在的边界问题，这两个地点虽然很近，但是刚好在分界点45两侧，导致Geohash完全不同，单纯依靠Geohash匹配前缀的方式并不能解决这种问题。在一维空间解决不了这个问题，回到二维空间中，将当前Geohash这块区域周围的八块区域的Geohash计算出来。
         */

        public const int MaxCharacterLength = 12;
        //public const int MaxBitLength = 60;
        public const int MaxBitLength = MaxCharacterLength * Base32Bits;
        public const int Base32Bits = 5;
        private static readonly char[] _base32 = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'j', 'k', 'm', 'n', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };        
        public long Bits { get; private set; } = 0;
        public RectangleD Bound { get; private set; }        
        public PointD Point { get; private set; }
        public int DesiredPrecision { get; private set; }
        public int CharacterLength => DesiredPrecision / Base32Bits;
        public static GeoHash EncodeWithCharacterPrecision(double longitude, double latitude, int characterLength)
        {
            if (characterLength > MaxCharacterLength)
                throw new NotSupportedException($"string length no longer than {MaxCharacterLength}");
            int desiredPrecision = Base32Bits * characterLength;
            return new GeoHash(longitude, latitude, desiredPrecision);
        }
        public static GeoHash EncodeWithCharacterPrecision(PointD pd, int characterLength)
        {
            return EncodeWithCharacterPrecision(pd.X, pd.Y, characterLength);
        }
        public static GeoHash EncodeWithBitPrecision(double longitude, double latitude, int bitsLength)
        {
            if (bitsLength > MaxBitLength)
                throw new NotSupportedException($"bit length no longer than{MaxBitLength}");
            if (bitsLength % Base32Bits != 0)
                throw new Exception($"bit length is an integer multipe of {Base32Bits}");
            return new GeoHash(longitude, latitude, bitsLength);
        }
        public static GeoHash EncodeWithBitPrecision(PointD pd, int bitsLength)
        {
            return EncodeWithBitPrecision(pd.X, pd.Y, bitsLength);
        }

        private GeoHash(double longitude, double latitude, int desiredPrecision)
        {            
            Point = new PointD(longitude, latitude);
            DesiredPrecision = desiredPrecision;
            double[] longitudeRange = new double[] { -180, 180 };
            double[] latitudeRange = new double[] { -90, 90 };
            bool isEvenNumbers = true;
            for (int i = 0; i < desiredPrecision; i++)
            {
                if (isEvenNumbers)
                    divideRangeEncode(longitude, longitudeRange);
                else
                    divideRangeEncode(latitude, latitudeRange);
                isEvenNumbers = !isEvenNumbers;
            }
            //可以获取Bounds
            Bound = RectangleD.FromLTRB(longitudeRange[0], latitudeRange[0], longitudeRange[1], latitudeRange[1]);
        }
        /// <summary>
        /// Base32编码
        /// </summary>
        /// <returns></returns>
        public string ToBase32()
        {
            StringBuilder builer = new StringBuilder();
            string binaryStr = Convert.ToString(Bits, 2);
            for (int i = 0; i < binaryStr.Length; i += Base32Bits)
            {
                string temp = binaryStr.Substring(i, Base32Bits);
                int index = Convert.ToInt32(temp, 2);
                builer.Append(_base32[index]);
            }
            return builer.ToString();
        }
        /// <summary>
        /// 返回8方向GeoHash
        /// GeoHash存在边界问题：两个位置距离得越近是否意味着Geohash前面相同的越多呢？答案是否定的，两个很近的地点[116.3967,44.9999]和[116.3967,45.0009]的Geohash分别是wxfzbxvr和y84b08j2，
        /// 这就是Geohash存在的边界问题，这两个地点虽然很近，但是刚好在分界点45两侧，导致Geohash完全不同，单纯依靠Geohash匹配前缀的方式并不能解决这种问题。
        /// 在一维空间解决不了这个问题，回到二维空间中，将当前Geohash这块区域周围的八块区域的Geohash计算出来。
        /// </summary>
        /// <returns></returns>
        public GeoHash[] GetEightDirection()
        {
            List<GeoHash> hashes = new List<GeoHash>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (!(i == 0 && j == 0))
                    {
                        hashes.Add(new GeoHash(Point.X + Bound.Width * i, Point.Y + Bound.Height * j, DesiredPrecision));
                    }
                }
            }
            return hashes.ToArray(); ;
        }
        /// <summary>
        /// 返回8方向GeoHash的Base32编码
        /// </summary>
        /// <returns></returns>
        public string[] GetEightDirectionBase32()
        {
            GeoHash[] hashes = GetEightDirection();
            string[] result = new string[hashes.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = hashes[i].ToBase32();
            }
            return result;
        }

        private void divideRangeEncode(double value, double[] range)
        {
            double middle = (range[0] + range[1]) / 2;
            if (value < middle)
            {
                //0
                addOffBitToEnd();
                range[1] = middle;
            }
            else
            {
                //1
                addOnBitToEnd();
                range[0] = middle;
            }
        }

        private void addOnBitToEnd()
        {
            Bits <<= 1;
            Bits |= 1;
        }

        private void addOffBitToEnd() => Bits <<= 1;
        
        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            else
            {
                GeoHash hash = obj as GeoHash;
                if (hash != null && DesiredPrecision == hash.DesiredPrecision && Bits == hash.Bits)
                    return true;
            }
            return false;
        }
    }

}
