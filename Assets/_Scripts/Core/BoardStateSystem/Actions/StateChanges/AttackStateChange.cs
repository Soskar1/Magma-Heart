using MagmaHeart.AI.States;
using MagmaHeart.Core.Entities;
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
            if (AttackType == AttackType.Melee)
            {
                ApplyDamageStateChange damageStateChange = new ApplyDamageStateChange(Attacker, Target, Damage);
                simulation.ProduceStateChange(damageStateChange);
            }
            else
            {
                Debug.Log("[AttackStateChange] Handle ranged attacks!");
                // TODO:
                // 1) Get all tiles in line between attacker and target
                // 2) Find walls
                // 3) If found at least one wall, then do not produce any changes to the state
                // 4) No walls - produce ApplyDamageStateChange
            }
        }
    }
}
