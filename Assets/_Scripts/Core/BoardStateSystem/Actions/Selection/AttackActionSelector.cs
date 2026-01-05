using MagmaHeart.AI.Actions;
using MagmaHeart.Core.BoardStateSystem.Actions.Data;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public class AttackActionSelector : ActionSelector
    {
        private readonly AttackAction m_attack;
        private readonly AttackActionResolver m_resolver;

        public AttackActionSelector(AttackAction action)
        {
            m_attack = action;
            m_resolver = new AttackActionResolver();
        }

        protected override ActionSelectionResult TrySelectAction(CombatBoardState combatBoardState, EntityModel executor, RoomTile selectedTile)
        {
            if (combatBoardState.Room.EntityIsOnTile(selectedTile, out EntityModel target))
            {
                AttackActionData attackActionData = executor.PossibleActions.Get<AttackActionData>();
                bool argsExist = m_resolver.TryResolve(attackActionData.GetDefinition(), executor, combatBoardState, out ActionArgs args);

                if (argsExist && !target.IsPlayer && m_attack.CanExecute(args, combatBoardState))
                {
                    int energyCost = m_attack.GetEnergyCost((AttackActionArgs)args, combatBoardState);
                    return new ActionSelectionResult(m_attack, args, energyCost);
                }
            }

            return null;
        }
    }
}
