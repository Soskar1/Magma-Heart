using MagmaHeart.Abilities;
using MagmaHeart.Core.Abilities.Presentation.Execution;
using MagmaHeart.Core.Abilities.Selection;
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
        private readonly AbilityExecutionRunner m_abilityExecutionRunner;
        private readonly MouseListener m_mouseListener;
        private readonly MouseHover m_mouseHover;
        private readonly AbilitySelectionState m_abilitySelectionState;

        private AbilityPlan m_currentSelectedAbility;
        private HoverResult m_currentHover;

        public event EventHandler<OnAbilitySelectedEventArgs> OnAbilitySelected;
        public event EventHandler<OnCanExecuteActionsChangedEventArgs> OnCanExecuteActionsChanged;
        public event EventHandler OnAbilityDisarmed;

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

        public PlayerTurnController(MouseListener mouseListener, MouseHover mouseHover, AbilityExecutionRunner abilityExecutionRunner, AbilitySelector abilitySelector)
        {
            m_mouseListener = mouseListener;
            m_mouseHover = mouseHover;
            m_abilitySelector = abilitySelector;
            m_abilityExecutionRunner = abilityExecutionRunner;
            m_abilitySelectionState = new AbilitySelectionState();

            m_mouseHover.OnHoverResultChanged += HandleOnHoverResultChanged;
        }

        public void Dispose()
        {
            m_mouseHover.OnHoverResultChanged -= HandleOnHoverResultChanged;
        }

        private void HandleOnHoverResultChanged(object _, OnHoverResultChangedEventArgs args)
        {
            if (m_currentHover != null && m_currentHover == args.HoverResult)
                return;

            m_currentHover = args.HoverResult;

            if (m_currentExecutor == null || !CanExecuteActions)
            {
                OnAbilitySelected?.Invoke(this, new OnAbilitySelectedEventArgs(null, args.HoverResult));
                return;
            }

            AbilityDefinition armedAbility = m_abilitySelectionState.HasArmedAbility ? m_abilitySelectionState.ArmedAbility : null;

            m_currentSelectedAbility = m_abilitySelector.SelectAbility(args.HoverResult, m_currentExecutor, armedAbility);
            OnAbilitySelected?.Invoke(this, new OnAbilitySelectedEventArgs(m_currentSelectedAbility, args.HoverResult));
        }

        public void ArmAbility(AbilityDefinition ability) => m_abilitySelectionState.Arm(ability);
        public void DisarmAbility()
        {
            m_abilitySelectionState.Disarm();
            OnAbilityDisarmed?.Invoke(this, EventArgs.Empty);
        }

        private async void HandleOnGameLeftMouseButtonClick(object _, EventArgs __)
        {
            if (m_currentSelectedAbility == null)
                return;

            await Execute(m_currentSelectedAbility);
        }

        private void HandleOnGameRightMouseButtonClick(object _, EventArgs __)
        {
            if (m_abilitySelectionState.HasArmedAbility)
                DisarmAbility();
        }

        public async Task StartTurn(EntityModel executor)
        {
            m_currentExecutor = executor;
            CanExecuteActions = true;

            m_mouseListener.OnGameLeftMouseButtonClick += HandleOnGameLeftMouseButtonClick;
            m_mouseListener.OnGameRightMouseButtonClick += HandleOnGameRightMouseButtonClick;

            m_turnFinished = new TaskCompletionSource<bool>();
            await m_turnFinished.Task;
        }

        public void EndTurn()
        {
            Disable();

            m_turnFinished.SetResult(true);
        }

        public void Disable()
        {
            if (m_cancellationTokenSource != null)
                m_cancellationTokenSource.Cancel();
            
            m_currentExecutor = null;
            CanExecuteActions = false;
            m_mouseListener.OnGameLeftMouseButtonClick -= HandleOnGameLeftMouseButtonClick;
            m_mouseListener.OnGameRightMouseButtonClick -= HandleOnGameRightMouseButtonClick;
        }

        private async Task Execute(AbilityPlan ability)
        {
            if (!CanExecuteActions)
                return;

            m_cancellationTokenSource = new CancellationTokenSource();
            
            CanExecuteActions = false;

            await m_abilityExecutionRunner.Run(ability, m_currentExecutor, m_cancellationTokenSource.Token);

            if (m_currentExecutor != null)
                CanExecuteActions = true;

            if (m_abilitySelectionState.HasArmedAbility)
                DisarmAbility();

            Vector2 hoverWorldPosition = m_currentHover.WorldPosition;
            m_currentHover = null;
            m_mouseHover.Hover(hoverWorldPosition);
        }
    }
}
