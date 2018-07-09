using GeoHash.Unit;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GeoHash.Test
{
    public class GeoHashSizeTableTest
    {
        [Fact]
        public void LonCharacterRangeTest()
        {
            int strLength = 8;
            double lonExpect = 360d / (1 << (strLength * GeoHash.Base32Bits / 2));
            double lonValue = GeoHashSizeTable.LonCharacterRange[strLength];
            Assert.Equal(lonExpect, lonValue);
            double latExpect = 180d / (1 << (strLength * GeoHash.Base32Bits / 2));
            double latValue = GeoHashSizeTable.LatCharacterRange[strLength];
            Assert.Equal(latExpect, latValue);
        }
        [Theory]
        [InlineData(1)]
        [InlineData(8)]
        public void NumberOfCharacterTest(int characterLength)
        {
            int bitLength = characterLength * GeoHash.Base32Bits;
            double xUnit = 360d / (1 << ((bitLength + 1) / 2));
            double yUnit = 180d / (1 << (bitLength / 2));
            RectangleD rect = RectangleD.FromLTRB(0, 0, xUnit, yUnit);
            Assert.Equal(characterLength, GeoHashSizeTable.NumberOfCharacter(rect));
        }
    }
}
