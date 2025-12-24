using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem.Services;
using MagmaHeart.Core.Spawning;

namespace MagmaHeart.Core.BoardStateSystem
{
    public class CombatBoardState : ActualBoardState
    {
        public Room Room { get; init; }
        public EntityMovementService MovementService { get; init; }
        public EntityAttackService AttackService { get; init; }

        public CombatBoardState(Room room) : base(room)
        {
            Room = room;
        }

        public CombatBoardState(Room room, MagmaHeartSpawner spawner) : base(room)
        {
            Room = room;
            MovementService = new EntityMovementService();
            AttackService = new EntityAttackService(this, spawner);
        }
    }
}
