using MagmaHeart.Core.BoardStateSystem;
using MagmaHeart.Core.BoardStateSystem.Actions;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class AttackActionSelector : ActionSelector
    {
        private readonly AttackAction m_attack;

        public AttackActionSelector(AttackAction action) => m_attack = action;

        protected override ActionSelectionResult TrySelectAction(CombatBoardState combatBoardState, RoomTile selectedTile)
        {
            if (combatBoardState.Board.EntityIsOnTile(selectedTile, out EntityModel entity))
            {
                AttackActionArgs args = new AttackActionArgs(entity);

                if (!entity.IsPlayer && m_attack.CanExecute(args, combatBoardState))
                {
                    int energyCost = m_attack.GetEnergyCost(args, combatBoardState);
                    return new ActionSelectionResult(m_attack, args, energyCost);
                }
            }

            return null;
        }
    }
}
