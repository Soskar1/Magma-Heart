using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.AI.States
{
    public abstract class TurnContext
    {
        public AIUnit Owner { get; init; }

        public TurnContext(AIUnit owner) => Owner = owner;

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

        internal void UndoTurn(SimulatedBoardState simulatedBoardState) => simulatedBoardState.Undo();
    }
}
