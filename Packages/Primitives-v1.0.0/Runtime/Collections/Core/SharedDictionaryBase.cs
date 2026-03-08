namespace Koboldgames.Primitives.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class SharedDictionaryBase<TKey, TValue> : ScriptableObject, IDictionary<TKey, TValue>, IDictionary, IReadOnlyDictionary<TKey, TValue>
    {
        [SerializeField, Multiline] protected string description;
        [SerializeField] protected int initialCapacity;
        [NonSerialized] protected Dictionary<TKey, TValue> inner;

        /// <summary>
        /// Gets the description of this dictionary.
        /// </summary>
        /// <value>The description of this dictionary.</value>
        public string Description => description;

        public IEqualityComparer<TKey> Comparer => inner.Comparer;
        public int Count => inner.Count;

        bool ICollection.IsSynchronized => ((ICollection)inner).IsSynchronized;
        object ICollection.SyncRoot => ((ICollection)inner).SyncRoot;
        bool IDictionary.IsFixedSize => ((IDictionary)inner).IsFixedSize;
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)inner).IsReadOnly;
        bool IDictionary.IsReadOnly => ((IDictionary)inner).IsReadOnly;

        public Dictionary<TKey, TValue>.KeyCollection Keys => inner.Keys;
        ICollection<TKey> IDictionary<TKey, TValue>.Keys => ((IDictionary<TKey, TValue>)inner).Keys;
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => ((IReadOnlyDictionary<TKey, TValue>)inner).Keys;
        ICollection IDictionary.Keys => ((IDictionary)inner).Keys;

        public Dictionary<TKey, TValue>.ValueCollection Values => inner.Values;
        ICollection<TValue> IDictionary<TKey, TValue>.Values => ((IDictionary<TKey, TValue>)inner).Values;
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => ((IReadOnlyDictionary<TKey, TValue>)inner).Values;
        ICollection IDictionary.Values => ((IDictionary)inner).Values;

        public TValue this[TKey key]
        {
            get { return inner[key]; }
            set { inner[key] = value; }
        }

        object IDictionary.this[object key]
        {
            get { return ((IDictionary)inner)[key]; }
            set { ((IDictionary)inner)[key] = value; }
        }

        public void Add(TKey key, TValue value) => inner.Add(key, value);

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => ((ICollection<KeyValuePair<TKey, TValue>>)inner).Add(item);

        void IDictionary.Add(object key, object value) => ((IDictionary)inner).Add(key, value);

        public void Clear() => inner.Clear();

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair) => ((ICollection<KeyValuePair<TKey, TValue>>)inner).Contains(keyValuePair);

        bool IDictionary.Contains(object key) => ((IDictionary)inner).Contains(key);

        public bool ContainsKey(TKey key) => inner.ContainsKey(key);

        public bool ContainsValue(TValue value) => inner.ContainsValue(value);

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int index) => ((ICollection<KeyValuePair<TKey, TValue>>)inner).CopyTo(array, index);

        void ICollection.CopyTo(Array array, int index) => ((ICollection)inner).CopyTo(array, index);

        public Dictionary<TKey, TValue>.Enumerator GetEnumerator() => inner.GetEnumerator();

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => ((IEnumerable<KeyValuePair<TKey, TValue>>)inner).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)inner).GetEnumerator();

        IDictionaryEnumerator IDictionary.GetEnumerator() => ((IDictionary)inner).GetEnumerator();

        public bool Remove(TKey key) => inner.Remove(key);

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair) => ((ICollection<KeyValuePair<TKey, TValue>>)inner).Remove(keyValuePair);

        void IDictionary.Remove(object key) => ((IDictionary)inner).Remove(key);

        public bool TryGetValue(TKey key, out TValue value) => inner.TryGetValue(key, out value);

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            if(inner == null)
                inner = new Dictionary<TKey, TValue>(initialCapacity);
            else
                inner.Clear();
        }
    }
}
