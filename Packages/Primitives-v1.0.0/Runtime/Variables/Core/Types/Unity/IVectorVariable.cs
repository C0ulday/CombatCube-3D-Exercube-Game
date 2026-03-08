namespace Koboldgames.Primitives.Variables
{
    using UnityEngine;

    public interface IVectorVariable
    {
        Vector2 Vector2Value { get; }
        Vector3 Vector3Value { get; }
        Vector4 Vector4Value { get; }
        Vector2Int Vector2IntValue { get; }
        Vector3Int Vector3IntValue { get; }
        Color ColorValue { get; }
    }
}
