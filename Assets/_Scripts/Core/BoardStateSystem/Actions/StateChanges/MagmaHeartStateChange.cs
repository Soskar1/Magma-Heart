using MagmaHeart.AI.States;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.BoardStateSystem.Actions.StateChanges
{
    public abstract record MagmaHeartStateChange : StateChange
    {
        public virtual Task ApplyChangeToActualState(CombatBoardState actualBoard, CancellationToken cancellationToken) => Task.CompletedTask;

        public override Task ApplyChangeToActualStateAsync(ActualBoardState actualBoard, CancellationToken cancellationToken) => ApplyChangeToActualState((CombatBoardState)actualBoard, cancellationToken);
    }
}
