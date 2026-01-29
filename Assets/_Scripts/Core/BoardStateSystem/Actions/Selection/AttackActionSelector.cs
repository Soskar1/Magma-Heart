using MagmaHeart.Core.BoardStateSystem.Actions.Data;
using MagmaHeart.Core.BoardStateSystem.Actions.Input;
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
            if (combatBoardState.Room.TryGetEntity(selectedTile.Position, out Entity target))
            {
                AttackActionData attackActionData = executor.PossibleActions.Get<AttackActionData>();
                TargetEntityActionInput input = new TargetEntityActionInput(executor, target.Model);

                if (m_attack.TryCreateArgs(input, attackActionData, combatBoardState.Board, out AttackActionArgs args))
                {
                    int energyCost = m_attack.GetEnergyCost(args, combatBoardState.Board);
                    return new ActionSelectionResult(m_attack, args, energyCost);
                }
            }

            return null;
        }
    }
}
