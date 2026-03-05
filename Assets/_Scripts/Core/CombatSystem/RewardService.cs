using MagmaHeart.Core.Artifacts;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.Core.CombatSystem
{
    public class RewardService
    {
        private readonly Inventory m_inventory;
        private readonly ArtifactDatabase m_artifactDatabase;

        public RewardService(Inventory inventory, ArtifactDatabase database)
        {
            m_inventory = inventory;
            m_artifactDatabase = database;
        }

        public List<ArtifactData> GenerateRewards()
        {
            // TODO: Randomly select rarity

            return m_artifactDatabase.Artifacts
                .Where(artifact => !m_inventory.IsArtifactMaxed(artifact))
                .ToList();
        }
    }
}
