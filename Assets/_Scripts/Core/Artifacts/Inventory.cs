using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Presentation.UI;
using System;
using System.Collections.Generic;

namespace MagmaHeart.Core.Artifacts
{
    public class Inventory
    {
        private readonly Dictionary<ArtifactData, Artifact> m_artifacts;
        private readonly EntityModel m_owner;

        public event EventHandler<Artifact> OnArtifactPicked;

        public Inventory(EntityModel owner)
        {
            m_artifacts = new Dictionary<ArtifactData, Artifact>();
            m_owner = owner;
        }

        public void TryPick(ArtifactData data)
        {
            if (!m_artifacts.TryGetValue(data, out Artifact artifact))
            {
                artifact = new Artifact(data);
                m_artifacts[data] = artifact;
                artifact.Apply(m_owner);

                OnArtifactPicked?.Invoke(this, artifact);
                return;
            }

            if (artifact.IsMaxLevel)
                return;

            artifact.Revert(m_owner);
            artifact.LevelUp();
            artifact.Apply(m_owner);
        }

        public bool IsArtifactMaxed(ArtifactData data)
        {
            if (!m_artifacts.TryGetValue(data, out Artifact artifact))
                return false;

            return artifact.IsMaxLevel;
        }
    }
}
