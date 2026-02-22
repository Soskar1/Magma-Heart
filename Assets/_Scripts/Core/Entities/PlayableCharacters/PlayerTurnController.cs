using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Effects;
using MagmaHeart.Core.Abilities.Effects;
using MagmaHeart.Core.Abilities.Effects.Handlers;
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
        private readonly GameWorld m_gameWorld;
        private readonly EffectDispatcher m_effectDispatcher;

        private AbilityPlan m_currentSelectedAbility;
        private HoverResult m_currentHover;

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

        public PlayerTurnController(MouseListener mouseListener, MouseHover mouseHover, ActionExecutor actionRunner, GameWorld gameWorld)
        {
            m_mouseListener = mouseListener;
            m_gameWorld = gameWorld;
            m_mouseHover = mouseHover;
            m_abilitySelector = new AbilitySelector(gameWorld);
            m_actionRunner = actionRunner;

            m_effectDispatcher = new EffectDispatcher();
            m_effectDispatcher.Register(new SpendResourceHandler());
            m_effectDispatcher.Register(new DamageHandler());
            m_effectDispatcher.Register(new MoveHandler());

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

            if (m_currentExecutor == null)
            {
                OnAbilitySelected?.Invoke(this, new OnAbilitySelectedEventArgs(null, args.HoverResult));
                return;
            }

            if (!CanExecuteActions)
            {
                OnAbilitySelected?.Invoke(this, new OnAbilitySelectedEventArgs(null, args.HoverResult));
                return;
            }

            m_currentSelectedAbility = m_abilitySelector.SelectAbility(args.HoverResult, m_currentExecutor);
            OnAbilitySelected?.Invoke(this, new OnAbilitySelectedEventArgs(m_currentSelectedAbility, args.HoverResult));
        }

        private async void HandleOnGameLeftMouseButtonClick()
        {
            if (m_currentSelectedAbility == null)
                return;

            await Execute(m_currentSelectedAbility);
        }

        public async Task StartTurn(EntityModel executor)
        {
            m_currentExecutor = executor;
            CanExecuteActions = true;

            m_mouseListener.OnGameLeftMouseButtonClick += HandleOnGameLeftMouseButtonClick;

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
        }

        private async Task Execute(AbilityPlan ability)
        {
            if (!CanExecuteActions)
                return;

            m_cancellationTokenSource = new CancellationTokenSource();
            
            CanExecuteActions = false;
            OnCombatActionExecutionStarted?.Invoke();

            foreach (AbilityEffect effect in ability.Effects)
                m_effectDispatcher.Apply(m_gameWorld, effect);

            OnCombatActionExecuted?.Invoke();
            
            if (m_currentExecutor != null)
                CanExecuteActions = true;
            
            m_mouseHover.Hover(m_currentHover.WorldPosition);
        }
    }
}
