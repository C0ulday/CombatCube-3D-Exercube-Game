namespace Koboldgames.Primitives
{
    using System.Collections.Generic;

    public interface ISequence<T> : IEnumerable<T>
    {
        int Count { get; }
        bool Has(int index);
        bool Contains(T item);
        void Add(T item);
        bool Remove(T item);
        void RemoveAt(int index);
        void Clear();
    }
}
