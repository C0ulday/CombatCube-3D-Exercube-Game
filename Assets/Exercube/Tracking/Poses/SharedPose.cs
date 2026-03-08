using UnityEngine;
using Koboldgames.Primitives.Variables;

namespace Sphery.ExerCube
{
    [CreateAssetMenu(fileName = "SharedPose.asset", menuName = "Exercube/Primitives/Variables/Pose", order = 100)]
    public class SharedPose : SharedVariableBase<Pose>, IReadableVariable<Pose>, IWriteableVariable<Pose>
    {
        /// <summary>
        /// Gets or sets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public Pose Value
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
        /// Compares this instance to a specified pose variable and returns a new integer that
        /// indicates whether the value of this instance is less than, equal to, or greater than the
        /// value of the specified pose variable.
        /// </summary>
        /// <param name="other">A pose variable number to compare.</param>
        /// <returns>
        /// A signed number that indicates whether this instance precedes, follows, or appears in
        /// the same position in the sort order as the <c>other</c> parameter.
        /// </returns>
        public override int CompareTo(Pose other)
        {
            int id1 = value.GetHashCode();
            int id2 = other.GetHashCode();

            return id2 - id1;
        }

        /// <summary>
        /// Returns a value indicating whether this instance and a specified pose variable
        /// represent the same value.
        /// </summary>
        /// <returns><c>true</c> if <c>other</c> equals the value of this instance; otherwise <c>false</c>.</returns>
        public override bool Equals(Pose other) => value.Equals(other);

        #endregion
    }
}
