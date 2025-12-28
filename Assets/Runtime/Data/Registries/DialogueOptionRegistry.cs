using Runtime.Data.ScriptableObjects;

namespace Runtime.Data.Registries
{
    /// <summary>
    /// Registry for all dialogue options.
    /// </summary>
    public class DialogueOptionRegistry : ContentRegistry<DialogueOptionData, DialogueOptionRegistry>
    {
        protected override string GetId(DialogueOptionData data) => data.Id;
    }
}
