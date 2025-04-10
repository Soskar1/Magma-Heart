using MagmaHeart.Core.Dungeon;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class EnemyMeleeBehaviour : MonoBehaviour
    {
        [SerializeField] private float m_triggerAttackDistance;

        [Header("Animation Events")]
        [SerializeField] private string m_takeDamageAnimationName;
        [SerializeField] private string m_deathAnimationName;
        [SerializeField] private string m_attackAnimationName;

        private Entity m_entity;
        private IMeleeAttacker m_attack;
        private IMovable m_movement;
        private AnimationPlayer m_animation;

        private Entity m_entityToChase;
        private Rigidbody2D m_rigidbody;

        private bool m_canMove = true;
        private bool m_canAttack = true;

        public Collider2D AttackHitCollider => m_attack.HitCollider;

        public void Initialize(Entity entityToChase, RoomData roomData)
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_entity = GetComponent<Entity>();
            m_entityToChase = entityToChase;
            
            m_entity.Initialize();
            m_attack = m_entity.MeleeAttack;
            m_movement = m_entity.Movement;
            m_animation = m_entity.Animation;
        }

        public void Enable()
        {
            m_entity.Health.OnTakeDamage += DisableMovement;
            m_entity.Health.OnTakeDamage += DisableAttack;
            m_entity.Health.OnTakeDamage += Unfreeze;
            m_entity.Health.OnDeath += Freeze;

            m_attack.OnAttackStarted += DisableMovement;
            m_attack.OnAttackStarted += DisableAttack;
            m_attack.OnAttackStarted += Stop;
            m_attack.OnAttackStarted += Freeze;

            m_attack.OnAttackEnded += EnableMovement;
            m_attack.OnAttackEnded += Unfreeze;

            m_animation.OnAnimationEnded += ProcessAnimationOnEndEvents;
            m_entity.Enable();
        }

        public void Disable()
        {
            m_entity.Health.OnTakeDamage -= DisableMovement;
            m_entity.Health.OnTakeDamage -= DisableAttack;
            m_entity.Health.OnTakeDamage -= Unfreeze;
            m_entity.Health.OnDeath -= Freeze;

            m_attack.OnAttackStarted -= DisableMovement;
            m_attack.OnAttackStarted -= DisableAttack;
            m_attack.OnAttackStarted -= Stop;
            m_attack.OnAttackStarted -= Freeze;

            m_attack.OnAttackEnded -= EnableMovement;
            m_attack.OnAttackEnded -= Unfreeze;

            m_animation.OnAnimationEnded -= ProcessAnimationOnEndEvents;
            m_entity.Disable();
        }

        private void Update()
        {
            m_animation.PlayAnimations();

            if (m_entityToChase == null)
                return;

            if (!m_canAttack)
                return;

            if (Vector2.Distance(transform.position, m_entityToChase.transform.position) < m_triggerAttackDistance)
                m_attack.Attack();
        }

        public void FixedUpdate()
        {
            if (m_entityToChase == null)
                return;

            if (!m_canMove)
                return;

            Vector2 directionToMove = m_entityToChase.transform.position - transform.position;
            m_movement.Move(directionToMove.normalized);
        }

        private void EnableMovement() => m_canMove = true;
        private void DisableMovement() => m_canMove = false;
        private void EnableAttack() => m_canAttack = true;
        private void DisableAttack() => m_canAttack = false;
        private void Stop() => m_rigidbody.linearVelocity = Vector2.zero;
        private void Freeze() => m_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        private void Unfreeze() => m_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        private void DisableEntity()
        {
            Disable();
            enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }

        private void ProcessAnimationOnEndEvents(string endedAnimationName)
        {
            if (endedAnimationName == m_attackAnimationName)
            {
                EnableAttack();
            }
            else if (endedAnimationName == m_takeDamageAnimationName)
            {
                EnableMovement();
                EnableAttack();
            }
            else if (endedAnimationName == m_deathAnimationName)
            {
                DisableEntity();
            }
        }
    }
}