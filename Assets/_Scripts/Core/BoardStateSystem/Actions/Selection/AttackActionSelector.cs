using MagmaHeart.Core.BoardStateSystem.Actions.Data;
using MagmaHeart.Core.Dungeon;
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
                AttackActionData attackActionData = executor.PossibleActions.Get<AttackActionData>();
                AttackActionArgs args = new AttackActionArgs(executor, target, attackActionData);

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
