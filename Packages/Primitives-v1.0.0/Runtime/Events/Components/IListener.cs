namespace Koboldgames.Primitives.Events
{
    public interface IListener
    {
        bool Active { get; set; }
        void Subscribe();
        void Unsubscribe();
    }
}
