using MagmaHeart.AI.Actions;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Input;
using System;
using System.Threading;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class PlayerCombatController
    {
        private readonly ActionSelector m_actionSelector;
        private readonly EntityModel m_player;
        private readonly Battle m_battle;
        private readonly UserInput m_userInput;

        private CombatBoardState m_currentBoard;
        private ActionSelectionResult m_currentSelectionResult;

        private CancellationTokenSource m_cancellationTokenSource;

        public event EventHandler<OnCanExecuteActionsChangedEventArgs> OnCanExecuteActionsChanged;

        private bool m_canExecuteActions;
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

        public PlayerCombatController(EntityModel player, Battle battle, UserInput input)
        {
            m_player = player;
            m_battle = battle;
            m_userInput = input;

            MovementAction movementAction = new MovementAction(m_player);
            AttackAction attackAction = new AttackAction(m_player);
            m_actionSelector = new AttackActionSelector(attackAction);
            m_actionSelector.Next = new MovementActionSelector(movementAction);
        }

        public void Enable()
        {
            m_battle.OnBattleStarted += HandleOnBattleStarted;
            m_userInput.OnLeftMouseButtonClick += HandleOnLeftMouseButtonClick;
        }

        public void Disable()
        {
            m_battle.OnBattleStarted -= HandleOnBattleStarted;
            m_userInput.OnLeftMouseButtonClick -= HandleOnLeftMouseButtonClick;
        }

        public UnitAction SelectAction(RoomTile tile)
        {
            if (!CanExecuteActions)
                return null;

            m_currentSelectionResult = m_actionSelector.GetAction(m_currentBoard, tile);

            if (m_currentSelectionResult != null)
            {
                int energyCost = Math.Min(m_currentSelectionResult.EnergyCost, m_player.Energy.CurrentEnergy);
                m_player.Energy.PreviewCost = energyCost;
            }
            else
            {
                m_player.Energy.PreviewCost = 0;
            }

            return m_currentSelectionResult.Action;
        }

        private void HandleOnBattleStarted(object obj, OnBattleStartedEventArgs args) => m_currentBoard = args.CombatBoardState;

        private async void HandleOnLeftMouseButtonClick(object sender, EventArgs e)
        {
            if (m_currentSelectionResult == null || !CanExecuteActions)
                return;

            m_cancellationTokenSource = new CancellationTokenSource();
            
            CanExecuteActions = false;
            
            await m_currentSelectionResult.Action.ExecuteAsync(m_currentSelectionResult.Args, m_currentBoard, m_cancellationTokenSource.Token);

            if (m_cancellationTokenSource.IsCancellationRequested)
                return;

            CanExecuteActions = true;
        }
    }
}
