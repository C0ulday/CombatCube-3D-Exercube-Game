namespace Koboldgames.NonNull
{
    using System;
    using UnityEngine;

    [AttributeUsage(AttributeTargets.Field)]
    public class AllowNullAttribute : PropertyAttribute { }
}
