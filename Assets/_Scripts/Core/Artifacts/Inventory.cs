using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Presentation.UI;
using System.Collections.Generic;

namespace MagmaHeart.Core.Artifacts
{
    public class Inventory
    {
        private readonly Dictionary<ArtifactData, Artifact> m_artifacts;
        private readonly EntityModel m_owner;
        private readonly RewardUI m_rewardUI;

        public Inventory(EntityModel owner, RewardUI rewardUI)
        {
            m_artifacts = new Dictionary<ArtifactData, Artifact>();
            m_owner = owner;
            m_rewardUI = rewardUI;

            m_rewardUI.OnRewardPicked += HandleOnRewardPicked;
        }

        public void Disable() => m_rewardUI.OnRewardPicked -= HandleOnRewardPicked;

        private void Pick(ArtifactData data)
        {
            if (!m_artifacts.TryGetValue(data, out Artifact artifact))
            {
                artifact = new Artifact(data);
                m_artifacts[data] = artifact;
                artifact.Apply(m_owner);
                return;
            }

            if (artifact.IsMaxLevel)
                return;

            artifact.Revert(m_owner);
            artifact.LevelUp();
            artifact.Apply(m_owner);
        }

        private void HandleOnRewardPicked(object obj, OnRewardPickedArgs args) => Pick(args.ArtifactData);

        public bool IsArtifactMaxed(ArtifactData data)
        {
            if (!m_artifacts.TryGetValue(data, out Artifact artifact))
                return false;

            return artifact.IsMaxLevel;
        }
    }
}
