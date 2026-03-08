namespace Koboldgames.Primitives.Variables
{
    using UnityEngine;

    public abstract class SharedQuaternionBase : SharedQuaternionVariable<Quaternion>
    {
        #region Comparison And Equality

        /// <summary>
        /// Compares this instance to a specified quaternion and returns a new integer that
        /// indicates whether the value of this instance is less than, equal to, or greater than the
        /// value of the specified quaternion.
        /// </summary>
        /// <param name="other">A quaternion to compare.</param>
        /// <returns>
        /// A signed number that indicates whether this instance precedes, follows, or appears in
        /// the same position in the sort order as the <c>other</c> parameter.
        /// </returns>
        public override int CompareTo(Quaternion other) =>
            value.eulerAngles.magnitude.CompareTo(other.eulerAngles.magnitude);

        /// <summary>
        /// Returns a value indicating whether this instance and a specified quaternion represent
        /// the same value.
        /// </summary>
        /// <returns><c>true</c> if <c>other</c> equals the value of this instance; otherwise <c>false</c>.</returns>
        public override bool Equals(Quaternion other) => value.Equals(other);

        #endregion
    }
}
