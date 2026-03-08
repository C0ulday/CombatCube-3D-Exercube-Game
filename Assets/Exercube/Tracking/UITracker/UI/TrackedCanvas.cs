using System.Collections.Generic;
using UnityEngine;

namespace Sphery.ExerCube.UI
{
    [RequireComponent(typeof(Canvas))]
    public class TrackedCanvas : MonoBehaviour
    {
        private static readonly Dictionary<Camera, List<TrackedCanvas>> trackedCanvasPerCamera = new Dictionary<Camera, List<TrackedCanvas>>();

        public Canvas Canvas { get; private set; }

        private List<TrackedElement> trackedElements = new List<TrackedElement>();

        void Start()
        {
            Canvas = GetComponent<Canvas>();

            if (!trackedCanvasPerCamera.ContainsKey(Canvas.worldCamera))
                trackedCanvasPerCamera.Add(Canvas.worldCamera, new List<TrackedCanvas>());

            trackedCanvasPerCamera[Canvas.worldCamera].Add(this);
        }

        public void AddTrackedElement(TrackedElement element)
        {
            if (!trackedElements.Contains(element))
                trackedElements.Add(element);
        }

        public void RemoveTrackedElement(TrackedElement element)
        {
            if (trackedElements.Contains(element))
                trackedElements.Remove(element);
        }

        public TrackedElement GetTrackedElement(Vector2 position)
        {
            foreach (TrackedElement element in trackedElements)
            {
                if (!element.isActiveAndEnabled || !element.Contains(position))
                    continue;

                return element;
            }

            return null;
        }

        public static TrackedElement GetTrackedElement(Camera camera, Vector2 position)
        {
            if (trackedCanvasPerCamera.ContainsKey(camera))
            {
                foreach (TrackedCanvas canvas in trackedCanvasPerCamera[camera])
                {
                    TrackedElement element = canvas.GetTrackedElement(position);

                    if (element != null)
                        return element;
                }
            }

            return null;
        }
    }
}
