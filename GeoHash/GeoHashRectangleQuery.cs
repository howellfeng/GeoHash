using GeoHash.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoHash
{
    public class GeoHashRectangleQuery
    {
        public RectangleD Rect { get; private set; }
        public RectangleD Bound { get; private set; } = RectangleD.Empty;
        private List<GeoHash> _geoHashes = new List<GeoHash>();
        public IEnumerable<GeoHash> GeoHashes => _geoHashes;
        public GeoHashRectangleQuery(RectangleD rect)
        {
            Rect = rect;
            int length = GeoHashSizeTable.NumberOfCharacter(rect);
            GeoHash hash = GeoHash.EncodeWithCharacterPrecision(rect.Center, length);
            if (hash.Bound.Contains(rect))
                addSearchHash(hash);
            else
                expandSearch(hash);
        }

        private void expandSearch(GeoHash hash)
        {
            addSearchHash(hash);
            foreach (GeoHash gh in hash.GetEightDirection())
            {
                if (gh.Bound.IsIntersect(Rect) && !_geoHashes.Contains(gh))
                    addSearchHash(gh);
            }
        }

        private void addSearchHash(GeoHash hash)
        {
            Bound.Union(hash.Bound);
            _geoHashes.Add(hash);
        }
        public string[] ToBase32() => _geoHashes.Select(p => p.ToBase32()).ToArray();
    }
}
