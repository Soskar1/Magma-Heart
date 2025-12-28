using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem.Services;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Spawning;

namespace MagmaHeart.Core.BoardStateSystem
{
    public class CombatBoardState : ActualBoardState
    {
        public Room Room { get; init; }
        public EntityMovementService MovementService { get; init; }
        public EntityAttackService AttackService { get; init; }

        public CombatBoardState(Room room, MagmaHeartSpawner spawner, EntityMovementService movementService) : base(room)
        {
            Room = room;
            MovementService = movementService;
            AttackService = new EntityAttackService(this, spawner);
        }
    }
}
