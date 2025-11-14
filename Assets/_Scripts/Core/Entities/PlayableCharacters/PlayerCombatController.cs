using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.Collections;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.CombatSystem;
using MagmaHeart.Core.Input;
using MagmaHeart.Core.UI;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class PlayerCombatController : CombatController
    {
        private readonly EnergyHUD m_energyHUD;
        private readonly CombatUI m_combatUI;
        private readonly CombatUserInput m_userInput;

        private RoomTile m_currentMouseTile;
        private AIUnit m_currentMouseOverEntity;

        private readonly AttackAction m_attackAction;
        private MagmaHeart.AI.Actions.Action m_currentAction;
        private ActionArgs m_currentActionArgs;

        private bool m_canExecuteActions;

        public bool CanExecuteAction
        {
            get => m_canExecuteActions;
            set
            {
                m_canExecuteActions = value;
                if (m_canExecuteActions)
                    m_userInput.MouseControl.ForceTriggerOnMouseChangedTile();
            }
        }

        public PlayerCombatController(Entity entity, GameUI gameUI, CombatUserInput userInput) : base(entity)
        {
            m_energyHUD = gameUI.EnergyHUD;
            m_combatUI = gameUI.CombatUI;
            m_userInput = userInput;

            m_attackAction = Entity.Model.PossibleActions.Get<AttackAction>();
        }

        public override void StartBattle(Room room, CircularList<Entity> turnOrder)
        {
            base.StartBattle(room, turnOrder);
            m_userInput.Enable();

            Entity.Energy.OnEnergyChanged += m_energyHUD.DisplayEnergy;

            // Move player at the center of the current standing tile
            RoomTile roomTile = CurrentRoom.GetRoomTile(Entity.transform.position);
            m_movementAction.MoveWithoutEnergyUsage(roomTile);
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

            CanExecuteAction = true;

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

            if (m_currentMouseOverEntity != null)
                m_currentMouseOverEntity = null;

            base.EndTurn();
        }

        private void HandleOnMouseChangedTile(object obj, OnMouseChangedTileEventArgs e)
        {
            if (!CanExecuteAction)
                return;

            if (m_currentMouseTile != null)
                CurrentRoom.HideCombatTileAt(m_currentMouseTile);

            RoomTile roomTile = CurrentRoom.GetRoomTile(e.TilePosition);

            if (CurrentRoom.EntityIsOnTile(roomTile, out EntityModel entity))
            {
                m_currentMouseOverEntity = entity;
                if (!entity.IsPlayer && m_attackAction.CanAttack(entity))
                {
                    // TODO: Outline the entity that can be attacked or display some kind of visual feedback
                    m_currentAction = m_attackAction;
                    m_currentActionArgs = new AttackActionArgs(entity);

                    m_energyHUD.DisplayEnergyPrice(AttackAction.ENERGY_COST);
                }
                else
                {
                    m_currentAction = null;
                    m_energyHUD.DisplayEnergyPrice(0);
                }
            }
            else if (CurrentRoom.TileIsAccessable(roomTile))
            {
                m_currentMouseOverEntity = null;

                m_currentAction = m_movementAction;
                m_currentActionArgs = new MovementActionArgs(roomTile);

                CurrentRoom.TryDisplayCombatTile(roomTile);

                int energyUsage = m_movementAction.GetEnergyUsage(roomTile);
                m_energyHUD.DisplayEnergyPrice(Math.Min(energyUsage, Entity.Energy.CurrentEnergy));
                if (!Entity.Energy.HasEnough(energyUsage))
                {
                    // TODO: Display some kind of visual feedback that the player can't move there
                }

                // m_energyHUD.DisplayEnergyPrice(m_movementAction.CurrentTheoreticalEnergyUsage);
                // m_aStarPathRenderer.CurrentPath = m_movementAction.CurrentPath.Select(tile => tile.TileCenter).ToList();
            }
            else
            {
                m_currentAction = null;
                m_currentActionArgs = null;
                m_currentMouseOverEntity = null;
            }

            m_currentMouseTile = roomTile;
        }

        private void HandleOnMouseClicked(object obj, EventArgs e)
        {
            if (m_currentAction == null || !CanExecuteAction)
                return;

            m_currentAction.Execute(m_currentActionArgs);
            m_userInput.MouseControl.ForceTriggerOnMouseChangedTile();
        }
    }
}
