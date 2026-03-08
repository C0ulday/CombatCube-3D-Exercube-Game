namespace Exercube
{
    using UnityEngine;
    using Koboldgames.Primitives.Variables;

    [CreateAssetMenu(fileName = "SharedCamera.asset", menuName = "Exercube/Primitives/Variables/Camera", order = 100)]
    public sealed class SharedCamera : SharedVariableBase<Camera>, IReadableVariable<Camera>, IWriteableVariable<Camera>
    {
        /// <summary>
        /// Gets or sets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public Camera Value
        {
            get { return value; }
            set { base.value = value; }
        }

        /// <summary>
        /// Resets the value of the shared variablee.
        /// </summary>
        public void Reset() => value = null;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;
            Reset();
        }

        #region Comparison And Equality

        /// <summary>
        /// Compares this instance to a specified Camera variable and returns a new integer that
        /// indicates whether the value of this instance is less than, equal to, or greater than the
        /// value of the specified Camera variable.
        /// </summary>
        /// <param name="other">An Camera variable number to compare.</param>
        /// <returns>
        /// A signed number that indicates whether this instance precedes, follows, or appears in
        /// the same position in the sort order as the <c>other</c> parameter.
        /// </returns>
        public override int CompareTo(Camera other)
        {
            int id1 = value.GetHashCode();
            int id2 = other.GetHashCode();

            return id2 - id1;
        }

        /// <summary>
        /// Returns a value indicating whether this instance and a specified Camera variable
        /// represent the same value.
        /// </summary>
        /// <returns><c>true</c> if <c>other</c> equals the value of this instance; otherwise <c>false</c>.</returns>
        public override bool Equals(Camera other) => value.Equals(other);

        #endregion
    }
}
