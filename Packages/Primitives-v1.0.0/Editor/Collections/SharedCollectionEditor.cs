namespace Koboldgames.Primitives.Collections
{
    using UnityEditor;

    [CustomEditor(typeof(SharedListBase<>), true)]
    internal class SharedListEditor : SharedAssetEditor { }

    [CustomEditor(typeof(SharedDictionaryBase<,>), true)]
    internal class SharedDictionaryEditor : SharedAssetEditor { }

    [CustomEditor(typeof(SharedHashSetBase<>), true)]
    internal class SharedHashSetEditor : SharedAssetEditor { }
}
