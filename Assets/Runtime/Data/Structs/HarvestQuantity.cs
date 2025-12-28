using System;

namespace Runtime.Data.Structs
{
    // GDD 13.5 - Min/max harvest quantity for crops
    [Serializable]
    public struct HarvestQuantity
    {
        public int Min;
        public int Max;

        public HarvestQuantity(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }
}
