using UnityEngine;

namespace Sphery.ExerCube.Helpers
{
    public class MinMaxRangeAttribute : PropertyAttribute
    {
        public MinMaxRangeAttribute(bool useIntegers)
        {
            this.Max = 10f;
            this.Integer = useIntegers;
        }

        public MinMaxRangeAttribute(float min, float max)
        {
            this.Min = min;
            this.Max = max;
        }

        public MinMaxRangeAttribute(int min, int max)
        {
            this.Min = min;
            this.Max = max;
            this.Integer = true;
        }

        public float Min { get; protected set; } = 0f;
        public float Max { get; protected set; } = 1f;
        public bool Integer { get; protected set; } = false;
    }
}
