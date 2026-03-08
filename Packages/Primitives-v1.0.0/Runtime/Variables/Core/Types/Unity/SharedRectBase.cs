namespace Koboldgames.Primitives.Variables
{
    using UnityEngine;

    public abstract class SharedRectBase : SharedRectVariable<Rect>
    {
        #region Comparison And Equality

        /// <summary>
        /// Compares this instance to a specified rectangle and returns a new integer that indicates
        /// whether the value of this instance is less than, equal to, or greater than the value of
        /// the specified rectangle.
        /// </summary>
        /// <param name="other">A rectangle to compare.</param>
        /// <returns>
        /// A signed number that indicates whether this instance precedes, follows, or appears in
        /// the same position in the sort order as the <c>other</c> parameter.
        /// </returns>
        public override int CompareTo(Rect other) => GetArea(value).CompareTo(GetArea(other));

        /// <summary>
        /// Returns a value indicating whether this instance and a specified rectangle represent the
        /// same value.
        /// </summary>
        /// <returns><c>true</c> if <c>other</c> equals the value of this instance; otherwise <c>false</c>.</returns>
        public override bool Equals(Rect other) => value.Equals(other);

        /// <summary>
        /// Get the area of a rectangle.
        /// </summary>
        /// <param name="rect">The rectangle to be calculated.</param>
        /// <returns>The area of the rectangle.</returns>
        protected float GetArea(Rect rect) => rect.width * rect.height;

        #endregion
    }
}
