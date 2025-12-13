using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Extensions;
using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class MovementActionSelector : ActionSelector
    {
        private readonly MovementAction m_movementAction;
        private readonly EntityModel m_executor;

        public MovementActionSelector(MovementAction movementAction, EntityModel executor)
        {
            m_movementAction = movementAction;
            m_executor = executor;
        }

        protected override ActionSelectionResult TrySelectAction(CombatBoardState combatBoardState, RoomTile selectedTile)
        {
            Vector2 sourceTile = m_executor.GetCurrentTilePosition().ToVector2();
            MovementActionArgs args = new MovementActionArgs(m_executor, sourceTile, selectedTile.Position.ToVector2());

            if (combatBoardState.Room.TileIsAccessable(selectedTile) && m_movementAction.CanExecute(args, combatBoardState))
            {
                int energyCost = m_movementAction.GetEnergyCost(args, combatBoardState);
                return new ActionSelectionResult(m_movementAction, args, energyCost);
            }

            return null;
        }
    }
}
