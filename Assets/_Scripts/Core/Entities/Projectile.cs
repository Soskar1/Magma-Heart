using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float m_speed;
        [SerializeField] private float m_lifeTimeInSeconds;

        private EntityModel m_attacker;
        private EntityModel m_target;
        private float m_damage;
        private CombatBoardState m_boardState;
        
        private float m_lifeTimeTimer;

        private CancellationTokenSource m_cancellationTokenSource;
        private TaskCompletionSource<bool> m_projectileHit;

        public void Initialize(CombatBoardState boardState, EntityModel attacker, EntityModel target, float damage)
        {
            m_boardState = boardState;
            m_attacker = attacker;
            m_target = target;
            m_damage = damage;

            m_projectileHit = new TaskCompletionSource<bool>();
            m_lifeTimeTimer = m_lifeTimeInSeconds;
        }

        public void Update()
        {
            transform.Translate(Vector2.right * m_speed * Time.deltaTime);

            if (m_lifeTimeTimer > 0)
            {
                m_lifeTimeTimer -= Time.deltaTime;
            }
            else
            {
                if (m_projectileHit != null)
                {
                    Debug.LogWarning($"{nameof(m_projectileHit)} is null");
                    m_projectileHit.SetResult(true);
                }

                Destroy(gameObject);
            }
        }

        public Task OnHit() => m_projectileHit.Task;

        public async void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Entity entity) && entity.Model == m_target)
                await Hit();

            m_projectileHit.SetResult(true);
        }

        private async Task Hit()
        {
            m_cancellationTokenSource = new CancellationTokenSource();
            await m_boardState.ApplyStateChangesAsync(new List<StateChange>()
            {
                new ApplyDamageStateChange(m_attacker, m_target, m_damage)
            }, m_cancellationTokenSource.Token);
        }
    }
}