using MagmaHeart.BoardStateSystem.Actions;
using MagmaHeart.Core.Entities;
using MagmaHeart.Extensions;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public class MovementActionSelector : ActionSelector
    {
        private readonly MovementAction m_movementAction;

        public MovementActionSelector(MovementAction movementAction)
        {
            m_movementAction = movementAction;
        }

        protected override ActionSelectionResult TrySelectAction(CombatBoardState combatBoardState, EntityModel executor, RoomTile selectedTile)
        {
            Vector2 sourceTile = executor.GetCurrentTilePosition().ToVector2();

            // TODO: remove constant. Make it configurable per character or action.
            MovementActionPayload payload = new MovementActionPayload(MovementAction.MOVEMENT_DISTANCE_IN_TILES_FOR_ONE_ENERGY);
            MovementActionArgs args = new MovementActionArgs(executor, sourceTile, selectedTile.Position.ToVector2(), payload);

            if (combatBoardState.Room.TileIsAccessable(selectedTile) && m_movementAction.CanExecute(args, combatBoardState))
            {
                int energyCost = m_movementAction.GetEnergyCost(args, combatBoardState);
                return new ActionSelectionResult(m_movementAction, args, energyCost);
            }

            return null;
        }
    }
}
