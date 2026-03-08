using UnityEngine;

namespace Sphery.ExerCube.UI
{
    public abstract class TrackerPointerProviderBase : MonoBehaviour
    {
        public abstract Vector3 GetPosition();
        public abstract Vector3 GetDirection();

        public abstract bool IsPointing();
    }
}
