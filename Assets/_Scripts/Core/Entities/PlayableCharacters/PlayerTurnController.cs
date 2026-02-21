using MagmaHeart.Abilities;
using MagmaHeart.Core.Abilities.Selection;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.Input.Mouse;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class PlayerTurnController : IDisposable
    {
        private readonly AbilitySelector m_abilitySelector;
        private readonly ActionExecutor m_actionRunner;
        private readonly MouseListener m_mouseListener;
        private readonly MouseHover m_mouseHover;
        private readonly IGameWorld m_gameWorld;

        private AbilityPlan m_currentSelectedAbility;

        public event EventHandler<OnAbilitySelectedEventArgs> OnAbilitySelected;
        public event EventHandler<OnCanExecuteActionsChangedEventArgs> OnCanExecuteActionsChanged;
        public event Action OnCombatActionExecutionStarted;
        public event Action OnCombatActionExecuted;

        private TaskCompletionSource<bool> m_turnFinished;
        private CancellationTokenSource m_cancellationTokenSource;

        private EntityModel m_currentExecutor;

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

        public PlayerTurnController(MouseListener mouseListener, MouseHover mouseHover, ActionExecutor actionRunner, IGameWorld gameWorld)
        {
            m_mouseListener = mouseListener;
            m_gameWorld = gameWorld;
            m_mouseHover = mouseHover;
            m_abilitySelector = new AbilitySelector(gameWorld);
            m_actionRunner = actionRunner;
        }

        public void Enable()
        {
            m_mouseListener.OnGameLeftMouseButtonClick += HandleOnGameLeftMouseButtonClick;
            m_mouseHover.OnHoverResultChanged += HandleOnHoverResultChanged;
        }

        public void Dispose()
        {
            m_mouseListener.OnGameLeftMouseButtonClick -= HandleOnGameLeftMouseButtonClick;
            m_mouseHover.OnHoverResultChanged -= HandleOnHoverResultChanged;
        }

        private void HandleOnHoverResultChanged(object _, OnHoverResultChangedEventArgs args)
        {
            if (m_currentExecutor == null)
            {
                Debug.Log("Current executor is null. Cannot select ability.");

                OnAbilitySelected?.Invoke(this, new OnAbilitySelectedEventArgs(null, args.HoverResult));
                return;
            }

            if (!CanExecuteActions)
            {
                Debug.Log("Cannot execute actions. Cannot select ability.");

                OnAbilitySelected?.Invoke(this, new OnAbilitySelectedEventArgs(null, args.HoverResult));
                return;
            }

            AbilityPlan plan = m_abilitySelector.SelectAbility(args.HoverResult, m_currentExecutor);
            Debug.Log($"Successfully selected ability: {plan}");
            OnAbilitySelected?.Invoke(this, new OnAbilitySelectedEventArgs(plan, args.HoverResult));
        }

        private async void HandleOnGameLeftMouseButtonClick()
        {
            if (m_currentSelectedAbility == null)
                return;

            await Execute(m_currentSelectedAbility);
        }

        public void EndBattle()
        {
            m_cancellationTokenSource.Cancel();
        }

        public async Task StartTurn(EntityModel executor)
        {
            m_currentExecutor = executor;
            CanExecuteActions = true;

            Enable();

            m_turnFinished = new TaskCompletionSource<bool>();
            await m_turnFinished.Task;
        }

        public void EndTurn()
        {
            m_currentExecutor = null;
            CanExecuteActions = false;

            Dispose();

            m_turnFinished.SetResult(true);
        }

        private async Task Execute(AbilityPlan ability)
        {
            if (!CanExecuteActions)
                return;

            m_cancellationTokenSource = new CancellationTokenSource();
            
            CanExecuteActions = false;
            OnCombatActionExecutionStarted?.Invoke();

            Debug.Log($"Executing ability: {ability}. TODO");
            //IEnumerable<IBoardCommand> commands = ability.Action.Execute(ability.Args, m_currentRoom);
            //await m_actionRunner.ApplyAsync(m_currentRoom, commands, m_cancellationTokenSource.Token);

            //if (m_cancellationTokenSource.IsCancellationRequested)
            //    return;

            CanExecuteActions = true;
            OnCombatActionExecuted?.Invoke();
        }
    }
}
