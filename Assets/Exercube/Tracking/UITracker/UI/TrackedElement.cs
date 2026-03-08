using UnityEngine;
using UnityEngine.EventSystems;

namespace Sphery.ExerCube.UI
{
    [RequireComponent(typeof(RectTransform), typeof(ISubmitHandler))]
    public class TrackedElement : MonoBehaviour
    {
        public Sprite pointer;

        private TrackedCanvas trackerCanvas;

        public RectTransform RectTransform { get; private set; }
        public ISubmitHandler SubmitHandler { get; private set; }

        void Start()
        {
            trackerCanvas = GetComponentInParent<TrackedCanvas>();
            if (trackerCanvas == null)
                Debug.LogWarning(nameof(TrackedElement) + " " + name + " needs to be a child of a " + nameof(TrackedCanvas), gameObject);
            else
                trackerCanvas.AddTrackedElement(this);

            RectTransform = GetComponent<RectTransform>();
            if (RectTransform == null)
                Debug.LogError(nameof(RectTransform) + " is required on " + name, gameObject);

            SubmitHandler = GetComponent<ISubmitHandler>();
            if (SubmitHandler == null)
                Debug.LogError(nameof(ISubmitHandler) + " is required on " + name, gameObject);
        }

        private void OnDestroy() =>
            trackerCanvas?.RemoveTrackedElement(this);

        public bool Contains(Vector2 screenPoint) =>
            RectTransformUtility.RectangleContainsScreenPoint(RectTransform, screenPoint, trackerCanvas.Canvas.worldCamera);

        public void Submit(BaseEventData baseEventData) =>
            SubmitHandler?.OnSubmit(baseEventData);
    }
}
