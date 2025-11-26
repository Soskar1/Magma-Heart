using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Extensions;
using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class MovementActionSelector : ActionSelector
    {
        private readonly MovementAction m_movementAction;

        public MovementActionSelector(MovementAction movementAction)
        {
            m_movementAction = movementAction;
        }

        protected override ActionSelectionResult TrySelectAction(CombatBoardState combatBoardState, RoomTile selectedTile)
        {
            Vector2 sourceTile = m_movementAction.ActionPossessor.GetCurrentTilePosition().ToVector2();
            MovementActionArgs args = new MovementActionArgs(sourceTile, selectedTile.Position.ToVector2());

            if (combatBoardState.Room.TileIsAccessable(selectedTile) && m_movementAction.CanExecute(args, combatBoardState))
            {
                int energyCost = m_movementAction.GetEnergyCost(args, combatBoardState);
                return new ActionSelectionResult(m_movementAction, args, energyCost);
            }

            return null;
        }
    }
}
