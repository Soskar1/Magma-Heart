using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Presentation.UI;
using MagmaHeart.Core.SceneLoading;

namespace MagmaHeart.Core.Artifacts
{
    public class ArtifactInstaller : IInstaller
    {
        private Inventory m_inventory;

        public RewardService Install(EntityModel inventoryOwner, RewardUI rewardUI)
        {
            m_inventory = new Inventory(inventoryOwner, rewardUI);
            return new RewardService(m_inventory);
        }

        public void Dispose()
        {
            m_inventory.Disable();
        }
    }
}
