using MagmaHeart.AI.Actions;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.Input;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class PlayerTurnContext : EntityTurnContext
    {
        private readonly UserInput m_userInput;

        private readonly AttackAction m_attackAction;
        private readonly ActionSelector m_actionSelectorChain;
        private ActionSelectionResult m_currentAction;

        private bool m_canExecuteActions;
        private CancellationTokenSource m_cancellationTokenSource;

        public event EventHandler<OnCanExecuteActionsChangedEventArgs> OnCanExecuteActionsChanged;

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

        public PlayerTurnContext(EntityModel model, UserInput userInput) : base(model)
        {
            m_userInput = userInput;

            m_attackAction = model.PossibleActions.Get<AttackAction>();
            MovementAction movementAction = model.PossibleActions.Get<MovementAction>();

            m_actionSelectorChain = new AttackActionSelector(m_attackAction);
            m_actionSelectorChain.Next = new MovementActionSelector(movementAction);
        }

        public override void StartBattle(CombatBoardState combatBoardState)
        {
            base.StartBattle(combatBoardState);
            m_userInput.Enable();

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

            m_userInput.OnLeftMouseButtonClick += HandleOnLeftMouseButtonClick;
            CanExecuteActions = true;

            return task;
        }

        public override void EndTurn()
        {
            m_userInput.OnLeftMouseButtonClick -= HandleOnLeftMouseButtonClick;

            CanExecuteActions = false;

            base.EndTurn();
        }

        public UnitAction SelectAction(RoomTile tile)
        {
            if (!CanExecuteActions)
                return null;

            m_currentAction = m_actionSelectorChain.GetAction(CurrentCombatBoardState, tile);

            if (m_currentAction != null)
            {
                int energyCost = Math.Min(m_currentAction.EnergyCost, TypedModel.Energy.CurrentEnergy);
                TypedModel.Energy.PreviewCost = energyCost;
                return m_currentAction.Action;
            }

            TypedModel.Energy.PreviewCost = 0;
            return null;
        }

        private async void HandleOnLeftMouseButtonClick(object sender, EventArgs e)
        {
            if (m_currentAction == null || !CanExecuteActions)
                return;

            m_cancellationTokenSource = new CancellationTokenSource();

            CanExecuteActions = false;
            await m_currentAction.Action.ExecuteAsync(m_currentAction.Args, CurrentCombatBoardState, m_cancellationTokenSource.Token);

            if (m_cancellationTokenSource.IsCancellationRequested)
                return;

            CanExecuteActions = true;
        }
    }
}
