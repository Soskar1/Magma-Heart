using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.Entities.CombatSystem;
using MagmaHeart.Core.Input;
using MagmaHeart.Core.UI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class PlayerCombatController : CombatController
    {
        private readonly CombatUI m_combatUI;
        private readonly CombatUserInput m_userInput;

        private RoomTile m_currentMouseTile;

        private readonly AttackAction m_attackAction;
        private readonly ActionSelector m_actionSelectorChain;
        private ActionSelectionResult m_currentAction;

        private bool m_canExecuteActions;
        private CancellationTokenSource m_cancellationTokenSource;

        public bool CanExecuteActions
        {
            get => m_canExecuteActions;
            set
            {
                m_canExecuteActions = value;
                if (m_canExecuteActions)
                {
                    m_userInput.MouseControl.ForceTriggerOnMouseChangedTile();
                    m_combatUI.Enable();
                }
                else
                {
                    m_combatUI.Disable();
                }
            }
        }

        public PlayerCombatController(EntityModel model, GameUI gameUI, CombatUserInput userInput) : base(model)
        {
            m_combatUI = gameUI.CombatUI;
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
            RoomTile roomTile = CurrentRoom.GetRoomTile(EntityPresenter.transform.position);
            combatBoardState.MovementService.MoveEntityAsync(Entity, new List<RoomTile>() { roomTile });
        }

        public override void EndBattle()
        {
            base.EndBattle();
            m_userInput.Disable();

            m_cancellationTokenSource.Cancel();
        }

        public override Task StartTurnTask()
        {
            Task task = base.StartTurnTask();

            m_userInput.MouseControl.OnMouseChangedTile += HandleOnMouseChangedTile;
            m_userInput.MouseControl.OnMouseClicked += HandleOnMouseClicked;

            CanExecuteActions = true;

            return task;
        }

        public override void EndTurn()
        {
            m_userInput.MouseControl.OnMouseChangedTile -= HandleOnMouseChangedTile;
            m_userInput.MouseControl.OnMouseClicked -= HandleOnMouseClicked;

            if (m_currentMouseTile != null)
            {
                CurrentRoom.HideCombatTileAt(m_currentMouseTile);
                m_currentMouseTile = null;
            }

            CanExecuteActions = false;

            base.EndTurn();
        }

        private void HandleOnMouseChangedTile(object obj, OnMouseChangedTileEventArgs e)
        {
            if (!CanExecuteActions)
                return;

            if (m_currentMouseTile != null)
                CurrentRoom.HideCombatTileAt(m_currentMouseTile);

            RoomTile mouseTilePosition = CurrentRoom.GetRoomTile(e.TilePosition);
            m_currentAction = m_actionSelectorChain.GetAction(CurrentCombatBoardState, mouseTilePosition);
            
            if (m_currentAction != null)
            {
                CurrentRoom.TryDisplayCombatTile(mouseTilePosition);
                
                int energyCost = Math.Min(m_currentAction.EnergyCost, TypedModel.Energy.CurrentEnergy);
                TypedModel.Energy.PreviewCost = energyCost;
            }
            else
            {
                TypedModel.Energy.PreviewCost = 0;
            }

            m_currentMouseTile = mouseTilePosition;
        }

        private async void HandleOnMouseClicked(object obj, OnMouseClickedEventArgs e)
        {
            if (m_currentAction == null || !CanExecuteActions || e.IsOverUIElement)
                return;

            m_cancellationTokenSource = new CancellationTokenSource();

            CanExecuteActions = false;
            await m_currentAction.Action.ExecuteAsync(m_currentAction.Args, CurrentCombatBoardState, m_cancellationTokenSource.Token);

            if (m_cancellationTokenSource.IsCancellationRequested)
                return;

            CanExecuteActions = true;
            m_userInput.MouseControl.ForceTriggerOnMouseChangedTile();
        }
    }
}
