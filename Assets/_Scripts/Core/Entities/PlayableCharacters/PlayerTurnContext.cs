using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class PlayerTurnContext : EntityTurnContext
    {
        private bool m_canExecuteActions;
        private CancellationTokenSource m_cancellationTokenSource;

        public event EventHandler<OnCanExecuteActionsChangedEventArgs> OnCanExecuteActionsChanged;
        public event Action OnCombatActionExecutionStarted;
        public event Action OnCombatActionExecuted;

        public bool CanExecuteActions
        {
            get => m_canExecuteActions;
            set
            {
                m_canExecuteActions = value;

                OnCanExecuteActionsChangedEventArgs args = new OnCanExecuteActionsChangedEventArgs(m_canExecuteActions);
                OnCanExecuteActionsChanged?.Invoke(this, args);
            }
        }

        public PlayerTurnContext(EntityModel model) : base(model) {}

        public override void StartBattle(CombatBoardState combatBoardState)
        {
            base.StartBattle(combatBoardState);

            // Move player at the center of the current standing tile
            RoomTile roomTile = CurrentRoom.GetRoomTile(TypedModel.GetCurrentTilePosition());
            CurrentRoom.TryGetEntity(TypedModel, out Entity entity);
            combatBoardState.MovementService.MoveEntityAsync(entity, new List<RoomTile>() { roomTile });
        }

        public override void EndBattle()
        {
            base.EndBattle();

            m_cancellationTokenSource.Cancel();
        }

        public override Task StartTurnTask()
        {
            Task task = base.StartTurnTask();

            CanExecuteActions = true;

            return task;
        }

        public override void EndTurn()
        {
            CanExecuteActions = false;

            base.EndTurn();
        }

        public async Task Execute(ActionPreview preview)
        {
            if (!CanExecuteActions)
                return;

            m_cancellationTokenSource = new CancellationTokenSource();
            
            CanExecuteActions = false;
            OnCombatActionExecutionStarted?.Invoke();
            await preview.Action.ExecuteAsync(preview.Args, CurrentCombatBoardState, m_cancellationTokenSource.Token);
            
            if (m_cancellationTokenSource.IsCancellationRequested)
                return;
            
            CanExecuteActions = true;
            OnCombatActionExecuted?.Invoke();
        }
    }
}
