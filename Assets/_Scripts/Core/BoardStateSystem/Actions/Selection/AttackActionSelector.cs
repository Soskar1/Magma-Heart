using MagmaHeart.Core.BoardStateSystem.Actions.Data;
using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public class AttackActionSelector : ActionSelector
    {
        private readonly AttackAction m_attack;

        public AttackActionSelector(AttackAction action) => m_attack = action;

        protected override ActionSelectionResult TrySelectAction(CombatBoardState combatBoardState, EntityModel executor, RoomTile selectedTile)
        {
            if (combatBoardState.Room.EntityIsOnTile(selectedTile, out EntityModel target))
            {
                AttackActionData attackActionData = executor.PossibleActionDatas.Get<AttackActionData>();

                // TODO: use argument creator here
                AttackActionArgs args = new AttackActionArgs(executor, target, attackActionData.EnergyCost, attackActionData.AttackDistance, attackActionData.AttackDamage, attackActionData.AttackType);

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
