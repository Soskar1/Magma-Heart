using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Entities.PlayableCharacters;

namespace MagmaHeart.Core.StateMachines
{
    public class RewardState : IState
    {
        private readonly BattleReward m_battleReward;
        private readonly Player m_player;

        public RewardState(BattleReward battleReward, Player player)
        {
            m_battleReward = battleReward;
            m_player = player;
        }

        public void Enter()
        {
            m_battleReward.Calculate();
            m_player.Animation.PlayIdleAnimation();
        }

        public void Exit()
        {

        }
    }
}