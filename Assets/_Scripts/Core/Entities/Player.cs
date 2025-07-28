using System;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float m_health;
        private PlayerNonCombatBehaviour m_behaviour;

        public Action<Collider2D> OnTriggerEnter;
        public Action<Collider2D> OnTriggerExit;

        public IMovable Movement { get; private set; }
        public Entity ControllingEntity { get; private set; }
        public Health Health => ControllingEntity.Health;

        public void Initialize()
        {
            UserInput userInput = new UserInput();
            Movement = GetComponent<IMovable>();
            m_behaviour = new PlayerNonCombatBehaviour(userInput, Movement, this);

            AnimationPlayer animationPlayer = GetComponent<AnimationPlayer>();
            ControllingEntity = new Entity(m_health, animationPlayer);
        }

        public void Enable()
        {
            m_behaviour.Enable();
            ControllingEntity.Enable();
        }

        public void Disable()
        {
            m_behaviour.Disable();
            ControllingEntity.Disable();
        }

        private void Update() => ControllingEntity.RunAnimations();

        private void FixedUpdate() => m_behaviour.Update();

        private void OnTriggerEnter2D(Collider2D collision) => OnTriggerEnter?.Invoke(collision);

        private void OnTriggerExit2D(Collider2D collision) => OnTriggerExit?.Invoke(collision);
    }
}