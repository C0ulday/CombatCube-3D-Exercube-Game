using UnityEngine;
using TMPro;

namespace Sphery.ExerCube
{
    [RequireComponent(typeof(Canvas))]
    public class ProjectionCalibration : MonoBehaviour
    {
        [SerializeField] protected GameObject calibrationPanel;
        [SerializeField] protected TMP_Text calibrationText;
        [SerializeField] protected GameObject[] calibrationStatePanels;
        [SerializeField] protected bool activateOnStart = false;

        [Header("Input")]
        [SerializeField] protected KeyCode activateKeyCode = KeyCode.F2;
        [SerializeField] protected KeyCode deactivateKeyCode = KeyCode.Tab;
        [SerializeField] protected KeyCode rightKeyCode = KeyCode.RightArrow;
        [SerializeField] protected KeyCode leftKeyCode = KeyCode.LeftArrow;
        [SerializeField] protected KeyCode upKeyCode = KeyCode.UpArrow;
        [SerializeField] protected KeyCode downKeyCode = KeyCode.DownArrow;
        [SerializeField] protected KeyCode submitKeyCode = KeyCode.Return;

        protected Canvas canvas;
        protected ICameraRotator rotator;

        private bool active = false;
        private bool previousActive = false;
        private Display activeCamera;
        private CalibrationState currentState;

        [Header("Displays")]
        public Display left;
        public Display front;
        public Display right;

        /// <summary>
        /// The calibration states for each camera.
        /// </summary>
        protected enum CalibrationState
        {
            Menu = 0,
            ManipulateUpper,
            ManipulateLower,
            MoveImage,
            ManipulateRight,
            ManipulateLeft
        }

        /// <summary>
        /// Gets a value indicating if the calibration is currently active.
        /// </summary>
        /// <value><c>true</c> if calibration is currently active; otherwise <c>false</c>.</value>
        public bool IsActive => active;

        /// <summary>
        /// Activate the screen calibration.
        /// </summary>
        public void Activate()
        {
            if (active)
                return;

            calibrationPanel.SetActive(true);
            SelectDisplay(front);
            currentState = CalibrationState.Menu;
            ShowStatePanel(0);

            active = true;
            previousActive = false;
        }

        /// <summary>
        /// Deactivate the screen calibration and save the last active screen.
        /// </summary>
        public void Deactivate()
        {
            calibrationPanel.SetActive(false);
            SaveRect(activeCamera);
            active = false;
        }

        /// <summary>
        ///  Completely resets the screen calibration.
        /// </summary>
        [ContextMenu("Reset Calibration")]
        public void ResetCalibration()
        {
            Rect resetRect = new Rect(0f, 0f, 1f, 1f);

            left.cameraRect = resetRect;
            SaveRect(left);

            front.cameraRect = resetRect;
            SaveRect(front);

            right.cameraRect = resetRect;
            SaveRect(right);
        }

        /// <summary>
        /// Switch canvas camera in order to render the panel on a specific display.
        /// </summary>
        /// <param name="displayIdentifier">The display identifier of the camera.</param>
        protected void SelectDisplay(Display displayIdentifier)
        {
            calibrationText.text = "Calibrate " + displayIdentifier.displayName;
            activeCamera = displayIdentifier;
            canvas.worldCamera = GetCamera(displayIdentifier);
        }

        /// <summary>
        /// Move to the next calibration step.
        /// </summary>
        protected void NextStep()
        {
            if (!active)
                return;

            int newStateIndex = (int)currentState + 1;

            // Still in one of the calibration state
            if (newStateIndex <= (int)CalibrationState.ManipulateLeft)
            {
                currentState = (CalibrationState)newStateIndex;

                // Check if automatic ratio calculation is needed
                if (currentState == CalibrationState.MoveImage)
                    SetRatio(1f);

                ShowStatePanel(newStateIndex);
                return;
            }

            // Go to the menu
            currentState = CalibrationState.Menu;
            SaveRect(activeCamera);
            SelectDisplay(front);
            ShowStatePanel(0);
        }

        /// <summary>
        /// Set the camera rect to a given ratio from the current height of the screen.
        /// This will only set the rect width.
        /// </summary>
        /// <param name="ratio">The adjust ratio.</param>
        protected void SetRatio(float ratio)
        {
            Camera cam = GetCamera(activeCamera);
            Rect camRect = cam.rect;
            float newWidth = ratio * camRect.height * Screen.height;

            camRect.width = newWidth / Screen.width;
            cam.rect = camRect;
        }

        /// <summary>
        /// Translate the camera rect of the current camera.
        /// </summary>
        /// <param name="x">The horizontal translate position.</param>
        /// <param name="y">The vertical translate position.</param>
        protected void TranslateRect(float x, float y)
        {
            Camera cam = GetCamera(activeCamera);
            Rect camRect = cam.rect;

            camRect.x = Mathf.Clamp(camRect.x + x, 0f, 1f - camRect.width);
            camRect.y = Mathf.Clamp(camRect.y + y, 0f, 1f - camRect.height);
            cam.rect = camRect;
        }

        /// <summary>
        /// Translate the upper border of the current camera rect.
        /// </summary>
        /// <param name="y">The vertical translation.</param>
        protected void TranslateUpperBorder(float y)
        {
            Camera cam = GetCamera(activeCamera);
            Rect camRect = cam.rect;

            camRect.height = Mathf.Clamp(camRect.height + y, camRect.y, 1f - camRect.y);
            cam.rect = camRect;
        }

