using MagmaHeart.Core.Artifacts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MagmaHeart.Core.Entities
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] private MeleeWeapon m_weapon;
        private UserInput m_userInput;
        private IMeleeAttacker m_attack;
        private IMovable m_movement;
        private AnimationPlayer m_animation;

        public Entity ControllingEntity { get; private set; }
        public ArtifactApplier ArtifactApplier { get; private set; }

        public void Initialize()
        {
            m_userInput = new UserInput();

            Entity entity = GetComponent<Entity>();
            entity.Initialize();
            m_attack = entity.MeleeAttack;
            m_movement = entity.Movement;
            m_animation = entity.Animation;

            ArtifactApplier = GetComponent<ArtifactApplier>();

            ControllingEntity = entity;
        }

        public void Enable()
        {
            m_userInput.Controls.Player.Attack.performed += Attack;
            m_userInput.Enable();
            ControllingEntity.Enable();

            ArtifactApplier.IncreaseHealth += ControllingEntity.Health.IncreaseMaxHealth;
            ArtifactApplier.IncreaseDamage += m_weapon.IncreaseDamage;
            ArtifactApplier.IncreaseSpeed += m_movement.IncreaseMaxSpeed;
            ArtifactApplier.IncreaseAttackSpeed += m_animation.IncreaseAnimationSpeed;
        }

        public void Disable()
        {
            m_userInput.Controls.Player.Attack.performed -= Attack;
            m_userInput.Disable();
            ControllingEntity.Disable();

            ArtifactApplier.IncreaseHealth -= ControllingEntity.Health.IncreaseMaxHealth;
            ArtifactApplier.IncreaseDamage -= m_weapon.IncreaseDamage;
            ArtifactApplier.IncreaseSpeed -= m_movement.IncreaseMaxSpeed;
            ArtifactApplier.IncreaseAttackSpeed -= m_animation.IncreaseAnimationSpeed;
        }

        public void Update() => m_animation.PlayAnimations();

        public void FixedUpdate() => m_movement.Move(m_userInput.Movement);

        public void Attack(InputAction.CallbackContext context) => m_attack.Attack();
    }
}