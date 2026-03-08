using UnityEngine;
using Koboldgames.Primitives.Events;

namespace Sphery.ExerCube
{
    [CreateAssetMenu(fileName = "SharedPoseEvent.asset", menuName = "Exercube/Primitives/Events/Pose", order = 100)]
    public class SharedPoseEvent : SharedEvent<Pose> { }
}
