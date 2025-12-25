using MagmaHeart.Core.BoardStateSystem.Actions.Data;
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
            MovementActionData data = executor.PossibleActionDatas.Get<MovementActionData>();

            Vector2 sourceTile = executor.GetCurrentTilePosition().ToVector2();
            MovementActionArgs args = new MovementActionArgs(executor, sourceTile, selectedTile.Position.ToVector2(), data.MovementDistanceInTilesForOneEnergy);

            if (combatBoardState.Room.TileIsAccessable(selectedTile) && m_movementAction.CanExecute(args, combatBoardState))
            {
                int energyCost = m_movementAction.GetEnergyCost(args, combatBoardState);
                return new ActionSelectionResult(m_movementAction, args, energyCost);
            }

            return null;
        }
    }
}
