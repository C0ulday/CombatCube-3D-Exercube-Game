using UnityEngine;
using UnityEngine.UI;

namespace Sphery.ExerCube.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class TrackedPointerCursor : MonoBehaviour
    {
        [SerializeField]
        protected RectTransform cursor;

        [Space]
        [SerializeField]
        protected CanvasGroup canvasGroup;

        [Space]
        [SerializeField]
        protected Image timeIndicator;
        [SerializeField]
        protected Image pointer;

        protected RectTransform rectTransform;

        public Canvas Canvas { get; protected set; }

        public Sprite Pointer
        {
            get => pointer.sprite;
            set
            {
                pointer.sprite = value;
                pointer.enabled = value != null;
            }
        }

        public float FillAmount
        {
            get => timeIndicator.fillAmount;
            set => timeIndicator.fillAmount = value;
        }

        public float Alpha
        {
            get => canvasGroup.alpha;
            set => canvasGroup.alpha = value;
        }

        private void Start()
        {
            rectTransform = transform.parent.GetComponent<RectTransform>();
            Canvas = GetComponentInParent<Canvas>();
        }

        public void SetPositionFromScreenPoint(Vector3 screenPoint)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, Canvas.worldCamera, out Vector2 localPoint))
                cursor.anchoredPosition = localPoint;
        }
    }
}
