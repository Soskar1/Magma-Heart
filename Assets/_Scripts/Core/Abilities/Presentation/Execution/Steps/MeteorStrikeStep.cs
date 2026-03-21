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

        public async Task Run(AbilityExecutionContext context, CancellationToken cancellationToken)
        {
            var camera = Camera.main.GetComponent<CameraController>();
            camera.DisableManualMovement();

            Meteor meteor = Object.Instantiate(m_meteorPrefab, m_spawnPoint, Quaternion.identity);
            var task = meteor.Initialize(context.World.CurrentRoom.RoomModel.WorldPosition);

            await task;

            KillEveryoneEffect kilEveryoneEffect = context.Plan.Effects
                .OfType<KillEveryoneEffect>()
                .FirstOrDefault();

            context.EffectDispatcher.Apply(context.World, kilEveryoneEffect);
        }
    }
}
