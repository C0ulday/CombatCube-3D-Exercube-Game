using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Sphery.ExerCube
{
    [RequireComponent(typeof(Canvas))]
    public class DisplaySelection : MonoBehaviour
    {
        [SerializeField] protected Display[] identifiers;
        [SerializeField] protected GameObject panel;
        [SerializeField] protected TMP_Text text;
        [SerializeField] protected bool activateOnAwake;

        [Header("Input")]
        [SerializeField] protected KeyCode activateKeyCode = KeyCode.F1;
        [SerializeField] protected KeyCode rightKeyCode = KeyCode.RightArrow;
        [SerializeField] protected KeyCode leftKeyCode = KeyCode.LeftArrow;
        [SerializeField] protected KeyCode submitKeyCode = KeyCode.Return;

        protected Canvas canvas;

        private bool active = false;
        private bool previousActive = false;
        private int index = 0;
        private int scrollIndex = 0;
        private List<int> displayList;

        /// <summary>
        /// Gets a value indicating if the display selection is currently active.
        /// </summary>
        /// <value><c>true</c> if selection is currently active; otherwise <c>false</c>.</value>
        public bool IsActive => active;

        /// <summary>
        /// Activate display selection.
        /// </summary>
        public void Activate()
        {
            if (active)
                return;

            if (displayList == null)
                displayList = new List<int>(identifiers.Length);

            displayList.Clear();

            // Prefill list
            for (int i = 0; i < identifiers.Length; i++)
                displayList.Add(i);

            panel.SetActive(true);
            index = 0;
            canvas.targetDisplay = 0;
            text.text = identifiers[0].displayName;

            active = true;
            previousActive = false;
        }

        /// <summary>
        /// Deactivate and quit display selection.
        /// </summary>
        public void Deactivate()
        {
            panel.SetActive(false);
            active = false;

            // We go with this here...
            // It's simple, it works, it performs awfully but we're doing setup here anyway.
            CameraRenderTarget[] crt = Object.FindObjectsOfType<CameraRenderTarget>();

            // Reinitialize all camera render targets
            for (int i = 0; i < crt.Length; i++)
                crt[i].SetRenderTarget();
        }

        /// <summary>
        /// Invert the render targets.
        /// </summary>
        /// <param name="displayCount">Process only with a specific display count.</param>
        public void InvertRenderTargets(int displayCount = 0)
        {
#if UNITY_EDITOR
            if (displayCount != 1)
#else
            if((displayCount > 0) && (displayCount != UnityEngine.Display.displays.Length))
#endif
            {
                return;
            }

            CameraRenderTarget[] crt = Object.FindObjectsOfType<CameraRenderTarget>();
            int highest = identifiers.Length;

            // Reinitialize all camera render targets
            for (int i = 0; i < crt.Length; i++)
            {
                int low = 0;
                int high = highest - 1;

                // Switch inverted
                while (high > low)
                {
                    crt[i].SwitchRenderTarget(identifiers[low], identifiers[high]);
                    low++;
                    high--;
                }
            }
        }

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            if (panel == null)
                Debug.LogError("[DisplaySelection] No UI panel assigned!");


            if ((identifiers == null) || (identifiers.Length == 0))
                Debug.LogError("[DisplaySelection] No display identifiers assigned!");

            foreach (Display displayIdentifier in identifiers)
                displayIdentifier.Load();

            canvas = GetComponent<Canvas>();

            // Activate on awake
            if (activateOnAwake)
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

            // Selection through keyboard
            if (Input.GetKeyDown(rightKeyCode))
            {
                scrollIndex++;

                if (scrollIndex >= displayList.Count)
                    scrollIndex = 0;

                text.text = identifiers[displayList[scrollIndex]].displayName;
            }
            else if (Input.GetKeyDown(leftKeyCode))
            {
                scrollIndex--;

                if (scrollIndex < 0)
                    scrollIndex = displayList.Count - 1;

                text.text = identifiers[displayList[scrollIndex]].displayName;
            }
            else if (Input.GetKeyDown(submitKeyCode) && previousActive)
            {
                identifiers[displayList[scrollIndex]].targetDisplay = index;
                identifiers[displayList[scrollIndex]].Save();
                displayList.RemoveAt(scrollIndex);
                index++;

#if UNITY_EDITOR
                if (index < identifiers.Length)
#else
                if(index < Mathf.Min(identifiers.Length, UnityEngine.Display.displays.Length))
#endif
                {
                    canvas.targetDisplay = index;
                    scrollIndex = 0;
                    text.text = identifiers[displayList[scrollIndex]].displayName;
                }
                else
                {
                    // Saved all displays...
                    Deactivate();
                    return;
                }
            }

            // Delay one frame
            // Preventing double-tap of the <return> key.
            if (!previousActive)
                previousActive = true;
        }
    }
}
