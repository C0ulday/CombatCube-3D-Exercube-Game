using UnityEngine;

namespace Sphery.ExerCube.UI
{
    public class TestPointerProvider : TrackerPointerProviderBase
    {
        [Header("Pointing")]
        [SerializeField]
        protected bool isPointing = false;
        [SerializeField]
        protected KeyCode isPointingKey = KeyCode.LeftShift;

        [Header("Turning")]
        [SerializeField]
        protected float turnSpeed = 20f;
        [SerializeField]
        protected KeyCode rotateLeftKey = KeyCode.LeftArrow;
        [SerializeField]
        protected KeyCode rotateRightKey = KeyCode.RightArrow;
        [SerializeField]
        protected KeyCode rotateUpKey = KeyCode.UpArrow;
        [SerializeField]
        protected KeyCode rotateDownKey = KeyCode.DownArrow;

        [Header("Walls")]
        [SerializeField]
        protected Transform[] cameras;

        protected virtual void Update()
        {
            // Get input
            Vector2 turnInput = Vector2.zero;

            if (Input.GetKey(rotateLeftKey)) turnInput.x--;
            if (Input.GetKey(rotateRightKey)) turnInput.x++;
            if (Input.GetKey(rotateUpKey)) turnInput.y--;
            if (Input.GetKey(rotateDownKey)) turnInput.y++;

            // Apply rotation
            turnInput = turnInput.normalized * turnSpeed * Time.deltaTime;

            transform.RotateAround(transform.position, Vector3.up, turnInput.x);
            transform.RotateAround(transform.position, Vector3.right, turnInput.y);

            // Handle is pointing
            if (Input.GetKeyDown(isPointingKey))
                isPointing = true;
            if (Input.GetKeyUp(isPointingKey))
                isPointing = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 3f);

            float wallSize = 2.2f;
            float halfSize = wallSize * 0.5f;

            for (int i = 0; i < cameras.Length; i++)
            {
                if (cameras[i] == null)
                    continue;

                Vector3 center = cameras[i].position + cameras[i].forward;

                Vector3 upperLeftCorner = center + cameras[i].rotation * (-Vector3.right + Vector3.up) * halfSize;
                Vector3 upperRightCorner = center + cameras[i].rotation * (Vector3.right + Vector3.up) * halfSize;
                Vector3 lowerLeftCorner = center + cameras[i].rotation * (-Vector3.right + -Vector3.up) * halfSize;
                Vector3 lowerRightCorner = center + cameras[i].rotation * (Vector3.right + -Vector3.up) * halfSize;

                Gizmos.DrawLine(lowerLeftCorner, upperLeftCorner);
                Gizmos.DrawLine(upperLeftCorner, upperRightCorner);
                Gizmos.DrawLine(upperRightCorner, lowerRightCorner);
                Gizmos.DrawLine(lowerRightCorner, lowerLeftCorner);

                Ray ray = new Ray(transform.position, transform.forward);
                Plane cameraPlane = new Plane(cameras[i].forward, center);

                if (cameraPlane.Raycast(ray, out float enter))
                {
                    Vector3 hitPosition = ray.GetPoint(enter);
                    Gizmos.DrawSphere(hitPosition, 0.05f);
                }
            }
        }

        public override Vector3 GetPosition() => transform.position;
        public override Vector3 GetDirection() => transform.forward;

        public override bool IsPointing() => isPointing;
    }
}
