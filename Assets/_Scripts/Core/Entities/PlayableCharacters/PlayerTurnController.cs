using MagmaHeart.AI.Execution;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Input;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class PlayerTurnController : ITurnController
    {
        private readonly IActionPreviewProvider m_actionPreviewProvider;
        private readonly MouseListener m_mouseListener;
        private readonly ActionExecutor m_actionRunner;

        private ActionPreview m_currentPreview;
        private Room m_currentRoom;

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

        public PlayerTurnController(MouseListener mouseListener, IActionPreviewProvider previewProvider, ActionExecutor actionRunner)
        {
            m_mouseListener = mouseListener;
            m_actionPreviewProvider = previewProvider;
            m_actionRunner = actionRunner;
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
            m_currentRoom = null;
            m_cancellationTokenSource.Cancel();
        }

        public async Task StartTurn(Room room, TurnOrder turnOrder)
        {
            m_currentRoom = room;

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
            
            IEnumerable<IBoardCommand> commands = preview.Action.Execute(preview.Args, m_currentRoom);
            await m_actionRunner.ApplyAsync(m_currentRoom, commands, m_cancellationTokenSource.Token);

            if (m_cancellationTokenSource.IsCancellationRequested)
                return;
            
            CanExecuteActions = true;
            OnCombatActionExecuted?.Invoke();
        }
    }
}
