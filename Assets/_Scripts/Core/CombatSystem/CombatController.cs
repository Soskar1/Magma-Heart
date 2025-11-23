using MagmaHeart.AI.States;
using MagmaHeart.Collections;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions.StateChanges;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Entities.CombatSystem
{
    public abstract class CombatController : TurnContext
    {
        public Entity Entity { get; init; }
        public Room CurrentRoom => CurrentCombatBoardState.Board;
        public CombatBoardState CurrentCombatBoardState { get; private set; }

        private TaskCompletionSource<bool> m_turnFinished;
        private CancellationTokenSource m_cancellationTokenSource;

        public CombatController(EntityModel model) : base(model) => Entity = model.Entity;

        public virtual void StartBattle(CombatBoardState combatBoardState)
        {
            CurrentCombatBoardState = combatBoardState;
        }

        public virtual void EndBattle()
        {
            Entity.Energy.Reset();
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
            EntityModel model = (EntityModel)Owner;
            int newEnergyValue = model.Energy.CurrentEnergy + model.Stats.EnergyRegenerationPerTurn;
            return new List<StateChange>()
            {
                new UpdateEnergyStateChange(model, newEnergyValue)
            };
        }

        public virtual void EndTurn()
        {
            m_turnFinished.SetResult(true);
        }
    }
}