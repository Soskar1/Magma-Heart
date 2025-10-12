using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MagmaHeart.Core.Artifacts
{
    public class ArtifactDatabase
    {
        private readonly Dictionary<Rarity, List<ArtifactData>> m_artifacts;

        public ArtifactDatabase()
        {
            ArtifactData[] artifacts = Resources.LoadAll<ArtifactData>("ArtifactData");
            
            foreach (ArtifactData artifact in artifacts)
                artifact.Initialize();

            m_artifacts = artifacts.GroupBy(a => a.Rarity)
                                   .ToDictionary(g => g.Key, g => g.ToList());
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
