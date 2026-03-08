using System;
using UnityEngine;
using UnityEngine.UI;

// TODO: https://github.com/CiaccoDavide/Unity-UI-Polygon

// TODO: Project transform on axis onto camera plane

namespace Sphery.ExerCube
{
    [RequireComponent(typeof(Canvas))]
    public class TrackerDebugger : MonoBehaviour
    {
        [Serializable]
        public struct TrackerData
        {
            public Transform transform;
            public Color color;
        }

        public RectTransform prefab;
        public Vector3 direction;

        public TrackerData[] trackers = new TrackerData[4]
        {
        new TrackerData() { color = Color.red },
        new TrackerData() { color = Color.green },
        new TrackerData() { color = Color.blue },
        new TrackerData() { color = Color.yellow }
        };

        private Canvas canvas;
        private RectTransform[] instances;

        private void Start()
        {
            canvas = GetComponent<Canvas>();
            instances = new RectTransform[trackers.Length];

            for (int i = 0; i < trackers.Length; i++)
            {
                instances[i] = Instantiate(prefab, transform);
                instances[i].GetComponentInChildren<Image>().color = trackers[i].color;
            }
        }

        private void Update()
        {
            for (int i = 0; i < instances.Length; i++)
            {
                //Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, trackers[i].transform.position);
                //instances[i].anchoredPosition = screenPoint - canvas.GetComponent<RectTransform>().sizeDelta * 0.5f;

                Vector2 viewportPosition = canvas.worldCamera.WorldToViewportPoint(trackers[i].transform.position);
                RectTransform canvasRect = canvas.GetComponent<RectTransform>();

                instances[i].anchoredPosition = new Vector2
                (
                    (viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
                    (viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)
                );
            }
        }
    }
}
