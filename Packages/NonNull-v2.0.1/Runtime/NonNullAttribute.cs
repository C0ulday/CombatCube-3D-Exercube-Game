namespace Koboldgames.NonNull
{
    using System;
    using UnityEngine;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
    public class NonNullAttribute : PropertyAttribute { }
}
