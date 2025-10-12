using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random=UnityEngine.Random;

namespace MagmaHeart.Core.Artifacts
{
    public class ArtifactDatabase
    {
        private readonly Dictionary<Rarity, List<ArtifactData>> m_artifacts;

        public ArtifactDatabase()
        {
            m_artifacts = new Dictionary<Rarity, List<ArtifactData>>();

            foreach (Rarity rarity in Enum.GetValues(typeof(Rarity)))
                m_artifacts.Add(rarity, new List<ArtifactData>());

            TextAsset[] artifactDataFiles = ExternalResources.LoadAddTextAssets("ArtifactData");
            ArtifactDataDeserializer deserializer = new ArtifactDataDeserializer();

            foreach (TextAsset rawData in artifactDataFiles)
            {
                ArtifactData data = deserializer.Deserialize(rawData);
                m_artifacts[data.Rarity].Add(data);
            }
        }

        public List<ArtifactData> GetRandomArtifacts(Rarity rarity, int count)
        {
            return m_artifacts[rarity]
                .OrderBy(x => Random.value)
                .Take(count)
                .ToList();
        }
    }
}
