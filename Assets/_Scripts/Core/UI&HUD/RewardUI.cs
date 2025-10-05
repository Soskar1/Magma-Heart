using MagmaHeart.Core.StateMachines;
using UnityEngine;

namespace MagmaHeart.Core.UI
{
    public class RewardUI : MonoBehaviour, IDisplayable, IRewardStateListener
    {
        [SerializeField] private GameObject m_visual;
        private GameStateMachine m_stateMachine;

        public void Initialize(GameStateMachine stateMachine) => m_stateMachine = stateMachine;
        
        public void Show() => m_visual.SetActive(true);
        public void Hide() => m_visual.SetActive(false);

        public void GetReward()
        {
            m_stateMachine.ChangeState(StateMachineStates.Action);
        }

        public void EnterRewardState() => Show();
        public void ExitRewardState() => Hide();
    }
}