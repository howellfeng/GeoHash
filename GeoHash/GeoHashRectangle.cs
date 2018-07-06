using GeoHash.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoHash
{
    public class GeoHashRectangle
    {
        public RectangleD Rect { get; private set; }
        private List<GeoHash> _geoHashes = new List<GeoHash>();
        public IEnumerable<GeoHash> GeoHashes => _geoHashes;
        public int DesiredPrecision => CharacterLength * GeoHash.Base32Bits;
        public int CharacterLength { get; private set; }
        public GeoHashRectangle(RectangleD rect, int characterLenth)
        {
            if (characterLenth > GeoHash.MaxCharacterLength)
                throw new NotSupportedException($"string length no longer than {GeoHash.MaxCharacterLength}");
            CharacterLength = characterLenth;
            Rect = rect;
            splitRectangle(rect, CharacterLength);
        }
        /// <summary>
        /// 1.获取字符串长度的网格范围
        /// 2.从leftTop开始，根据
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="characterLength"></param>
        private void splitRectangle(RectangleD rect, int characterLength)
        {
            double xRange = GeoHashSizeTable.LonCharacterRange[characterLength];
            double yRange = GeoHashSizeTable.LatCharacterRange[characterLength];
            //由leftTop开始分裂
            double x = rect.Left;
            while (x <= rect.Right)
            {
                double y = rect.Top;
                while (y <= rect.Bottom)
                {
                    GeoHash hash = GeoHash.EncodeWithCharacterPrecision(x, y, characterLength);
                    if (!_geoHashes.Contains(hash))
                        _geoHashes.Add(hash);
                    y = nextLonLat(y, yRange, rect.Bottom);
                }
                x = nextLonLat(x, xRange, rect.Right);
            }
        }
        private double nextLonLat(double current, double range, double max)
        {
            current += range;
            return current <= max ? current : max;
        }

        public string[] ToBase32() => _geoHashes.Select(p => p.ToBase32()).ToArray();
    }
}
