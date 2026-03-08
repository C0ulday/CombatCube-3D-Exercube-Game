namespace Koboldgames.Primitives.Variables
{
    using System;

    public abstract class SharedStringVariable<T> : SharedVariableBase<T>, IStringVariable
    {
        public string StringValue => Convert.ToString(value);
    }
}
