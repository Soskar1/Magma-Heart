using MagmaHeart.Core.CombatSystem;
using System.Collections.Generic;

namespace MagmaHeart.Core.StateMachines
{
    public class RewardState : IState
    {
        private readonly BattleReward m_battleReward;

        public RewardState(BattleReward battleReward)
        {
            m_battleReward = battleReward;
        }

        public void Enter()
        {
            m_battleReward.Calculate();
        }

        public void Exit()
        {

        }
    }
}