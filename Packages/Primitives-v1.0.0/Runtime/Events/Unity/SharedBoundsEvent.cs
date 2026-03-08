namespace Koboldgames.Primitives.Events
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedBoundsEvent.asset", menuName = "Koboldgames/Primitives/Events/Bounds Event", order = 100)]
    public sealed class SharedBoundsEvent : SharedEvent<Bounds> { }
}
