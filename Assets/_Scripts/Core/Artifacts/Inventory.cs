using MagmaHeart.Core.Entities;
using System.Collections.Generic;

namespace MagmaHeart.Core.Artifacts
{
    public class Inventory
    {
        private readonly Dictionary<ArtifactData, Artifact> m_artifacts;
        private readonly EntityModel m_owner;

        public Inventory(EntityModel owner)
        {
            m_artifacts = new Dictionary<ArtifactData, Artifact>();
            m_owner = owner;
        }

        public void Pick(ArtifactData data)
        {
            if (m_artifacts.ContainsKey(data))
            {
                Artifact artifact = m_artifacts[data];

                if (artifact.IsMaxLevel)
                    return;

                artifact.Revert(m_owner);
                m_artifacts[data].LevelUp();
                artifact.Apply(m_owner);
            }
            else
            {
                Artifact artifact = new Artifact(data);
                m_artifacts[data] = artifact;
                artifact.Apply(m_owner);
            }
        }
    }
}
