using UnityEngine;

namespace Sphery.ExerCube
{
    [RequireComponent(typeof(Camera))]
    public class CameraRenderTarget : MonoBehaviour
    {
        [SerializeField] protected Display identifier;

        protected new Camera camera;

        /// <summary>
        /// Switch render target with another one.
        /// Is ignored if render target unknown.
        /// </summary>
        /// <param name="target1">The first render target.</param>
        /// <param name="target2">The second render target.</param>
        public void SwitchRenderTarget(Display target1, Display target2)
        {
            if (identifier == target1)
                camera.targetDisplay = target2.targetDisplay;
            else if (identifier == target2)
                camera.targetDisplay = target1.targetDisplay;
        }

        /// <summary>
        /// Set the camera render target.
        /// </summary>
        public void SetRenderTarget()
        {
            camera.targetDisplay = identifier.targetDisplay;
        }

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            camera = GetComponent<Camera>();
            SetRenderTarget();
        }
    }
}
