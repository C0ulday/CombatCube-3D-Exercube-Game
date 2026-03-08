using UnityEngine;

namespace Sphery.ExerCube
{
    public interface ITrackerData
    {
        Vector3 Position { get; }
        Vector3 GlobalPosition { get; }
        Vector3 Direction { get; }
        Vector3 GlobalDirection { get; }
        float Velocity { get; }
    }

    public class TrackerData : ITrackerData
    {
        private Transform transform;
        public Transform Transform
        {
            get => transform;
            set
            {
                transform = value;

                if (transform == null)
                {
                    Position = Vector3.up;
                    GlobalPosition = Vector3.up;
                }
                else
                {
                    Position = transform.localPosition;
                    GlobalPosition = transform.parent?.TransformPoint(Position) ?? Vector3.up;
                }

                Direction = Vector3.down;
                GlobalDirection = Vector3.down;
                Velocity = 0f;

                previousPosition = Position;
                previousGlobalPosition = GlobalPosition;
            }
        }

        public Vector3 Position { get; set; } = Vector3.up;
        public Vector3 GlobalPosition { get; set; } = Vector3.up;
        public Vector3 Direction { get; set; } = Vector3.down;
        public Vector3 GlobalDirection { get; set; } = Vector3.down;
        public float Velocity { get; set; } = 0f;

        private Vector3 previousPosition;
        private Vector3 previousGlobalPosition;

        public void Update(Vector3 correction)
        {
            if (transform == null)
            {
                Position = Vector3.up + correction;
                GlobalPosition = Vector3.up;
                Direction = Vector3.down;
                Velocity = 0f;
                return;
            }
            else
            {
                Position = transform.localPosition + correction;
                GlobalPosition = transform.parent?.TransformPoint(Position) ?? Vector3.up;
                Direction = (Position - previousPosition).normalized;
                GlobalDirection = (GlobalPosition - previousGlobalPosition).normalized;
                Velocity = Vector3.Distance(previousPosition, Position) / Time.deltaTime; // Extrapolate to unit per second
            }

            // Prepare for next frame
            previousPosition = Position;
            previousGlobalPosition = GlobalPosition;
        }
    }
}
