namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "SharedObservableRect.asset", menuName = "Koboldgames/Primitives/Variables/Observable/Rect", order = 100)]
    public sealed class SharedObservableRect : SharedRectBase, IReadableVariable<Rect>, IWriteableVariable<Rect>
    {
        [SerializeField] private Rect initialValue = default(Rect);

        public event Action<Rect> OnChange;

        /// <summary>
        /// Gets or sets the value of this variable.
        /// </summary>
        /// <value>The value of this variable.</value>
        public Rect Value
        {
            get { return value; }
            set
            {
                if(!base.value.Equals(value))
                {
                    base.value = value;
                    OnChange?.Invoke(value);
                }
            }
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
