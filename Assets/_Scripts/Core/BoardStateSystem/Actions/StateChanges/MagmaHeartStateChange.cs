using MagmaHeart.AI.States;
using System.Threading.Tasks;

namespace MagmaHeart.Core.BoardStateSystem.Actions.StateChanges
{
    public abstract record MagmaHeartStateChange : StateChange
    {
        public virtual Task ApplyChangeToActualState(CombatBoardState actualBoard) => Task.CompletedTask;

        public override Task ApplyChangeToActualStateAsync(ActualBoardState actualBoard) => ApplyChangeToActualState((CombatBoardState)actualBoard);
    }
}
