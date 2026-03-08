namespace Koboldgames.Primitives.Events
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedColorEvent.asset", menuName = "Koboldgames/Primitives/Events/Color Event", order = 100)]
    public sealed class SharedColorEvent : SharedEvent<Color> { }
}
