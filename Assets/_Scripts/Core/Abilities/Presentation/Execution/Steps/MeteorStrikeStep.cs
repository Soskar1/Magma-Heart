using MagmaHeart.Core.Abilities.Effects;
using MagmaHeart.Core.CameraControls;
using MagmaHeart.Core.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Abilities.Presentation.Execution.Steps
{
    [System.Serializable]
    public class MeteorStrikeStep : IAbilityExecutionStep
    {
        [SerializeField] private Meteor m_meteorPrefab;
        [SerializeField] private Vector2 m_spawnPoint;
        [SerializeField] private Shockwave m_shockwavePrefab;
        [SerializeField] private Explosion m_explosionPrefab;
        [SerializeField] private float m_cameraShakeDuration;
        [SerializeField] private float m_cameraShakeMagnitude;
        [SerializeField] private ParticleSystem m_explosionParticles;

        public async Task Run(AbilityExecutionContext context, CancellationToken cancellationToken)
        {
            var camera = Camera.main.GetComponent<CameraController>();
            camera.DisableManualMovement();
            camera.EnableMeteorStrikeBehaviour();

            Meteor meteor = Object.Instantiate(m_meteorPrefab, m_spawnPoint, Quaternion.identity);
            var task = meteor.Initialize(context.World.CurrentRoom.RoomModel.WorldPosition);

            await task;

            camera.Shake(m_cameraShakeDuration, m_cameraShakeMagnitude);
            var effectSpawnPoint = context.World.CurrentRoom.RoomModel.WorldPosition.ToVector3();
            Object.Instantiate(m_shockwavePrefab, effectSpawnPoint, Quaternion.identity);
            Object.Instantiate(m_explosionPrefab, effectSpawnPoint, Quaternion.identity);
            var particles = Object.Instantiate(m_explosionParticles, effectSpawnPoint, Quaternion.identity);
            GameObject.Destroy(particles.gameObject, 5);

            var entities = context.World.GetAllEntities();

            foreach (var entityId in entities)
            {
                if (entityId != context.ExecutorId)
                {
                    context.World.TryGetEntity(entityId, out var entity);
                    entity.Animation.PlayAnimation(AnimationType.Death);
                }
            }

            await Task.Delay(1000);

            KillEveryoneEffect kilEveryoneEffect = context.Plan.Effects
                .OfType<KillEveryoneEffect>()
                .FirstOrDefault();

            context.EffectDispatcher.Apply(context.World, kilEveryoneEffect);
        }
    }
}