        /// <summary>
        /// Translate the lower border of the current camera rect.
        /// </summary>
        /// <param name="y">The vertical translation.</param>
        protected void TranslateLowerBorder(float y)
        {
            Camera cam = GetCamera(activeCamera);
            Rect camRect = cam.rect;
            float newPos = Mathf.Clamp(camRect.y + y, 0f, camRect.y + camRect.height);
            float diff = newPos - camRect.y;

            camRect.y = newPos;
            camRect.height -= diff;
            cam.rect = camRect;
        }

        /// <summary>
        /// Translate the right border of the current camera rect.
        /// </summary>
        /// <param name="x">The horizontal translation.</param>
        protected void TranslateRightBorder(float x)
        {
            Camera cam = GetCamera(activeCamera);
            Rect camRect = cam.rect;

            camRect.width = Mathf.Clamp(camRect.width + x, 0f, 1f - camRect.x);
            cam.rect = camRect;
        }

        /// <summary>
        /// Translate the left border of the current camera rect.
        /// </summary>
        /// <param name="x">The horizontal translation.</param>
        protected void TranslateLeftBorder(float x)
        {
            Camera cam = GetCamera(activeCamera);
            Rect camRect = cam.rect;
            float newPos = Mathf.Clamp(camRect.x + x, 0f, camRect.x + camRect.width);
            float diff = newPos - camRect.x;

            camRect.x = newPos;
            camRect.width -= diff;
            cam.rect = camRect;
        }

        /// <summary>
        /// Find the reference to the camera by providing a display identifier.
        /// </summary>
        /// <param name="displayIdentifier">The display identifier of the camera.</param>
        /// <returns>The reference to the corresponding camera.</returns>
        protected Camera GetCamera(Display displayIdentifier)
        {
            if (displayIdentifier == right)
                return rotator.Right;
            else if (displayIdentifier == left)
                return rotator.Left;
            else
                return rotator.Front;
        }

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;

            GameObject camera = GameObject.FindWithTag("MainCamera");

            if (camera != null)
            {
                rotator = camera.GetComponent<CameraRotator>();

                if (rotator == null)
                {
                    Debug.LogError("[ProjectionCalibration] No camera rotator component found on main camera rig!");
                    enabled = false;
                }
            }
            else
            {
                Debug.LogError("[ProjectionCalibration] No main camera rig found in scene!");
                enabled = false;
            }
        }

        /// <summary>
        /// Activate a specifc state panel.
        /// </summary>
        /// <param name="index">The index of the state panel.</param>
        private void ShowStatePanel(int index)
        {
            for (int i = 0; i < calibrationStatePanels.Length; i++)
                calibrationStatePanels[i].SetActive(i == index);
        }

        /// <summary>
        /// Save the rect of a specified display.
        /// </summary>
        /// <param name="displayIdentifier">The index of the display to save the rect from.</param>
        private void SaveRect(Display displayIdentifier)
        {
            displayIdentifier.cameraRect = GetCamera(displayIdentifier).rect;
            displayIdentifier.Save();
        }

        /// <summary>
        /// Load the rect of a specified display.
        /// </summary>
        /// <param name="displayIdentifier">The index of the display to load the rect for.</param>
        private void LoadRect(Display displayIdentifier)
        {
            displayIdentifier.Load();
            GetCamera(displayIdentifier).rect = displayIdentifier.cameraRect;
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            // Load camera rects
            LoadRect(left);
            LoadRect(front);
            LoadRect(right);

            if (activateOnStart)
                Activate();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            if (!active)
            {
                if (Input.GetKeyDown(activateKeyCode))
                    Activate();
                else
                    return;
            }

            // What to manipulate/translate?
            switch (currentState)
            {
                case CalibrationState.ManipulateUpper:
                    if (Input.GetKey(upKeyCode))
                        TranslateUpperBorder(0.05f * Time.deltaTime);
                    else if (Input.GetKey(downKeyCode))
                        TranslateUpperBorder(-0.05f * Time.deltaTime);
                    break;

                case CalibrationState.ManipulateLower:
                    if (Input.GetKey(upKeyCode))
                        TranslateLowerBorder(0.05f * Time.deltaTime);
                    else if (Input.GetKey(downKeyCode))
                        TranslateLowerBorder(-0.05f * Time.deltaTime);
                    break;

                case CalibrationState.MoveImage:
                    if (Input.GetKey(rightKeyCode))
                        TranslateRect(0.05f * Time.deltaTime, 0f);
                    else if (Input.GetKey(leftKeyCode))
                        TranslateRect(-0.05f * Time.deltaTime, 0f);
                    break;

                case CalibrationState.ManipulateRight:
                    if (Input.GetKey(rightKeyCode))
                        TranslateRightBorder(0.05f * Time.deltaTime);
                    else if (Input.GetKey(leftKeyCode))
                        TranslateRightBorder(-0.05f * Time.deltaTime);
                    break;

                case CalibrationState.ManipulateLeft:
                    if (Input.GetKey(rightKeyCode))
                        TranslateLeftBorder(0.05f * Time.deltaTime);
                    else if (Input.GetKey(leftKeyCode))
                        TranslateLeftBorder(-0.05f * Time.deltaTime);
                    break;

                default:
                    if (Input.GetKeyDown(rightKeyCode))
                    {
                        if (activeCamera == left)
                            SelectDisplay(front);
                        else if (activeCamera == front)
                            SelectDisplay(right);
                    }
                    else if (Input.GetKeyDown(leftKeyCode))
                    {
                        if (activeCamera == right)
                            SelectDisplay(front);
                        else if (activeCamera == front)
                            SelectDisplay(left);
                    }
                    else if (Input.GetKeyDown(deactivateKeyCode))
                        Deactivate();
                    break;
            }

            // Next step
            if (Input.GetKeyDown(submitKeyCode) && previousActive)
                NextStep();

            // Delay one frame
            // Preventing double-tap of the <return> key.
            if (!previousActive)
                previousActive = true;
        }
    }
}
