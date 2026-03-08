namespace Koboldgames.NonNull
{
    using System.Reflection;

    internal class NonNullData
    {
        public FieldInfo Field { get; set; }
        public bool NonNull { get; set; }
        public bool NonEmpty { get; set; }
    }
}
