namespace Koboldgames.Primitives.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class SharedHashSetBase<T> : ScriptableObject, ICollection<T>, ISet<T>, IReadOnlyCollection<T>
    {
        [SerializeField, Multiline] protected string description;
        [NonSerialized] protected HashSet<T> inner;

        /// <summary>
        /// Gets the description of this hash set.
        /// </summary>
        /// <value>The description of this hash set.</value>
        public string Description => description;

        public int Count => inner.Count;

        public IEqualityComparer<T> Comparer => inner.Comparer;
        bool ICollection<T>.IsReadOnly => ((ICollection<T>)inner).IsReadOnly;

        public bool Add(T item) => inner.Add(item);

        void ICollection<T>.Add(T item) => ((ICollection<T>)inner).Add(item);

        public void Clear() => inner.Clear();

        public bool Contains(T item) => inner.Contains(item);

        public void CopyTo(T[] array) => inner.CopyTo(array);

        public void CopyTo(T[] array, int arrayIndex) => inner.CopyTo(array, arrayIndex);

        public void CopyTo(T[] array, int arrayIndex, int count) => inner.CopyTo(array, arrayIndex, count);

        public void ExceptWith(IEnumerable<T> other) => inner.ExceptWith(other);

        public HashSet<T>.Enumerator GetEnumerator() => inner.GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => ((IEnumerable<T>)inner).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)inner).GetEnumerator();

        public void IntersectWith(IEnumerable<T> other) => inner.IntersectWith(other);

        public bool IsProperSubsetOf(IEnumerable<T> other) => inner.IsProperSubsetOf(other);

        public bool IsProperSupersetOf(IEnumerable<T> other) => inner.IsProperSupersetOf(other);

        public bool IsSubsetOf(IEnumerable<T> other) => inner.IsSubsetOf(other);

        public bool IsSupersetOf(IEnumerable<T> other) => inner.IsSupersetOf(other);

        public bool Overlaps(IEnumerable<T> other) => inner.Overlaps(other);

        public bool Remove(T item) => inner.Remove(item);

        public int RemoveWhere(Predicate<T> match) => inner.RemoveWhere(match);

        public bool SetEquals(IEnumerable<T> other) => inner.SetEquals(other);

        public void SymmetricExceptWith(IEnumerable<T> other) => inner.SymmetricExceptWith(other);

        public void TrimExcess() => inner.TrimExcess();

        public void UnionWith(IEnumerable<T> other) => inner.UnionWith(other);

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            if(inner == null)
                inner = new HashSet<T>();
            else
                inner.Clear();
        }
    }
}
