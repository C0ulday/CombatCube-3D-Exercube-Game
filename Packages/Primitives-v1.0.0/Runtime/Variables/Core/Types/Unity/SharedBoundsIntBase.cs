namespace Koboldgames.Primitives.Variables
{
    using UnityEngine;

    public abstract class SharedBoundsIntBase : SharedBoundsVariable<BoundsInt>
    {
        #region Comparison And Equality

        /// <summary>
        /// Compares this instance to a specified box boundary and returns a new integer that
        /// indicateswhether the value of this instance is less than, equal to, or greater than the
        /// value of the specified box boundary.
        /// </summary>
        /// <param name="other">A box boundary to compare.</param>
        /// <returns>
        /// A signed number that indicates whether this instance precedes, follows, or appears in
        /// the same position in the sort order as the <c>other</c> parameter.
        /// </returns>
        public override int CompareTo(BoundsInt other) => GetVolume(value).CompareTo(GetVolume(other));

        /// <summary>
        /// Returns a value indicating whether this instance and a specified box boundary represent
        /// the same value.
        /// </summary>
        /// <returns><c>true</c> if <c>other</c> equals the value of this instance; otherwise <c>false</c>.</returns>
        public override bool Equals(BoundsInt other) => value.Equals(other);

        /// <summary>
        /// Get the volume of a box boundary.
        /// </summary>
        /// <param name="bounds">The box boundary to be calculated.</param>
        /// <returns>The volume of the box boundary.</returns>
        protected int GetVolume(BoundsInt bounds) => bounds.size.x * bounds.size.y * bounds.size.z;

        #endregion
    }
}
