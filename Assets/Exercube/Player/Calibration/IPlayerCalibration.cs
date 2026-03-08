using UnityEngine;

namespace Sphery.ExerCube
{
    public interface IPlayerCalibration
    {
        float PlayerSizeFactor { get; }
        float PlayerZPositionNormalized { get; }
        Vector3 LocalPositionCorrection { get; }

        void Calibrate();
        void SkipCalibration();
        void Reset();
    }
}
