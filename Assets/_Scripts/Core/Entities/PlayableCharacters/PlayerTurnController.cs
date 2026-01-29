using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Input;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class PlayerTurnController : ITurnController
    {
        private readonly IActionPreviewProvider m_actionPreviewProvider;
        private readonly MouseListener m_mouseListener;

        private ActionPreview m_currentPreview;
        private CombatBoardState m_currentBoardState;

        public event EventHandler<OnCanExecuteActionsChangedEventArgs> OnCanExecuteActionsChanged;
        public event Action OnCombatActionExecutionStarted;
        public event Action OnCombatActionExecuted;

        private TaskCompletionSource<bool> m_turnFinished;
        private CancellationTokenSource m_cancellationTokenSource;

        private bool m_canExecuteActions;
        public bool CanExecuteActions
        {
            get => m_canExecuteActions;
            private set
            {
                m_canExecuteActions = value;

                OnCanExecuteActionsChangedEventArgs args = new OnCanExecuteActionsChangedEventArgs(m_canExecuteActions);
                OnCanExecuteActionsChanged?.Invoke(this, args);
            }
        }

        public PlayerTurnController(MouseListener mouseListener, IActionPreviewProvider previewProvider)
        {
            m_mouseListener = mouseListener;
            m_actionPreviewProvider = previewProvider;
        }

        public void Enable()
        {
            m_mouseListener.OnGameLeftMouseButtonClick += HandleOnGameLeftMouseButtonClick;
            m_actionPreviewProvider.OnActionPreviewChanged += HandleOnActionPreviewChanged;
        }

        public void Disable()
        {
            m_mouseListener.OnGameLeftMouseButtonClick -= HandleOnGameLeftMouseButtonClick;
            m_actionPreviewProvider.OnActionPreviewChanged -= HandleOnActionPreviewChanged;
        }

        private void HandleOnActionPreviewChanged(object obj, OnActionPreviewChangedEventArgs args) => m_currentPreview = args.ActionPreview;

        private async void HandleOnGameLeftMouseButtonClick()
        {
            if (m_currentPreview == null)
                return;

            await Execute(m_currentPreview);
        }

        public void EndBattle()
        {
            m_currentBoardState = null;
            m_cancellationTokenSource.Cancel();
        }

        public async Task StartTurn(CombatBoardState state, TurnOrder turnOrder)
        {
            m_currentBoardState = state;

            CanExecuteActions = true;

            m_turnFinished = new TaskCompletionSource<bool>();
            await m_turnFinished.Task;
        }

        public void EndTurn()
        {
            CanExecuteActions = false;

            m_turnFinished.SetResult(true);
        }

        private async Task Execute(ActionPreview preview)
        {
            if (!CanExecuteActions)
                return;

            m_cancellationTokenSource = new CancellationTokenSource();
            
            CanExecuteActions = false;
            OnCombatActionExecutionStarted?.Invoke();
            preview.Action.Execute(preview.Args, m_currentBoardState.Room);

            throw new Exception("FIX THIS. NEED RUNNER");

            if (m_cancellationTokenSource.IsCancellationRequested)
                return;
            
            CanExecuteActions = true;
            OnCombatActionExecuted?.Invoke();
        }
    }
}
