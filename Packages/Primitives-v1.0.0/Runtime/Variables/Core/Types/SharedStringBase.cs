namespace Koboldgames.Primitives.Variables
{
    public abstract class SharedStringBase : SharedStringVariable<string>
    {
        #region Comparison And Equality

        /// <summary>
        /// Compares this instance to a specified string and returns a new integer that indicates
        /// whether the value of this instance is less than, equal to, or greater than the value of
        /// the specified string.
        /// </summary>
        /// <param name="other">A string to compare.</param>
        /// <returns>
        /// A signed number that indicates whether this instance precedes, follows, or appears in
        /// the same position in the sort order as the <c>other</c> parameter.
        /// </returns>
        public override int CompareTo(string other) => value.CompareTo(other);

        /// <summary>
        /// Returns a value indicating whether this instance and a specified string represent the
        /// same value.
        /// </summary>
        /// <returns><c>true</c> if <c>other</c> equals the value of this instance; otherwise <c>false</c>.</returns>
        public override bool Equals(string other) => value.Equals(other);

        #endregion
    }
}
