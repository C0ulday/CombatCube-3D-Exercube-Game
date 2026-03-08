namespace Koboldgames.Primitives.Events
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedBooleanEvent.asset", menuName = "Koboldgames/Primitives/Events/Boolean Event", order = 100)]
    public sealed class SharedBooleanEvent : SharedEvent<bool> { }
}
