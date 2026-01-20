using UnityEngine;

public static class ExternalResources
{
    public static TextAsset LoadTextAsset(string fileName) => (TextAsset)Resources.Load(fileName, typeof(TextAsset));
    public static TextAsset[] LoadAddTextAssets(string folderPath) => Resources.LoadAll<TextAsset>(folderPath);
    public static Sprite LoadSprite(string filePath) => Resources.Load<Sprite>(filePath);
}
