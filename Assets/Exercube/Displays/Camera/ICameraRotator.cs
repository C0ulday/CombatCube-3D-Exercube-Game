using UnityEngine;

namespace Sphery.ExerCube
{
    public interface ICameraRotator
    {
        Camera Front { get; }
        Camera Right { get; }
        Camera Left { get; }

        event System.Action OnRecalculatedFOV;

        void Rotate();
    }
}
