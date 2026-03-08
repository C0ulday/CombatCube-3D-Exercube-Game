namespace Koboldgames.Primitives.Variables
{
    using UnityEngine;

    public abstract class SharedObjectBase : SharedVariableBase<Object>
    {
        #region Comparison And Equality

        /// <summary>
        /// Compares this instance to a specified Object variable and returns a new integer that
        /// indicates whether the value of this instance is less than, equal to, or greater than the
        /// value of the specified Object variable.
        /// </summary>
        /// <param name="other">An Object variable number to compare.</param>
        /// <returns>
        /// A signed number that indicates whether this instance precedes, follows, or appears in
        /// the same position in the sort order as the <c>other</c> parameter.
        /// </returns>
        public override int CompareTo(Object other)
        {
            int id1 = value.GetHashCode();
            int id2 = other.GetHashCode();

            return id2 - id1;
        }

        /// <summary>
        /// Returns a value indicating whether this instance and a specified Object variable
        /// represent the same value.
        /// </summary>
        /// <returns><c>true</c> if <c>other</c> equals the value of this instance; otherwise <c>false</c>.</returns>
        public override bool Equals(Object other) => value.Equals(other);

        #endregion
    }
}
