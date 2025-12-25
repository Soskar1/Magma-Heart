using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using MagmaHeart.Bresenham;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Properties;
using MagmaHeart.Extensions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions.StateChanges
{
    public record AttackStateChange(EntityModel Attacker, EntityModel Target, float Damage, AttackType AttackType) : MagmaHeartStateChange
    {
        public override Task ApplyChangeToActualState(CombatBoardState actualBoard, CancellationToken cancellationToken)
        {
            actualBoard.Room.TryGetEntity(Attacker, out Entity attackerEntity);
            actualBoard.Room.TryGetEntity(Target, out Entity targetEntity);

            return actualBoard.AttackService.AttackEntityAsync(attackerEntity, targetEntity, Damage, AttackType, cancellationToken);
        }

        public override void ApplyChangeToSimulation(SimulatedBoardState simulation)
        {
            if (AttackType == AttackType.Ranged)
            {
                Vector2Int attackerPosition = simulation.GetProperty<PositionPropertySnapshot>(Attacker).Position.ToVector2Int();
                Vector2Int targetPosition = simulation.GetProperty<PositionPropertySnapshot>(Target).Position.ToVector2Int();
                IEnumerable<Vector2Int> tiles = BresenhamLine.DrawLine(attackerPosition, targetPosition);

                foreach (Vector2Int tile in tiles)
                {
                    bool isObstacle = simulation.Board.GetNodeType(tile) == BoardNodeType.Obstacle;

                    if (isObstacle && tile != attackerPosition && tile != targetPosition)
                        return;
                }
            }

            ApplyDamageStateChange damageStateChange = new ApplyDamageStateChange(Attacker, Target, Damage);
            simulation.ProduceStateChange(damageStateChange);
        }
    }
}
