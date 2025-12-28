using System;

namespace Runtime.Data.Structs
{
    // Represents width/height in tiles for buildings, stations
    [Serializable]
    public struct TileSize
    {
        public int Width;
        public int Height;

        public TileSize(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
