using MagmaHeart.Core.BoardStateSystem.Actions.Data;
using MagmaHeart.Core.BoardStateSystem.Actions.Input;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Extensions;
using UnityEngine;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public class MovementActionSelector : ActionSelector
    {
        private readonly MovementAction m_movementAction;

        public MovementActionSelector(MovementAction movementAction) => m_movementAction = movementAction;

        protected override ActionSelectionResult TrySelectAction(CombatBoardState combatBoardState, EntityModel executor, RoomTile selectedTile)
        {
            MovementActionData data = executor.PossibleActions.Get<MovementActionData>();

            Vector2 sourceTile = executor.TilePosition.ToVector2();
            TargetPositionActionInput input = new TargetPositionActionInput(executor, selectedTile.Position.ToVector2());

            if (combatBoardState.Room.TileIsAccessable(selectedTile) && m_movementAction.TryCreateArgs(input, data, combatBoardState.Board, out MovementActionArgs args))
            {
                int energyCost = m_movementAction.GetEnergyCost(args, combatBoardState.Board);
                return new ActionSelectionResult(m_movementAction, args, energyCost);
            }

            return null;
        }
    }
}
