using System;

namespace Runtime.Data.Structs
{
    // GDD 13.7 - Quest completion rewards
    [Serializable]
    public struct QuestRewards
    {
        public int Gold;
        public ItemReward[] Items;
        public FriendshipReward[] Friendship;
    }

    // Item reward entry
    [Serializable]
    public struct ItemReward
    {
        public string ItemId;
        public int Quantity;

        public ItemReward(string itemId, int quantity)
        {
            ItemId = itemId;
            Quantity = quantity;
        }
    }

    // Friendship reward entry
    [Serializable]
    public struct FriendshipReward
    {
        public string NPCId;
        public int Points;

        public FriendshipReward(string npcId, int points)
        {
            NPCId = npcId;
            Points = points;
        }
    }
}
