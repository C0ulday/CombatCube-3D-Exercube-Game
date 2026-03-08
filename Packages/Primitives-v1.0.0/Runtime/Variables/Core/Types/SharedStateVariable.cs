namespace Koboldgames.Primitives.Variables
{
    using System;

    public abstract class SharedStateVariable<T> : SharedVariableBase<T>, IStateVariable
    {
        public bool StateValue => Convert.ToBoolean(value);
    }
}
