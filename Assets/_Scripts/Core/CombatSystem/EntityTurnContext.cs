using MagmaHeart.AI.States;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Entities.CombatSystem
{
    public abstract class EntityTurnContext : TurnContext<EntityModel>
    {
        public Room CurrentRoom => CurrentCombatBoardState.Room;
        public CombatBoardState CurrentCombatBoardState { get; private set; }

        private TaskCompletionSource<bool> m_turnFinished;
        private CancellationTokenSource m_cancellationTokenSource;

        public EntityTurnContext(EntityModel model) : base(model) { }

        public virtual void StartBattle(CombatBoardState combatBoardState)
        {
            CurrentCombatBoardState = combatBoardState;
        }

        public virtual void EndBattle()
        {
            TypedModel.Energy.Reset();
            CurrentCombatBoardState = null;

            m_cancellationTokenSource.Cancel();
        }

        public virtual async Task StartTurnTask()
        {
            m_cancellationTokenSource = new CancellationTokenSource();
            await StartTurnAsync(CurrentCombatBoardState, m_cancellationTokenSource.Token);

            if (m_cancellationTokenSource.IsCancellationRequested)
                return;

            m_turnFinished = new TaskCompletionSource<bool>();
            await m_turnFinished.Task;
        }

        public override IEnumerable<StateChange> ProduceStartTurnChanges()
        {
            int newEnergyValue = TypedModel.Energy.CurrentEnergy + TypedModel.Stats.EnergyRegenerationPerTurn;
            return new List<StateChange>()
            {
                new UpdateEnergyStateChange(TypedModel, newEnergyValue)
            };
        }

        public virtual void EndTurn()
        {
            m_turnFinished.SetResult(true);
        }
    }
}