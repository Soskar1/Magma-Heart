using System;
using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float m_health;
        private IPlayerBehaviour m_currentBehaviour;

        public Action<Collider2D> OnTriggerEnter;
        public Action<Collider2D> OnTriggerExit;

        public IMovable Movement { get; private set; }
        public Entity ControllingEntity { get; private set; }
        public Health Health => ControllingEntity.Health;

        private ActionPlayerBehaviour m_actionBehaviour;
        private TurnBasedPlayerBehaviour m_turnBasedBehaviour;

        public void Initialize(UserInput userInput)
        {
            Movement = GetComponent<IMovable>();
            m_actionBehaviour = new ActionPlayerBehaviour(userInput, Movement, this);
            m_turnBasedBehaviour = new TurnBasedPlayerBehaviour(userInput);
            m_currentBehaviour = m_actionBehaviour;

            AnimationPlayer animationPlayer = GetComponent<AnimationPlayer>();
            ControllingEntity = new Entity(m_health, animationPlayer);
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