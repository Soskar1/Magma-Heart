using MagmaHeart.AI.States;

namespace MagmaHeart.Core.BoardStateSystem
{
    public class CombatBoardState : ActualBoardState
    {
        public new Room Board { get; init; }

        public CombatBoardState(Room room) : base(room)
        {
            Board = room;
        }
    }
}
