namespace Koboldgames.Primitives.Variables
{
    using System;

    public abstract class SharedNumericVariable<T> : SharedVariableBase<T>, INumericVariable
    {
        public int IntValue => Convert.ToInt32(value);

        public long LongValue => Convert.ToInt64(value);

        public float FloatValue => Convert.ToSingle(value);

        public double DoubleValue => Convert.ToDouble(value);
    }
}
