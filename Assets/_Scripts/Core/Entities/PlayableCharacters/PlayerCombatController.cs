using MagmaHeart.Collections;
using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.Entities.CombatSystem;
using MagmaHeart.Core.Input;
using MagmaHeart.Core.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class PlayerCombatController : CombatController
    {
        private readonly EnergyHUD m_energyHUD;
        private readonly CombatUI m_combatUI;
        private readonly CombatUserInput m_userInput;

        private RoomTile m_currentMouseTile;

        private readonly AttackAction m_attackAction;
        private readonly ActionSelector m_actionSelectorChain;
        private ActionSelectionResult m_currentAction;

        private bool m_canExecuteActions;

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

        public PlayerCombatController(Entity entity, GameUI gameUI, CombatUserInput userInput) : base(entity)
        {
            m_energyHUD = gameUI.EnergyHUD;
            m_combatUI = gameUI.CombatUI;
            m_userInput = userInput;

            m_attackAction = Entity.Model.PossibleActions.Get<AttackAction>();
            MovementAction movementAction = Entity.Model.PossibleActions.Get<MovementAction>();

            m_actionSelectorChain = new AttackActionSelector(m_attackAction);
            m_actionSelectorChain.Next = new MovementActionSelector(movementAction);
        }

        public override void StartBattle(CombatBoardState combatBoardState, CircularList<Entity> turnOrder)
        {
            base.StartBattle(combatBoardState, turnOrder);
            m_userInput.Enable();

            Entity.Energy.OnEnergyChanged += m_energyHUD.DisplayEnergy;

            // Move player at the center of the current standing tile
            RoomTile roomTile = CurrentRoom.GetRoomTile(Entity.transform.position);
            combatBoardState.MovementService.MoveEntityAsync(Entity, new List<RoomTile>() { roomTile });
        }

        public override void EndBattle()
        {
            base.EndBattle();
            m_userInput.Disable();

            Entity.Energy.OnEnergyChanged -= m_energyHUD.DisplayEnergy;
        }

        public override Task StartTurn()
        {
            Task task = base.StartTurn();

            m_combatUI.Show();
            m_energyHUD.Show();
            m_energyHUD.DisplayEnergy();

            m_userInput.MouseControl.OnMouseChangedTile += HandleOnMouseChangedTile;
            m_userInput.MouseControl.OnMouseClicked += HandleOnMouseClicked;

            CanExecuteActions = true;

            return task;
        }

        public override void EndTurn()
        {
            m_combatUI.Hide();
            m_energyHUD.Hide();

            m_userInput.MouseControl.OnMouseChangedTile -= HandleOnMouseChangedTile;
            m_userInput.MouseControl.OnMouseClicked -= HandleOnMouseClicked;

            if (m_currentMouseTile != null)
            {
                CurrentRoom.HideCombatTileAt(m_currentMouseTile);
                m_currentMouseTile = null;
            }

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
                
                int energyCost = Math.Min(m_currentAction.EnergyCost, Entity.Energy.CurrentEnergy);
                m_energyHUD.DisplayEnergyPrice(energyCost);
            }
            else
            {
                m_energyHUD.DisplayEnergyPrice(0);
            }

            m_currentMouseTile = mouseTilePosition;
        }

        private async void HandleOnMouseClicked(object obj, OnMouseClickedEventArgs e)
        {
            if (m_currentAction == null || !CanExecuteActions || e.IsOverUIElement)
                return;

            CanExecuteActions = false;
            await m_currentAction.Action.ExecuteAsync(m_currentAction.Args, CurrentCombatBoardState);
            CanExecuteActions = true;

            m_userInput.MouseControl.ForceTriggerOnMouseChangedTile();
        }
    }
}
