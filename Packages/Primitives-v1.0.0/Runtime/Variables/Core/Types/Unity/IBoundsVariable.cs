namespace Koboldgames.Primitives.Variables
{
    using UnityEngine;

    public interface IBoundsVariable
    {
        Bounds BoundsValue { get; }
        BoundsInt BoundsIntValue { get; }
    }
}
