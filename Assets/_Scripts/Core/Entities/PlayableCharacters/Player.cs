using System;
using MagmaHeart.Core.Input;
using MagmaHeart.Core.StateMachines;
using MagmaHeart.Core.UI;
using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class Player : MonoBehaviour, IActionStateListener, ICombatStateListener, IRewardStateListener
    {
        [SerializeField] private EntityData m_data;

        public Action<Collider2D> OnTriggerEnter;
        public Action<Collider2D> OnTriggerExit;

        private Entity m_controllingEntity;

        private PlayerAnimation m_animation;

        public Entity ControllingEntity => m_controllingEntity;
        public Health Health => ControllingEntity.Health;
        public Energy Energy => ControllingEntity.Energy;
        public EntityStats Stats => ControllingEntity.Stats;

        private IPlayerBehaviour m_currentBehaviour;
        private ActionPlayerBehaviour m_actionBehaviour;
        private CombatPlayerBehaviour m_turnBasedBehaviour;
        private RewardPlayerBehaviour m_rewardPlayerBehaviour;

        public CombatPlayerBehaviour TurnBasedPlayerBehaviour => m_turnBasedBehaviour;

        public void Initialize(ActionUserInput actionUserInput, CombatUserInput turnBasedUserInput, EnergyHUD energyHUD)
        {
            m_animation = GetComponent<PlayerAnimation>();
            m_controllingEntity = new Entity(m_data, transform);

            m_actionBehaviour = new ActionPlayerBehaviour(this, actionUserInput);
            m_turnBasedBehaviour = new CombatPlayerBehaviour(this, turnBasedUserInput, energyHUD);
            m_rewardPlayerBehaviour = new RewardPlayerBehaviour(actionUserInput.UserInput, m_animation);
            m_currentBehaviour = m_actionBehaviour;
        }

        public void Enable() => m_currentBehaviour.Enable();
        public void Disable() => m_currentBehaviour.Disable();

        public void EnterActionState() => SwitchState(m_actionBehaviour);
        public void ExitActionState() { }

        public void EnterCombatState() => SwitchState(m_turnBasedBehaviour);
        public void ExitCombatState() { }

        public void EnterRewardState() => SwitchState(m_rewardPlayerBehaviour);
        public void ExitRewardState() { }

        private void SwitchState(IPlayerBehaviour newState)
        {
            m_currentBehaviour.Disable();
            m_currentBehaviour = newState;
            m_currentBehaviour.Enable();
        }

        private void FixedUpdate()
        {
            m_currentBehaviour.Update();
            m_animation.PlayAnimations();
        }

        private void OnTriggerEnter2D(Collider2D collision) => OnTriggerEnter?.Invoke(collision);

        private void OnTriggerExit2D(Collider2D collision) => OnTriggerExit?.Invoke(collision);

        
    }
}