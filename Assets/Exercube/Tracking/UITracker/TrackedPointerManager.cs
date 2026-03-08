using UnityEngine;
using UnityEngine.EventSystems;

namespace Sphery.ExerCube.UI
{
    public class TrackedPointerManager : MonoBehaviour
    {
        public TrackerPointerProviderBase pointerProvider;

        public TrackedPointerCursor[] cursors;

        public Vector3 offset = Vector3.up * 30f;

        private TrackedElement _target;

        public float timeToActivate = 2f;
        private float currentTimeToActivate = 0f;

        public float canvasAlphaSpeed = 0.5f;
        private float currentAlpha = 0f;

        private void LateUpdate()
        {
            // Reset data
            TrackedElement oldTarget = _target;
            _target = null;

            // Check for new submit handler
            PointerEventData pointerData = new PointerEventData(EventSystem.current);

            Ray pointerRay = new Ray(pointerProvider.GetPosition(), pointerProvider.GetDirection());

            foreach (TrackedPointerCursor cursor in cursors)
            {
                if (RayToScreenPoint(cursor.Canvas.worldCamera, pointerRay, out Vector3 screenPoint))
                {
                    screenPoint += offset;
                    cursor.SetPositionFromScreenPoint(screenPoint);
                    pointerData.position = screenPoint;

                    // Only try to set if none was found already
                    if (_target == null)
                        _target = TrackedCanvas.GetTrackedElement(cursor.Canvas.worldCamera, screenPoint);
                }
            }

            // Handle hovering
            if (pointerProvider.IsPointing() && oldTarget != null && oldTarget == _target)
                currentTimeToActivate += Time.deltaTime;
            else
                currentTimeToActivate = 0f;

            // Handle submit
            if (currentTimeToActivate >= timeToActivate)
            {
                _target.Submit(pointerData);
                currentTimeToActivate = 0f;
            }

            // Update cursor visuals
            currentAlpha = Mathf.MoveTowards(currentAlpha, pointerProvider.IsPointing() ? 1f : 0f, canvasAlphaSpeed * Time.deltaTime);
            float fillAmount = currentTimeToActivate / timeToActivate;

            foreach (TrackedPointerCursor cursor in cursors)
            {
                cursor.Pointer = _target?.pointer;
                cursor.Alpha = currentAlpha;
                cursor.FillAmount = fillAmount;
            }
        }

        protected bool RayToScreenPoint(Camera camera, Ray ray, out Vector3 screenPoint)
        {
            // Calculate camera far plane
            Plane wallPlaneForCamera = new Plane
            (
                -camera.transform.forward,
                camera.transform.position + camera.transform.forward * 1.1f // 1.1f is approximate distance to wall
            );

            // Raycast ray onto plane and convert result to screen point
            if (wallPlaneForCamera.Raycast(ray, out float enter))
            {
                Vector3 worldPoint = ray.GetPoint(enter);
                screenPoint = camera.WorldToScreenPoint(worldPoint);
                return true;
            }

            // Raycast failed
            screenPoint = Vector3.zero;
            return false;
        }
    }
}
