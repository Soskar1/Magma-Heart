using UnityEngine;

public static class ExternalResources
{
    public static TextAsset LoadTextAsset(string fileName) => (TextAsset)Resources.Load(fileName, typeof(TextAsset));
}
