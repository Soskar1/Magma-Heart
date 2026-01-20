using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using System;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public class ActionPreviewService : IActionPreviewService
    {
        private readonly ActionSelector m_selector;

        public ActionPreviewService(ActionSelector selector)
        {
            m_selector = selector;
        }

        public ActionPreview Preview(CombatBoardState combatBoardState, EntityModel executor, RoomTile selectedTile)
        {
            ActionSelectionResult result = m_selector.GetAction(combatBoardState, executor, selectedTile);

            if (result == null)
                return null;

            int energyCost = Math.Min(result.EnergyCost, executor.Energy.CurrentEnergy);

            return new ActionPreview(result.Action, result.Args, energyCost);
        }
    }
}
