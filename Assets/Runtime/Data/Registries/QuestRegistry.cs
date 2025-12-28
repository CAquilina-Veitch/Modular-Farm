using Runtime.Data.ScriptableObjects;

namespace Runtime.Data.Registries
{
    /// <summary>
    /// Registry for all quests.
    /// </summary>
    public class QuestRegistry : ContentRegistry<QuestData, QuestRegistry>
    {
        protected override string GetId(QuestData data) => data.Id;
    }
}
