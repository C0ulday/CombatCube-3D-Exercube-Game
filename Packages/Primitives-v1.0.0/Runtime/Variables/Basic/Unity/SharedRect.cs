namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedRect.asset", menuName = "Koboldgames/Primitives/Variables/Rect", order = 100)]
    public sealed class SharedRect : SharedRectBase, IReadableVariable<Rect>, IWriteableVariable<Rect>
    {
        [SerializeField] private Rect initialValue = default(Rect);

        /// <summary>
        /// Gets or sets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public Rect Value
        {
            get { return value; }
            set { base.value = value; }
        }

        /// <summary>
        /// Resets the value of the shared variable to its initial value.
        /// </summary>
        public void Reset() => value = initialValue;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable() => Reset();
    }
}
