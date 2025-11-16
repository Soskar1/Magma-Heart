using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
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

        protected override ActionSelectionResult TrySelectAction(Room room, RoomTile roomTile)
        {
            if (room.TileIsAccessable(roomTile) && m_movementAction.GetPath(roomTile) != null)
            {
                MovementActionArgs args = new MovementActionArgs(roomTile);
                int energyCost = m_movementAction.GetEnergyCost(args);
                return new ActionSelectionResult(m_movementAction, args, energyCost);
            }

            return null;
        }
    }
}
