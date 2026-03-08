namespace Koboldgames.Primitives.Events
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedStringEvent.asset", menuName = "Koboldgames/Primitives/Events/String Event", order = 100)]
    public sealed class SharedStringEvent : SharedEvent<string> { }
}
