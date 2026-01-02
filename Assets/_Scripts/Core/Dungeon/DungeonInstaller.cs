using MagmaHeart.Core.SceneLoading;
using MagmaHeart.DungeonGeneration;
using UnityEngine;

namespace MagmaHeart.Core.Dungeon
{
    public class DungeonInstaller : IInstaller
    {
        public DungeonGenerator Install(System.Random random, string configFileName)
        {
            TextAsset configFile = ExternalResources.LoadTextAsset(configFileName);
            DungeonGeneratorData data = DungeonGeneratorDataDeserializer.Deserialize(configFile, random);
            return new DungeonGenerator(data);
        }

        public void Dispose() { }
    }
}
