namespace Koboldgames.Primitives.Variables
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public abstract class SharedVariableBase<T> : ScriptableObject, ISharedVariable, IComparable, IComparable<T>, IEquatable<T>
    {
        [SerializeField, Multiline] protected string description = String.Empty;
        [SerializeField] protected T value = default(T);

        /// <summary>
        /// Gets the description of this variable.
        /// </summary>
        /// <value>The description of this variable.</value>
        public string Description => description;

        /// <summary>
        /// Checks if two instances of a shared variable are valid (not <c>null</c>).
        /// </summary>
        /// <param name="variable1">The first variable to check.</param>
        /// <param name="variable2">The second variable to check.</param>
        /// <returns><c>true</c> if both of the variables ar valid instances; otherwise <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static bool AreValid(SharedVariableBase<T> variable1, SharedVariableBase<T> variable2) =>
            (variable1 != null) && (variable2 != null);

        #region Comparison And Equality

        /// <summary>
        /// Compares this instance with a specified object and indicates whether this instance
        /// precedes, follows, or appears in the same position in the sort order as the specified
        /// object.
        /// </summary>
        /// <param name="obj">An object that evaluates to <see cref="T"/>.</param>
        /// <returns>
        /// A signed number that indicates whether this instance precedes, follows, or appears in
        /// the same position in the sort order as the <c>obj</c> parameter.
        /// </returns>
        public int CompareTo(object obj)
        {
            var v = obj as SharedVariableBase<T>;

            if(v == null)
                return 1;

            if((value == null) && (v.value == null))
                return 0;

            if(value.Equals(v.value))
                return 0;

            return -1;
        }

        public abstract int CompareTo(T other);

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns><c>true</c> if the specified object is equal to the current object; otherwise <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            var v = obj as SharedVariableBase<T>;

            if(v == null)
                return false;

            if((value == null) && (v.value == null))
                return true;

            return value.Equals(v.value);
        }

        public abstract bool Equals(T other);

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => value == null ? 0 : value.GetHashCode();

        #endregion

        #region Convert/Casting Overloads

        public static implicit operator T(SharedVariableBase<T> variable) =>
            variable != null ? variable.value : default(T);

        #endregion
    }
}
