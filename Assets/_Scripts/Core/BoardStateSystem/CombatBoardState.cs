using MagmaHeart.AI.Execution;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Services;

namespace MagmaHeart.Core.BoardStateSystem
{
    public class CombatBoardState
    {
        public Room Room { get; init; }
        public MagmaHeartServices Services { get; init; }

        public CombatBoardState(Room room, MagmaHeartServices services)
        {
            Room = room;
            Services = services;
        }
    }
}
