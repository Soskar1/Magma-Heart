using System;
using System.Collections.Generic;
using MagmaHeart.Core.UI;
using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class Player : MonoBehaviour, IEntity
    {
        [SerializeField] private EntityData m_data;

        public Action<Collider2D> OnTriggerEnter;
        public Action<Collider2D> OnTriggerExit;

        private Entity m_controllingEntity;

        public IMovable Movement { get; private set; }
        public Entity ControllingEntity => m_controllingEntity;
        public Health Health => ControllingEntity.Health;
        public Energy Energy => ControllingEntity.Energy;
        public EntityStats Stats => ControllingEntity.Stats;

        private IPlayerBehaviour m_currentBehaviour;
        private ActionPlayerBehaviour m_actionBehaviour;
        private TurnBasedPlayerBehaviour m_turnBasedBehaviour;

        public TurnBasedPlayerBehaviour TurnBasedPlayerBehaviour => m_turnBasedBehaviour;

        public void Initialize(UserInput userInput, List<IDisplayable> combatUI)
        {
            AnimationPlayer animationPlayer = GetComponent<AnimationPlayer>();
            m_controllingEntity = new Entity(m_data, animationPlayer);

            Movement = GetComponent<IMovable>();
            m_actionBehaviour = new ActionPlayerBehaviour(userInput, Movement, this);
            m_turnBasedBehaviour = new TurnBasedPlayerBehaviour(Energy, userInput, combatUI);
            m_currentBehaviour = m_actionBehaviour;
        }

        public void Enable()
        {
            m_currentBehaviour.Enable();
            ControllingEntity.Enable();
        }

        public void Disable()
        {
            m_currentBehaviour.Disable();
            ControllingEntity.Disable();
        }

        public void EnterCombat() => SwitchState(m_turnBasedBehaviour);
        public void ExitCombat() => SwitchState(m_actionBehaviour);

        private void SwitchState(IPlayerBehaviour newState)
        {
            m_currentBehaviour.Disable();
            m_currentBehaviour = newState;
            m_currentBehaviour.Enable();
        }

        private void Update() => ControllingEntity.RunAnimations();

        private void FixedUpdate() => m_currentBehaviour.Update();

        private void OnTriggerEnter2D(Collider2D collision) => OnTriggerEnter?.Invoke(collision);

        private void OnTriggerExit2D(Collider2D collision) => OnTriggerExit?.Invoke(collision);
    }
}