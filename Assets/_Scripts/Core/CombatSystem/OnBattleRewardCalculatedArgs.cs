using MagmaHeart.Core.Artifacts;
using System;
using System.Collections.Generic;

namespace MagmaHeart.Core.CombatSystem
{
    public class OnBattleRewardCalculatedArgs : EventArgs
    {
        public List<ArtifactData> Rewards { get; init; }

        public OnBattleRewardCalculatedArgs(List<ArtifactData> rewards) => Rewards = rewards;
    }
}
