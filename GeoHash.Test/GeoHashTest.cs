using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GeoHash.Test
{
    public class GeoHashTest
    {
        [Fact(DisplayName = "鸟巢")]
        public void BridNestTest()
        {
            GeoHash hash = GeoHash.EncodeWithCharacterPrecision(116.402843, 39.999375, 8);            
            string result = hash.ToBase32();
            Assert.Equal("wx4g8c9v", result);
        }
        [Theory(DisplayName = "水立方,故宫")]
        [InlineData(116.3967, 39.99932, 8, "wx4g89tk")]
        [InlineData(116.40382, 39.918118, 8, "wx4g0ffe")]
        [InlineData(116.3967, 44.9999, 8, "wxfzbxvr")]
        [InlineData(116.3967, 45.0009, 8, "y84b08jm")]
        [InlineData(121.805809020996, 30.0665111541748, 10, "wtqe5c2q15")]
        [InlineData(122.941070556641, 29.8183994293213, 10, "wtr3ts6by8")]
        [InlineData(122.278099060059, 30.4423389434814, 10, "wtqvntb5qp")]
        public void MultiTest(double longitude, double latitude, int length, string hashCode)
        {
            GeoHash hash = GeoHash.EncodeWithCharacterPrecision(longitude, latitude, length);
            Assert.Equal(hashCode, hash.ToBase32());
        }

        [Theory(DisplayName = "8方向测试")]
        [InlineData(116.3967, 45.0009, 8, new string[] { "y84b08jh", "y84b08jj", "y84b08jn", "y84b08jk", "y84b08jq", "y84b08js", "y84b08jt", "y84b08jw" })]
        [InlineData(116.3967, 44.9999, 8, new string[] { "y84b08j2", "wxfzbxvq", "wxfzbxvx", "wxfzbxvp", "y84b08j8", "y84b08j0", "wxfzbxvw", "wxfzbxvn" })]
        public void EightDirectionTest(double longitude, double latitude, int length, string[] hashCodes)
        {
            GeoHash hash = GeoHash.EncodeWithCharacterPrecision(longitude, latitude, length);
            string[] eights = hash.GetEightDirectionBase32();
            foreach (string code in eights)
            {
                Assert.Contains(code, hashCodes);
            }
        }
        [Fact]
        public void EqualTest()
        {
            GeoHash hash = GeoHash.EncodeWithCharacterPrecision(120, 30, 10);
            GeoHash hash2 = hash;
            Assert.True(hash.Equals(hash2));
        }
        [Fact]
        public void EqualTest2()
        {
            GeoHash hash = GeoHash.EncodeWithCharacterPrecision(120, 30, 10);
            GeoHash hash2 = GeoHash.EncodeWithCharacterPrecision(120, 30, 10);
            Assert.True(hash.Equals(hash2));
        }
    }
}
