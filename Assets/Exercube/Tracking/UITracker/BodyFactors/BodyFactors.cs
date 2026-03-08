using UnityEngine;

namespace Sphery.ExerCube
{
    [CreateAssetMenu(fileName = "BodyFactors.asset", menuName = "ExerCube/BodyFactors")]
    public class BodyFactors : ScriptableObject
    {
        public float shoulderHeight = 0.818f;
        public float hipHeight = 0.530f;
        public float kneeHeight = 0.285f;
        public float ankleHeight = 0.039f;

        public float upperArmLength = 0.186f;
        public float lowerArmLength = 0.146f;
        public float ArmLength { get => upperArmLength + lowerArmLength; }

        public float WristHeight { get => shoulderHeight + upperArmLength + lowerArmLength; }

        public float CalculatePlayerHeightFromCalibration(float calibrationHeight) => calibrationHeight - lowerArmLength - upperArmLength;
    }
}
