namespace Koboldgames.Primitives.Variables
{
    using UnityEngine;

    public interface IRectVariable
    {
        Rect RectValue { get; }
        RectInt RectIntValue { get; }
    }
}
