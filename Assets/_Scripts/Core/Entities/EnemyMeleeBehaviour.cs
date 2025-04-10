using System;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class EnemyMeleeBehaviour : MonoBehaviour, IPoolable
    {
        [SerializeField] private float m_triggerAttackDistance;

        [Header("Animation Events")]
        [SerializeField] private string m_takeDamageAnimationName;
        [SerializeField] private string m_deathAnimationName;
        [SerializeField] private string m_attackAnimationName;

        private IMeleeAttacker m_attack;
        private IMovable m_movement;
        private AnimationPlayer m_animation;

        private Entity m_entityToChase;
        private Rigidbody2D m_rigidbody;
        private Collider2D m_collider;

        private bool m_canMove = true;
        private bool m_canAttack = true;

        public Collider2D AttackHitCollider => m_attack.HitCollider;
        public Entity ControllingEntity { get; private set; }
        public Collider2D Collider => m_collider;

        public Action<EnemyMeleeBehaviour> OnDisable;

        public void Initialize(Entity entityToChase)
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_collider = GetComponent<Collider2D>();
            ControllingEntity = GetComponent<Entity>();
            m_entityToChase = entityToChase;
            
            ControllingEntity.Initialize();
            m_attack = ControllingEntity.MeleeAttack;
            m_movement = ControllingEntity.Movement;
            m_animation = ControllingEntity.Animation;
        }

        public void Enable()
        {
            ControllingEntity.Health.OnTakeDamage += DisableMovement;
            ControllingEntity.Health.OnTakeDamage += DisableAttack;
            ControllingEntity.Health.OnTakeDamage += Unfreeze;
            ControllingEntity.Health.OnDeath += Freeze;

            m_attack.OnAttackStarted += DisableMovement;
            m_attack.OnAttackStarted += DisableAttack;
            m_attack.OnAttackStarted += Stop;
            m_attack.OnAttackStarted += Freeze;

            m_attack.OnAttackEnded += EnableMovement;
            m_attack.OnAttackEnded += Unfreeze;

            m_animation.OnAnimationEnded += ProcessAnimationOnEndEvents;
            ControllingEntity.Enable();
        }

        public void Disable()
        {
            ControllingEntity.Health.OnTakeDamage -= DisableMovement;
            ControllingEntity.Health.OnTakeDamage -= DisableAttack;
            ControllingEntity.Health.OnTakeDamage -= Unfreeze;
            ControllingEntity.Health.OnDeath -= Freeze;

            m_attack.OnAttackStarted -= DisableMovement;
            m_attack.OnAttackStarted -= DisableAttack;
            m_attack.OnAttackStarted -= Stop;
            m_attack.OnAttackStarted -= Freeze;

            m_attack.OnAttackEnded -= EnableMovement;
            m_attack.OnAttackEnded -= Unfreeze;

            m_animation.OnAnimationEnded -= ProcessAnimationOnEndEvents;
            ControllingEntity.Disable();
        }

        public void OnSpawn()
        {
            enabled = true;
            m_collider.enabled = true;
            ControllingEntity.Reset();
            EnableMovement();
            EnableAttack();
            Unfreeze();
            Enable();
        }

        public void OnReturnToPool()
        {
            Disable();
            enabled = false;
            m_collider.enabled = false;
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
                OnDisable?.Invoke(this);
            }
        }
    }
}