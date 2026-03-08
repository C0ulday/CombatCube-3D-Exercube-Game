using UnityEngine;
using UnityEngine.UI;

namespace Sphery.ExerCube
{
    [RequireComponent(typeof(Image))]
    public class PunchIndicator : MonoBehaviour
    {
        public Color punchColor = Color.red;
        public Color defaultColor = Color.white;

        public float fadeOutTime = 1f;

        private Image image;
        private float timer;

        private void Awake()
        {
            image = GetComponent<Image>();
            image.color = defaultColor;

            timer = 0f;
        }

        private void Update()
        {
            timer = Mathf.Max(timer - Time.deltaTime, 0f);
            image.color = Color.Lerp(defaultColor, punchColor, timer / fadeOutTime);
        }

        [ContextMenu("Punch")]
        public void Punch()
        {
            timer = fadeOutTime;
        }
    }
}
