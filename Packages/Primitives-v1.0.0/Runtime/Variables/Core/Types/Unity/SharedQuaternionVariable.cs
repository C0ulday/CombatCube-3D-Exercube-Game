namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    public abstract class SharedQuaternionVariable<T> : SharedVariableBase<T>, IQuaternionVariable
    {
        public Quaternion QuaternionValue => (value as Quaternion?) ?? throw new InvalidCastException($"Can not cast type '{typeof(T).Name}' to 'Quaternion'!");
    }
}
