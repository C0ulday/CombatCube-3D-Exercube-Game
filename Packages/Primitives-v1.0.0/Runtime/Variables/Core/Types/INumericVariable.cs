namespace Koboldgames.Primitives.Variables
{
    public interface INumericVariable
    {
        int IntValue { get; }
        long LongValue { get; }
        float FloatValue { get; }
        double DoubleValue { get; }
    }
}
