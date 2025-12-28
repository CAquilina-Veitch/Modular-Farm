using System;

namespace Runtime.Data.Structs
{
    // Represents x/y position in tiles (e.g., door position on building)
    [Serializable]
    public struct TilePosition
    {
        public int X;
        public int Y;

        public TilePosition(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
