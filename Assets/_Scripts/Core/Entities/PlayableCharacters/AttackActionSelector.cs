using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class AttackActionSelector : ActionSelector
    {
        private readonly AttackAction m_attack;
        private readonly EntityModel m_executor;

        public AttackActionSelector(AttackAction action, EntityModel executor)
        {
            m_attack = action;
            m_executor = executor;
        }

        protected override ActionSelectionResult TrySelectAction(CombatBoardState combatBoardState, RoomTile selectedTile)
        {
            if (combatBoardState.Room.EntityIsOnTile(selectedTile, out EntityModel target))
            {
                // TODO: remove constants. Move these parameters to config or entity stats
                AttackActionPayload payload = new AttackActionPayload(AttackAction.ENERGY_COST, AttackAction.ATTACK_DISTANCE, AttackAction.ATTACK_DAMAGE);
                AttackActionArgs args = new AttackActionArgs(m_executor, payload, target);

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
