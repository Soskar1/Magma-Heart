using UnityEngine;
using UnityEngine.InputSystem;

namespace MagmaHeart.Core.Entities
{
    public class PlayerBehaviour : MonoBehaviour
    {
        private UserInput m_userInput;
        private IMeleeAttacker m_attack;
        private IMovable m_movement;
        private AnimationPlayer m_animation;

        public Entity ControllingEntity { get; private set; }

        public void Initialize()
        {
            m_userInput = new UserInput();

            Entity entity = GetComponent<Entity>();
            entity.Initialize();
            m_attack = entity.MeleeAttack;
            m_movement = entity.Movement;
            m_animation = entity.Animation;

            ControllingEntity = entity;
        }

        public void Enable()
        {
            m_userInput.Controls.Player.Attack.performed += Attack;
            m_userInput.Enable();
            ControllingEntity.Enable();
        }

        public void Disable()
        {
            m_userInput.Controls.Player.Attack.performed -= Attack;
            m_userInput.Disable();
            ControllingEntity.Disable();
        }

        public void Update() => m_animation.PlayAnimations();

        public void FixedUpdate() => m_movement.Move(m_userInput.Movement);

        public void Attack(InputAction.CallbackContext context) => m_attack.Attack();
    }
}