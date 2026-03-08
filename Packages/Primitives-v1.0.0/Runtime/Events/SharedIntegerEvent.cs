namespace Koboldgames.Primitives.Events
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedIntegerEvent.asset", menuName = "Koboldgames/Primitives/Events/Integer Event", order = 100)]
    public sealed class SharedIntegerEvent : SharedEvent<int> { }
}
