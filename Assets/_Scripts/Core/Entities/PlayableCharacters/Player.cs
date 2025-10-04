using System;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Input;
using MagmaHeart.Core.UI;
using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class Player : MonoBehaviour, ICombatStateListener
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
        private TurnBasedPlayerBehaviour m_turnBasedBehaviour;

        public TurnBasedPlayerBehaviour TurnBasedPlayerBehaviour => m_turnBasedBehaviour;

        public void Initialize(ActionUserInput actionUserInput, TurnBasedUserInput turnBasedUserInput, EnergyHUD energyHUD)
        {
            m_animation = GetComponent<PlayerAnimation>();
            m_controllingEntity = new Entity(m_data, transform);

            m_actionBehaviour = new ActionPlayerBehaviour(this, actionUserInput);
            m_turnBasedBehaviour = new TurnBasedPlayerBehaviour(this, turnBasedUserInput, energyHUD);
            m_currentBehaviour = m_actionBehaviour;
        }

        public void Enable() => m_currentBehaviour.Enable();
        public void Disable() => m_currentBehaviour.Disable();

        public void EnterCombatState() => SwitchState(m_turnBasedBehaviour);
        public void ExitCombatState() => SwitchState(m_actionBehaviour);

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