using MagmaHeart.AI.States;

namespace MagmaHeart.Core.BoardStateSystem.Actions.StateChanges
{
    public abstract record MagmaHeartStateChange : StateChange
    {
        public abstract void ApplyChangeToActualState(CombatBoardState actualBoard);

        public override void ApplyChangeToActualState(ActualBoardState actualBoard) => ApplyChangeToActualState((CombatBoardState)actualBoard);
    }
}
