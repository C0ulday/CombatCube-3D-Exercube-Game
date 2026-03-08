namespace Koboldgames.Primitives.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using UnityEngine;

    public abstract class SharedListBase<T> : ScriptableObject, IList<T>, IList, IReadOnlyList<T>
    {
        [SerializeField, Multiline] protected string description;
        [SerializeField] protected int initialCapacity;
        [NonSerialized] protected List<T> inner;

        /// <summary>
        /// Gets the description of this list.
        /// </summary>
        /// <value>The description of this list.</value>
        public string Description => description;

        public int Count => inner.Count;
        public int Capacity
        {
            get { return inner.Capacity; }
            set { inner.Capacity = value; }
        }

        bool IList.IsFixedSize => ((IList)inner).IsFixedSize;
        bool ICollection<T>.IsReadOnly => ((ICollection<T>)inner).IsReadOnly;
        bool IList.IsReadOnly => ((IList)inner).IsReadOnly;
        bool ICollection.IsSynchronized => ((ICollection)inner).IsSynchronized;
        object ICollection.SyncRoot => ((ICollection)inner).SyncRoot;

        public T this[int index]
        {
            get { return inner[index]; }
            set { inner[index] = value; }
        }

        object IList.this[int index]
        {
            get { return ((IList)inner)[index]; }
            set { ((IList)inner)[index] = value; }
        }

        public void Add(T item) => inner.Add(item);

        int IList.Add(object value) => ((IList)inner).Add(value);

        public void AddRange(IEnumerable<T> collection) => inner.AddRange(collection);

        public ReadOnlyCollection<T> AsReadOnly() => inner.AsReadOnly();

        public int BinarySearch(int index, int count, T item, IComparer<T> comparer) => inner.BinarySearch(index, count, item, comparer);

        public int BinarySearch(T item) => inner.BinarySearch(item);

        public int BinarySearch(T item, IComparer<T> comparer) => inner.BinarySearch(item, comparer);

        public void Clear() => inner.Clear();

        public bool Contains(T item) => inner.Contains(item);

        bool IList.Contains(object value) => ((IList)inner).Contains(value);

        public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter) => inner.ConvertAll(converter);

        public void CopyTo(T[] array) => inner.CopyTo(array);

        void ICollection.CopyTo(Array array, int arrayIndex) => ((ICollection)inner).CopyTo(array, arrayIndex);

        public void CopyTo(int index, T[] array, int arrayIndex, int count) => inner.CopyTo(index, array, arrayIndex, count);

        public void CopyTo(T[] array, int arrayIndex) => inner.CopyTo(array, arrayIndex);

        public bool Exists(Predicate<T> match) => inner.Exists(match);

        public T Find(Predicate<T> match) => inner.Find(match);

        public List<T> FindAll(Predicate<T> match) => inner.FindAll(match);

        public int FindIndex(Predicate<T> match) => inner.FindIndex(match);

        public int FindIndex(int startIndex, Predicate<T> match) => inner.FindIndex(startIndex, match);

        public int FindIndex(int startIndex, int count, Predicate<T> match) => inner.FindIndex(startIndex, count, match);

        public T FindLast(Predicate<T> match) => inner.FindLast(match);

        public int FindLastIndex(Predicate<T> match) => inner.FindLastIndex(match);

        public int FindLastIndex(int startIndex, Predicate<T> match) => inner.FindLastIndex(startIndex, match);

        public int FindLastIndex(int startIndex, int count, Predicate<T> match) => inner.FindLastIndex(startIndex, count, match);

        public void ForEach(Action<T> action) => inner.ForEach(action);

        public List<T>.Enumerator GetEnumerator() => inner.GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => ((IEnumerable<T>)inner).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)inner).GetEnumerator();

        public List<T> GetRange(int index, int count) => inner.GetRange(index, count);

        public int IndexOf(T item) => inner.IndexOf(item);

        int IList.IndexOf(object item) => ((IList)inner).IndexOf(item);

        public int IndexOf(T item, int index) => inner.IndexOf(item, index);

        public int IndexOf(T item, int index, int count) => inner.IndexOf(item, index, count);

        public void Insert(int index, T item) => inner.Insert(index, item);

        void IList.Insert(int index, object value) => ((IList)inner).Insert(index, value);

        public void InsertRange(int index, IEnumerable<T> collection) => inner.InsertRange(index, collection);

        public int LastIndexOf(T item) => inner.LastIndexOf(item);

        public int LastIndexOf(T item, int index) => inner.LastIndexOf(item, index);

        public int LastIndexOf(T item, int index, int count) => inner.LastIndexOf(item, index, count);

        public bool Remove(T item) => inner.Remove(item);

        void IList.Remove(object value) => ((IList)inner).Remove(value);

        public int RemoveAll(Predicate<T> match) => inner.RemoveAll(match);

        public void RemoveAt(int index) => inner.RemoveAt(index);

        public void RemoveRange(int index, int count) => inner.RemoveRange(index, count);

        public void Reverse() => inner.Reverse();

        public void Reverse(int index, int count) => inner.Reverse(index, count);

        public void Sort() => inner.Sort();

        public void Sort(IComparer<T> comparer) => inner.Sort(comparer);

        public void Sort(int index, int count, IComparer<T> comparer) => inner.Sort(index, count, comparer);

        public void Sort(Comparison<T> comparison) => inner.Sort(comparison);

        public T[] ToArray() => inner.ToArray();

        public void TrimExcess() => inner.TrimExcess();

        public bool TrueForAll(Predicate<T> match) => inner.TrueForAll(match);

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            if(inner == null)
                inner = new List<T>(initialCapacity);
            else
                inner.Clear();
        }
    }
}
