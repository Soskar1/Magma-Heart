using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.Entities.Presenters;
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
                //if (m_canExecuteActions)
                //    m_userInput.MouseControl.ForceTriggerOnMouseChangedTile();

                throw new Exception("TODO");

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
            CurrentRoom.TryGetEntityPresenter(TypedModel, out Entity entity);
            combatBoardState.MovementService.MoveEntityAsync(entity, new List<RoomTile>() { roomTile });
        }

        public override void EndBattle()
        {
            base.EndBattle();
            m_userInput.Disable();

            m_cancellationTokenSource.Cancel();
        }
    }
}
