using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.AI.States
{
    public abstract class TurnContext
    {
        public abstract IEnumerable<StateChange> ProduceStartTurnChanges();

        internal void StartTurn(SimulatedBoardState simulatedBoardState)
        {
            IEnumerable<StateChange> changes = ProduceStartTurnChanges();
            simulatedBoardState.ApplyStateChanges(changes);
        }

        public async Task StartTurnAsync(ActualBoardState actualBoardState, CancellationToken cancellationToken)
        {
            IEnumerable<StateChange> changes = ProduceStartTurnChanges();
            await actualBoardState.ApplyStateChangesAsync(changes, cancellationToken);
        }
    }
}
