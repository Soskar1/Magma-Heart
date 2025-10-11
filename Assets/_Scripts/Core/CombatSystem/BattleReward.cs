using MagmaHeart.Core.Artifacts;
using MagmaHeart.Core.StateMachines;
using System;
using System.Collections.Generic;

namespace MagmaHeart.Core.CombatSystem
{
    public class BattleReward : IRewardStateListener
    {
        private readonly ArtifactDatabase m_database;
        public event EventHandler<OnBattleRewardCalculatedArgs> OnBattleRewardCalculated;

        public BattleReward(ArtifactDatabase database)
        {
            m_database = database;
        }

        public void EnterRewardState()
        {
            // TODO: Randomly select rarity
            List<ArtifactData> artifacts = m_database.GetRandomArtifacts(Rarity.Common, 3);
            OnBattleRewardCalculatedArgs args = new OnBattleRewardCalculatedArgs(artifacts);
            OnBattleRewardCalculated?.Invoke(this, args);
        }

        public void ExitRewardState() { }
    }
}
