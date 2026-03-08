namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    public abstract class SharedVectorVariable<T> : SharedVariableBase<T>, IVectorVariable
    {
        public Vector2 Vector2Value => (value as Vector2?) ?? throw new InvalidCastException($"Can not cast type '{typeof(T).Name}' to 'Vector2'!");

        public Vector3 Vector3Value => (value as Vector3?) ?? throw new InvalidCastException($"Can not cast type '{typeof(T).Name}' to 'Vector3'!");

        public Vector4 Vector4Value => (value as Vector4?) ?? throw new InvalidCastException($"Can not cast type '{typeof(T).Name}' to 'Vector4'!");

        public Vector2Int Vector2IntValue => (value as Vector2Int?) ?? throw new InvalidCastException($"Can not cast type '{typeof(T).Name}' to 'Vector2Int'!");

        public Vector3Int Vector3IntValue => (value as Vector3Int?) ?? throw new InvalidCastException($"Can not cast type '{typeof(T).Name}' to 'Vector3Int'!");

        public Color ColorValue => Vector4Value;
    }
}
