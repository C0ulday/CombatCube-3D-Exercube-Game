namespace Koboldgames.Primitives.Events
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedObjectEvent.asset", menuName = "Koboldgames/Primitives/Events/Object Event", order = 100)]
    public sealed class SharedObjectEvent : SharedEvent<Object> { }
}
