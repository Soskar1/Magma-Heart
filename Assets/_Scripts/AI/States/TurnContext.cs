using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.AI.States
{
    public abstract class TurnContext
    {
        public abstract IEnumerable<StateChange> ProduceStartTurnChanges(AIUnitModel model);

        internal void StartTurn(SimulatedBoardState simulatedBoardState, AIUnitModel model)
        {
            IEnumerable<StateChange> changes = ProduceStartTurnChanges(model);
            simulatedBoardState.ApplyStateChanges(changes);
        }

        public async Task StartTurnAsync(ActualBoardState actualBoardState, AIUnitModel model, CancellationToken cancellationToken)
        {
            IEnumerable<StateChange> changes = ProduceStartTurnChanges(model);
            await actualBoardState.ApplyStateChangesAsync(changes, cancellationToken);
        }

        internal void UndoTurn(SimulatedBoardState simulatedBoardState) => simulatedBoardState.Undo();
    }
}
