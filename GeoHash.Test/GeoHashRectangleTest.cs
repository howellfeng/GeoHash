using GeoHash.Unit;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GeoHash.Test
{
    public class GeoHashRectangleTest
    {
        [Fact(DisplayName ="测试矩形geohash")]
        public void RectangleTest()
        {
            double lon = 121.805809020996, lat = 30.0665111541748;
            int length = 10;
            List<string> hashes = new List<string>();
            GeoHash gh = GeoHash.EncodeWithCharacterPrecision(lon, lat, length);
            hashes.Add(gh.ToBase32());
            hashes.AddRange(gh.GetEightDirectionBase32());
            RectangleD rect = RectangleD.FromLTRB(lon - gh.Bound.Width, lat - gh.Bound.Height, lon + gh.Bound.Width, lat + gh.Bound.Height);
            GeoHashRectangle ghRect = new GeoHashRectangle(rect, length);
            string[] strs = ghRect.ToBase32();
            Assert.Equal(hashes.Count, strs.Length);
            foreach (string str in strs)
            {
                Assert.Contains(str, hashes);
            }
        }
    }
}
