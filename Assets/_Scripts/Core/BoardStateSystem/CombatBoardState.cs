using MagmaHeart.AI.States;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Services;

namespace MagmaHeart.Core.BoardStateSystem
{
    public class CombatBoardState : ActualBoardState
    {
        public Room Room { get; init; }
        public MagmaHeartServices Services { get; init; }

        public CombatBoardState(Room room, MagmaHeartServices services) : base(room)
        {
            Room = room;
            Services = services;
        }
    }
}
