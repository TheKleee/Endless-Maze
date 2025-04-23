using UnityEngine;

namespace Lavirint
{
    public static class TileCalculator
    {
        static float tileSize = 5f;
        public static float SetTileSize(this float size) => tileSize = size > 0 ? size : tileSize;
        public static float TileAnchor(this float t) => tileSize / 2;
    }
}
