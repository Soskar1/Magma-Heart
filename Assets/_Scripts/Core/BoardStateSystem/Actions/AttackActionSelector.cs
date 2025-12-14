using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public class AttackActionSelector : ActionSelector
    {
        private readonly AttackAction m_attack;

        public AttackActionSelector(AttackAction action)
        {
            m_attack = action;
        }

        protected override ActionSelectionResult TrySelectAction(CombatBoardState combatBoardState, EntityModel executor, RoomTile selectedTile)
        {
            if (combatBoardState.Room.EntityIsOnTile(selectedTile, out EntityModel target))
            {
                // TODO: remove constants. Move these parameters to config or entity stats
                AttackActionPayload payload = new AttackActionPayload(AttackAction.ENERGY_COST, AttackAction.ATTACK_DISTANCE, AttackAction.ATTACK_DAMAGE);
                AttackActionArgs args = new AttackActionArgs(executor, payload, target);

                if (!target.IsPlayer && m_attack.CanExecute(args, combatBoardState))
                {
                    int energyCost = m_attack.GetEnergyCost(args, combatBoardState);
                    return new ActionSelectionResult(m_attack, args, energyCost);
                }
            }

            return null;
        }
    }
}
