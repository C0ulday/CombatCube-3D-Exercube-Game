using UnityEngine;

namespace Sphery.ExerCube
{
    public class MultiDisplay : MonoBehaviour
    {
        public int displayCount = 1;
        public bool visibleCursor = true;
        public bool showCursorOnMovement = true;
        public float cursorCheckInterval = 0.05f;
        public float cursorMovementTolerance = 10f;
        public float hideAfter = 2f;

        private Vector3 previousCursorPosition;
        private float time;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            if (!Screen.fullScreen)
            {
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Screen.fullScreen = true;
            }

            Cursor.visible = visibleCursor;
            Cursor.lockState = CursorLockMode.None;
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            int count = Mathf.Min(UnityEngine.Display.displays.Length, displayCount < 1 ? 1 : displayCount);

            for (int i = 0; i < count; i++)
            {
                if (!UnityEngine.Display.displays[i].active)
                    UnityEngine.Display.displays[i].Activate();
            }
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            if (visibleCursor || !showCursorOnMovement)
                return;

            if (!Cursor.visible && (time >= cursorCheckInterval))
            {
                float dist = Vector3.Distance(Input.mousePosition, previousCursorPosition);

                if (!Mathf.Approximately(dist, 0f) && (dist < cursorMovementTolerance))
                    Cursor.visible = true;

                previousCursorPosition = Input.mousePosition;
                time = 0f;
            }
            else if (Cursor.visible)
            {
                if (Input.mousePosition != previousCursorPosition)
                    time = 0f;

                Cursor.visible = time < hideAfter;
                previousCursorPosition = Input.mousePosition;
            }

            time += Time.deltaTime;
        }
    }
}
