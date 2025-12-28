using MagmaHeart.Core.BoardStateSystem.Actions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class PlayerTurnContext : EntityTurnContext
    {
        private bool m_canExecuteActions;

        public event EventHandler<OnCanExecuteActionsChangedEventArgs> OnCanExecuteActionsChanged;
        public event Action OnCombatActionExecutionStarted;
        public event Action OnCombatActionExecuted;

        private TaskCompletionSource<bool> m_turnFinished;
        private CancellationTokenSource m_cancellationTokenSource;

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

        public override void EndBattle()
        {
            base.EndBattle();

            m_cancellationTokenSource.Cancel();
        }

        public override async Task StartTurnTask()
        {
            m_cancellationTokenSource = new CancellationTokenSource();
            await StartTurnAsync(CurrentCombatBoardState, m_cancellationTokenSource.Token);

            if (m_cancellationTokenSource.IsCancellationRequested)
                return;

            CanExecuteActions = true;

            m_turnFinished = new TaskCompletionSource<bool>();
            await m_turnFinished.Task;
        }

        public override void EndTurn()
        {
            CanExecuteActions = false;

            m_turnFinished.SetResult(true);
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
