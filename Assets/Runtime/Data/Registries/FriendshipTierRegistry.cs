using Runtime.Data.ScriptableObjects;

namespace Runtime.Data.Registries
{
    /// <summary>
    /// Registry for all friendship tiers.
    /// </summary>
    public class FriendshipTierRegistry : ContentRegistry<FriendshipTierData, FriendshipTierRegistry>
    {
        protected override string GetId(FriendshipTierData data) => data.Id;
    }
}
