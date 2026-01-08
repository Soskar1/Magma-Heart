using MagmaHeart.Core.Artifacts;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.Core.CombatSystem
{
    public class RewardService
    {
        private readonly ArtifactDatabase m_artifactDatabase;
        private readonly Inventory m_inventory;

        public RewardService(Inventory inventory)
        {
            m_artifactDatabase = new ArtifactDatabase();
            m_inventory = inventory;
        }

        public IEnumerable<ArtifactData> GenerateRewards()
        {
            // TODO: Randomly select rarity

            return m_artifactDatabase
                .GetArtifactsByRarity(Rarity.Common)
                .Where(artifact => !m_inventory.IsArtifactMaxed(artifact));
        }
    }
}
