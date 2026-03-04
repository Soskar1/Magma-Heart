using MagmaHeart.Core.Entities;
using UnityEditor;

namespace MagmaHeart.Core.Tests
{
    public static class EntityDatabase
    {
        private const string DataFolderPath = "Assets/Data/EntityData/";

        public static EntityData Warrior => LoadEntityData("Warrior.asset");
        public static EntityData SkeletonWarrior => LoadEntityData("SkeletonWarrior.asset");
        public static EntityData SkeletonBoss => LoadEntityData("SkeletonBoss.asset");
        public static EntityData Vampire => LoadEntityData("Vampire.asset");

        private static EntityData LoadEntityData(string entityDataFileName) => AssetDatabase.LoadAssetAtPath<EntityData>(DataFolderPath + entityDataFileName);
    }
}
