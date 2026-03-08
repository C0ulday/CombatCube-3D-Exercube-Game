using UnityEngine;

namespace Sphery.ExerCube
{
    public class BlockDirectionCopier : MonoBehaviour
    {
        public BlockTracker blockTracker;

        private void Update()
        {
            if (blockTracker.Direction != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(blockTracker.Direction);
        }
    }
}
