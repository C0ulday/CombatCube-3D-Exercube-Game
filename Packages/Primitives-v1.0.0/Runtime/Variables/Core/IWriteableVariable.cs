namespace Koboldgames.Primitives.Variables
{
    public interface IWriteableVariable<T>
    {
        T Value { set; }
        void Reset();
    }
}
