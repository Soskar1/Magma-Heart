using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class AttackActionSelector : ActionSelector
    {
        private readonly AttackAction m_attack;

        public AttackActionSelector(AttackAction action) => m_attack = action;

        protected override ActionSelectionResult TrySelectAction(Room room, RoomTile roomTile)
        {
            if (room.EntityIsOnTile(roomTile, out EntityModel entity))
            {
                if (!entity.IsPlayer && m_attack.CanAttack(entity))
                {
                    AttackActionArgs args = new AttackActionArgs(entity);
                    int energyCost = m_attack.GetEnergyCost(args);
                    return new ActionSelectionResult(m_attack, args, energyCost);
                }
            }

            return null;
        }
    }
}
