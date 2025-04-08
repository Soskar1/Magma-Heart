using System.Collections.Generic;
using MagmaHeart.Core.Dungeon;
using UnityEngine;
using UnityEngine.UI;

namespace MagmaHeart.Core.Entities
{
    public class EnemyBehaviour : MonoBehaviour
    {
        [Header("Field Of View")]
        [SerializeField] private float m_triggerAttackDistance;
        [SerializeField] private float m_searchRadius;
        [SerializeField] private int m_amountOfRaycasts;

        [Header("Animation Events")]
        [SerializeField] private string m_takeDamageAnimationName;
        [SerializeField] private string m_deathAnimationName;
        [SerializeField] private string m_attackAnimationName;

        private Entity m_entity;
        private IAttacker m_entityAttack;
        private IMovable m_entityMovement;
        private IAnimatable m_entityAnimations;

        private Entity m_entityToChase;
        private Rigidbody2D m_rigidbody;

        private bool m_canMove = true;
        private bool m_canAttack = true;

        private AStarNavigation m_navigation;

        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_entity = GetComponent<Entity>();
            m_entityAttack = m_entity as IAttacker;
            m_entityMovement = m_entity as IMovable;
            m_entityAnimations = m_entity as IAnimatable;

            if (m_entityAttack == null)
                Debug.LogError($"Entity {transform.name} does not support IAttacker interface");

            if (m_entityMovement == null)
                Debug.LogError($"Entity {transform.name} does not support IMovable interface");

            if (m_entityAnimations == null)
                Debug.LogError($"Entity {transform.name} does not support IAnimatable interface");

            m_entity.Initialize();
        }

        public void Initialize(Entity entityToChase, RoomData roomData)
        {
            m_entityToChase = entityToChase;
            m_navigation = new AStarNavigation(roomData);
        }

        private void OnEnable()
        {
            m_entity.Health.OnTakeDamage += DisableMovement;
            m_entity.Health.OnTakeDamage += DisableAttack;
            m_entity.Health.OnDeath += Freeze;

            m_entityAttack.OnAttackStarted += DisableMovement;
            m_entityAttack.OnAttackStarted += DisableAttack;
            m_entityAttack.OnAttackStarted += Stop;
            m_entityAttack.OnAttackEnded += EnableMovement;

            m_entityAnimations.AnimationPlayer.OnAnimationEnded += ProcessAnimationOnEndEvents;
        }

        private void OnDisable()
        {
            m_entity.Health.OnTakeDamage -= DisableMovement;
            m_entity.Health.OnTakeDamage -= DisableAttack;
            m_entity.Health.OnDeath -= Freeze;

            m_entityAttack.OnAttackStarted -= DisableMovement;
            m_entityAttack.OnAttackStarted -= DisableAttack;
            m_entityAttack.OnAttackStarted -= Freeze;
            m_entityAttack.OnAttackEnded -= EnableMovement;

            m_entityAnimations.AnimationPlayer.OnAnimationEnded -= ProcessAnimationOnEndEvents;
        }

        private void Update()
        {
            if (m_entityToChase == null)
                return;

            if (!m_canAttack)
                return;

            if (Vector2.Distance(transform.position, m_entityToChase.transform.position) < m_triggerAttackDistance)
                m_entityAttack.Attack();
        }

        public void FixedUpdate()
        {
            if (m_entityToChase == null)
                return;

            if (!m_canMove)
                return;

            if (m_navigation == null)
                return;

            List<Vector2Int> path = m_navigation.ConstructPath(transform.position.ToVector2Int(), m_entityToChase.transform.position.ToVector2Int());
            if (path != null && path.Count > 1)
            {
                Vector2 directionToMove = path[1] - path[0];
                m_entityMovement.Move(directionToMove.normalized);
            }
        }

        private void EnableMovement() => m_canMove = true;
        private void DisableMovement() => m_canMove = false;
        private void EnableAttack() => m_canAttack = true;
        private void DisableAttack() => m_canAttack = false;
        private void Stop() => m_rigidbody.linearVelocity = Vector2.zero;
        private void Freeze() => m_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        private void DisableEntity()
        {
            m_entity.enabled = false;
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